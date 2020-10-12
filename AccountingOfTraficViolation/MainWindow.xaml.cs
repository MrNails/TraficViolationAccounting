using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using AccountingOfTraficViolation.Services;
using AccountingOfTraficViolation.Views;

namespace AccountingOfTraficViolation
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Models.User user;
        private ILogger logger;
        private AccountSettingsWindow accountSettingsWindow;

        public MainWindow()
        {
            InitializeComponent();
            logger = new FileLogger("Errors.txt");

            DataContext = user;
        }

        private void InitUser()
        {
            if (user.Role == (byte)UserRole.Debug || user.Role == (byte)UserRole.Admin)
            {
                AdminWindowMenuItem.Visibility = Visibility.Visible;
            }
            else
            {
                AdminWindowMenuItem.Visibility = Visibility.Hidden;
            }

            DataContext = user;
            GC.Collect();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            AuthorizationWindow logInWindow = new AuthorizationWindow();

            try
            {
                if (logInWindow.ShowDialog() != true)
                {
                    this.Close();
                }
                else
                {
                    user = logInWindow.User;
                    InitUser();
                }
            }
            catch (Exception ex)
            {
                CatchError(ex);

                this.Close();
            }
        }

        private void OpenCaseClick(object sender, RoutedEventArgs e)
        {
            if (user == null)
            {
                MessageBox.Show("Вы не вошли в аккаунт и не можете открывать дело.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (this.user.Role != (byte)UserRole.User && this.user.Role != (byte)UserRole.Debug)
            {
                MessageBox.Show("У вас не хватает привелегий на создание дела.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            OpenNewCaseWindow caseWindow = new OpenNewCaseWindow(user);

            try
            {
                caseWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                CatchError(ex);
            }
        }
        private void ShowCaseClick(object sender, RoutedEventArgs e)
        {
            ShowCaseWindow showCaseWindow = new ShowCaseWindow(user);

            try
            {
                showCaseWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                CatchError(ex);
            }
        }

        private void ExitAccountClick(object sender, RoutedEventArgs e)
        {
            AuthorizationWindow logInWindow = new AuthorizationWindow();
            this.Visibility = Visibility.Hidden;

            try
            {
                if (logInWindow.ShowDialog() == true)
                {
                    user = logInWindow.User;
                    InitUser();

                    this.Visibility = Visibility.Visible;
                }
                else
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                CatchError(ex);

                this.Close();
            }
        }

        private void AdminWindowMenuItem_Click(object sender, RoutedEventArgs e)
        {
            AdminWindow adminWindow = new AdminWindow();

            try
            {
                adminWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                CatchError(ex);
            }
        }

        private void AccountSettings_Click(object sender, RoutedEventArgs e)
        {
            if (accountSettingsWindow == null)
            {
                try
                {
                    accountSettingsWindow = new AccountSettingsWindow(user);
                    accountSettingsWindow.Show();
                    accountSettingsWindow.Closed += (obj, arg) =>
                    {
                        accountSettingsWindow = null;
                    };
                }
                catch (Exception ex)
                {
                    CatchError(ex);

                    this.Close();
                }
            }
        }

        private void CatchError(Exception ex)
        {
            string innerExceptionMessage = ex.GetInnerExceptionMessage();
            string exceptionMessage = "Ошибка: ";

            if (string.IsNullOrEmpty(innerExceptionMessage))
            {
                exceptionMessage += ex.Message;
            }
            else
            {
                exceptionMessage += innerExceptionMessage;
            }

            exceptionMessage += "\nСтек трейс:\n" + ex.StackTrace + "\n";

            MessageBox.Show("Возникла ошибка, смотри подробности в файле Errors.txt в папке приложения.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            logger.ErrorMessage = exceptionMessage;
            logger.Log();
        }

        private void TextBox_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("1111");
        }
    }
}
