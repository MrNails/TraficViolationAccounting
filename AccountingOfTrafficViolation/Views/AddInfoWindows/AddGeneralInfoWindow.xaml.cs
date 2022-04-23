using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using AccountingOfTrafficViolation.Models;
using AccountingOfTrafficViolation.Services;
using AccountOfTrafficViolationDB.Models;

namespace AccountingOfTrafficViolation.Views.AddInfoWindows
{
    /// <summary>
    /// Логика взаимодействия для AddGeneralInfoWindow.xaml
    /// </summary>
    public partial class AddGeneralInfoWindow : Window
    {
        public GeneralInfo GeneralInfo { get; private set; }

        public AddGeneralInfoWindow(GeneralInfo generalInfo)
        {
            InitializeComponent();

            if (generalInfo != null)
            {
                GeneralInfo = generalInfo.Clone();
            }
            else
            {
                GeneralInfo = new GeneralInfo();
                GeneralInfo.FillDate = DateTime.Now;

            }

            var dayOfWeek = (int)GeneralInfo.FillDate.DayOfWeek;
            DayOfWeekComboBox.SelectedIndex = dayOfWeek - 1 < 0 ? 6 : dayOfWeek - 1;

            DataContext = GeneralInfo;
        }

        private void AcceptClick(object sender, RoutedEventArgs e)
        {
            if (MainGrid.CheckIfExistValidationError())
                return;

            // GeneralInfo.DayOfWeek = byte.Parse(((ComboBoxItem)DayOfWeekComboBox.SelectedItem).Content.ToString()[0].ToString());

            DialogResult = true;
        }
        private void RejectClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void FillTimeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
                textBox.SeparatorTemplate(':', 2, 5);
        }

        private void FillDateCalendar_OnSelectedDateChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (sender is DatePicker datePicker && datePicker.SelectedDate.HasValue)
            {
                var dayOfWeek = (int)datePicker.SelectedDate.Value.DayOfWeek;
                
                DayOfWeekComboBox.SelectedIndex = dayOfWeek - 1 < 0 ? 6 : dayOfWeek - 1;
            }
        }
    }
}
