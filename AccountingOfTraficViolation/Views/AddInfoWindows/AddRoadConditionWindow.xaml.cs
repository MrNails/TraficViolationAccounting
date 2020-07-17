﻿using System;
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
        private RoadConditionVM RoadConditionVM;

        public RoadCondition RoadCondition { get; private set; }

        public AddRoadConditionWindow() : this(null)
        { }
        public AddRoadConditionWindow(RoadCondition roadCondition)
        {
            InitializeComponent();

            RoadConditionVM = new RoadConditionVM(roadCondition);

            if (roadCondition != null)
            {
                RoadCondition = roadCondition.Clone();
            }
            else
            {
                RoadCondition = new RoadCondition();
            }

            DataContext = RoadConditionVM;
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

            RoadCondition = RoadConditionVM.RoadCondition;

            DialogResult = true;
        }
        private void RejectClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void TemplateTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            string tempStr = textBox.Text;
            int caretIndex = textBox.CaretIndex;
            int oldLength = textBox.Text.Length;
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

            tempStr = tempStr.GetStrWithoutSeparator(',').AddSeparator(',', indexes);

            textBox.Text = tempStr;

            //set caret after string change
            if (caretIndex + (tempStr.Length - oldLength) >= 0)
            {
                textBox.CaretIndex = caretIndex + (tempStr.Length - oldLength);
            }
        }

    }
}
