using System;
using System.Data;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using AccountingOfTrafficViolation.Services;
using AccountOfTrafficViolationDB.Context;
using AccountOfTrafficViolationDB.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace AccountingOfTrafficViolation.Views.UserControls;

public partial class AuthorizationUC : UserControl
{
    private SqlConnection m_connection;

    public AuthorizationUC()
    {
        InitializeComponent();

        m_connection = new SqlConnection(GlobalSettings.ConnectionStrings[Constants.DefaultDB]);
    }

    public Action<Officer, Credential>? AcceptAction { get; set; }
    public Action? CancelAction { get; set; }

    private async void AcceptClick(object sender, RoutedEventArgs e)
    {
        try
        {
            LoadScreen.Visibility = Visibility.Visible;
            Officer? currentOfficer = null;
            
            currentOfficer = await CheckCredentialsAsync(LoginTextBox.Text, PwdBox.SecurePassword);
            
            LoadScreen.Visibility = Visibility.Collapsed;

            if (currentOfficer == null)
                return;
            
            var pwd = PwdBox.SecurePassword;
            pwd.MakeReadOnly();

            AcceptAction?.Invoke(currentOfficer, new Credential(LoginTextBox.Text, PwdBox.Password));
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            LoadScreen.Visibility = Visibility.Collapsed;
        }
    }

    private void RefuseClick(object sender, RoutedEventArgs e) => CancelAction?.Invoke();

    private async Task<Officer?> CheckCredentialsAsync(string login, SecureString password)
    {
        if (m_connection.State == ConnectionState.Broken)
            throw new Exception("Cannot connect to database");
        
        password.MakeReadOnly();
        m_connection.Credential = new SqlCredential(login, password);

        try
        {
            if (m_connection.State == ConnectionState.Closed)
                await m_connection.OpenAsync();

            return (await m_connection.QueryAsync<Officer?>("SELECT OfficerId as Id, Name, Surname, Phone FROM Officers (nolock) WHERE OfficerId = @Login",
                    new { Login = login }))
                .FirstOrDefault();
        }
        catch (SqlException e) when (e.Number == 18456)
        {
            MessageBox.Show("Не правильный логин или пароль. Попробуйте снова.", "Ошибка",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
        catch (Exception e)
        {
            GlobalSettings.Logger.Log(e.Message + Environment.NewLine + e.StackTrace);
            MessageBox.Show("Возникла непредвиденная ошибка.", "Ошибка",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            await m_connection.CloseAsync();
        }
        
        return null;
    }
}