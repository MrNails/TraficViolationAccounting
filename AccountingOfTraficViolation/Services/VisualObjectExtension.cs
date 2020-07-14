using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;


namespace AccountingOfTraficViolation.Services
{
    public static class TextBoxExtension
    {
        public static bool TrySetCaretOnIndexes(this TextBox textBox, Dictionary<int, int> desired_New_Indexes)
        {

            if (desired_New_Indexes == null)
            {
                throw new ArgumentNullException("desired_New_Indexes");
            }

            foreach (var pair in desired_New_Indexes)
            {
                if (pair.Key <= textBox.Text.Length && pair.Value <= textBox.Text.Length
                    && pair.Key >= 0 && pair.Value >= 0 && pair.Key == textBox.CaretIndex)
                {
                    textBox.CaretIndex = pair.Value;
                    return true;
                }
            }

            return false;
        }
    }
}
