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
using AccountingOfTraficViolation.Views.AddInfoWindows;
using AccountingOfTraficViolation.Models;

namespace AccountingOfTraficViolation.Views
{
    /// <summary>
    /// Логика взаимодействия для OpenNewCaseWindow.xaml
    /// </summary>
    public partial class OpenNewCaseWindow : Window
    {
        private GeneralInfo generalInfo;
        private AccidentOnHighway accidentOnHighway;
        private AccidentOnVillage accidentOnVillage;
        private RoadCondition roadCondition;
        private ObservableCollection<ParticipantsInformation> participantsInformations;
        private ObservableCollection<Vehicle> vehicles;

        public OpenNewCaseWindow()
        {
            InitializeComponent();
        }

        private void GeneralInfoClick(object sender, RoutedEventArgs e)
        {
            AddGeneralInfoWindow generalInfoAddWindow = new AddGeneralInfoWindow(generalInfo);

            if (generalInfoAddWindow.ShowDialog() == true)
            {
                generalInfo = generalInfoAddWindow.GeneralInfo;
                GeneralInfoProgresImage.Source = new BitmapImage(new Uri(System.IO.Path.GetFullPath("./Images/AcceptIcon.jpg")));
            }
        }
        private void AccidentPlaceClick(object sender, RoutedEventArgs e)
        {
            AddAccidentPlaceWindow accidentPlaceWinow = new AddAccidentPlaceWindow(accidentOnHighway, accidentOnVillage);
            if (accidentPlaceWinow.ShowDialog() == true)
            {
                AccidentPlaceProgresImage.Source = new BitmapImage(new Uri(System.IO.Path.GetFullPath("./Images/AcceptIcon.jpg")));
                accidentOnHighway = accidentPlaceWinow.AccidentOnHighway;
                accidentOnVillage = accidentPlaceWinow.AccidentOnVillage;
            }
        }
        private void RoadConditionClick(object sender, RoutedEventArgs e)
        {
            AddRoadConditionWindow roadConditionWindow = new AddRoadConditionWindow(roadCondition);
            if (roadConditionWindow.ShowDialog() == true)
            {
                roadCondition = roadConditionWindow.RoadCondition;
                RoadConditionProgresImage.Source = new BitmapImage(new Uri(System.IO.Path.GetFullPath("./Images/AcceptIcon.jpg")));
            }
        }
        private void ParticipanInfoClick(object sender, RoutedEventArgs e)
        {
            AddParticipantInfoWindow participantInfoWindow = new AddParticipantInfoWindow(participantsInformations);
            if (participantInfoWindow.ShowDialog() == true)
            {
                participantsInformations = participantInfoWindow.ParticipantsInformations;
                ParticipanInfoProgresImage.Source = new BitmapImage(new Uri(System.IO.Path.GetFullPath("./Images/AcceptIcon.jpg")));
            }
        }
        private void VehicleClick(object sender, RoutedEventArgs e)
        {
            AddVehiclesWindow vehiclesWindow = new AddVehiclesWindow(vehicles);
            if (vehiclesWindow.ShowDialog() == true)
            {
                vehicles = vehiclesWindow.Vehicles;
                VehicleProgresImage.Source = new BitmapImage(new Uri(System.IO.Path.GetFullPath("./Images/AcceptIcon.jpg")));
            }
        }
        private void VictimClick(object sender, RoutedEventArgs e)
        {
            VictimProgresImage.Source = new BitmapImage(new Uri(System.IO.Path.GetFullPath("./Images/AcceptIcon.jpg")));
        }
    }
}
