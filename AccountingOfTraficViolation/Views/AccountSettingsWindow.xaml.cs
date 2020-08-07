using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using AccountingOfTraficViolation.Models;

namespace AccountingOfTraficViolation.Views
{
    /// <summary>
    /// Логика взаимодействия для AccountSettingsWindow.xaml
    /// </summary>
    public partial class AccountSettingsWindow : Window
    {
        private TVAContext TVAContext;
        private User user;
        private User origUser;
        private bool isChanged;

        public AccountSettingsWindow(User user)
        {
            InitializeComponent();

            TVAContext = new TVAContext();
            origUser = user;
            this.user = TVAContext.Users.Where(u => u.Id == user.Id).FirstOrDefault();

            isChanged = false;

            DataContext = this.user;
        }

        private void SaveChangeClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                if (button.Tag == null)
                {
                    return;
                }

                switch (button.Tag.ToString())
                {
                    case "1":
                        SavePersonalDataButton.IsEnabled = false;
                        DiscardPersonalDataButton.IsEnabled = false;
                        isChanged = false;
                        break;
                    case "2":
                        if (string.IsNullOrEmpty(FirstPasswordTextBox.Password) || string.IsNullOrEmpty(SecondPasswordTextBox.Password))
                        {
                            MessageBox.Show("Не все поля с паролем заполнены.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        else if (FirstPasswordTextBox.Password != SecondPasswordTextBox.Password)
                        {
                            MessageBox.Show("Пароли не совпадают. Попробуйте ещё раз.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        if (MessageBox.Show("Вы уверены, что хотите изменить пароль?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                        {
                            return;
                        }

                        user.Password = FirstPasswordTextBox.Password;
                        break;
                    default:
                        break;
                }

                TVAContext.SaveChanges();
                MessageBox.Show("Данные успешно изменены");
            }
        }
        private void CancelClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                if (button.Tag == null)
                {
                    return;
                }

                switch (button.Tag.ToString())
                {
                    case "1":
                        SavePersonalDataButton.IsEnabled = false;
                        DiscardPersonalDataButton.IsEnabled = false;
                        isChanged = false;

                        TVAContext.CancelAllChanges();
                        break;
                    default:
                        break;
                }
            }
        }
        private void ClearPwdClick(object sender, RoutedEventArgs e)
        {
            FirstPasswordTextBox.Password = "";
            SecondPasswordTextBox.Password = "";
        }

        private void PersonalTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            isChanged = true;
            SavePersonalDataButton.IsEnabled = true;
            DiscardPersonalDataButton.IsEnabled = true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (isChanged)
            {
                if (MessageBox.Show("Изменение не сохранены, продолжить?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                {
                    e.Cancel = true;
                    return;
                }
                TVAContext.CancelAllChanges();
            }

            origUser.Assign(user);
            TVAContext.Dispose();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            isChanged = false;
            SavePersonalDataButton.IsEnabled = false;
            DiscardPersonalDataButton.IsEnabled = false;
        }
    }
}
