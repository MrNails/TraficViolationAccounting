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
using System.Windows.Media.Animation;
using System.Data.Entity.Validation;
using System.Runtime.ExceptionServices;

namespace AccountingOfTraficViolation.Views
{
    /// <summary>
    /// Логика взаимодействия для OpenNewCaseWindow.xaml
    /// </summary>
    public partial class OpenNewCaseWindow : Window
    {
        private User user;
        private GeneralInfo generalInfo;
        private CaseAccidentPlace caseAccidentPlace;
        private RoadCondition roadCondition;
        private ObservableCollection<ParticipantsInformation> participantsInformations;
        private ObservableCollection<Vehicle> vehicles;
        private ObservableCollection<Victim> victims;
        private Case _case;

        public OpenNewCaseWindow(User user)
        {
            InitializeComponent();

            this.user = user;
            _case = new Case();

            DataContext = _case;
        }

        private void GeneralInfoClick(object sender, RoutedEventArgs e)
        {
            AddGeneralInfoWindow generalInfoAddWindow = new AddGeneralInfoWindow(generalInfo);

            if (generalInfoAddWindow.ShowDialog() == true)
            {
                generalInfo = generalInfoAddWindow.GeneralInfo;
                GeneralInfoProgresImage.Source = new BitmapImage(new Uri("/Images/AcceptIcon.jpg", UriKind.Relative));
                StopBorderAnimation(GeneralInfoBorder);
            }
        }
        private void AccidentPlaceClick(object sender, RoutedEventArgs e)
        {
            AddAccidentPlaceWindow accidentPlaceWinow = new AddAccidentPlaceWindow(caseAccidentPlace);
            if (accidentPlaceWinow.ShowDialog() == true)
            {
                AccidentPlaceProgresImage.Source = new BitmapImage(new Uri("/Images/AcceptIcon.jpg", UriKind.Relative));
                caseAccidentPlace = accidentPlaceWinow.CaseAccidentPlace;
                StopBorderAnimation(AccidentPlaceBorder);
            }
        }
        private void RoadConditionClick(object sender, RoutedEventArgs e)
        {
            AddRoadConditionWindow roadConditionWindow = new AddRoadConditionWindow(roadCondition);
            if (roadConditionWindow.ShowDialog() == true)
            {
                roadCondition = roadConditionWindow.RoadCondition;
                RoadConditionProgresImage.Source = new BitmapImage(new Uri("/Images/AcceptIcon.jpg", UriKind.Relative));
                StopBorderAnimation(RoadConditionBorder);
            }
        }
        private void ParticipanInfoClick(object sender, RoutedEventArgs e)
        {
            AddParticipantInfoWindow participantInfoWindow = new AddParticipantInfoWindow(participantsInformations);
            if (participantInfoWindow.ShowDialog() == true)
            {
                participantsInformations = participantInfoWindow.ParticipantsInformations;
                ParticipanInfoProgresImage.Source = new BitmapImage(new Uri("/Images/AcceptIcon.jpg", UriKind.Relative));
                StopBorderAnimation(ParticipanInfoBorder);
            }
        }
        private void VehicleClick(object sender, RoutedEventArgs e)
        {
            AddVehiclesWindow vehiclesWindow = new AddVehiclesWindow(vehicles);
            if (vehiclesWindow.ShowDialog() == true)
            {
                vehicles = vehiclesWindow.Vehicles;
                VehicleProgresImage.Source = new BitmapImage(new Uri("/Images/AcceptIcon.jpg", UriKind.Relative));
                StopBorderAnimation(VehicleBorder);
            }
        }
        private void VictimClick(object sender, RoutedEventArgs e)
        {
            AddVictimsWindow victimsWindow = new AddVictimsWindow(victims);
            if (victimsWindow.ShowDialog() == true)
            {
                victims = victimsWindow.Victims;
                VictimProgresImage.Source = new BitmapImage(new Uri("/Images/AcceptIcon.jpg", UriKind.Relative));
                StopBorderAnimation(VictimBorder);
            }
        }

