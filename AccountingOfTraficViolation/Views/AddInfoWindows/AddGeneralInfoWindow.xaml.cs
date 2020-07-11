using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using AccountingOfTraficViolation.Models;

namespace AccountingOfTraficViolation.Views.AddInfoWindows
{
    /// <summary>
    /// Логика взаимодействия для AddGeneralInfoWindow.xaml
    /// </summary>
    public partial class AddGeneralInfoWindow : Window
    {
        public GeneralInfo GeneralInfo { get; set; }
        public AddGeneralInfoWindow(GeneralInfo generalInfo)
        {
            InitializeComponent();
            
            if (generalInfo != null)
            {
                GeneralInfo = (GeneralInfo)generalInfo.Clone();
            }
            else
            {
                GeneralInfo = new GeneralInfo();
            }

            DataContext = GeneralInfo;
            
        }

        private void AcceptClick(object sender, RoutedEventArgs e)
        {

        }
        private void RejectClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void CardNumberTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox)
            {
                Regex regex = new Regex(@"\d{2}-\d{7}(-[0-9])?$");

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

                if (regex.IsMatch(tempStr))
                {

                }
            }
        }
    }
}
