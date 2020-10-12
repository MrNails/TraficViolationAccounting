﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows;
using System.Linq;
using System.Data.Entity;

namespace AccountingOfTraficViolation.Services
{
    public static class CloneExtension
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

    public static class CommonExtension
    {
        public static bool IsIntegerNumber(this object obj)
        {
            return obj is byte ||
                   obj is short ||
                   obj is int ||
                   obj is long ||
                   obj is ulong ||
                   obj is ushort ||
                   obj is uint;
        }

        public static bool IsUnsignedNumber(this object obj)
        {
            return obj is byte ||
                   obj is ushort ||
                   obj is uint ||
                   obj is ulong;
        }

        public static bool IsSignedNumber(this object obj)
        {
            return obj is byte ||
                   obj is short ||
                   obj is int ||
                   obj is long;
        }

        public static bool IsFractionalNumber(this object obj)
        {
            return obj is double ||
                   obj is float ||
                   obj is decimal;
        }

        public static bool IsNumber(this object obj)
        {
            return obj is byte ||
                   obj is short ||
                   obj is int ||
                   obj is long ||
                   obj is ulong ||
                   obj is double ||
                   obj is float ||
                   obj is decimal;
        }
    }

    public static class StringExtension
    {
        /// <summary>
        /// Adding separator to string
        /// </summary>
        /// <param name="separator"></param>
        /// <param name="indexes"></param>
        /// <returns>New string with separators</returns>
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

            StringBuilder tempStr = new StringBuilder(str);
            int count = 0;

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

            return tempStr.ToString();
        }

        /// <summary>
        /// Adding separator to string
        /// </summary>
        /// <param name="pairs">Separator with their position index</param>
        /// <returns>New string with separators</returns>
        public static string AddSeparator(this string str, Dictionary<char, int[]> pairs)
        {
            if (pairs == null)
            {
                throw new ArgumentNullException("pairs");
            }

            if (str == null)
            {
                return null;
            }

            StringBuilder tempStr = new StringBuilder(str);

            foreach (var pair in pairs)
            {
                foreach (var value in pair.Value)
                {
                    if (tempStr.Length > value)
                    {
                        tempStr.Insert(value, pair.Key);
                    }
                }
            }

            return tempStr.ToString();
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
        public static string GetStrWithoutSeparator(this string str, Dictionary<char, int[]> pairs)
        {
            StringBuilder tempStr = new StringBuilder(str);

            for (int i = 0; i < tempStr.Length; i++)
            {
                if (pairs.ContainsKey(tempStr[i]))
                {
                    tempStr.Remove(i, 1);
                    i--;
                }
            }

            return tempStr.ToString();
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
