using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

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

            string tempStr = str;
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
            string tempStr = str;

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
