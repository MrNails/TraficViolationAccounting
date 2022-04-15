using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using AccountingOfTraficViolation.Views.AddInfoWindows;
using AccountingOfTraficViolation.Models;
using System.Windows.Media.Animation;
using AccountingOfTraficViolation.Services;
using Microsoft.Win32;
using AccountingOfTraficViolation.ViewModels;

namespace AccountingOfTraficViolation.Views
{
    /// <summary>
    /// Логика взаимодействия для OpenNewCaseWindow.xaml
    /// </summary>
    public partial class OpenNewCaseWindow : Window
    {
        private bool isStarted;
        private bool isSaving;

        private Officer officer;
        private GeneralInfo generalInfo;
        private CaseAccidentPlace caseAccidentPlace;
        private RoadCondition roadCondition;
        private ObservableCollection<ParticipantsInformation> participantsInformations;
        private ObservableCollection<Vehicle> vehicles;
        private ObservableCollection<Victim> victims;
        private Case _case;

        private SaveCaseToWordVM saveCaseToWordVM;

        public OpenNewCaseWindow(Officer officer)
        {
            InitializeComponent();

            this.officer = officer;
            _case = new Case();

            DataContext = _case;

            isStarted = false;
        }

        private void FillCase()
        {
            if (_case == null) 
            {
                _case = new Case();
            }

            _case.GeneralInfo = generalInfo;
            _case.CaseAccidentPlace = caseAccidentPlace;
            _case.RoadCondition = roadCondition;
            _case.ParticipantsInformations = participantsInformations;
            _case.Vehicles = vehicles;
            _case.Victims = victims;
            _case.State = "PROCESSING";
            _case.CaseAccidentPlace = caseAccidentPlace;
            _case.CreaterLogin = officer.Login;
        }

        private void GeneralInfoClick(object sender, RoutedEventArgs e)
        {
            AddGeneralInfoWindow generalInfoAddWindow = new AddGeneralInfoWindow(generalInfo);

            if (generalInfoAddWindow.ShowDialog() == true)
            {
                generalInfo = generalInfoAddWindow.GeneralInfo;
                GeneralInfoProgresImage.Source = new BitmapImage(new Uri("/Images/AcceptIcon.jpg", UriKind.Relative));
                StopBorderAnimation(GeneralInfoBorder);
                isStarted = true;
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
                isStarted = true;
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
                isStarted = true;
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
                isStarted = true;
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
                isStarted = true;
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
                isStarted = true;
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
                    isStarted = false;
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

            Brush borderBrush = new SolidColorBrush(Colors.Red);
            border.BorderBrush = borderBrush;

            DoubleAnimation widthAnimation = new DoubleAnimation()
            {
                From = border.Width + 20,
                To = border.Width + 4,
                Duration = TimeSpan.FromSeconds(1.5)
            };
            widthAnimation.Completed += (obj, arg) =>
            {
                ColorAnimation animationBorderBrush = new ColorAnimation()
                {
                    To = Colors.Purple,
                    Duration = TimeSpan.FromSeconds(1.5),
                    AutoReverse = true,
                    RepeatBehavior = new RepeatBehavior(new TimeSpan(0, 0, 15))
                };

                border.BorderBrush.BeginAnimation(SolidColorBrush.ColorProperty, animationBorderBrush);
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
            using (TVAContext context = new TVAContext(GlobalSettings.ConnectionStrings[Constants.DefaultDB]))
            {
                context.GeneralInfos.Add(generalInfo);
                context.RoadConditions.Add(roadCondition);

                FillCase();
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

        private async void WordSaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckValidationAndSetAnimation())
            {
                return;
            }

            if (saveCaseToWordVM == null)
            {
                string filePath = $@"{Environment.CurrentDirectory}\WordPattern\Accounting form.docx";

                if (System.IO.File.Exists(filePath))
                {
                    saveCaseToWordVM = new SaveCaseToWordVM(filePath);
                    saveCaseToWordVM.Saved += SavedDocumentMessage;
                    saveCaseToWordVM.ExceptionCaptured += ErrorDocumentMessage;
                }
                else
                {
                    MessageBox.Show("Не найден файл-шаблон для сохранения дела в на устройство." +
                                    "\nПоследующее сохранение не возможно без файла.",
                             "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

            }

            if (isSaving)
            {
                MessageBox.Show("Операция не возможна, так как на " +
                                "данный момент сохраняется другой файл.",
                                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            FillCase();

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

            ActionTextStatusBarItem.Content = $"Сохранение файла {saveFileDialog.SafeFileName}.";
            ActionProgress.Minimum = 0;
            ActionProgress.Maximum = 100;
            ActionProgress.Visibility = Visibility.Visible;

            Binding binding = new Binding()
            {
                Source = saveCaseToWordVM,
                Path = new PropertyPath("SavingProgress"),
                Mode = BindingMode.OneWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };

            ActionProgress.SetBinding(ProgressBar.ValueProperty, binding);

            saveCaseToWordVM.SaveFilePath = path;
            saveCaseToWordVM.Case = _case;
            saveCaseToWordVM.Officer = officer;

            try
            {
                isSaving = true;
                await saveCaseToWordVM.SaveAsync(documentSaveType);
            }
            catch (Exception ex)
            {
                ErrorDocumentMessage(ex, null);
            }
            finally
            {
                isSaving = false;
                ActionTextStatusBarItem.Content = "";
                ActionProgress.Value = 0;
                ActionProgress.Visibility = Visibility.Hidden;
            }

        }

        private void SavedDocumentMessage(WordSaveActionArgs args)
        {
            MessageBox.Show("Файл успешно сохранён.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void ErrorDocumentMessage(Exception ex, WordSaveActionArgs args)
        {
            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (isStarted &&
                MessageBox.Show("У вас есть не сохранённые данные." +
                                "Вы уверены, что хотите выйти?", 
                                "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
            {
                e.Cancel = true;
                DialogResult = null;

                return;
            }

            if (isSaving)
            {
                MessageBox.Show("Идёт сохранение файла. Не возможно закрыть окно.",
                                "Внимание", MessageBoxButton.OK, MessageBoxImage.Error);
                e.Cancel = true;
                DialogResult = null;

                return;
            }

            saveCaseToWordVM?.Dispose();
        }
    }
}
