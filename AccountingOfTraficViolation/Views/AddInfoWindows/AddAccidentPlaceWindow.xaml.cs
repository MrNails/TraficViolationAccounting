using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using AccountingOfTraficViolation.Models;
using AccountingOfTraficViolation.Services;

namespace AccountingOfTraficViolation.Views.AddInfoWindows
{
    /// <summary>
    /// Логика взаимодействия для AddAccidentPlaceWindow.xaml
    /// </summary>
    public partial class AddAccidentPlaceWindow : Window
    {
        private bool isRoadIndexAndNumberValid;
        private string roadIndexAndNumber;

        public AccidentOnHighway AccidentOnHighway { get; private set; }
        public AccidentOnVillage AccidentOnVillage { get; private set; }

        public AddAccidentPlaceWindow() : this(null, null)
        { }

        public AddAccidentPlaceWindow(AccidentOnHighway accidentOnHighway, AccidentOnVillage accidentOnVillage)
        {
            InitializeComponent();

            if (accidentOnVillage != null)
            {
                AccidentOnVillage = accidentOnVillage.Clone();

                DataContext = AccidentOnVillage;

                if (AccidentOnHighwayGroup.Content is Grid)
                {
                    ((Grid)AccidentOnHighwayGroup.Content).IsEnabled = false;
                }

                AccidentOnHighway = new AccidentOnHighway();
            }
            else if (accidentOnHighway != null)
            {
                AccidentOnHighway = accidentOnHighway.Clone();

                if (AccidentOnVillageGroup.Content is Grid)
                {
                    ((Grid)AccidentOnVillageGroup.Content).IsEnabled = false;
                }

                RoadIndexAndBorderTextBox.Text = AccidentOnHighway.HighwayIndexAndNumber;

                DataContext = AccidentOnHighway;

                AccidentOnVillage = new AccidentOnVillage();
            }
            else
            {
                AccidentOnVillage = new AccidentOnVillage();
                AccidentOnHighway = new AccidentOnHighway();

                if (AccidentOnHighwayGroup.Content is Grid)
                {
                    ((Grid)AccidentOnHighwayGroup.Content).IsEnabled = false;
                }

                DataContext = AccidentOnVillage;
            }

            AccidentOnHighway.ErrorInput += ShowErrorMessage;
            AccidentOnVillage.ErrorInput += ShowErrorMessage;

            isRoadIndexAndNumberValid = false;
        }

        private void AcceptClick(object sender, RoutedEventArgs e)
        {
            if (((Grid)AccidentOnHighwayGroup.Content).IsEnabled)
            {
                if (!isRoadIndexAndNumberValid || !int.TryParse(KilometerTextBox.Text, out int km) 
                    || !int.TryParse(MeterTextBox.Text, out int m) || string.IsNullOrEmpty(RoadBindingTextBox.Text))
                {
                    return;
                }

                AccidentOnVillage = null;
            } 
            else
            {
                if (string.IsNullOrEmpty(VillageNameTextBox.Text) || string.IsNullOrEmpty(VillageDistrictTextBox.Text)
                    || string.IsNullOrEmpty(VillageStreetTextBox.Text) || string.IsNullOrEmpty(VillageBindingTextBox.Text))
                {
                    return;
                }

                AccidentOnHighway = null;
            }

            DialogResult = true;
        }
        private void RejectClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void RoadIndexAndNumberTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox)
            {
                Regex regex = new Regex(@"\d{5}[0-9]?$");

                TextBox textBox = (TextBox)sender;

                string tempStr = textBox.Text;
                int caretIndex = textBox.CaretIndex;
                int oldLength = textBox.Text.Length;

                tempStr = tempStr.GetStrWithoutSeparator('-');

                if (regex.IsMatch(tempStr) || !textBox.IsEnabled)
                {
                    AccidentOnHighway.HighwayIndexAndNumber = tempStr;

                    if (RoadIndexAndNumberBorder.ToolTip != null)
                    {
                        textBox.Foreground = new SolidColorBrush(Colors.Black);
                    }

                    RoadIndexAndNumberBorder.BorderBrush = null;
                    RoadIndexAndNumberBorder.ToolTip = null;
                    isRoadIndexAndNumberValid = true;
                }
                else
                {
                    if (RoadIndexAndNumberBorder.BorderBrush == null)
                    {
                        RoadIndexAndNumberBorder.BorderBrush = new SolidColorBrush(Colors.Red);
                        RoadIndexAndNumberBorder.ToolTip = "Строка не соответствует шаблону:\n\t0-00-00-0*\n\n* - не обязательный элемент.";

                        textBox.Foreground = new SolidColorBrush(Colors.Red);

                        isRoadIndexAndNumberValid = false;
                    }

                    roadIndexAndNumber = tempStr;
                }

                tempStr = tempStr.AddSeparator('-', 1, 4, 7);

                textBox.Text = tempStr;
                textBox.CaretIndex = caretIndex;


                Dictionary<int, int> caretIndexes = new Dictionary<int, int>();
                caretIndexes.Add(2, 3);
                caretIndexes.Add(5, 6);
                caretIndexes.Add(8, 9);

                //set caret after string change
                if (!textBox.TrySetCaretOnIndexes(caretIndexes))
                {
                    if (tempStr.Length < oldLength && caretIndex - 1 >= 0)
                    {
                        textBox.CaretIndex = caretIndex - 1;
                    }
                }
            }
        }

        private void AccidentOnVillageGroup_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (AccidentOnVillageGroup.Content is Grid && AccidentOnHighwayGroup.Content is Grid)
            {
                ((Grid)AccidentOnVillageGroup.Content).IsEnabled = true;
                ((Grid)AccidentOnHighwayGroup.Content).IsEnabled = false;
                RoadIndexAndBorderTextBox.Text = "";

                RoadIndexAndNumberBorder.BorderBrush = null;

                DataContext = AccidentOnVillage;
            }
        }
        private void AccidentOnHighwayGroup_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (AccidentOnVillageGroup.Content is Grid && AccidentOnHighwayGroup.Content is Grid)
            {
                ((Grid)AccidentOnVillageGroup.Content).IsEnabled = false;
                ((Grid)AccidentOnHighwayGroup.Content).IsEnabled = true;

                RoadIndexAndBorderTextBox.Text = roadIndexAndNumber;

                DataContext = AccidentOnHighway;
            }
        }

        private void ShowErrorMessage(string msg)
        {
            MessageBox.Show(msg, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
