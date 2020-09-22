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
        public AccidentObjectsVM<ParticipantsInformation> AccidentObjectsVM { get; private set; }

        public ObservableCollection<ParticipantsInformation> ParticipantsInformations => AccidentObjectsVM.AccidentObjects;

        public AddParticipantInfoWindow() : this(null)
        { }
        public AddParticipantInfoWindow(ObservableCollection<ParticipantsInformation> participantsInfo, bool isEditable = true)
        {
            AccidentObjectsVM = new AccidentObjectsVM<ParticipantsInformation>(participantsInfo);

            InitializeComponent();

            DataContext = AccidentObjectsVM;

            ParticipantsListBox.ItemsSource = AccidentObjectsVM.AccidentObjects;

            if (isEditable)
            {
                ParticipantInfoGroupBox.Header = "Учавствующий № 1";
            }
            else
            {
                ParticipantInfoGroupBox.Header = "Учавствующий";
                AddParticipantInfo.IsEnabled = false;
                RemoveParticipantInfo.IsEnabled = false;
            }
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
                        AccidentObjectsVM.CurrentAccidentObject.Gender = true;
                    }
                    else
                    {
                        AccidentObjectsVM.CurrentAccidentObject.Gender = false;
                    }
                }
            }
        }

        private void ParticipantsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ParticipantsListBox.SelectedIndex >= 0)
            {
                AccidentObjectsVM.CurrentIndex = ParticipantsListBox.SelectedIndex;

                GenderComboBox.SelectedIndex = AccidentObjectsVM.CurrentAccidentObject.Gender ? 1 : 0;

                ParticipantInfoGroupBox.Header = "Учавствующий № " + (AccidentObjectsVM.CurrentIndex + 1).ToString();
            }
        }
        
        private void AcceptClick(object sender, RoutedEventArgs e)
        {
            int count = 0;

            foreach (var pInfo in AccidentObjectsVM.AccidentObjects)
            {
                AccidentObjectsVM.CurrentIndex = count;
                if (ParticipantInfoGroupBox.CheckIfExistValidationError())
                {
                    ParticipantsListBox.SelectedIndex = count;
                    return;
                }
                count++;
            }

            DialogResult = true;
        }
        private void RejectClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
