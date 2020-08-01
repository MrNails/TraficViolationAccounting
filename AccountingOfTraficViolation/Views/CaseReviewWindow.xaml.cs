using System;
using System.Collections.Generic;
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

namespace AccountingOfTraficViolation.Views
{
    /// <summary>
    /// Логика взаимодействия для CaseReviewWindow.xaml
    /// </summary>
    public partial class CaseReviewWindow : Window
    {
        public Case Case { get; private set; }

        public CaseReviewWindow(Case @case)
        {
            Case = @case.Clone();
            InitializeComponent();

            if (Case.State == "PROCESSING")
            {
                CaseStatusComboBox.SelectedIndex = 0;
            }
            else
            {
                CaseStatusComboBox.SelectedIndex = 1;
            }

            DataContext = Case;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Case == null)
            {
                MessageBox.Show("Дело не может отсутствовать.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AcceptClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void RejectClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
