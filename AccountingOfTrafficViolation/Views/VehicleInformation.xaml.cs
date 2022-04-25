using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using AccountOfTrafficViolationDB.Models;

namespace AccountingOfTrafficViolation.Views;

public partial class VehicleInformation : Window
{
    enum ItemManagementMode
    {
        None,
        Add,
        Edit,
    }

    private ItemManagementMode m_imMode;

    public VehicleInformation()
    {
        InitializeComponent();

        m_imMode = ItemManagementMode.None;
    }

    public Vehicle? SelectedVehicle { get; private set; }

    private void TextBox_Click(object sender, RoutedEventArgs e)
    {
        if (sender is TextBox textBox)
        {
            var codesWindow = new CodesWindow();

            if (codesWindow.ShowDialog() == true)
                textBox.Text = codesWindow.Code;
        }
    }

    private void AddBtn_OnClick(object sender, RoutedEventArgs e)
    {
        if (m_imMode == ItemManagementMode.None)
        {
            m_imMode = ItemManagementMode.Add;
        }
    }

    private void EditBtn_OnClick(object sender, RoutedEventArgs e)
    {
    }

    private void DiscardBtn_OnClick(object sender, RoutedEventArgs e)
    {
    }

    private void AcceptBtn_OnClick(object sender, RoutedEventArgs e)
    {
        DialogResult = true;
    }

    private void CancelBtn_OnClick(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
    }

    private void VehicleInformation_OnClosing(object? sender, CancelEventArgs e)
    {
        if (m_imMode != ItemManagementMode.None)
        {
            var askResult = MessageBox.Show("Есть не сохранённые изменения. Вы уверены, что хотите продолжить?",
                "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question);
            
            e.Cancel = askResult != MessageBoxResult.Yes;
        }
    }
}