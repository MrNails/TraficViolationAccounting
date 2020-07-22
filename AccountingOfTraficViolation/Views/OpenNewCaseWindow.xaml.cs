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
        private User user;
        private GeneralInfo generalInfo;
        private AccidentOnHighway accidentOnHighway;
        private AccidentOnVillage accidentOnVillage;
        private RoadCondition roadCondition;
        private ObservableCollection<ParticipantsInformation> participantsInformations;
        private ObservableCollection<Vehicle> vehicles;
        private ObservableCollection<Victim> victims;
        private Case _case;
        private CaseAccidentPlace caseAccidentPlace;

        public OpenNewCaseWindow(User user)
        {
            InitializeComponent();

            if (user != null)
            {
                this.user = user;
            }
            else
            {
                MessageBox.Show("Вы не вошли в аккаунт и не можете открывать дело.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }

            if (this.user.Role != 1)
            {
                MessageBox.Show("У вас не хватает привелегий на создание дела.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
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
            AddVictimsWindow victimsWindow = new AddVictimsWindow(victims);
            if (victimsWindow.ShowDialog() == true)
            {
                victims = victimsWindow.Victims;
                VictimProgresImage.Source = new BitmapImage(new Uri(System.IO.Path.GetFullPath("./Images/AcceptIcon.jpg")));
            }
        }

        private void AcceptClick(object sender, RoutedEventArgs e)
        {
            if (generalInfo == null || (accidentOnHighway == null && accidentOnVillage == null) ||
                roadCondition == null || participantsInformations == null ||
                vehicles == null || victims == null)
            {
                return;
            }

            AddCaseToDB();

            DialogResult = true;
        }
        private void RejectClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private async void AddCaseToDB()
        {
            using (TVAContext context = new TVAContext())
            {
                _case = new Case();
                caseAccidentPlace = new CaseAccidentPlace();

                context.GeneralInfos.Add(generalInfo);
                context.RoadConditions.Add(roadCondition);

                _case.GeneralInfo = generalInfo;
                _case.RoadCondition = roadCondition;
                _case.State = "PROCESSING";
                _case.CaseAccidentPlace = caseAccidentPlace;
                _case.CreaterLogin = user.Login;

                caseAccidentPlace.AccidentOnHighway = accidentOnHighway;
                caseAccidentPlace.AccidentOnVillage = accidentOnVillage;
                caseAccidentPlace.Case = _case;

                foreach (var participantsInformation in participantsInformations)
                {
                    participantsInformation.Case = _case;
                }
                foreach (var vehicle in vehicles)
                {
                    vehicle.Case = _case;
                }
                foreach (var victim in victims)
                {
                    victim.Case = _case;
                }

                context.ParticipantsInformations.AddRange(participantsInformations);
                context.Vehicles.AddRange(vehicles);
                context.Victims.AddRange(victims);

                if (accidentOnHighway != null)
                {
                    context.AccidentOnHighways.Add(accidentOnHighway);
                }
                if (accidentOnVillage != null)
                {
                    context.AccidentOnVillages.Add(accidentOnVillage);
                }

                await context.SaveChangesAsync();
            }
            
        }
    }
}
