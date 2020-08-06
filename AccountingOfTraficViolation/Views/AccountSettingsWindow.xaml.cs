using System;
using System.Windows;
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
        private bool isChanged;

        public AccountSettingsWindow(User user)
        {
            InitializeComponent();
            TVAContext = new TVAContext();
            this.user = user;
            isChanged = false;
            DataContext = user;
        }

        private void SaveChangeClick(object sender, RoutedEventArgs e)
        {
            TVAContext.SaveChanges();
        }
        private void CancelClick(object sender, RoutedEventArgs e)
        {
            TVAContext.CancelAllChanges();
        }

        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            isChanged = true;
            SaveChangeButton.IsEnabled = true;
            CancelButton.IsEnabled = true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (isChanged)
            {
                if (MessageBox.Show("Изменение не сохранены, продолжить?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No){
                    return;
                }
                TVAContext.CancelAllChanges();
            }

            TVAContext.Dispose();
        }


    }
}
