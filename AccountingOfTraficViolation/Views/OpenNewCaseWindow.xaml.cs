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
            AddAccidentPlaceWindow accidentPlaceWinow = new AddAccidentPlaceWindow(caseAccidentPlace, user: user);
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

        private void WordSaveButton_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement frameworkElement = sender as FrameworkElement;

            if (frameworkElement == null)
            {
                return;
            }

            string tag = frameworkElement.Tag.ToString();
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

            WordSaver wordSaver = new WordSaver(@"C:\Users\popov\source\repos\AccountingOfTraficViolation\AccountingOfTraficViolation\TestWord\Accounting form.docx"); ;

            try
            {
                wordSaver.OpenDocument();

                if (generalInfo != null)
                {
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
                }

                if (caseAccidentPlace != null)
                {
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
                }

                if (roadCondition != null)
                {
                    wordSaver.Replace<RoadCondition>(roadCondition,
                                                     Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne,
                                                     propName => $"%{propName}%",
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
                }

                if (participantsInformations != null)
                {
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
                        wordSaver.Replace($"%DrivingTimeBeforeAccident{i + 1}%", drivingTimeBeforeAccident, Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%PDDViolation{i + 1}%", pddViolation, Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                    }
                }

                wordSaver.SaveDocumentAs(path, documentSaveType);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                wordSaver.Dispose();
            }

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
    }
}
