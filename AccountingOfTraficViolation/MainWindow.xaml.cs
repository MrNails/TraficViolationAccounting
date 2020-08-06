﻿using System;
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
        }

        private void Welocme()
        {
            if (!string.IsNullOrEmpty(user.Name) && !string.IsNullOrEmpty(user.Surname))
            {
                WelcomeTextBlock.Text = $"Добро пожаловать, {user.Name} {user.Surname}";
            }
            else
            {
                WelcomeTextBlock.Text = "Добро пожаловать";
            }

            if (user.Role == 0)
            {
                AdminWindowMenuItem.Visibility = Visibility.Visible;
            }
            else
            {
                AdminWindowMenuItem.Visibility = Visibility.Hidden;
            }
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

                user = logInWindow.User;
            }
            catch (Exception ex)
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

                MessageBox.Show("Возникла ошибка, смотри подробности в файле Errors.txt в папке приложения", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                logger.ErrorMessage = exceptionMessage;
                logger.Log();

                this.Close();
            }

            Welocme();
        }

        private void OpenCaseClick(object sender, RoutedEventArgs e)
        {
            if (user == null)
            {
                MessageBox.Show("Вы не вошли в аккаунт и не можете открывать дело.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (this.user.Role != 1)
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

                exceptionMessage += "\nСтек трейс:\n" + ex.StackTrace + "\n" + "\n";

                MessageBox.Show("Возникла ошибка, смотри подробности в файле Errors.txt в папке приложения", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                logger.ErrorMessage = exceptionMessage;
                logger.Log();
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

                MessageBox.Show("Возникла ошибка, смотри подробности в файле Errors.txt в папке приложения", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                logger.ErrorMessage = exceptionMessage;
                logger.Log();
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
                    this.Visibility = Visibility.Visible;
                    Welocme();
                }
                else
                {
                    this.Close();
                }
            }
            catch (Exception ex)
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

                MessageBox.Show("Возникла ошибка, смотри подробности в файле Errors.txt в папке приложения", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                logger.ErrorMessage = exceptionMessage;
                logger.Log();

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

                MessageBox.Show(exceptionMessage, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AccountSettings_Click(object sender, RoutedEventArgs e)
        {
            if (accountSettingsWindow == null)
            {
                accountSettingsWindow = new AccountSettingsWindow(user);
                accountSettingsWindow.Show();
                accountSettingsWindow.Closed += (obj, arg) => 
                {
                    accountSettingsWindow = null;
                };
            }
        }
    }
}
