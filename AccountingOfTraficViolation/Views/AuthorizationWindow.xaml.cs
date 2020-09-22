

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
        public User User { get; set; }

        public AuthorizationWindow()
        {
            InitializeComponent();
            User = new User();
        }

        private async void AcceptClick(object sender, RoutedEventArgs e)
        {
            try
            {
                MainGrid.IsEnabled = false;

                User = await CheckCerdentialsAsync();

                MainGrid.IsEnabled = true;

                if (User == null)
                {
                    MessageBox.Show("Пользователь не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                MainGrid.IsEnabled = true;
            }
        }
        private void RefuseClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
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
