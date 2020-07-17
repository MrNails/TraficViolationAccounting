using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AccountingOfTraficViolation.Services
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
                    throw new InvalidCastException("Elem.Content is not Panel.");
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

                if (child is Panel)
                {
                    if (((Panel)child).CheckIfExistValidationError())
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
