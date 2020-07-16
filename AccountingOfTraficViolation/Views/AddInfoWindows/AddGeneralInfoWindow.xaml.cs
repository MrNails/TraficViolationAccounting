﻿using System;
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
        private bool isCardNumberValid;

        public GeneralInfo GeneralInfo { get; private set; }

        public AddGeneralInfoWindow(GeneralInfo generalInfo)
        {
            InitializeComponent();

            if (generalInfo != null)
            {
                GeneralInfo = generalInfo.Clone();
                isCardNumberValid = true;
            }
            else
            {
                GeneralInfo = new GeneralInfo();
                GeneralInfo.DayOfWeek = 1;
                isCardNumberValid = false;
            }

            GeneralInfo.ErrorInput += msg =>
            {
                MessageBox.Show(msg, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            };

            DayOfWeekComboBox.SelectedIndex = GeneralInfo.DayOfWeek - 1;
            CardNumberTextBox.Text = GeneralInfo.CardNumber;

            DataContext = GeneralInfo;
        }

        private void AcceptClick(object sender, RoutedEventArgs e)
        {
            if (!isCardNumberValid)
            {
                CardNumberBorder.BorderBrush = new SolidColorBrush(Colors.Red);
                CardNumberBorder.ToolTip = "Строка не соответствует шаблону:\n\t00-0000000-0*\n\n* - не обязательный элемент.";
                return;
            }

            if (Validation.GetHasError(IncidentTypeTextBox) || Validation.GetHasError(CardTypeTextBox)
                || Validation.GetHasError(FillTimeTextBox))
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
                Regex regex = new Regex(@"\d{9}[0-9]?$");

                TextBox textBox = (TextBox)sender;

                string tempStr = textBox.Text;
                int caretIndex = textBox.CaretIndex;
                int oldLength = textBox.Text.Length;

                tempStr = tempStr.GetStrWithoutSeparator('-');

                if (regex.IsMatch(tempStr))
                {
                    GeneralInfo.CardNumber = tempStr;

                    if (CardNumberBorder.ToolTip != null)
                    {
                        textBox.Foreground = new SolidColorBrush(Colors.Black);
                    }

                    CardNumberBorder.BorderBrush = null;
                    CardNumberBorder.ToolTip = null;
                    isCardNumberValid = true;
                }
                else
                {
                    if (CardNumberBorder.BorderBrush == null)
                    {
                        CardNumberBorder.BorderBrush = new SolidColorBrush(Colors.Red);
                        CardNumberBorder.ToolTip = "Строка не соответствует шаблону:\n\t00-0000000-0*\n\n* - не обязательный элемент.";

                        textBox.Foreground = new SolidColorBrush(Colors.Red);

                        isCardNumberValid = false;
                    }
                }

                tempStr = tempStr.AddSeparator('-', 2, 10);

                textBox.Text = tempStr;
                textBox.CaretIndex = caretIndex;

                //set caret after string change
                if (caretIndex + (tempStr.Length - oldLength) >= 0)
                {
                    textBox.CaretIndex = caretIndex + (tempStr.Length - oldLength);
                }
            }
        }
    }
}
