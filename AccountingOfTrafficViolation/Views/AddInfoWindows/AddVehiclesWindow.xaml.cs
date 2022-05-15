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
using AccountingOfTrafficViolation.Models;
using AccountingOfTrafficViolation.ViewModels;
using AccountingOfTrafficViolation.Services;
using AccountOfTrafficViolationDB.Models;

namespace AccountingOfTrafficViolation.Views.AddInfoWindows
{
    /// <summary>
    /// Логика взаимодействия для AddVehiclesWindow.xaml
    /// </summary>
    public partial class AddVehiclesWindow : Window
    {
        private readonly SolidColorBrush m_redColor;
        private readonly SolidColorBrush m_defaultTBColor;

        private readonly List<Vehicle> m_bannedVehicles;
        
        public AccidentObjectsVM<CaseVehicle> AccidentObjectsVM { get; private set; }

        public ObservableCollection<CaseVehicle> Vehicles => AccidentObjectsVM.AccidentObjects;

        public AddVehiclesWindow() : this(null)
        {
        }

        public AddVehiclesWindow(ObservableCollection<CaseVehicle> vehicles, bool isEditable = true)
        {
            InitializeComponent();

            AccidentObjectsVM = new AccidentObjectsVM<CaseVehicle>(vehicles);

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

            m_redColor = new SolidColorBrush(Colors.Red);
            m_defaultTBColor = new SolidColorBrush(Color.FromArgb(0xFF, 0xAB, 0xAD, 0xB3));

            VehicleTB.BorderBrush = m_redColor;
        }

        private void VehiclesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (VehiclesListBox.SelectedIndex >= 0)
            {
                AccidentObjectsVM.CurrentIndex = VehiclesListBox.SelectedIndex;

                VehicleGroupBox.Header = $"Транспортное средство № {AccidentObjectsVM.CurrentIndex + 1}";

                if (AccidentObjectsVM.CurrentAccidentObject.Vehicle != null)
                {
                    VehicleTB.Text =
                        $"{AccidentObjectsVM.CurrentAccidentObject.Vehicle.Make} ({AccidentObjectsVM.CurrentAccidentObject.Vehicle.Model})";

                    VehicleTB.BorderBrush = m_defaultTBColor;
                }
                else
                {
                    VehicleTB.Text = string.Empty;
                    VehicleTB.BorderBrush = m_redColor;
                }
            }
        }

        private void AcceptClick(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < AccidentObjectsVM.AccidentObjects.Count; i++)
            {
                AccidentObjectsVM.CurrentIndex = i;

                if (AccidentObjectsVM.CurrentAccidentObject.Vehicle == null)
                {
                    VehiclesListBox.SelectedIndex = i;
                    VehicleTB.BorderBrush = m_redColor;

                    return;
                }

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
            if (sender is TextBox textBox)
            {
                var codesWindow = new CodesWindow( "VehicleInformation");

                if (codesWindow.ShowDialog() == true)
                    textBox.Text = codesWindow.Code;
            }
        }

        private void VehicleTBoxBtnClick(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                VehicleInformation? vInfo = null;

                try
                {
                    vInfo = new VehicleInformation();
                    
                    foreach (var accidentObject in AccidentObjectsVM.AccidentObjects)
                        if (accidentObject.Vehicle != null)
                            vInfo.BannedVehicles.Add(accidentObject.Vehicle.Id);

                    if (vInfo.ShowDialog() == true)
                    {
                        AccidentObjectsVM.CurrentAccidentObject.Vehicle = vInfo.SelectedVehicle;
                        AccidentObjectsVM.CurrentAccidentObject.VehicleId = vInfo.SelectedVehicle.Id;

                        textBox.Text = $"{vInfo.SelectedVehicle.Make} ({vInfo.SelectedVehicle.Model})";

                        textBox.BorderBrush = m_defaultTBColor;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    vInfo?.Dispose();
                }
            }
        }
    }
}