        private void AcceptClick(object sender, RoutedEventArgs e)
        {
            bool isValid = true;

            if (generalInfo == null)
            {
                SetBorderAnimation(GeneralInfoBorder);
                isValid = false;
            }
            else
            {
                StopBorderAnimation(GeneralInfoBorder);
            }

            if (caseAccidentPlace == null)
            {
                SetBorderAnimation(AccidentPlaceBorder);
                isValid = false;
            }
            else
            {
                StopBorderAnimation(AccidentPlaceBorder);
            }

            if (roadCondition == null)
            {
                SetBorderAnimation(RoadConditionBorder);
                isValid = false;
            }
            else
            {
                StopBorderAnimation(RoadConditionBorder);
            }

            if (participantsInformations == null)
            {
                SetBorderAnimation(ParticipanInfoBorder);
                isValid = false;
            }
            else
            {
                StopBorderAnimation(ParticipanInfoBorder);
            }

            if (vehicles == null)
            {
                SetBorderAnimation(VehicleBorder);
                isValid = false;
            }
            else
            {
                StopBorderAnimation(VehicleBorder);
            }

            if (victims == null)
            {
                SetBorderAnimation(VictimBorder);
                isValid = false;
            }
            else
            {
                StopBorderAnimation(VictimBorder);
            }

            if (isValid)
            {
                try
                {
                    AddCaseToDB();
                    MessageBox.Show("Дело успешно добавлено.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    string entitiesInfo = "";
                    entitiesInfo += generalInfo?.ToDebugString();
                    entitiesInfo += caseAccidentPlace?.AccidentOnHighway.ToDebugString();
                    entitiesInfo += caseAccidentPlace?.AccidentOnVillage.ToDebugString();
                    entitiesInfo += roadCondition?.ToDebugString();

                    if (participantsInformations != null)
                    {
                        foreach (var participantsInformation in participantsInformations)
                        {
                            entitiesInfo += participantsInformation.ToDebugString();
                        }
                    }
                    if (vehicles != null)
                    {
                        foreach (var vehicle in vehicles)
                        {
                            entitiesInfo += vehicle.ToDebugString();
                        }
                    }
                    if (victims != null)
                    {
                        foreach (var victim in victims)
                        {
                            entitiesInfo += victim.ToDebugString();
                        }
                    }

                    throw new Exception($"{ex.Message}{Environment.NewLine}Информация о добавленных объектах:" +
                                       $"{Environment.NewLine}{entitiesInfo}");
                }
                finally
                {
                    DialogResult = true;
                }
            }
        }
        private void RejectClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void SetBorderAnimation(Border border)
        {
            if (border.HasAnimatedProperties)
            {
                return;
            }

            LinearGradientBrush borderBrush = new LinearGradientBrush(Colors.Blue, Colors.Red, 0);
            borderBrush.StartPoint = new Point(0.5, 1);
            borderBrush.EndPoint = new Point(0.5, 0);
            borderBrush.RelativeTransform = new RotateTransform(0, 0.5, 0.5);

            border.BorderBrush = borderBrush;

            DoubleAnimation widthAnimation = new DoubleAnimation()
            {
                From = border.Width + 20,
                To = border.Width + 4,
                Duration = TimeSpan.FromSeconds(1.5)
            };
            widthAnimation.Completed += (obj, arg) =>
            {
                DoubleAnimation animationBorderBrush = new DoubleAnimation()
                {
                    From = 0,
                    To = 360,
                    Duration = TimeSpan.FromSeconds(1.5),
                    AutoReverse = true,
                    RepeatBehavior = new RepeatBehavior(new TimeSpan(0, 0, 15))
                };

                border.BorderBrush.RelativeTransform.BeginAnimation(RotateTransform.AngleProperty, animationBorderBrush);
            };

            DoubleAnimation heightAnimation = new DoubleAnimation()
            {
                From = border.Height + 10,
                To = border.Height + 4,
                Duration = TimeSpan.FromSeconds(1.5)
            };

            ThicknessAnimation borderThicknessAnimation = new ThicknessAnimation()
            {
                From = border.BorderThickness,
                To = new Thickness(3),
                Duration = TimeSpan.FromSeconds(1.5),
            };

            border.BeginAnimation(Border.WidthProperty, widthAnimation);
            border.BeginAnimation(Border.HeightProperty, heightAnimation);
            border.BeginAnimation(Border.BorderThicknessProperty, borderThicknessAnimation);
        }
        private void StopBorderAnimation(Border border)
        {
            border.BorderBrush = new SolidColorBrush(Colors.Transparent);

            border.BeginAnimation(Border.WidthProperty, null);
            border.BeginAnimation(Border.HeightProperty, null);
            border.BeginAnimation(Border.BorderThicknessProperty, null);
            border.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, null);

            border.Width = (double)border.GetAnimationBaseValue(Border.WidthProperty);
            border.Height = (double)border.GetAnimationBaseValue(Border.HeightProperty);
            border.BorderThickness = (Thickness)border.GetAnimationBaseValue(Border.BorderThicknessProperty);
        }

        private async void AddCaseToDB()
        {
            using (TVAContext context = new TVAContext())
            {
                context.GeneralInfos.Add(generalInfo);
                context.RoadConditions.Add(roadCondition);

                _case.GeneralInfo = generalInfo;
                _case.RoadCondition = roadCondition;
                _case.State = "PROCESSING";
                _case.CaseAccidentPlace = caseAccidentPlace;
                _case.CreaterLogin = user.Login;

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

                if (caseAccidentPlace.AccidentOnHighway != null)
                {
                    context.AccidentOnHighways.Add(caseAccidentPlace.AccidentOnHighway);
                }
                if (caseAccidentPlace.AccidentOnVillage != null)
                {
                    context.AccidentOnVillages.Add(caseAccidentPlace.AccidentOnVillage);
                }

                try
                {
                    await context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Error: {e.Message}\nStack Trace: {e.StackTrace}\nInner exception: {(e.InnerException != null ? e.InnerException.Message : "")}");
                }
            }
            
        }
    }
}
