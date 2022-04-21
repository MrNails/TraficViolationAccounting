#nullable enable
using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using AccountingOfTrafficViolation;
using AccountingOfTrafficViolation.Services;
using AccountOfTrafficViolationDB.Models;
using AccountOfTrafficViolationDB.ProxyModels;
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

    public Action<UserInfo>? AcceptAction { get; set; }
    public Action? CancelAction { get; set; }

    private async void AcceptClick(object sender, RoutedEventArgs e)
    {
        try
        {
            LoadScreen.Visibility = Visibility.Visible;
            UserInfo? currentOfficer = null;
#if DEBUG
            currentOfficer = new UserInfo { Name = "Debug", Surname = "Debug", Role = (byte)UserRole.Debug };
#else
            currentOfficer = await CheckCerdentialsAsync();
#endif

            LoadScreen.Visibility = Visibility.Collapsed;

            if (currentOfficer == null)
            {
                MessageBox.Show("Пользователь не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            AcceptAction?.Invoke(currentOfficer);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void RefuseClick(object sender, RoutedEventArgs e) => CancelAction?.Invoke();

    private Task<UserInfo?> CheckCerdentialsAsync(string login, string password)
    {
        if (m_connection.State == ConnectionState.Broken)
        {
            return Task.FromException<UserInfo?>(new Exception("Cannot connect to database"));
        }

        if (m_connection.State == ConnectionState.Closed)
            m_connection.OpenAsync();

        var _pars = new
        {
            Login = login,
            Password = password
        };

        return m_connection.QueryFirstAsync<UserInfo?>(@"
DECLARE @officerId int,
        @role tinyint;

EXEC AccountOfTrafficViolation.dbo.AuthorizeUser @Login, @User, @officerId out, @role out

SELECT OfficerId, @role as Role, Name, Surname, Phone  
FROM AccountOfTrafficViolation.dbo.Officers (nolock)
WHERE OfficerId = @officerId
", _pars);
    }
}