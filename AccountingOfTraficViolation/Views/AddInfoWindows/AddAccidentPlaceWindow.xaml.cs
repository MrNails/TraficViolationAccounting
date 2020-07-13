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

        public AccidentOnHighway AccidentOnHighway { get; set; }
        public AccidentOnVillage AccidentOnVillage { get; set; }

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


                //delete all '-'
                for (int i = 0; i < tempStr.Length; i++)
                {
                    if (tempStr[i] == '-')
                    {
                        tempStr = tempStr.Remove(i, 1);
                        i--;
                    }
                }

                if (regex.IsMatch(tempStr))
                {
                    AccidentOnHighway.HighwayIndexAndNumber = tempStr;

                    if (RoadIndexAndNumberBorder.ToolTip != null)
                    {
                        ((ToolTip)RoadIndexAndNumberBorder.ToolTip).IsOpen = false;
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
                        RoadIndexAndNumberBorder.ToolTip = new ToolTip
                        {
                            Content = "Строка не соответствует шаблону:\n\t0-00-00-0*\n\n* - не обязательный элемент",
                            FontSize = 13,
                            PlacementTarget = RoadIndexAndNumberBorder,
                            Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom
                        };

                        ((ToolTip)RoadIndexAndNumberBorder.ToolTip).IsOpen = true;

                        textBox.Foreground = new SolidColorBrush(Colors.Red);

                        isRoadIndexAndNumberValid = false;
                    }
                }


                //check string length to input '-' on index=2 and, if last symbol exist, on index=10 
                if (tempStr.Length > 1)
                {
                    tempStr = tempStr.Insert(1, "-");
                }
                if (tempStr.Length > 4)
                {
                    tempStr = tempStr.Insert(4, "-");
                }
                if (tempStr.Length > 7)
                {
                    tempStr = tempStr.Insert(7, "-");
                }

                textBox.Text = tempStr;

                //set caret after string change
                if (caretIndex == 2)
                {
                    textBox.CaretIndex = 3;
                }
                else if (caretIndex == 5)
                {
                    textBox.CaretIndex = 6;
                }
                else if (caretIndex == 8)
                {
                    textBox.CaretIndex = 9;
                }
                else if (tempStr.Length < oldLength)
                {
                    textBox.CaretIndex = caretIndex - 1;
                }
                else
                {
                    textBox.CaretIndex = caretIndex;
                }
            }
        }

        private void AccidentOnVillageGroup_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (AccidentOnVillageGroup.Content is Grid && AccidentOnHighwayGroup.Content is Grid)
            {
                ((Grid)AccidentOnVillageGroup.Content).IsEnabled = true;
                ((Grid)AccidentOnHighwayGroup.Content).IsEnabled = false;

                DataContext = AccidentOnVillage;
            }
        }
        private void AccidentOnHighwayGroup_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (AccidentOnVillageGroup.Content is Grid && AccidentOnHighwayGroup.Content is Grid)
            {
                ((Grid)AccidentOnVillageGroup.Content).IsEnabled = false;
                ((Grid)AccidentOnHighwayGroup.Content).IsEnabled = true;

                DataContext = AccidentOnHighway;
            }
        }
    }
}
