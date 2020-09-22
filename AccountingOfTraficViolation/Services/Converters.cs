using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace AccountingOfTraficViolation.Services
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
            
            return ((string)value).AddSeparator(separator, indexes);
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

            string str = ((string)value).GetStrWithoutSeparator(separator);
            return str;
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
}
