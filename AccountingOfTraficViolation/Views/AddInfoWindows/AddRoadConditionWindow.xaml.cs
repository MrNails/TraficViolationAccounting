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

namespace AccountingOfTraficViolation.Views.AddInfoWindows
{
    /// <summary>
    /// Логика взаимодействия для AddRoadConditionWindow.xaml
    /// </summary>
    public partial class AddRoadConditionWindow : Window
    {
        private bool isTechnicalToolValid;
        private bool isRoadDisadvantagesValid;
        private bool isPlaceElementValid;
        private bool isSurfaceStateValid;

        public RoadCondition RoadCondition { get; private set; }

        public AddRoadConditionWindow() : this(null)
        {  }
        public AddRoadConditionWindow(RoadCondition roadCondition)
        {
            InitializeComponent();

            if (roadCondition != null)
            {
                RoadCondition = roadCondition.Clone();

                RoadDisadvantagesTextBox.Text = RoadCondition.RoadDisadvantages;
                TechnicalToolTextBox.Text = RoadCondition.TechnicalTool;
                PlaceElementTextBox.Text = RoadCondition.PlaceElement;
                SurfaceStateTextBox.Text = RoadCondition.SurfaceState;

                isTechnicalToolValid = true;
                isRoadDisadvantagesValid = true;
                isPlaceElementValid = true;
                isSurfaceStateValid = true;
            } 
            else
            {
                RoadCondition = new RoadCondition();
                isTechnicalToolValid = false;
                isRoadDisadvantagesValid = false;
                isPlaceElementValid = false;
                isSurfaceStateValid = false;
            }

            DataContext = RoadCondition;
        }
        private void AcceptClick(object sender, RoutedEventArgs e)
        {
            if (Validation.GetHasError(SurfaceTypeTextBox) || Validation.GetHasError(IlluminationTextBox)
                || Validation.GetHasError(ArtificialConstructionsTextBox) || Validation.GetHasError(EngineeringTranpsortEquipmentTextBox)
                || Validation.GetHasError(WeatherConditionTextBox) || Validation.GetHasError(IncidentPlaceTextBox))
            {
                return;
            }

            bool isValid = true;

            if (!isTechnicalToolValid)
            {
                TechnicalToolBorder.BorderBrush = new SolidColorBrush(Colors.Red);
                TechnicalToolBorder.ToolTip = "Строка не соответствует шаблону:\n\t00-0000000-0*\n\n* - не обязательный элемент.";
                isValid = false;
            }

            if (!isRoadDisadvantagesValid)
            {
                RoadDisadvantagesBorder.BorderBrush = new SolidColorBrush(Colors.Red);
                RoadDisadvantagesBorder.ToolTip = "Строка не соответствует шаблону:\n\t00-0000000-0*\n\n* - не обязательный элемент.";
                isValid = false;
            }

            if (!isPlaceElementValid)
            {
                PlaceElementBorder.BorderBrush = new SolidColorBrush(Colors.Red);
                PlaceElementBorder.ToolTip = "Строка не соответствует шаблону:\n\t00-0000000-0*\n\n* - не обязательный элемент.";
                isValid = false;
            }

            if (!isSurfaceStateValid)
            {
                SurfaceStateBorder.BorderBrush = new SolidColorBrush(Colors.Red);
                SurfaceStateBorder.ToolTip = "Строка не соответствует шаблону:\n\t00-0000000-0*\n\n* - не обязательный элемент.";
                isValid = false;
            }

            if (isValid)
            {
                DialogResult = true;
            }
        }
        private void RejectClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void TemplateTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Regex regex = null;
            Action<string, bool> setRoadCondProp = null;

            TextBox textBox = (TextBox)sender;
            Border border = null;

            string tempStr = textBox.Text;
            string toolTipText = null;
            int caretIndex = textBox.CaretIndex;
            int oldLength = textBox.Text.Length;
            int[] indexes = null;

            switch (textBox.Name)
            {
                case "TechnicalToolTextBox":
                    border = TechnicalToolBorder;
                    indexes = new int[] { 2, 5, 8, 11 };

                    toolTipText = "Строка не соответствует шаблону:\n\t00,00,00,00,00";

                    regex = new Regex(@"\d{10}");

                    setRoadCondProp = (str, validationResult) =>
                    {
                        if (validationResult)
                        {
                            RoadCondition.TechnicalTool = str;
                        }
                        isTechnicalToolValid = validationResult;
                    };
                    break;
                case "RoadDisadvantagesTextBox":
                    border = RoadDisadvantagesBorder;
                    indexes = new int[] { 2, 5, 8, 11 };

                    toolTipText = "Строка не соответствует шаблону:\n\t00,00,00,00,00";

                    regex = new Regex(@"\d{10}");

                    setRoadCondProp = (str, validationResult) =>
                    {
                        if (validationResult)
                        {
                            RoadCondition.RoadDisadvantages = str;
                        }
                        isRoadDisadvantagesValid = validationResult;
                    };
                    break;
                case "PlaceElementTextBox":
                    border = PlaceElementBorder;
                    indexes = new int[] { 2, 5 };

                    toolTipText = "Строка не соответствует шаблону:\n\t00,00,00";

                    regex = new Regex(@"\d{6}");

                    setRoadCondProp = (str, validationResult) =>
                    {
                        if (validationResult)
                        {
                            RoadCondition.PlaceElement = str;
                        }
                        isPlaceElementValid = validationResult;
                    };
                    break;
                case "SurfaceStateTextBox": 
                    border = SurfaceStateBorder;
                    indexes = new int[] { 1 };

                    toolTipText = "Строка не соответствует шаблону:\n\t0,0";

                    regex = new Regex(@"\d{2}");

                    setRoadCondProp = (str, validationResult) =>
                    {
                        if (validationResult)
                        {
                            RoadCondition.SurfaceState = str;
                        }
                        isSurfaceStateValid = validationResult;
                    };
                    break;
                default:
                    return;
            }
             
            tempStr = tempStr.GetStrWithoutSeparator(',');

            if (regex.IsMatch(tempStr))
            {
                setRoadCondProp(tempStr, true);
                if (border.ToolTip != null)
                {
                    textBox.Foreground = new SolidColorBrush(Colors.Black);
                }

                border.BorderBrush = null;
                border.ToolTip = null;
            }
            else
            {
                if (border.BorderBrush == null)
                {
                    setRoadCondProp(tempStr, true);

                    border.BorderBrush = new SolidColorBrush(Colors.Red);
                    border.ToolTip = toolTipText;

                    textBox.Foreground = new SolidColorBrush(Colors.Red);
                }

            }

            tempStr = tempStr.AddSeparator(',', indexes);

            textBox.Text = tempStr;

            //set caret after string change
            if (caretIndex + (tempStr.Length - oldLength) >= 0)
            {
                textBox.CaretIndex = caretIndex + (tempStr.Length - oldLength);
            }
        }

    }
}
