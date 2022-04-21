﻿using System;
using System.Windows;
using System.Windows.Controls;
using AccountingOfTrafficViolation.Services;

namespace AccountingOfTrafficViolation.Views.UserControls;

public partial class MainPageUC : UserControl
{
    private Action? m_logOutAction;
    private AccountSettingsWindow? m_accountSettingsWindow;
    
    public MainPageUC()
    {
        GlobalSettings.Logger = new FileLogger("Errors.txt");

        InitializeComponent();

        DataContext = GlobalSettings.ActiveOfficer;
    }

    public Action? LogOutAction
    {
        get => m_logOutAction;
        set => m_logOutAction = value ?? throw new ArgumentNullException(nameof(value));
    }
    
    private void InitUser()
    {
        if (GlobalSettings.ActiveOfficer.Role == (byte)UserRole.Debug || GlobalSettings.ActiveOfficer.Role == (byte)UserRole.Admin)
            AdminWindowMenuItem.Visibility = Visibility.Visible;
        else
            AdminWindowMenuItem.Visibility = Visibility.Hidden;

        DataContext = GlobalSettings.ActiveOfficer;
    }

    private void OpenCaseClick(object sender, RoutedEventArgs e)
    {
        if (GlobalSettings.ActiveOfficer.Role != (byte)UserRole.User && GlobalSettings.ActiveOfficer.Role != (byte)UserRole.Debug)
        {
            MessageBox.Show("У вас не хватает привелегий на создание дела.", "Ошибка", MessageBoxButton.OK,
                MessageBoxImage.Error);
            return;
        }

        OpenNewCaseWindow caseWindow = new OpenNewCaseWindow();

        try
        {
            caseWindow.ShowDialog();
        }
        catch (Exception ex)
        {
            CatchError(ex);
        }
    }

    private void ShowCaseClick(object sender, RoutedEventArgs e)
    {
        ShowCaseWindow showCaseWindow = new ShowCaseWindow();
        
        try
        {
            showCaseWindow.ShowDialog();
        }
        catch (Exception ex)
        {
            CatchError(ex);
        }
    }

    private void ExitAccountClick(object sender, RoutedEventArgs e) => m_logOutAction?.Invoke();

    private void AdminWindowMenuItem_Click(object sender, RoutedEventArgs e)
    {
        AdminWindow adminWindow = new AdminWindow();

        try
        {
            adminWindow.ShowDialog();
        }
        catch (Exception ex)
        {
            CatchError(ex);
        }
    }

    private void AccountSettings_Click(object sender, RoutedEventArgs e)
    {
        if (m_accountSettingsWindow == null)
        {
            try
            {
                m_accountSettingsWindow = new AccountSettingsWindow();
                m_accountSettingsWindow.Show();
                m_accountSettingsWindow.Closed += (obj, arg) => { m_accountSettingsWindow = null; };
            }
            catch (Exception ex)
            {
                CatchError(ex);
            }
        }
    }

    private void CatchError(Exception ex)
    {
        string innerExceptionMessage = ex.GetInnerExceptionMessage();
        string exceptionMessage = "Ошибка: ";

        if (string.IsNullOrEmpty(innerExceptionMessage))
            exceptionMessage += ex.Message;
        else
            exceptionMessage += innerExceptionMessage;

        exceptionMessage += "\nСтек трейс:\n" + ex.StackTrace + "\n";

        MessageBox.Show("Возникла ошибка, смотри подробности в файле Errors.txt в папке приложения.", "Ошибка",
            MessageBoxButton.OK, MessageBoxImage.Error);
        GlobalSettings.Logger.ErrorMessage = exceptionMessage;
        GlobalSettings.Logger.Log();
    }
}