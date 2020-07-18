using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using AccountingOfTraficViolation.Models;
using AccountingOfTraficViolation.Services;

namespace AccountingOfTraficViolation.ViewModels
{
    public class AccidentOnHighwayVM : IDataErrorInfo
    {
        private string error;

        public AccidentOnHighwayVM() : this(null)
        { }
        public AccidentOnHighwayVM(AccidentOnHighway accidentOnHighway)
        {
            if (accidentOnHighway != null)
            {
                AccidentOnHighway = accidentOnHighway.Clone();
            }
            else
            {
                AccidentOnHighway = new AccidentOnHighway();
            }

            AccidentOnHighway.ErrorInput += ShowErrorMessage;

            RoadIndexAndNumber = AccidentOnHighway.HighwayIndexAndNumber;
            RoadBinding = AccidentOnHighway.Binding;
            Kilometer = AccidentOnHighway.Kilometer;
            Meter = AccidentOnHighway.Meter;
        }

        public string this[string columnName]
        {
            get
            {
                string _error = null;
                Regex regex = new Regex(@"\d{1}-\d{2}-\d{2}(-[0-9])?$");

                switch (columnName)
                {
                    case "RoadIndexAndNumber":
                        if (int.TryParse(RoadIndexAndNumber, out int ind))
                        {
                            RoadIndexAndNumber = RoadIndexAndNumber.AddSeparator('-', 1, 4, 7);
                        }

                        if (string.IsNullOrEmpty(RoadIndexAndNumber))
                        {
                            _error = "Поле 'Индекс и номер дороги' не может быть пустым.";
                        }
                        else if (!regex.IsMatch(RoadIndexAndNumber))
                        {
                            _error = "Поле не соответствует шаблону:\n\t0-00-00-0*\n\n* - не обязательный элемент.";
                        }
                        else
                        {
                            AccidentOnHighway.HighwayIndexAndNumber = RoadIndexAndNumber.GetStrWithoutSeparator('-');
                        }
                        break;
                    case "RoadBinding":
                        if (string.IsNullOrEmpty(RoadBinding))
                        {
                            _error = "Поле 'Привязка' не может быть пустым.";
                        }
                        else
                        {
                            AccidentOnHighway.Binding = RoadBinding;
                        }
                        break;
                    case "Kilometer":
                        if (string.IsNullOrEmpty(Kilometer))
                        {
                            _error = "Поле 'км' не может быть пустым.";
                        }
                        else if (!int.TryParse(Kilometer, out int km) || km < 0)
                        {
                            _error = $"Невозможно преобразовать значение '{Kilometer}'";
                        }
                        else
                        {
                            AccidentOnHighway.Kilometer = Kilometer;
                        }
                        break;
                    case "Meter":
                        if (string.IsNullOrEmpty(Meter))
                        {
                            _error = "Поле 'м' не может быть пустым.";
                        }
                        else if (!int.TryParse(Meter, out int m) || m < 0)
                        {
                            _error = $"Невозможно преобразовать значение '{Meter}'";
                        }
                        else
                        {
                            AccidentOnHighway.Meter = Meter;
                        }
                        break;
                    default:
                        break;
                }

                return _error;
            }
        }

        public AccidentOnHighway AccidentOnHighway { get; set; }
        public string RoadIndexAndNumber { get; set; }
        public string RoadBinding { get; set; }
        public string Kilometer { get; set; }
        public string Meter { get; set; }

        public string Error => error;

        private void ShowErrorMessage(string msg)
        {
            MessageBox.Show(msg, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
