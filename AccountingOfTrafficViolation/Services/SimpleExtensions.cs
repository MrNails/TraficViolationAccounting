using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace AccountingOfTrafficViolation.Services
{
    public static class CloneExtension
    {
        public static T Clone<T>(this T obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };

            return JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(obj, options), options);
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

    public static class IEnumerableExtensions
    {
        public static int IndexOf<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
        {
            ArgumentNullException.ThrowIfNull(enumerable, nameof(enumerable));
            ArgumentNullException.ThrowIfNull(predicate, nameof(predicate));

            int idx = 0;
            foreach (var element in enumerable)
            {
                if (predicate(element))
                    return idx;

                ++idx;
            }

            return -1;
        }
    }

    public static class StringExtension
    {
        /// <summary>
        /// Adding separator to string
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
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
        /// <exception cref="ArgumentNullException"></exception>
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

        public static string AddSymbols(this string str, char smbl, int count)
        {
            if (count < 0)
            {
                return str;
            }

            StringBuilder stringBuilder = new StringBuilder(str);

            stringBuilder.Append(smbl, count);

            return stringBuilder.ToString();
        }
        public static string AddSymbols(this string str, char smbl, int start, int count)
        {
            if (start < 0 || count < 0 || start > str.Length)
            {
                return str;
            }

            StringBuilder stringBuilder = new StringBuilder(str);

            for (int i = start; i < count; i++)
            {
                stringBuilder.Insert(i, smbl);
            }

            return stringBuilder.ToString();
        }

        public static string GetStringWithUpperSymbols(this string str)
        {
            StringBuilder newString = new StringBuilder();

            for (int i = 0; i < str.Length; i++)
            {
                if (char.IsUpper(str[i]))
                {
                    newString.Append(str[i]);
                }
            }

            return newString.ToString();
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

            return string.Empty;
        }
    }
}
