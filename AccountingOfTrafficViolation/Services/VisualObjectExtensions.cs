using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AccountingOfTrafficViolation.Services
{
    public static class VisualObjectExtensions
    {
        public static bool CheckIfExistValidationError(this UIElement elem)
        {
            Panel panel = null;

            if (elem is ContentControl)
            {
                if (((ContentControl)elem).Content is Panel)
                {
                    panel = (Panel)((ContentControl)elem).Content;
                }
                else
                {
                    return false;
                }
            }
            else if (elem is Panel)
            {
                panel = (Panel)elem;
            }
            else
            {
                throw new InvalidCastException("Elem.Content is not Panel or ContentControl.");
            }

            foreach (var child in panel.Children)
            {
                if (Validation.GetHasError((DependencyObject)child))
                {
                    return true;
                }

                if (child is Panel || child is ContentControl)
                {
                    if (((UIElement)child).CheckIfExistValidationError())
                    {
                        return true;
                    }
                }

            }

            return false;
        }

    }

    public static class TextBoxExtension
    {
        public static void SeparatorTemplate(this TextBox textBox, char separator, params int[] indexes)
        {
            string tempStr = textBox.Text;
            int caretIndex = textBox.CaretIndex;
            int oldLength = textBox.Text.Length;

            tempStr = tempStr.GetStrWithoutSeparator(separator).AddSeparator(separator, indexes);

            textBox.Text = tempStr;

            //set caret after string change
            if (caretIndex + (tempStr.Length - oldLength) >= 0)
            {
                textBox.CaretIndex = caretIndex + (tempStr.Length - oldLength);
            }
        }
    }
}
