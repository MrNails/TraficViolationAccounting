using System;
using System.Collections.Generic;
using System.Data.Entity;
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

namespace AccountingOfTraficViolation.Views
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationWindow.xaml
    /// </summary>
    public partial class AuthorizationWindow : Window
    {
        private TVAContext context;

        public User User { get; set; }

        public AuthorizationWindow()
        {
            InitializeComponent();
            User = new User();

            context = new TVAContext();
        }

        private async void AcceptClick(object sender, RoutedEventArgs e)
        {
            var result = await (from user in context.Users
                                where user.Login == LoginTextBox.Text && user.Password == PwdBox.Password
                                select user).ToListAsync();

            User findUser = result.FirstOrDefault(user => user.Login == LoginTextBox.Text && user.Password == PwdBox.Password);

            if (findUser == null)
            {
                MessageBox.Show("Пользователь не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DialogResult = true;
        }
        private void RefuseClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            context.Dispose();
        }
    }
}
