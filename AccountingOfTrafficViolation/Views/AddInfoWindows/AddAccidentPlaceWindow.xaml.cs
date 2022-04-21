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
using AccountingOfTrafficViolation.Models;
using AccountingOfTrafficViolation.Services;
using AccountingOfTrafficViolation.ViewModels;
using AccountOfTrafficViolationDB.Models;

namespace AccountingOfTrafficViolation.Views.AddInfoWindows
{
    /// <summary>
    /// Логика взаимодействия для AddAccidentPlaceWindow.xaml
    /// </summary>
    public partial class AddAccidentPlaceWindow : Window
    {
        private AccidentOnHighway AccidentOnHighway;
        private AccidentOnVillage AccidentOnVillage;
        private bool isEditable;

        public CaseAccidentPlace CaseAccidentPlace { get; private set; }

        public AddAccidentPlaceWindow() : this(null)
        { }
        public AddAccidentPlaceWindow(CaseAccidentPlace caseAccidentPlace, bool isEditable = true)
        {
            InitializeComponent();

            if (caseAccidentPlace == null)
            {
                CaseAccidentPlace = new CaseAccidentPlace();
            }
            else
            {
                CaseAccidentPlace = caseAccidentPlace.Clone();
            }


            AccidentOnHighway = CaseAccidentPlace.AccidentOnHighway;
            AccidentOnVillage = CaseAccidentPlace.AccidentOnVillage;

            this.isEditable = isEditable;

            if (CaseAccidentPlace.AccidentOnHighway != null)
            {
                AccidentOnVillage = new AccidentOnVillage();

                ((Grid)AccidentOnVillageGroup.Content).IsEnabled = false;
                if (isEditable)
                {
                    AccidentOnVillageGroup.IsEnabled = false;
                }

                DataContext = AccidentOnHighway;
            }
            else if (CaseAccidentPlace.AccidentOnVillage != null)
            {
                AccidentOnHighway = new AccidentOnHighway();

                ((Grid)AccidentOnHighwayGroup.Content).IsEnabled = false;

                if (isEditable)
                {
                    AccidentOnHighwayGroup.IsEnabled = false;
                }

                DataContext = AccidentOnVillage;
            }
            else
            {
                AccidentOnVillage = new AccidentOnVillage();
                AccidentOnHighway = new AccidentOnHighway();

                ((Grid)AccidentOnHighwayGroup.Content).IsEnabled = false;

                DataContext = AccidentOnVillage;
            }

        }

        private void AcceptClick(object sender, RoutedEventArgs e)
        {
            if (((Grid)AccidentOnHighwayGroup.Content).IsEnabled)
            {
                if (AccidentOnHighwayGroup.CheckIfExistValidationError())
                {
                    return;
                }

                AccidentOnVillage = null;
            }
            else
            {
                if (AccidentOnVillageGroup.CheckIfExistValidationError())
                {
                    return;
                }

                AccidentOnHighway = null;
            }

            CaseAccidentPlace.AccidentOnVillage = AccidentOnVillage;
            CaseAccidentPlace.AccidentOnHighway = AccidentOnHighway;

            DialogResult = true;
        }
        private void RejectClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void AccidentOnVillageGroup_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (AccidentOnVillageGroup.Content is Grid && AccidentOnHighwayGroup.Content is Grid && isEditable)
            {
                ((Grid)AccidentOnVillageGroup.Content).IsEnabled = true;
                ((Grid)AccidentOnHighwayGroup.Content).IsEnabled = false;

                DataContext = AccidentOnVillage;
            }
        }
        private void AccidentOnHighwayGroup_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (AccidentOnVillageGroup.Content is Grid && AccidentOnHighwayGroup.Content is Grid && isEditable)
            {
                ((Grid)AccidentOnVillageGroup.Content).IsEnabled = false;
                ((Grid)AccidentOnHighwayGroup.Content).IsEnabled = true;

                DataContext = AccidentOnHighway;
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox)
            {
                TextBox textBox = (TextBox)sender;
                textBox.Text = textBox.Text.RemoveZeroBeforeText();
            }
        }
        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox)
            {
                TextBox textBox = (TextBox)sender;
                textBox.Text = textBox.Text.AddZeroBeforeText(textBox.MaxLength - textBox.Text.Length);
            }
        }

        private void OpenCodesWindowClick(object sender, RoutedEventArgs e)
        {
            CodesWindow codesWindow = new CodesWindow();
            
            if (codesWindow.ShowDialog() == true)
            {
                RoadIndexAndNumberTextBox.Text = codesWindow.Code;
            }
        }
    }
}
