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

        private void TemplateTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            int[] indexes = null;

            switch (textBox.Name)
            {
                case "TechnicalToolTextBox":
                    indexes = new int[] { 2, 5, 8, 11 };
                    break;
                case "RoadDisadvantagesTextBox":
                    indexes = new int[] { 2, 5, 8, 11 };
                    break;
                case "PlaceElementTextBox":
                    indexes = new int[] { 2, 5 };
                    break;
                case "SurfaceStateTextBox":
                    indexes = new int[] { 1 };
                    break;
                default:
                    return;
            }

            textBox.SeparatorTemplate(',', indexes);
        }

    }
}
