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
            casesVM = new CasesVM();
            this.user = user;

            FindLoginTextBox.Text = user.Login;

            DataContext = casesVM;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            casesVM.Dispose();
        }

        private async void FindCaseClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                Button button = (Button)sender;
                button.Content = "Отменить";
                ComboBoxItem statusItem = CaseStatusComboBox.SelectedItem as ComboBoxItem;

                string login = null;
                string status = null;
                DateTime exactDate = default(DateTime);
                DateTime startDate = default(DateTime);
                DateTime endDate = default(DateTime);

                if (string.IsNullOrEmpty(FindLoginTextBox.Text) && AllDateRadioButton.IsChecked == true &&
                    statusItem != null && statusItem.Tag.ToString() == "1")
                {
                    button.Content = "Найти";
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
                cancellationTokenSource.Token.Register(() =>
                {
                    MessageBox.Show("Операция прервана.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                });

                try
                {
                    await casesVM.FindCaseAsync(contex =>
                    {
                        return contex.Cases.Where(c => (string.IsNullOrEmpty(login) || c.CreaterLogin == login || c.CreaterLogin.Contains(login)) &&
                                                       (string.IsNullOrEmpty(status) || c.State == status || c.State.Contains(status)) &&
                                                       (exactDate == default(DateTime) || exactDate < MainTable.MinimumDate || c.OpenAt == exactDate) &&
                                                       (startDate == default(DateTime) || startDate < MainTable.MinimumDate || c.OpenAt >= startDate) &&
                                                       (endDate == default(DateTime) || c.OpenAt <= endDate));
                    }, cancellationTokenSource.Token);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    cancellationTokenSource = null;
                }

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

        private void CasesGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("+");
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
    }
}
