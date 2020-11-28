using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using AccountingOfTraficViolation.ViewModels;
using AccountingOfTraficViolation.Services;
using Microsoft.Win32;

namespace AccountingOfTraficViolation.Views
{
    /// <summary>
    /// Логика взаимодействия для ShowCaseWindow.xaml
    /// </summary>
    public partial class ShowCaseWindow : Window
    {
        private bool isSaving;

        private CasesVM casesVM;
        private User user;

        private CancellationTokenSource cancellationTokenSource;

        public ShowCaseWindow(User user)
        {
            InitializeComponent();

            casesVM = new CasesVM(user);
            this.user = user;

            if (user.Role == (byte)UserRole.User)
            {
                FindLoginTextBox.Text = user.Login;
            }

            DataContext = casesVM;

        }

        private async Task FindCase()
        {
            bool loadResult = false;
            CurrentAction.Content = "Загрузка данных из бд.";
            ActionProgress.IsIndeterminate = true;
            ActionProgress.Visibility = Visibility.Visible;

            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();
                cancellationTokenSource = null;
                return;
            }

            cancellationTokenSource = new CancellationTokenSource();

            ComboBoxItem statusItem = CaseStatusComboBox.SelectedItem as ComboBoxItem;

            string login = null;
            string status = null;
            DateTime exactDate = default(DateTime);
            DateTime startDate = default(DateTime);
            DateTime endDate = default(DateTime);

            try
            {
                if (string.IsNullOrEmpty(FindLoginTextBox.Text) && AllDateRadioButton.IsChecked == true &&
                    statusItem != null && statusItem.Tag.ToString() == "1")
                {
                    throw new Exception("Вы не можете выбрать все дела.");
                }

                login = string.Copy(FindLoginTextBox.Text);
                status = GetStatusFromComboBox(statusItem);

                if (ExactDateRadioButton.IsChecked == true && ExactDateDatePicker.SelectedDate.HasValue)
                {
                    exactDate = ExactDateDatePicker.SelectedDate.Value;
                }
                else if (RangeDateRadioButton.IsChecked == true && StartDateDatePicker.SelectedDate.HasValue &&
                         EndDateDatePicker.SelectedDate.HasValue)
                {
                    if (StartDateDatePicker.SelectedDate.Value >= MainTable.MinimumDate)
                    {
                        startDate = StartDateDatePicker.SelectedDate.Value;
                    }
                    else
                    {
                        startDate = MainTable.MinimumDate;
                    }

                    if (EndDateDatePicker.SelectedDate.Value >= MainTable.MinimumDate)
                    {
                        endDate = EndDateDatePicker.SelectedDate.Value;
                    }
                    else
                    {
                        endDate = MainTable.MinimumDate;
                    }
                }


                loadResult = await casesVM.FindCaseAsync(_case =>
                {
                    return _case.Where(c => (string.IsNullOrEmpty(login) || c.CreaterLogin == login || c.CreaterLogin.Contains(login)) &&
                                            (string.IsNullOrEmpty(status) || c.State == status) &&
                                            (exactDate == default(DateTime) || exactDate < MainTable.MinimumDate || c.OpenAt == exactDate) &&
                                            (startDate == default(DateTime) || startDate < MainTable.MinimumDate || c.OpenAt >= startDate) &&
                                            (endDate == default(DateTime) || c.OpenAt <= endDate));
                }, cancellationTokenSource.Token);
            }
            catch (OperationCanceledException ex)
            {
                MessageBox.Show(ex.Message, "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex) when (ex.InnerException != null)
            {
                MessageBox.Show(ex.InnerException.Message.Split('\n')[1], "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                cancellationTokenSource = null;
                if (loadResult)
                {
                    CurrentAction.Content = "Данные из бд загружены.";
                }
                else
                {
                    CurrentAction.Content = "Не удалось загрузить данные из бд.";
                }

                ActionProgress.IsIndeterminate = false;
                ActionProgress.Visibility = Visibility.Hidden;
            }


        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (casesVM.CaseChanged &&
                MessageBox.Show("Вы уверенны, что хотите выйти?\nВы можете потерять все изменения.", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                e.Cancel = true;
                return;
            }

            if (isSaving)
            {
                MessageBox.Show("Не возможно выйти, пока идёт сохранение файла.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Error);
                e.Cancel = true;
                return;
            }

            casesVM.Dispose();
        }

        private async void FindCaseClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                Button button = (Button)sender;
                button.Content = "Отменить";


                FindConditionExpander.IsExpanded = false;

                await FindCase();


                button.Content = "Найти";
            }
        }

        private string GetStatusFromComboBox(ComboBoxItem comboBoxItem)
        {
            string status = null;

            switch (comboBoxItem.Tag.ToString())
            {
                case "2":
                    status = "PROCESSING";
                    break;
                case "3":
                    status = "CLOSE";
                    break;
                default:
                    break;
            }

            return status;
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is DatePicker)
            {
                DatePicker datePicker = (DatePicker)sender;

                if (datePicker.SelectedDate < MainTable.MinimumDate)
                {
                    datePicker.SelectedDate = MainTable.MinimumDate;
                }
            }
        }

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGridRow)
            {
                DataGridRow dataGridRow = (DataGridRow)sender;

                if (casesVM.DoubleClickCaseInfo.CanExecute(dataGridRow.Item))
                {
                    casesVM.DoubleClickCaseInfo.Execute(dataGridRow.Item);
                }
            }
        }

        private void SaveChangesClick(object sender, RoutedEventArgs e)
        {
            try
            {
                casesVM.SaveChangesAsync();
                MessageBox.Show("Данные успешно сохранены.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}\nСтек трейс: {ex.StackTrace}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void DiscardChangesClick(object sender, RoutedEventArgs e)
        {
            try
            {
                casesVM.DiscardChanges();
                MessageBox.Show("Данные возвращены в начальное состояние.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}\nСтек трейс: {ex.StackTrace}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void SaveDocumentClick(object sender, RoutedEventArgs e)
        {
            FrameworkElement frameworkElement = sender as FrameworkElement;

            if (frameworkElement == null)
            {
                return;
            }

            if (isSaving)
            {
                MessageBox.Show("Операция не возможна, так как на " +
                                "данный момент сохраняется другой файл.", 
                                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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

            CurrentAction.Content = $"Сохранение файла {saveFileDialog.SafeFileName}.";
            ActionProgress.Minimum = 0;
            ActionProgress.Maximum = 100;
            ActionProgress.Visibility = Visibility.Visible;
            
            Binding savingProgressBinding = new Binding()
            {
                Source = casesVM.SaveCaseToWordVM,
                Path = new PropertyPath("SavingProgress"),
                Mode = BindingMode.OneWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };

            ActionProgress.SetBinding(ProgressBar.ValueProperty, savingProgressBinding);

            try
            {
                isSaving = true;
                await casesVM.SaveToDocument(path, documentSaveType);
            }
            finally
            {
                isSaving = false;
                CurrentAction.Content = "Ничего не происходит.";
                ActionProgress.Value = 0;
                ActionProgress.Visibility = Visibility.Hidden;
            }
        }
    }
}
