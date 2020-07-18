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
using AccountingOfTraficViolation.ViewModels;

namespace AccountingOfTraficViolation.Views.AddInfoWindows
{
    /// <summary>
    /// Логика взаимодействия для AddAccidentPlaceWindow.xaml
    /// </summary>
    public partial class AddAccidentPlaceWindow : Window
    {
        private AccidentOnHighwayVM AccidentOnHighwayVM;
        private AccidentOnVillageVM AccidentOnVillageVM;

        public AccidentOnHighway AccidentOnHighway { get; private set; }
        public AccidentOnVillage AccidentOnVillage { get; private set; }

        public AddAccidentPlaceWindow() : this(null, null)
        { }
        public AddAccidentPlaceWindow(AccidentOnHighway accidentOnHighway, AccidentOnVillage accidentOnVillage)
        {
            InitializeComponent();

            AccidentOnVillageVM = new AccidentOnVillageVM(accidentOnVillage);
            AccidentOnHighwayVM = new AccidentOnHighwayVM(accidentOnHighway);

            if (accidentOnVillage != null)
            {
                AccidentOnVillage = accidentOnVillage.Clone();

                if (AccidentOnHighwayGroup.Content is Grid)
                {
                    ((Grid)AccidentOnHighwayGroup.Content).IsEnabled = false;
                }

                AccidentOnHighway = new AccidentOnHighway();

                DataContext = AccidentOnVillageVM;
            }
            else if (accidentOnHighway != null)
            {
                AccidentOnHighway = accidentOnHighway.Clone();

                if (AccidentOnVillageGroup.Content is Grid)
                {
                    ((Grid)AccidentOnVillageGroup.Content).IsEnabled = false;
                }

                AccidentOnVillage = new AccidentOnVillage();

                DataContext = AccidentOnHighwayVM;
            }
            else
            {
                AccidentOnVillage = new AccidentOnVillage();
                AccidentOnHighway = new AccidentOnHighway();

                if (AccidentOnHighwayGroup.Content is Grid)
                {
                    ((Grid)AccidentOnHighwayGroup.Content).IsEnabled = false;
                }

                DataContext = AccidentOnVillageVM;
            }

            AccidentOnHighway.ErrorInput += ShowErrorMessage;
            AccidentOnVillage.ErrorInput += ShowErrorMessage;
        }

        private void AcceptClick(object sender, RoutedEventArgs e)
        {
            if (((Grid)AccidentOnHighwayGroup.Content).IsEnabled)
            {
                if (AccidentOnHighwayGroup.CheckIfExistValidationError())
                {
                    return;
                }

                AccidentOnHighway = AccidentOnHighwayVM.AccidentOnHighway;

                AccidentOnVillage = null;
            }
            else
            {
                if (AccidentOnVillageGroup.CheckIfExistValidationError())
                {
                    return;
                }

                AccidentOnVillage = AccidentOnVillageVM.AccidentOnVillage;

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
                TextBox textBox = (TextBox)sender;

                string tempStr = textBox.Text;
                int caretIndex = textBox.CaretIndex;
                int oldLength = textBox.Text.Length;

                tempStr = textBox.Text.GetStrWithoutSeparator('-').AddSeparator('-', 1, 4, 7);

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

                DataContext = AccidentOnVillageVM;
            }
        }
        private void AccidentOnHighwayGroup_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (AccidentOnVillageGroup.Content is Grid && AccidentOnHighwayGroup.Content is Grid)
            {
                ((Grid)AccidentOnVillageGroup.Content).IsEnabled = false;
                ((Grid)AccidentOnHighwayGroup.Content).IsEnabled = true;

                DataContext = AccidentOnHighwayVM;
            }
        }

        private void ShowErrorMessage(string msg)
        {
            MessageBox.Show(msg, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
