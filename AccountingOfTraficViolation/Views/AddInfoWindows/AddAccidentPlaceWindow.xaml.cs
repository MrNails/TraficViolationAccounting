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
        public AccidentOnHighway AccidentOnHighway { get; private set; }
        public AccidentOnVillage AccidentOnVillage { get; private set; }

        public AddAccidentPlaceWindow() : this(null, null)
        { }
        public AddAccidentPlaceWindow(AccidentOnHighway accidentOnHighway, AccidentOnVillage accidentOnVillage)
        {
            InitializeComponent();

            if (accidentOnHighway != null)
            {
                AccidentOnHighway = accidentOnHighway.Clone();
                AccidentOnVillage = new AccidentOnVillage();

                ((Grid)AccidentOnVillageGroup.Content).IsEnabled = false;

                DataContext = AccidentOnHighway;
            }
            else if (accidentOnVillage != null)
            {
                AccidentOnVillage = accidentOnVillage.Clone();
                AccidentOnHighway = new AccidentOnHighway();

                ((Grid)AccidentOnHighwayGroup.Content).IsEnabled = false;

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

                textBox.SeparatorTemplate('-', 1, 4, 7);
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
