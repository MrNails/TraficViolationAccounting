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
    /// Логика взаимодействия для AddVictimsWindow.xaml
    /// </summary>
    public partial class AddVictimsWindow : Window
    {
        public AccidentObjectsVM<Victim> AccidentObjectsVM { get; private set; }

        public ObservableCollection<Victim> Victims => AccidentObjectsVM.AccidentObjects;

        public AddVictimsWindow() : this(null)
        {  }
        public AddVictimsWindow(ObservableCollection<Victim> victims)
        {
            InitializeComponent();

            AccidentObjectsVM = new AccidentObjectsVM<Victim>(victims);

            VictimsListBox.ItemsSource = Victims;

            DataContext = AccidentObjectsVM;

            VictimsGroupBox.Header = "Пострадавший № 1";
        }

        private void GenderComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
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

        private void PDDViolationTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            textBox.SeparatorTemplate(',', 2);
        }

        private void VictimsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (VictimsListBox.SelectedIndex >= 0)
            {
                AccidentObjectsVM.CurrentIndex = VictimsListBox.SelectedIndex;

                GenderComboBox.SelectedIndex = AccidentObjectsVM.CurrentAccidentObject.Gender ? 1 : 0;

                VictimsGroupBox.Header = "Пострадавший № " + (AccidentObjectsVM.CurrentIndex + 1).ToString();
            }
        }

        private void AcceptClick(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < AccidentObjectsVM.AccidentObjects.Count; i++)
            {
                AccidentObjectsVM.CurrentIndex = i;
                if (VictimsGroupBox.CheckIfExistValidationError())
                {
                    VictimsListBox.SelectedIndex = i;
                    return;
                }
            }

            DialogResult = true;
        }
        private void RejectClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
