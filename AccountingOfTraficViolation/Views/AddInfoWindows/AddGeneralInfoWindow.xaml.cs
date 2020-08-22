using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using AccountingOfTraficViolation.Models;
using AccountingOfTraficViolation.Services;

namespace AccountingOfTraficViolation.Views.AddInfoWindows
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
                GeneralInfo.DayOfWeek = 1;
            }

            DayOfWeekComboBox.SelectedIndex = GeneralInfo.DayOfWeek - 1;

            DataContext = GeneralInfo;
        }

        private void AcceptClick(object sender, RoutedEventArgs e)
        {
            if (MainGrid.CheckIfExistValidationError())
            {
                return;
            }

            GeneralInfo.DayOfWeek = byte.Parse(((ComboBoxItem)DayOfWeekComboBox.SelectedItem).Content.ToString()[0].ToString());

            DialogResult = true;
        }
        private void RejectClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void CardNumberTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox)
            {
                TextBox textBox = (TextBox)sender;

                textBox.SeparatorTemplate('-', 2, 10);

                textBox.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
            }
        }

        private void FillTimeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox)
            {
                TextBox textBox = (TextBox)sender;

                textBox.SeparatorTemplate(':', 2, 5);
            }
        }
    }
}
