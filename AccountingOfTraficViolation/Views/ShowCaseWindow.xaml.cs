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

                string login = string.Copy(FindLoginTextBox.Text);

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
                        return contex.Cases.Where(c => c.CreaterLogin == login || c.CreaterLogin.Contains(login));
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

        private void CasesGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("+");
        }
    }
}
