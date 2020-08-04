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
using System.Windows.Shapes;
using AccountingOfTraficViolation.Models;
using AccountingOfTraficViolation.Services;
using AccountingOfTraficViolation.ViewModels;

namespace AccountingOfTraficViolation.Views
{
    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        private UserVM userVM;

        public AdminWindow()
        {
            InitializeComponent();
            userVM = new UserVM();

            DataContext = userVM;
            UserGroupBox.Header = "Пользователь отсутствует";

            DiscardChangeButton.IsEnabled = false;
            SaveChangeButton.IsEnabled = false;
            DeleteUserButton.IsEnabled = false;
        }

        private void AcceptClick(object sender, RoutedEventArgs e)
        {
            if (UserGroupBox.CheckIfExistValidationError())
            {
                return;
            }

            if (userVM.CheckIfCurrenUserLoginExist())
            {
                MessageBox.Show("Аккаунт с таким логином существует.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            userVM.ConfirmChange();
        }
        private void RejectClick(object sender, RoutedEventArgs e)
        {
            userVM.DiscardChange();
        }
        private void FindUserClick(object sender, RoutedEventArgs e)
        {
            if (userVM.IsCurrentUserChanged &&
                MessageBox.Show("Изменение не сохранены, продолжить?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                userVM.DiscardChange();
                return;
            }

            if (string.IsNullOrEmpty(FindUserLoginTextBox.Text))
            {
                MessageBox.Show("Поле с логином не может быть пустым.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            userVM.SetCurrentUser(FindUserLoginTextBox.Text);

            if (userVM.CurrentUser != null)
            {
                UserGroupBox.Header = "Найденный пользователь";
                DiscardChangeButton.IsEnabled = true;
                SaveChangeButton.IsEnabled = true;
                DeleteUserButton.IsEnabled = true;
            }
            else
            {
                UserGroupBox.Header = "Пользователь отсутствует";
                DiscardChangeButton.IsEnabled = false;
                SaveChangeButton.IsEnabled = false;
                DeleteUserButton.IsEnabled = false;
            }
        }
        private void DeleteClick(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите удалить пользователя?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                return;
            }

            userVM.DeleteCurrentUser();
            UserGroupBox.Header = "Пользователь отсутствует";

            DiscardChangeButton.IsEnabled = false;
            SaveChangeButton.IsEnabled = false;
            DeleteUserButton.IsEnabled = false;
        }
        private void CreateNewUserClick(object sender, RoutedEventArgs e)
        {
            userVM.AddNewUser();

            UserGroupBox.Header = "Новый пользователь";

            FindUserLoginTextBox.Text = "";

            SaveChangeButton.IsEnabled = true;
            DiscardChangeButton.IsEnabled = false;
            DeleteUserButton.IsEnabled = false;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (userVM.IsCurrentUserChanged &&
                MessageBox.Show("Изменение не сохранены, продолжить?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                userVM.DiscardChange();
                return;
            }

            userVM.Dispose();
        }
    }
}
