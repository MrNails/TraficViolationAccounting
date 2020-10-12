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
using System.Text.RegularExpressions;
using AccountingOfTraficViolation.Models;
using AccountingOfTraficViolation.Services;
using AccountingOfTraficViolation.ViewModels;

namespace AccountingOfTraficViolation.Views.AddInfoWindows
{
    /// <summary>
    /// Логика взаимодействия для AddRoadConditionWindow.xaml
    /// </summary>
    public partial class AddRoadConditionWindow : Window
    {
        public RoadCondition RoadCondition { get; private set; }

        public AddRoadConditionWindow() : this(null)
        { }
        public AddRoadConditionWindow(RoadCondition roadCondition)
        {
            InitializeComponent();

            if (roadCondition != null)
            {
                RoadCondition = roadCondition.Clone();
            }
            else
            {
                RoadCondition = new RoadCondition();
            }

            DataContext = RoadCondition;
        }
        private void AcceptClick(object sender, RoutedEventArgs e)
        {
            if (sender is UIElement)
            {
                ((UIElement)sender).Focus();
            }

            if (MainGrid.CheckIfExistValidationError())
            {
                return;
            }

            DialogResult = true;
        }
        private void RejectClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void TextBox_Click(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox)
            {
                TextBox textBox = (TextBox)sender;

                CodesWindow codesWindow = new CodesWindow();

                if (codesWindow.ShowDialog() == true)
                {
                    textBox.Text = codesWindow.Code;
                }
            }
        }
    }
}
