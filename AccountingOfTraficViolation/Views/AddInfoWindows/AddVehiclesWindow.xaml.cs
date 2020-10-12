using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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
    /// Логика взаимодействия для AddVehiclesWindow.xaml
    /// </summary>
    public partial class AddVehiclesWindow : Window
    {
        public AccidentObjectsVM<Vehicle> AccidentObjectsVM { get; private set; }

        public ObservableCollection<Vehicle> Vehicles => AccidentObjectsVM.AccidentObjects;

        public AddVehiclesWindow() : this(null)
        { }
        public AddVehiclesWindow(ObservableCollection<Vehicle> vehicles, bool isEditable = true)
        {
            InitializeComponent();

            AccidentObjectsVM = new AccidentObjectsVM<Vehicle>(vehicles);

            DataContext = AccidentObjectsVM;

            VehiclesListBox.ItemsSource = AccidentObjectsVM.AccidentObjects;

            if (isEditable)
            {
                VehicleGroupBox.Header = "Транспортное средство № 1";
            }
            else
            {
                VehicleGroupBox.Header = "Транспортное средство";
                AddVehicle.IsEnabled = false;
                RemoveVehicle.IsEnabled = false;
            }
        }

        private void VehiclesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (VehiclesListBox.SelectedIndex >= 0)
            {
                AccidentObjectsVM.CurrentIndex = VehiclesListBox.SelectedIndex;

                VehicleGroupBox.Header = "Транспортное средство № " + (AccidentObjectsVM.CurrentIndex + 1).ToString();
            }
        }

        private void AcceptClick(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < AccidentObjectsVM.AccidentObjects.Count; i++)
            {
                AccidentObjectsVM.CurrentIndex = i;
                if (VehicleGroupBox.CheckIfExistValidationError())
                {
                    VehiclesListBox.SelectedIndex = i;
                    return;
                }
            }

            DialogResult = true;
        }
        private void RejectClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void TextBox_Click(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox)
            {
                TextBox textBox = (TextBox)sender;

                CodesWindow codesWindow = new CodesWindow();

                if (codesWindow.ShowDialog() == true)
                {
                    textBox.Text = codesWindow.Code;
                }
            }
        }
    }
}
