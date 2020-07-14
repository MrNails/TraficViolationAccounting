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

namespace AccountingOfTraficViolation.Views.AddInfoWindows
{
    /// <summary>
    /// Логика взаимодействия для AddRoadConditionWindow.xaml
    /// </summary>
    public partial class AddRoadConditionWindow : Window
    {

        public RoadCondition RoadCondition { get; private set; }

        public AddRoadConditionWindow()
        {
            InitializeComponent();
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
