using System;
using System.Windows;
using AccountingOfTrafficViolation.Services;
using AccountingOfTrafficViolation.Views.UserControls;

namespace AccountingOfTrafficViolation
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainPageUC m_mainPage;
        private AuthorizationUC m_authorizationPage;

        public MainWindow()
        {
            GlobalSettings.Logger = new FileLogger("Errors.txt");

            m_mainPage = new MainPageUC();
            m_authorizationPage = new AuthorizationUC();

            m_mainPage.LogOutAction += SetAuthorizationPage;

            m_authorizationPage.AcceptAction += u =>
            {
                GlobalSettings.ActiveOfficer = u;
                SetMainPage();
            };

            m_authorizationPage.CancelAction += Close;
            
            InitializeComponent();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            SetAuthorizationPage();
        }

        private void SetAuthorizationPage()
        {
            Width = m_authorizationPage.Width;
            Height = m_authorizationPage.Height;

            MainGrid.Children.Clear();
            MainGrid.Children.Add(m_authorizationPage);
        }
        
        private void SetMainPage()
        {
            Width = m_mainPage.Width;
            Height = m_mainPage.Height;

            MainGrid.Children.Clear();
            MainGrid.Children.Add(m_mainPage);
        }
    }
}
