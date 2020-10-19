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
using AccountingOfTraficViolation.Services;
using System.Reflection;
using Microsoft.Win32;

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
            bool isValid = CheckValidationAndSetAnimation();

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

        private bool CheckValidationAndSetAnimation()
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

            return isValid;
        }

        #region WordSaver Region
        private async void WordSaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckValidationAndSetAnimation())
            {
                return;
            }

            GeneralInfo generalInfo = this.generalInfo.Clone();
            CaseAccidentPlace caseAccidentPlace = this.caseAccidentPlace.Clone();
            RoadCondition roadCondition = this.roadCondition.Clone();
            List<ParticipantsInformation> participantsInformations = new List<ParticipantsInformation>(this.participantsInformations);
            List<Vehicle> vehicles = new List<Vehicle>(this.vehicles);
            List<Victim> victims = new List<Victim>(this.victims);

            FrameworkElement frameworkElement = sender as FrameworkElement;

            await Task.Run(() =>
            {

                if (frameworkElement == null)
                {
                    return;
                }

                string tag = this.Dispatcher.Invoke(() => frameworkElement.Tag.ToString());

                string path = null;

                SaveFileDialog saveFileDialog = new SaveFileDialog();
                DocumentSaveType documentSaveType;

                switch (tag)
                {
                    case "1":
                        saveFileDialog.Filter = "Word Files | *.docx";
                        documentSaveType = DocumentSaveType.DOCX;
                        break;
                    case "2":
                        saveFileDialog.Filter = "PDF Files | *.pdf";
                        documentSaveType = DocumentSaveType.PDF;
                        break;
                    default:
                        saveFileDialog.Filter = "Word Files | *.docx";
                        documentSaveType = DocumentSaveType.DOCX;
                        break;
                }

                if (saveFileDialog.ShowDialog() == true)
                {
                    path = saveFileDialog.FileName;
                }
                else
                {
                    return;
                }

                WordSaver wordSaver = new WordSaver($@"{Environment.CurrentDirectory}\WordPattern\Accounting form.docx");

                try
                {
                    wordSaver.OpenDocument();

                    wordSaver.Replace<GeneralInfo>(generalInfo, Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne,
                                                   propName => $"%{propName}%",
                                                   (propName, propValue) =>
                                                   {
                                                       if (propValue == null)
                                                       {
                                                           return "";
                                                       }

                                                       string value = ConverToString(propValue);

                                                       if (propName == "CardNumber")
                                                       {
                                                           value.AddSeparator('-', 2, 10);
                                                       }

                                                       return WrapEachSbmlInVerticalLine(value);
                                                   });

                    if (caseAccidentPlace.AccidentOnVillage != null)
                    {
                        wordSaver.Replace<AccidentOnVillage>(caseAccidentPlace.AccidentOnVillage,
                                                             Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne,
                                                             propName => $"%{propName}%",
                                                             (propName, propValue) =>
                                                             {
                                                                 if (propValue == null)
                                                                 {
                                                                     return "";
                                                                 }

                                                                 string value = propValue.ToString();

                                                                 if (propName.Contains("Code"))
                                                                 {
                                                                     value = value.AddZeroBeforeText(4 - value.Length);
                                                                 }
                                                                 else
                                                                 {
                                                                     int maxLength = GetAttributeMaxLength(caseAccidentPlace.AccidentOnVillage, propName);

                                                                     if (maxLength != -1)
                                                                     {
                                                                         value = value.AddSymbols('_', maxLength - value.Length);
                                                                     }
                                                                 }

                                                                 return WrapEachSbmlInVerticalLine(value);
                                                             });

                        wordSaver.Replace("%HighwayIndexAndNumber%", "|__|-|__|__|-|__|__|-|__|", Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace("%AdditionalInfo%", "_______________________________________________________", Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace("%Kilometer%", "|__|__|__|__|", Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace("%Meter%", "|__|__|__|", Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace("%Binding%", "_______________________________________________________" +
                                                        " _______________________________________________________", Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                    }
                    else if (caseAccidentPlace.AccidentOnHighway != null)
                    {
                        wordSaver.Replace<AccidentOnVillage>(new AccidentOnVillage(),
                                                             Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne,
                                                             propName => $"%{propName}%",
                                                             (propName, propValue) =>
                                                             {
                                                                 if (propName == "Status")
                                                                 {
                                                                     return "|__|";
                                                                 }
                                                                 else if (propName.Contains("Code"))
                                                                 {
                                                                     return "|__|__|__|__|";
                                                                 }
                                                                 else if (propName == "VillageBinding")
                                                                 {
                                                                     return "|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|" +
                                                                            " |__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|";
                                                                 }
                                                                 else
                                                                 {
                                                                     return "|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|";
                                                                 }
                                                             });

                        wordSaver.Replace<AccidentOnHighway>(caseAccidentPlace.AccidentOnHighway,
                                                             Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne,
                                                             propName => $"%{propName}%",
                                                             (propName, propValue) =>
                                                             {
                                                                 if (propValue == null)
                                                                 {
                                                                     return "";
                                                                 }

                                                                 string value = propValue.ToString();

                                                                 if (propName == "AdditionalInfo" || propName == "Binding")
                                                                 {
                                                                     return value;
                                                                 }

                                                                 if (propName == "HighwayIndexAndNumber")
                                                                 {
                                                                     value = value.AddSeparator('-', 1, 4, 7);
                                                                 }

                                                                 return WrapEachSbmlInVerticalLine(value);
                                                             });

                    }

                    wordSaver.Replace<RoadCondition>(roadCondition,
                                                     Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne,
                                                     propName =>
                                                     {
                                                         StringBuilder propNameBuilder = new StringBuilder();

                                                         propNameBuilder.Append('%');
                                                         propNameBuilder.Append(propName.GetStringWithUpperSymbols());
                                                         propNameBuilder.Append('%');

                                                         return propNameBuilder.ToString();
                                                     },
                                                     (propName, propValue) =>
                                                     {
                                                         if (propValue == null)
                                                         {
                                                             return "";
                                                         }

                                                         string value = propValue.ToString();

                                                         if (propName == "SurfaceState")
                                                         {
                                                             value = value.AddSeparator(',', 1);
                                                         }
                                                         else if (propName == "PlaceElement")
                                                         {
                                                             value = value.AddSeparator(',', 2, 5);
                                                         }
                                                         else if (propName == "TechnicalTool" || propName == "RoadDisadvantages")
                                                         {
                                                             value = value.AddSeparator(',', 2, 5, 8, 11);
                                                         }

                                                         return WrapEachSbmlInVerticalLine(value);
                                                     });

                    List<ParticipantsInformation> tempList = null;

                    if (participantsInformations.Count < 5)
                    {
                        tempList = new List<ParticipantsInformation>();

                        tempList.AddRange(participantsInformations);
                        ParticipantsInformation participantsInformation = new ParticipantsInformation() { Id = -1 };

                        for (int i = tempList.Count; i < 5; i++)
                        {
                            tempList.Add(participantsInformation);
                        }
                    }

                    string qualification = null;
                    string age = null;
                    string gender = null;
                    string citizenship = null;
                    string driveExpirience = null;
                    string drivingTimeBeforeAccident = null;
                    string pddViolation = null;

                    for (int i = 0; i < tempList.Count; i++)
                    {
                        if (tempList[i].Id != -1)
                        {
                            qualification = tempList[i].Qualification.ToString();
                            age = tempList[i].Age.ToString();
                            gender = tempList[i].Gender.ToString();
                            citizenship = tempList[i].Citizenship.ToString();
                            driveExpirience = tempList[i].DriveExpirience.ToString();
                            drivingTimeBeforeAccident = tempList[i].DrivingTimeBeforeAccident.ToString();
                            pddViolation = tempList[i].PDDViolation.AddSeparator(',', 2);

                            qualification = WrapEachSbmlInVerticalLine(qualification.AddZeroBeforeText(2 - qualification.Length));
                            age = WrapEachSbmlInVerticalLine(age.AddZeroBeforeText(3 - age.Length));
                            citizenship = WrapEachSbmlInVerticalLine(citizenship.AddZeroBeforeText(GetAttributeMaxLength(tempList[i], "Citizenship") - citizenship.Length));
                            driveExpirience = WrapEachSbmlInVerticalLine(driveExpirience.AddZeroBeforeText(2 - driveExpirience.Length));
                            drivingTimeBeforeAccident = WrapEachSbmlInVerticalLine(drivingTimeBeforeAccident.AddZeroBeforeText(2 - drivingTimeBeforeAccident.Length));
                            pddViolation = WrapEachSbmlInVerticalLine(pddViolation);

                            if (gender != "True")
                            {
                                gender = "| М |";
                            }
                            else
                            {
                                gender = "| Ж |";
                            }
                        }
                        else
                        {
                            qualification = "|__|__|";
                            age = "|__|__|__|";
                            gender = "|__|";
                            citizenship = age;
                            driveExpirience = qualification;
                            drivingTimeBeforeAccident = qualification;
                            pddViolation = "|__|__|,|__|__|";
                        }

                        wordSaver.Replace($"%Surname{i + 1}%", tempList[i].Surname, Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%Name{i + 1}%", tempList[i].Name, Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%Patronymic{i + 1}%", tempList[i].Patronymic, Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%Address{i + 1}%", tempList[i].Address, Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%Qualification{i + 1}%", qualification, Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%Age{i + 1}%", age, Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%Gender{i + 1}%", gender, Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%Citizenship{i + 1}%", citizenship, Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%DriveExpirience{i + 1}%", driveExpirience, Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%DTBA{i + 1}%", drivingTimeBeforeAccident, Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%PDDViolation{i + 1}%", pddViolation, Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                    }


                    for (int i = 0; i < vehicles.Count; i++)
                    {
                        wordSaver.Replace<Vehicle>(vehicles[i],
                                                   Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne,
                                                         propName =>
                                                         {
                                                             StringBuilder propNameBuilder = new StringBuilder();

                                                             propNameBuilder.Append('%');

                                                             if (propName == "SeriesOfRegistrationSertificate" ||
                                                                 propName == "RegistrationSertificate" ||
                                                                 propName == "TrailerAvailability" ||
                                                                 propName == "LicenceSeries" ||
                                                                 propName == "LicenceNumber")
                                                             {
                                                                 propNameBuilder.Append(propName.GetStringWithUpperSymbols() + (i + 1));
                                                             }
                                                             else if (propName == "Surname")
                                                             {
                                                                 propNameBuilder.Append("v" + propName + (i + 1));
                                                             }
                                                             else
                                                             {
                                                                 propNameBuilder.Append(propName + (i + 1));
                                                             }

                                                             propNameBuilder.Append('%');

                                                             return propNameBuilder.ToString();
                                                         },
                                                         (propName, propValue) =>
                                                         {
                                                             if (propValue == null)
                                                             {
                                                                 return "";
                                                             }

                                                             string value = ConverToString(propValue);

                                                             if (propName == "TechnicalFaults")
                                                             {
                                                                 value = value.AddSeparator(',', 1);
                                                             }
                                                             else if (propName == "EDRPOU_Code1")
                                                             {
                                                                 value = value.AddSeparator('-', 7);
                                                             }
                                                             else if (propName == "CorruptionCode")
                                                             {
                                                                 value = value.AddSeparator(',', 2, 5, 8);
                                                             }

                                                             int maxLength = GetAttributeMaxLength<Vehicle>(vehicles[i], propName);

                                                             if (maxLength != -1)
                                                             {
                                                                 value = value.AddSymbols('_', maxLength - value.Length);
                                                             }

                                                             value = WrapEachSbmlInVerticalLine(value);

                                                             if (propName == "CorruptionCode")
                                                             {
                                                                 for (int j = 0; j < 3; j++)
                                                                 {
                                                                     value = value.Replace(" , ", ",");
                                                                 }
                                                             }

                                                             return value;
                                                         });
                    }

                    Vehicle vehicle = new Vehicle();

                    for (int i = vehicles.Count; i <= 5; i++)
                    {
                        wordSaver.Replace($"%PlateNumber{i}%", "|__|__|__|__|__|__|__|__|", Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%FrameNumber{i}%", "|__|__|__|__|__|__|__|__|", Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%ChasisNumber{i}%", "|__|__|__|__|__|__|__|__|__|", Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%Make{i}%", "|__|__|__|__|__|__|__|__|__|__|", Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%Model{i}%", "|__|__|__|__|__|__|__|__|__|__|", Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%Type{i}%", "|__|__|__|", Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%SORS{i}%", "|__|__|__|", Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%RS{i}%", "|__|__|__|__|__|__|", Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%TA{i}%", "|__|", Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%InsurerCode{i}%", "|__||__||__|", Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%PolicySeries{i}%", "|__|__|__|__|__|__|__|__|__||__|", Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%PolicyNumber{i}%", "|__|__|__|__|__|__|__|__|__||__|", Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%PolicyEndDate{i}%", "|__|__|-|__|__|-|__|__|__|__|", Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%vSurname{i}%", "|__|__|__|__|__|__|__|__|__|__|", Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%LS{i}%", "|__|__|__|", Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%LN{i}%", "|__|__|__|__|__|__|", Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%Owner{i}%", "|__|__|__|__|__|__|__|__|__|__| |__|__|__|__|__|__|__|__|__|__|", Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%TechnicalFaults{i}%", "|__|,|__|", Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%EDRPOU_Code{i}%", "|__|__|__|__|__|__|__| -|__|__|__|", Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%CorruptionCode{i}%", "|__|__|,|__|__|,|__|__|,|__|__|", Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%ActivityLicensingInfo{i}%", "|__||__|", Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                    }

                    List<Victim> victimTempList = null;

                    if (victims.Count < 10)
                    {
                        victimTempList = new List<Victim>();

                        victimTempList.AddRange(victims);
                        Victim victim = new Victim() { Id = -1 };

                        for (int i = victimTempList.Count; i < 10; i++)
                        {
                            victimTempList.Add(victim);
                        }
                    }

                    string victimIsDied = null;
                    string victimCategory = null;
                    string victimGender = null;
                    string victimAge = null;
                    string victimTORSerialNumber = null;
                    string victimSeatBelt = null;
                    string victimMedicalResult = null;
                    string victimCitizenship = null;

                    for (int i = 0; i < victimTempList.Count; i++)
                    {
                        if (victimTempList[i].Id != -1)
                        {
                            victimIsDied = victimTempList[i].IsDied.AddSymbols('_', GetAttributeMaxLength(victims[i], "IsDied") - victims[i].IsDied.Length);
                            victimCategory = victimTempList[i].Category.ToString();
                            victimGender = victimTempList[i].Gender.ToString();
                            victimAge = victimTempList[i].Age.ToString();
                            victimTORSerialNumber = victimTempList[i].TORSerialNumber.ToString();
                            victimSeatBelt = ConverToString(victimTempList[i].SeatBelt);
                            victimMedicalResult = victimTempList[i].MedicalResult.ToString();
                            victimCitizenship = victimTempList[i].Citizenship;

                            victimIsDied = WrapEachSbmlInVerticalLine(victimIsDied);
                            victimCategory = WrapEachSbmlInVerticalLine(victimCategory.AddZeroBeforeText(2 - victimCategory.Length));
                            victimAge = WrapEachSbmlInVerticalLine(victimAge.AddZeroBeforeText(3 - victimAge.Length));
                            victimTORSerialNumber = WrapEachSbmlInVerticalLine(victimTORSerialNumber.AddZeroBeforeText(2 - victimTORSerialNumber.Length));
                            victimSeatBelt = WrapEachSbmlInVerticalLine(victimSeatBelt);
                            victimMedicalResult = WrapEachSbmlInVerticalLine(victimMedicalResult);
                            victimCitizenship = WrapEachSbmlInVerticalLine(victimCitizenship.AddZeroBeforeText(GetAttributeMaxLength(victims[i], "Citizenship") - victimCitizenship.Length));

                            if (victimGender != "True")
                            {
                                victimGender = "| М |";
                            }
                            else
                            {
                                victimGender = "| Ж |";
                            }
                        }
                        else
                        {
                            victimIsDied = "|__|__|";
                            victimCategory = "|__|__|";
                            victimGender = "|__|";
                            victimAge = "|__|__|__|";
                            victimTORSerialNumber = "|__|__|";
                            victimSeatBelt = "|__|";
                            victimMedicalResult = "|__|";
                            victimCitizenship = "|__|__|__|";
                        }

                        wordSaver.Replace($"%IsDied{i + 1}%", victimIsDied, Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%Cy{i + 1}%", victimCategory, Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%Age{i + 1}%", victimAge, Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%TOR{i + 1}%", victimTORSerialNumber, Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%G{i + 1}%", victimGender, Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%SB{i + 1}%", victimSeatBelt, Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%MR{i + 1}%", victimMedicalResult, Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%S{i + 1}%", victimTempList[i].Surname, Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%N{i + 1}%", victimTempList[i].Name, Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%P{i + 1}%", victimTempList[i].Patronymic, Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%C{i + 1}%", victimCitizenship, Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                    }

                    string allVictims = victims.Count.ToString();
                    allVictims = WrapEachSbmlInVerticalLine(allVictims.AddZeroBeforeText(3 - allVictims.Length));

                    wordSaver.Replace($"%AV%", allVictims, Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);

                    if (user != null)
                    {
                        wordSaver.Replace("%UserFullName%", $"{user.Name} {user.Surname}", Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace("%UserPhone%", user.Phone, Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                    }

                    wordSaver.SaveDocumentAs(path, documentSaveType);

                    MessageBox.Show($"Файл {saveFileDialog.SafeFileName} успешно сохранён.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    wordSaver.Dispose();
                }
            });
        }

        private string WrapEachSbmlInVerticalLine(string textToWrap)
        {
            StringBuilder wrapedString = new StringBuilder();

            foreach (var smbl in textToWrap)
            {
                wrapedString.Append("| ");
                wrapedString.Append(smbl);
                wrapedString.Append(' ');
            }

            wrapedString.Append('|');

            return wrapedString.ToString();
        }

        private string ConverToString(object _object)
        {
            string formattedValue = null;

            switch (_object.GetType().Name)
            {
                case "DateTime":
                    formattedValue = ((DateTime)_object).ToString("d").GetStrWithoutSeparator('.').AddSeparator('-', 2, 5);
                    break;
                case "TimeSpan":
                    formattedValue = ((TimeSpan)_object).ToString(@"hh\:mm").Remove(2, 1).AddSeparator('-', 2);
                    break;
                case "Boolean":
                    if ((bool)_object)
                    {
                        formattedValue = "+";
                    }
                    else
                    {
                        formattedValue = "-";
                    }
                    break;
                default:
                    formattedValue = _object.ToString();
                    break;
            }

            return formattedValue;
        }

        private int GetAttributeMaxLength<T>(T _object, string propertyName)
        {
            int length = -1;

            if (_object == null || string.IsNullOrEmpty(propertyName))
            {
                return length;
            }

            Type objectType = _object.GetType();

            Attribute attr = objectType.GetProperty(propertyName).GetCustomAttribute(typeof(System.ComponentModel.DataAnnotations.StringLengthAttribute), false);

            if (attr != null)
            {
                length = ((System.ComponentModel.DataAnnotations.StringLengthAttribute)attr).MaximumLength;
            }

            return length;
        }
        #endregion
    }
}
