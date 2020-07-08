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
using AccountingOfTraficViolation.Views;

namespace AccountingOfTraficViolation
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Models.User user;
        public MainWindow()
        {
            InitializeComponent();
            //if (!string.IsNullOrEmpty(user.Name) && !string.IsNullOrEmpty(user.Surname))
            //{
            //    WelcomeTextBlock.Text = $"Добро пожаловать, {user.Name} {user.Surname}";
            //} 
            //else
            //{
            //    WelcomeTextBlock.Text = "Добро пожаловать";
            //}
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            AuthorizationWindow logInWindow = new AuthorizationWindow();
            if (logInWindow.ShowDialog() != true)
            {
                this.Close();
            }

            user = logInWindow.User;
        }

        private void OpenCaseClick(object sender, RoutedEventArgs e)
        {
            OpenNewCaseWindow caseWindow = new OpenNewCaseWindow();
            caseWindow.ShowDialog();
        }
        private void ShowCaseClick(object sender, RoutedEventArgs e)
        {
            //ShowCaseWindow showCaseWindow = new ShowCaseWindow();
            //showCaseWindow.ShowDialog();
        }
    }
}
