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
                isRoadIndexAndNumberValid = false;
            }
            else if (accidentOnHighway != null)
            {
                AccidentOnHighway = accidentOnHighway.Clone();

                if (AccidentOnVillageGroup.Content is Grid)
                {
                    ((Grid)AccidentOnVillageGroup.Content).IsEnabled = false;
                }

                RoadIndexAndNumberTextBox.Text = AccidentOnHighway.HighwayIndexAndNumber;

                DataContext = AccidentOnHighway;

                isRoadIndexAndNumberValid = true;

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
                isRoadIndexAndNumberValid = false;
            }

            AccidentOnHighway.ErrorInput += ShowErrorMessage;
            AccidentOnVillage.ErrorInput += ShowErrorMessage;


        }

        private void AcceptClick(object sender, RoutedEventArgs e)
        {
            bool isValid = true;

            if (((Grid)AccidentOnHighwayGroup.Content).IsEnabled)
            {
                if (!isRoadIndexAndNumberValid)
                {
                    RoadIndexAndNumberBorder.BorderBrush = new SolidColorBrush(Colors.Red);
                    RoadIndexAndNumberBorder.ToolTip = "Строка не соответствует шаблону:\n\t0-00-00-0*\n\n* - не обязательный элемент.";
                    isValid = false;
                }

                if (!int.TryParse(KilometerTextBox.Text, out int km))
                {
                    if (string.IsNullOrEmpty(KilometerTextBox.Text))
                    {
                        KilometerBorder.ToolTip = "Поле 'км' не может быть пустым.";
                    }
                    else
                    {
                        KilometerBorder.ToolTip = $"Не удалось преобразовать значение '{KilometerTextBox.Text}'.";
                    }
                    KilometerBorder.BorderBrush = new SolidColorBrush(Colors.Red);
                    isValid = false;
                }
                else
                {
                    KilometerBorder.BorderBrush = null;
                    KilometerBorder.ToolTip = null;
                }

                if (!int.TryParse(MeterTextBox.Text, out int m))
                {
                    if (string.IsNullOrEmpty(MeterTextBox.Text))
                    {
                        MeterBorder.ToolTip = "Поле 'м' не может быть пустым.";
                    }
                    else
                    {
                        MeterBorder.ToolTip = $"Не удалось преобразовать значение '{MeterTextBox.Text}'.";
                    }
                    MeterBorder.BorderBrush = new SolidColorBrush(Colors.Red);
                    isValid = false;
                }
                else
                {
                    MeterBorder.ToolTip = null;
                    MeterBorder.BorderBrush = null;
                }

                if (string.IsNullOrEmpty(RoadBindingTextBox.Text))
                {
                    RoadBindingBorder.ToolTip = "Поле 'Привязка' не может быть пустым.";
                    RoadBindingBorder.BorderBrush = new SolidColorBrush(Colors.Red);
                    isValid = false;
                }
                else
                {
                    RoadBindingBorder.ToolTip = null;
                    RoadBindingBorder.BorderBrush = null;
                }

                if (isValid) {
                    AccidentOnVillage = null;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(VillageNameTextBox.Text))
                {
                    VillageNameBorder.ToolTip = "Поле 'Название' не может быть пустым.";
                    VillageNameBorder.BorderBrush = new SolidColorBrush(Colors.Red);
                    isValid = false;
                }
                else
                {
                    VillageNameBorder.BorderBrush = null;
                    VillageNameBorder.ToolTip = null;
                }

                if (string.IsNullOrEmpty(VillageDistrictTextBox.Text))
                {
                    VillageDistrictBorder.ToolTip = "Поле 'Район' не может быть пустым.";
                    VillageDistrictBorder.BorderBrush = new SolidColorBrush(Colors.Red);
                    isValid = false;
                }
                else
                {
                    VillageDistrictBorder.BorderBrush = null;
                    VillageDistrictBorder.ToolTip = null;
                }

                if (string.IsNullOrEmpty(VillageStreetTextBox.Text))
                {
                    VillageStreetBorder.ToolTip = "Поле 'Улица' не может быть пустым.";
                    VillageStreetBorder.BorderBrush = new SolidColorBrush(Colors.Red);
                    isValid = false;
                }
                else
                {
                    VillageStreetBorder.BorderBrush = null;
                    VillageStreetBorder.ToolTip = null;
                }

                if (string.IsNullOrEmpty(VillageBindingTextBox.Text))
                {
                    VillageBindingBorder.ToolTip = "Поле 'Привязка' не может быть пустым.";
                    VillageBindingBorder.BorderBrush = new SolidColorBrush(Colors.Red);
                    isValid = false;
                }
                else
                {
                    VillageBindingBorder.BorderBrush = null;
                    VillageBindingBorder.ToolTip = null;
                }

                if (Validation.GetHasError(NameRegionalCodeTextBox) || Validation.GetHasError(DistrictRegionalCodeTextBox)
                    || Validation.GetHasError(StreetRegionalCodeTextBox) || Validation.GetHasError(BindingRegionalCodeTextBox))
                {
                    return;
                }

                if (isValid)
                {
                    AccidentOnHighway = null;
                }
            }

            if (isValid)
            {
                DialogResult = true;
            }

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

                if (regex.IsMatch(tempStr))
                {
                    AccidentOnHighway.HighwayIndexAndNumber = tempStr;
                    
                    textBox.Foreground = new SolidColorBrush(Colors.Black);

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
                }

                if (!string.IsNullOrEmpty(tempStr))
                {
                    roadIndexAndNumber = tempStr;
                }

                tempStr = tempStr.AddSeparator('-', 1, 4, 7);

                textBox.Text = tempStr;

                textBox.CaretIndex = caretIndex;

                //set caret after string change
                if (caretIndex + (tempStr.Length - oldLength) >= 0)
                {
                    textBox.CaretIndex = caretIndex + (tempStr.Length - oldLength);
                }
            }
        }

        private void AccidentOnVillageGroup_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (AccidentOnVillageGroup.Content is Grid && AccidentOnHighwayGroup.Content is Grid)
            {
                ((Grid)AccidentOnVillageGroup.Content).IsEnabled = true;
                ((Grid)AccidentOnHighwayGroup.Content).IsEnabled = false;
                RoadIndexAndNumberTextBox.Text = "";

                RoadIndexAndNumberBorder.BorderBrush = null;
                DisableAllBorder(AccidentOnHighwayGroup);

                DataContext = AccidentOnVillage;
            }
        }
        private void AccidentOnHighwayGroup_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (AccidentOnVillageGroup.Content is Grid && AccidentOnHighwayGroup.Content is Grid)
            {
                ((Grid)AccidentOnVillageGroup.Content).IsEnabled = false;
                ((Grid)AccidentOnHighwayGroup.Content).IsEnabled = true;

                if (roadIndexAndNumber != null)
                {
                    RoadIndexAndNumberTextBox.Text = roadIndexAndNumber;
                }

                DisableAllBorder(AccidentOnVillageGroup);

                DataContext = AccidentOnHighway;
            }
        }

        private void ShowErrorMessage(string msg)
        {
            MessageBox.Show(msg, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void DisableAllBorder(UIElement elem)
        {
            Panel panel = null;

            if (elem is ContentControl)
            {
                if (((ContentControl)elem).Content is Panel)
                {
                    panel = (Panel)((ContentControl)elem).Content;
                }
                else
                {
                    throw new InvalidCastException("Elem.Content is not Panel.");
                }
            }
            else if (elem is Panel)
            {
                panel = (Panel)elem;
            }
            else
            {
                throw new InvalidCastException("Elem.Content is not Panel or ContentControl.");
            }

            foreach (var child in panel.Children)
            {
                if (child is Border)
                {
                    ((Border)child).BorderBrush = null;
                    ((Border)child).ToolTip = null;
                }

                if (child is Panel)
                {
                    DisableAllBorder((Panel)child);
                }
            }
        }
    }
}
