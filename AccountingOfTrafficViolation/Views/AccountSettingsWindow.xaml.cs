using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using AccountingOfTrafficViolation.Models;
using AccountingOfTrafficViolation.Services;
using AccountingOfTrafficViolation.Views.UserControls;
using AccountOfTrafficViolationDB.Context;
using AccountOfTrafficViolationDB.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountingOfTrafficViolation.Views
{
    /// <summary>
    /// Логика взаимодействия для AccountSettingsWindow.xaml
    /// </summary>
    public partial class AccountSettingsWindow : Window
    {
        private bool m_isChanged;
        private Officer m_officer;
        private LoadView m_loadView;

        public AccountSettingsWindow()
        {
            InitializeComponent();

            m_isChanged = false;

            m_officer = GlobalSettings.ActiveOfficer.ToOfficer();
            DataContext = m_officer;
            m_loadView = new LoadView();
        }

        // private async void LoadContext(Action<Exception> action = null)
        // {
        //     LoadView loadView = new LoadView();
        //     try
        //     {
        //         Grid.SetRowSpan(loadView, 3);
        //         MainGrid.Children.Add(loadView);
        //
        //         TVAContext = await Task.Run<TVAContext>(() =>
        //         {
        //             TVAContext dbContext = new TVAContext(GlobalSettings.ConnectionStrings[Constants.DefaultDB]);
        //             return dbContext;
        //         });
        //
        //         TVAContext.Officers.Attach(officer);
        //     }
        //     catch (Exception ex) when (action != null)
        //     {
        //         action(ex);
        //     }
        //     finally
        //     {
        //         MainGrid.Children.Remove(loadView);
        //     }
        // }

        private async void SaveChangeClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                if (button.Tag == null)
                {
                    return;
                }

                Grid.SetRowSpan(m_loadView, 3);
                MainGrid.Children.Add(m_loadView);
                
                switch (button.Tag.ToString())
                {
                    case "1":
                        SavePersonalDataButton.IsEnabled = false;
                        DiscardPersonalDataButton.IsEnabled = false;
                        m_isChanged = false;
                        break;
                    case "2":
                        if (string.IsNullOrEmpty(FirstPasswordTextBox.Password) ||
                            string.IsNullOrEmpty(SecondPasswordTextBox.Password))
                        {
                            MessageBox.Show("Не все поля с паролем заполнены.", "Ошибка", MessageBoxButton.OK,
                                MessageBoxImage.Error);
                            return;
                        }
                        else if (FirstPasswordTextBox.Password != SecondPasswordTextBox.Password)
                        {
                            MessageBox.Show("Пароли не совпадают. Попробуйте ещё раз.", "Ошибка", MessageBoxButton.OK,
                                MessageBoxImage.Error);
                            return;
                        }

                        if (MessageBox.Show("Вы уверены, что хотите изменить пароль?", "Внимание",
                                MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                        {
                            return;
                        }

                        var saltBytes = RandomNumberGenerator.GetBytes(GlobalSettings.SaltSize);
                        var pwd = CryptoHelper.EncryptData(Encoding.UTF8.GetBytes(FirstPasswordTextBox.Password), saltBytes);

                        try
                        {
                            await GlobalSettings.GlobalContext.Database.ExecuteSqlRawAsync("EXEC AccountOfTrafficViolation.dbo.UpdateOfficerPwd @officerId, @password, @salt", 
                                new object[] {m_officer.Id, Encoding.UTF8.GetString(pwd), Encoding.UTF8.GetString(saltBytes)});
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Произошла ошибка при сохранении пароля.\n" + ex.Message, "Ошибка", MessageBoxButton.OK,
                                MessageBoxImage.Error);
                        }
                        break;
                    default:
                        break;
                }

                var officer = await GlobalSettings.GlobalContext.Officers.FirstAsync(o => o.Id == m_officer.Id);
                officer.Name = m_officer.Name;
                officer.Surname = m_officer.Surname;
                officer.Phone = m_officer.Phone;

                try
                {
                    await GlobalSettings.GlobalContext.SaveChangesAsync();
                    MessageBox.Show("Данные успешно изменены.", "Внимание", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Произошла ошибка при сохранении данных.\n" + ex.Message, "Ошибка",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
                finally
                {
                    MainGrid.Children.Remove(m_loadView);
                }
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
                        m_isChanged = false;

                        m_officer.Name = GlobalSettings.ActiveOfficer.Name;
                        m_officer.Surname = GlobalSettings.ActiveOfficer.Surname;
                        m_officer.Phone = GlobalSettings.ActiveOfficer.Phone;
                        break;
                    default:
                        break;
                }
            }
        }

        private void ClearPwdClick(object sender, RoutedEventArgs e)
        {
            FirstPasswordTextBox.Password = string.Empty;
            SecondPasswordTextBox.Password = string.Empty;
        }

        private void PersonalTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            m_isChanged = true;
            SavePersonalDataButton.IsEnabled = true;
            DiscardPersonalDataButton.IsEnabled = true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (m_isChanged)
                if (MessageBox.Show("Изменение не сохранены, продолжить?", "Внимание", MessageBoxButton.YesNo,
                        MessageBoxImage.Question) != MessageBoxResult.Yes)
                    e.Cancel = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            m_isChanged = false;
            SavePersonalDataButton.IsEnabled = false;
            DiscardPersonalDataButton.IsEnabled = false;
        }
    }
}