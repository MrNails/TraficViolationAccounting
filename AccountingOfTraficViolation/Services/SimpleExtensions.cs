using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace AccountingOfTraficViolation.Services
{
    public static class CloneObject
    {
        public static T Clone<T>(this T obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }

            var deserializeSettings = new JsonSerializerSettings { ObjectCreationHandling = ObjectCreationHandling.Replace, ReferenceLoopHandling = ReferenceLoopHandling.Ignore};

            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(obj, deserializeSettings), deserializeSettings);
        }
    }

    public static class StringExtension
    {
        public static string AddSeparator(this string str, char separator, params int[] indexes)
        {
            if (indexes == null)
            {
                throw new ArgumentNullException("Indexes");
            }

            if (str == null)
            {
                return null;
            }

            string tempStr = string.Copy(str);
            int count = 0;

            if (tempStr.Contains(separator.ToString()))
            {
                tempStr = tempStr.GetStrWithoutSeparator(separator);
            }

            for (int i = 0; i < tempStr.Length; i++)
            {
                if (count >= indexes.Length)
                {
                    break;
                }

                if (i == indexes[count])
                {
                    tempStr = tempStr.Insert(i, separator.ToString());
                    count++;
                }
                
            }

            return tempStr;
        }
        public static string GetStrWithoutSeparator(this string str, char separator)
        {
            string tempStr = string.Copy(str);

            for (int i = 0; i < tempStr.Length; i++)
            {
                if (tempStr[i] == separator)
                {
                    tempStr = tempStr.Remove(i, 1);
                    i--;
                }
            }

            return tempStr;
        }
        public static string AddZeroBeforeText(this string str, int zerosNumber)
        {
            if (string.IsNullOrEmpty(str) || zerosNumber < 0)
            {
                return str;
            }
            
            StringBuilder tempStr = new StringBuilder();

            tempStr.Append('0', zerosNumber);

            tempStr.Append(string.Copy(str));

            return tempStr.ToString();
        }
        public static string RemoveZeroBeforeText(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            string tempStr = string.Copy(str);

            for (int i = 0; i < tempStr.Length; i++)
            {
                if (tempStr[i] == '0')
                {
                    tempStr = tempStr.Remove(i, 1);
                    i--;
                }
                else
                {
                    break;
                }
            }

            return tempStr;
        }
    }

    public static class ObservableCollectionExtension
    {
        public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                collection.Add(item);
            }
        }
    }

    public static class ExceptionExtension
    {
        public static string GetInnerExceptionMessage(this Exception ex)
        {
            if (ex.InnerException != null)
            {
                return ex.GetInnerExceptionMessage();
            }

            return "";
        }
    }
}
