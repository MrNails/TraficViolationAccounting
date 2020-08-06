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

namespace AccountingOfTraficViolation.Views
{
    /// <summary>
    /// Логика взаимодействия для ShowCaseWindow.xaml
    /// </summary>
    public partial class ShowCaseWindow : Window
    {
        private CasesVM casesVM;
        private User user;
        private CancellationTokenSource cancellationTokenSource;

        public ShowCaseWindow(User user)
        {
            InitializeComponent();

            casesVM = new CasesVM(user);
            this.user = user;

            if (user.Role != 0)
            {
                FindLoginTextBox.Text = user.Login;
            }

            DataContext = casesVM;
        }

        private async void FindCase()
        {
            ComboBoxItem statusItem = CaseStatusComboBox.SelectedItem as ComboBoxItem;

            string login = null;
            string status = null;
            DateTime exactDate = default(DateTime);
            DateTime startDate = default(DateTime);
            DateTime endDate = default(DateTime);

            if (string.IsNullOrEmpty(FindLoginTextBox.Text) && AllDateRadioButton.IsChecked == true &&
                statusItem != null && statusItem.Tag.ToString() == "1")
            {
                MessageBox.Show("Вы не можете выбрать все дела.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
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

            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();
                cancellationTokenSource = null;
                return;
            }

            cancellationTokenSource = new CancellationTokenSource();

            try
            {
                await casesVM.FindCaseAsync(c =>
                {
                    return (string.IsNullOrEmpty(login) || c.CreaterLogin == login || c.CreaterLogin.Contains(login)) &&
                           (string.IsNullOrEmpty(status) || c.State == status) &&
                           (exactDate == default(DateTime) || exactDate < MainTable.MinimumDate || c.OpenAt == exactDate) &&
                           (startDate == default(DateTime) || startDate < MainTable.MinimumDate || c.OpenAt >= startDate) &&
                           (endDate == default(DateTime) || c.OpenAt <= endDate);
                }, cancellationTokenSource.Token);
            }
            catch (OperationCanceledException ex)
            {
                MessageBox.Show(ex.Message, "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                cancellationTokenSource = null;
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

            casesVM.Dispose();
        }

        private void FindCaseClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                Button button = (Button)sender;
                button.Content = "Отменить";
                FindConditionExpander.IsExpanded = false;

                FindCase();

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
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}\nСтек трейс: {ex.StackTrace}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
