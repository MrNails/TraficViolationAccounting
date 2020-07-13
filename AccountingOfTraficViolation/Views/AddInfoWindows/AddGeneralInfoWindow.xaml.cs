using System;
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

        public GeneralInfo GeneralInfo { get; set; }

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

            GeneralInfo.ErrorInput += msg =>
            {
                MessageBox.Show(msg, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            };

            DayOfWeekComboBox.SelectedIndex = GeneralInfo.DayOfWeek - 1;
            CardNumberTextBox.Text = GeneralInfo.CardNumber;

            DataContext = GeneralInfo;
            isCardNumberValid = false;
        }

        private void AcceptClick(object sender, RoutedEventArgs e)
        {
            if (Validation.GetHasError(IncidentTypeTextBox) || Validation.GetHasError(CardTypeTextBox)
                || Validation.GetHasError(FillTimeTextBox) || !isCardNumberValid)
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


                //delete all '-'
                for (int i = 0; i < tempStr.Length; i++)
                {
                    if (tempStr[i] == '-')
                    {
                        tempStr = tempStr.Remove(i, 1);
                        i--;
                    }
                }

                if (regex.IsMatch(tempStr))
                {
                    GeneralInfo.CardNumber = tempStr;

                    if (CardNumberBorder.ToolTip != null)
                    {
                        ((ToolTip)CardNumberBorder.ToolTip).IsOpen = false;
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
                        CardNumberBorder.ToolTip = new ToolTip
                        {
                            Content = "Строка не соответствует шаблону:\n\t00-0000000-0*\n\n* - не обязательный элемент",
                            FontSize = 13,
                            PlacementTarget = CardNumberBorder,
                            Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom
                        };

                        ((ToolTip)CardNumberBorder.ToolTip).IsOpen = true;

                        textBox.Foreground = new SolidColorBrush(Colors.Red);

                        isCardNumberValid = false;
                    }
                }


                //check string length to input '-' on index=2 and, if last symbol exist, on index=10 
                if (tempStr.Length > 2)
                {
                    tempStr = tempStr.Insert(2, "-");


                    if (tempStr.Length > 10)
                    {
                        tempStr = tempStr.Insert(10, "-");

                    }
                }

                textBox.Text = tempStr; 

                //set caret after string change
                if (caretIndex == 3)
                {
                    textBox.CaretIndex = 4;
                }
                else if (caretIndex == 11)
                {
                    textBox.CaretIndex = 12;
                }
                else if (tempStr.Length < oldLength)
                {
                    textBox.CaretIndex = caretIndex - 1;
                }
                else
                {
                    textBox.CaretIndex = caretIndex;
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
           if (CardNumberBorder.ToolTip != null)
            {
                ((ToolTip)CardNumberBorder.ToolTip).IsOpen = false;
            }
        }
    }
}
