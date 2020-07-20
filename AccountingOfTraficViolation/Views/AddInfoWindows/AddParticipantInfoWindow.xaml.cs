using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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
using AccountingOfTraficViolation.Services;
using AccountingOfTraficViolation.ViewModels;

namespace AccountingOfTraficViolation.Views.AddInfoWindows
{
    /// <summary>
    /// Логика взаимодействия для AddParticipantInfoWindow.xaml
    /// </summary>
    public partial class AddParticipantInfoWindow : Window
    {
        public ParticipantInfoVM ParticipantInfoVM { get; private set; }

        public AddParticipantInfoWindow() : this(null)
        { }
        public AddParticipantInfoWindow(ObservableCollection<ParticipantsInformation> participantsInfo)
        {
            ParticipantInfoVM = new ParticipantInfoVM(participantsInfo);

            InitializeComponent();

            DataContext = ParticipantInfoVM;

            ParticipantsListBox.ItemsSource = ParticipantInfoVM.ParticipantsInformations;
            ParticipantInfoGroupBox.Header = "Учавствующий № 1";
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox)
            {
                ComboBox comboBox = (ComboBox)sender;
                if (comboBox.SelectedItem is ComboBoxItem && comboBox.SelectedItem != null && ((ComboBoxItem)comboBox.SelectedItem).Tag != null)
                {
                    if (((ComboBoxItem)comboBox.SelectedItem).Tag.ToString() == "1")
                    {
                        ParticipantInfoVM.CurrentParticipantsInformation.Gender = true;
                    }
                    else
                    {
                        ParticipantInfoVM.CurrentParticipantsInformation.Gender = false;
                    }
                }
            }
        }

        private void PDDViolationTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            string tempStr = textBox.Text;
            int caretIndex = textBox.CaretIndex;
            int oldLength = textBox.Text.Length;

            tempStr = tempStr.GetStrWithoutSeparator(',').AddSeparator(',', 2);

            textBox.Text = tempStr;

            //set caret after string change
            if (caretIndex + (tempStr.Length - oldLength) >= 0)
            {
                textBox.CaretIndex = caretIndex + (tempStr.Length - oldLength);
            }
        }

        private void ParticipantsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ParticipantsListBox.SelectedIndex >= 0)
            {
                ParticipantInfoVM.CurrentIndex = ParticipantsListBox.SelectedIndex;

                GenderComboBox.SelectedIndex = ParticipantInfoVM.CurrentParticipantsInformation.Gender ? 1 : 0;

                ParticipantInfoGroupBox.Header = "Учавствующий № " + (ParticipantInfoVM.CurrentIndex + 1).ToString();
            }
        }
    }
}
