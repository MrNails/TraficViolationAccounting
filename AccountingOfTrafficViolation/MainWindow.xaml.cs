using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using AccountingOfTrafficViolation.Services;
using AccountingOfTrafficViolation.Views.UserControls;
using AccountOfTrafficViolationDB.Context;

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

            m_authorizationPage.AcceptAction += async (u, credential) =>
            {
                GlobalSettings.ActiveOfficer = u;
                
                if (GlobalSettings.GlobalContext != null)
                    await GlobalSettings.GlobalContext.DisposeAsync();
        
                GlobalSettings.GlobalContext = new TVAContext(GlobalSettings.ConnectionStrings[Constants.DefaultDB], credential);
                
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
            Height = m_authorizationPage.Height + 50;

            ResizeMode = ResizeMode.NoResize;
            
            MinWidth = 0;
            MinHeight = 0;

            MainContainer.Children.Clear();
            MainContainer.Children.Add(m_authorizationPage);
        }
        
        private void SetMainPage()
        {
            Width = m_mainPage.Width;
            Height = m_mainPage.Height + 50;

            m_mainPage.ChangeCurrentUser(GlobalSettings.ActiveOfficer);

            ResizeMode = ResizeMode.CanResize;

            MinWidth = m_mainPage.MinWidth;
            MinHeight = m_mainPage.MinHeight + 50;
            
            MainContainer.Children.Clear();
            MainContainer.Children.Add(m_mainPage);
        }

        private void MainWindow_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (MainContainer.Children.Count > 0 && MainContainer.Children[0] == m_mainPage)
            {
                m_mainPage.Width = Width;
                m_mainPage.Height = Height - 50;
            }
        }
    }
}
