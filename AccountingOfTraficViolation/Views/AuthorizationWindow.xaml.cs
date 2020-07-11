using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
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

namespace AccountingOfTraficViolation.Views
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationWindow.xaml
    /// </summary>
    public partial class AuthorizationWindow : Window
    {
        //private TVAContext context;
        //private object locker;

        public User User { get; set; }

        public AuthorizationWindow()
        {
            InitializeComponent();
            User = new User();
            //locker = new object();
            //context = new TVAContext();
        }

        private async void AcceptClick(object sender, RoutedEventArgs e)
        {
            MainGrid.IsEnabled = false;

            User findUser = await CheckCerdentialsAsync();

            MainGrid.IsEnabled = true;

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
            //context.Dispose();
        }

        private async Task<User> CheckCerdentialsAsync()
        {
            return await Task<User>.Run(() =>
            {
                User _user = null;
                using (TVAContext context = new TVAContext())
                {
                    var res = context.Users
                                     .AsNoTracking()
                                     .Where(user => user.Login == LoginTextBox.Text && user.Password == PwdBox.Password)
                                     .AsEnumerable()
                                     .Where(user => user.Login == LoginTextBox.Text && user.Password == PwdBox.Password);

                    this.Dispatcher.Invoke(() =>
                    {
                        _user = res.FirstOrDefault();
                    });

                }

                return _user;
            });
        }
    }
}
