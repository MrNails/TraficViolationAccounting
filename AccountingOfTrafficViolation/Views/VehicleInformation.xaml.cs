using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using AccountingOfTrafficViolation.Services;
using AccountingOfTrafficViolation.Views.UserControls;
using AccountOfTrafficViolationDB.Context;
using AccountOfTrafficViolationDB.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountingOfTrafficViolation.Views;

public partial class VehicleInformation : Window, IDisposable
{
    enum ItemManagementMode
    {
        None,
        Add,
        Edit,
    }

    private TVAContext m_context;

    private ObservableCollection<Vehicle> m_vehicles;

    private LoadView m_loadView;
    
    private ItemManagementMode m_imMode;
    private Vehicle? m_selectedVehicle;

    public VehicleInformation()
    {
        InitializeComponent();

        m_imMode = ItemManagementMode.None;
        m_loadView = new LoadView();
        
        Grid.SetRowSpan(m_loadView,10);
        Grid.SetColumnSpan(m_loadView,10);

        UpdateModeDependencyControls();
    }

    public Vehicle? SelectedVehicle
    {
        get => m_selectedVehicle;
        private set
        {
            m_selectedVehicle = value;
            CurrentVehicleGroupBox.DataContext = null;
            CurrentVehicleGroupBox.DataContext = m_selectedVehicle;
        }
    }

    public void Dispose()
    {
        m_context?.Dispose();
    }
    
    private void UpdateModeDependencyControls()
    {
        EditBtn.IsEnabled = m_imMode == ItemManagementMode.None;
        DiscardBtn.IsEnabled = m_imMode != ItemManagementMode.None;
        
        CurrentVehicleGroupBox.IsEnabled = m_imMode != ItemManagementMode.None;
        VehiclesGrid.IsEnabled = m_imMode == ItemManagementMode.None;
    }

    private void TextBox_Click(object sender, RoutedEventArgs e)
    {
        if (sender is TextBox textBox)
        {
            var codesWindow = new CodesWindow();

            if (codesWindow.ShowDialog() == true)
                textBox.Text = codesWindow.Code;
        }
    }

    private async void AddBtn_OnClick(object sender, RoutedEventArgs e)
    {
        if (m_imMode == ItemManagementMode.None)
        {
            m_imMode = ItemManagementMode.Add;

            SelectedVehicle = new Vehicle();
            CurrentVehicleGroupBox.Header = "Новое транспортное средство";

            AddBtn.ToolTip = "Сохраняет внесённые изменения";
            AddBtn.Content = Resources["CheckMarkPath"];
            
            UpdateModeDependencyControls();
            
            return;
        }
        
        if (m_imMode == ItemManagementMode.Add)
        {
            SelectedVehicle.Id = await m_context.Vehicles.MaxAsync(v => v.Id) + 1;
            m_vehicles.Add(SelectedVehicle);
        }

        SynchronizationContext.Current.Post(obj =>
        {
            CurrentVehicleGroupBox.Header = $"Транспортное средство № {SelectedVehicle.Id}";
        
            AddBtn.Content = Resources["PlusPath"];
            
            m_imMode = ItemManagementMode.None;
            UpdateModeDependencyControls();

            AddBtn.ToolTip = "Добавляет транспортное средство";
        }, null);

        await m_context.SaveChangesAsync();

        SynchronizationContext.Current.Post(obj => MessageBox.Show("Изменения успешно сохранены.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information), null);
    }

    private void EditBtn_OnClick(object sender, RoutedEventArgs e)
    {
        if (m_imMode == ItemManagementMode.None)
        {
            m_imMode = ItemManagementMode.Edit;
            
            AddBtn.Content = Resources["CheckMarkPath"];
            
            UpdateModeDependencyControls();
        }
    }

    private async void DiscardBtn_OnClick(object sender, RoutedEventArgs e)
    {
        if (m_imMode == ItemManagementMode.Edit)
            await m_context.Entry(SelectedVehicle).ReloadAsync();
        else
            SelectedVehicle = null;

        AddBtn.Content = Resources["PlusPath"];
        AddBtn.ToolTip = "Добавляет транспортное средство";
        
        m_imMode = ItemManagementMode.None;

        UpdateModeDependencyControls();
    }

    private void AcceptBtn_OnClick(object sender, RoutedEventArgs e)
    {
        DialogResult = true;
    }

    private void CancelBtn_OnClick(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
    }

    private void VehiclesGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is DataGrid dataGrid)
        {
            SelectedVehicle = (Vehicle)dataGrid.SelectedItem;

            CurrentVehicleGroupBox.Header = $"Транспортное средство № {SelectedVehicle.Id}";
        }
    }
    
    private void VehicleInformation_OnClosing(object? sender, CancelEventArgs e)
    {
        if (m_imMode != ItemManagementMode.None)
        {
            if (CurrentVehicleGroupBox.CheckIfExistValidationError())
            {
                MessageBox.Show("Заполните все необходимые поля!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                e.Cancel = true;
            }
            else
            {
                e.Cancel = MessageBox.Show("Есть не сохранённые изменения. Вы уверены, что хотите продолжить?",
                    "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes;
            }
        }

        if (SelectedVehicle == null && DialogResult == true)
        {
            MessageBox.Show("Выберите транспортное средство!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            e.Cancel = true;
        }
        
        
    }

    private async void VehicleInformation_OnLoaded(object sender, RoutedEventArgs e)
    {
        SynchronizationContext.Current?.Post(obj =>
        {
            m_loadView.Resume();
            MainGrid.Children.Add(m_loadView);
        }, null);

        m_context = await Task.Factory.StartNew(() => new TVAContext(GlobalSettings.ConnectionStrings[Constants.DefaultDB], GlobalSettings.Credential));

        await m_context.Vehicles.LoadAsync();

        m_vehicles = m_context.Vehicles.Local.ToObservableCollection();

        SynchronizationContext.Current?.Post(obj =>
        {
            VehiclesGrid.ItemsSource = m_vehicles;
            
            m_loadView.Pause();
            MainGrid.Children.Remove(m_loadView);
        }, null);
    }
}