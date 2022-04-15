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
        private readonly Officer officer;

        public Case Case { get; private set; }

        public CaseReviewWindow(Case @case, Officer officer)
        {
            this.officer = officer;

            Case = @case.Clone();
            InitializeComponent();

            if (Case.State == "CLOSE" || Case.CreaterLogin != officer.Login)
            {
                CloseCaseButton.IsEnabled = false;
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

        private void CloseCaseClick(object sender, RoutedEventArgs e)
        {
            Case.State = "CLOSE";
            CloseCaseButton.IsEnabled = false;
        }

        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            Calendar calendar = (Calendar)sender;

            calendar.SelectedDate = Case.OpenAt;
        }
    }
}
