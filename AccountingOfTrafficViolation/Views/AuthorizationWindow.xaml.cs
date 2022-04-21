using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using AccountingOfTrafficViolation.Services;
using AccountOfTrafficViolationDB.Context;
using AccountOfTrafficViolationDB.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountingOfTrafficViolation.Views
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationWindow.xaml
    /// </summary>
    public partial class AuthorizationWindow : Window
    {
        public Officer Officer { get; set; }

        public AuthorizationWindow()
        {
            InitializeComponent();
        }

        private async void AcceptClick(object sender, RoutedEventArgs e)
        {
//             try
//             {
//                 LoadScreen.Visibility = Visibility.Visible;
//
// #if DEBUG
//                 Officer = new Officer { Name = "Debug", Surname = "Debug", Role = (byte)UserRole.Debug };
// #else
//                 User = await CheckCerdentialsAsync();
// #endif
//
//                 LoadScreen.Visibility = Visibility.Collapsed;
//
//                 if (Officer == null)
//                 {
//                     MessageBox.Show("Пользователь не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
//                     return;
//                 }
//
//                 DialogResult = true;
//             }
//             catch (Exception ex)
//             {
//                 MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
//             }
        }
        private void RefuseClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private Task<Officer> CheckCerdentialsAsync()
        {
            return Task.FromResult<Officer>(null);
            // return await Task<Officer>.Run(() =>
            // {
            //     Officer officer = null;
            //     using (TVAContext context = new TVAContext(GlobalSettings.ConnectionStrings[Constants.DefaultDB]))
            //     {
            //         var res = context.Officers
            //                          .AsNoTracking()
            //                          .Where(user => user.Login == LoginTextBox.Text && user.Password == PwdBox.Password)
            //                          .AsEnumerable()
            //                          .Where(user => user.Login == LoginTextBox.Text && user.Password == PwdBox.Password);
            //
            //         this.Dispatcher.Invoke(() =>
            //         {
            //             officer = res.FirstOrDefault();
            //         });
            //
            //     }
            //
            //     return officer;
            // });
        }
    }
}
