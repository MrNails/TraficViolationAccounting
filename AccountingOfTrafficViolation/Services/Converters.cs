using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace AccountingOfTrafficViolation.Services
{
    public class MultiplyConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double result = 1.0;
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] is double)
                    result *= (double)values[i];
            }

            return result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new Exception("Not implemented");
        }
    }

    public class StatusCoverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string status = null;

            switch ((string)value)
            {
                case "PROCESSING":
                    status = "Рассматривается";
                    break;
                case "CLOSE":
                    status = "Закрыто";
                    break;
                default:
                    break;
            }

            return status;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    //parametr's format is { separator, separator's position, separator's position, ... }
    public class SeparatorCoverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(string) || parameter == null || value == null)
            {
                return null;
            }


            string[] parametrs = parameter.ToString().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            char separator;
            byte index = 0;
            int[] indexes = new int[parametrs.Length - 1];

            for (int i = 1; i < parametrs.Length; i++)
            {
                if (byte.TryParse(parametrs[i], out index))
                {
                    indexes[i - 1] = index;
                }
            }

            switch (parametrs[0])
            {
                case "1":
                    separator = '-';
                    break;
                case "2":
                    separator = ',';
                    break;
                default:
                    separator = ' ';
                    break;
            }


            return ((string)value).GetStrWithoutSeparator(separator).AddSeparator(separator, indexes);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string[] parametrs = parameter.ToString().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            char separator;

            switch (parametrs[0])
            {
                case "1":
                    separator = '-';
                    break;
                case "2":
                    separator = ',';
                    break;
                default:
                    separator = ' ';
                    break;
            }

            return ((string)value).GetStrWithoutSeparator(separator);
        }
    }

    //parametr's format is { smbl, max string size, symbols delete method }
    //Symbol delete method is a method which delete symbol when their quantity greater than max string size
    //Type of this method: 0 - not delete, 1 - delete only specified symbol, 2 - delete all symbols
    public class SymbolsAddCoverter : IValueConverter
    {
        private StringBuilder convertString;
        private StringBuilder convertBackString;

        public SymbolsAddCoverter()
        {
            convertString = new StringBuilder();
            convertBackString = new StringBuilder();
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string[] parameters = parameter.ToString().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            convertString.Clear();

            if (targetType != typeof(string) || value == null || parameters.Length != 3)
            {
                return value;
            }

            int maxStringLength;
            convertString.Append(value);

            if (!int.TryParse(parameters[1], out maxStringLength) || maxStringLength < convertString.Length)
            {
                return convertString.ToString();
            }

            convertString.Insert(convertString.Length, parameters[0], maxStringLength - convertString.Length);

            return convertString.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string[] parameters = parameter.ToString().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            convertBackString.Clear();

            if (targetType != typeof(string) || value == null || parameters.Length != 3 || parameters[2] == "0")
            {
                return value;
            }

            int maxStringLength;
            convertBackString.Append(value);

            if (!int.TryParse(parameters[1], out maxStringLength) || maxStringLength >= convertBackString.Length)
            {
                if (maxStringLength != 0)
                {
                    convertBackString.Insert(convertBackString.Length, parameters[0], maxStringLength - convertBackString.Length);
                    return convertBackString.ToString();
                }
                else
                {
                    return value;
                }
            }

            if (convertBackString.Length != 0)
            {
                if (parameters[2] == "1")
                {
                    int lastInputSymbol = -1;
                    for (int i = 0; i < convertBackString.Length; i++)
                    {
                        if (convertBackString[i] != parameters[0][0])
                        {
                            lastInputSymbol = i;
                        }
                    }

                    if (lastInputSymbol != -1)
                    {
                        convertBackString.Remove(lastInputSymbol + 1, convertBackString.Length - maxStringLength);
                    }
                }
                else if (parameters[2] == "2")
                {
                    convertBackString.Remove(maxStringLength, convertBackString.Length - maxStringLength);
                }
            }

            return convertBackString.ToString();
        }

    }

    //Format of MultiConverters parameters: {(S)tart, quantity of parameters for each converter, (E)nd }
    public class MultiConverters : List<IValueConverter>, IValueConverter
    {
        private string[] convertersParameter;

        public MultiConverters()
        {
            convertersParameter = null;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int currentConverter = 0;

            if (convertersParameter == null)
            {
                FillParameter(parameter);
            }

            if (convertersParameter.Length == 0)
            {
                return this.Aggregate(value, (current, converter) => converter.Convert(current, targetType, null, culture));
            }
            else
            {
                return this.Aggregate(value, (current, converter) =>
                {
                    if (currentConverter < convertersParameter.Length - 1)
                    {
                        return converter.Convert(current, targetType, convertersParameter[currentConverter++], culture);
                    }
                    else
                    {
                        return converter.Convert(current, targetType, convertersParameter[currentConverter], culture);
                    }
                });
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int currentConverter = convertersParameter.Length;

            if (convertersParameter.Length == 0)
            {
                return this.Reverse<IValueConverter>().Aggregate(value, (current, converter) => converter.ConvertBack(current, targetType, null, culture));
            }
            else
            {
                return this.Reverse<IValueConverter>().Aggregate(value, (current, converter) =>
                {
                    if (currentConverter > 0)
                    {
                        return converter.ConvertBack(current, targetType, convertersParameter[--currentConverter], culture);
                    }
                    else
                    {
                        return converter.ConvertBack(current, targetType, convertersParameter[currentConverter], culture);
                    }
                });
            }
        }

        private void FillParameter(object parameter)
        {
            string[] parameters = parameter.ToString().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder parameterBuilder = new StringBuilder();
            bool isStarted = false;
            bool isEnded = false;
            int startIndex = 0;

            List<int> converterParameterAmount = new List<int>();

            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i].ToUpper() == "S")
                {
                    isStarted = true;
                }
                else if (parameters[i].ToUpper() == "E")
                {
                    isEnded = true;
                    startIndex = i + 1;
                }
                else if (isStarted && !isEnded)
                {
                    if (int.TryParse(parameters[i], out int res))
                    {
                        converterParameterAmount.Add(res);
                    }
                    else
                    {
                        converterParameterAmount.Add(0);
                    }
                }
                else
                {
                    break;
                }
            }

            convertersParameter = new string[converterParameterAmount.Count];

            for (int i = 0; i < converterParameterAmount.Count; i++)
            {
                parameterBuilder.Clear();

                for (int j = startIndex; j < startIndex + converterParameterAmount[i]; j++)
                {
                    parameterBuilder.Append(parameters[j]);
                    parameterBuilder.Append(' ');
                }

                startIndex += converterParameterAmount[i];
                convertersParameter[i] = parameterBuilder.ToString();
            }
        }
    }

    public class DayOfWeekConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string dayOfWeek = null;

            switch (value.ToString())
            {
                case "1":
                    dayOfWeek = "Понедельник";
                    break;
                case "2":
                    dayOfWeek = "Вторник";
                    break;
                case "3":
                    dayOfWeek = "Среда";
                    break;
                case "4":
                    dayOfWeek = "Четверг";
                    break;
                case "5":
                    dayOfWeek = "Пятница";
                    break;
                case "6":
                    dayOfWeek = "Суббота";
                    break;
                case "7":
                    dayOfWeek = "Воскресенье";
                    break;
                default:
                    break;
            }

            return dayOfWeek;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class GenderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string gender = null;

            if ((bool)value)
            {
                gender = "Женщина";
            }
            else
            {
                gender = "Мужчина";
            }

            return gender;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class PhoneNumberCoverter : IValueConverter
    {
        private static Dictionary<char, int[]> pairs;

        static PhoneNumberCoverter()
        {
            pairs = new Dictionary<char, int[]>();
            pairs.Add('(', new int[] { 0 });
            pairs.Add(')', new int[] { 4 });
            pairs.Add(' ', new int[] { 5 });
            pairs.Add('-', new int[] { 9, 12 });
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || targetType != typeof(string))
            {
                return value;
            }

            string _value = value.ToString();

            if (_value.Length >= 3)
            {
                _value = _value.AddSeparator(pairs);
            }

            return _value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || targetType != typeof(string))
            {
                return value;
            }

            string _value = value.ToString();

            return _value.GetStrWithoutSeparator(pairs);
        }
    }
}


