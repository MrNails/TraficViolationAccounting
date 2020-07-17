using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using AccountingOfTraficViolation.Models;
using AccountingOfTraficViolation.Services;

namespace AccountingOfTraficViolation.ViewModels
{
    public class AccidentPlaceVM : IDataErrorInfo
    {
        private string error;

        public AccidentPlaceVM() : this(null, null)
        {}
        public AccidentPlaceVM(AccidentOnHighway accidentOnHighway, AccidentOnVillage accidentOnVillage)
        {
            if (accidentOnVillage != null)
            {
                AccidentOnVillage = accidentOnVillage.Clone();

                AccidentOnHighway = new AccidentOnHighway();
            }
            else if (accidentOnHighway != null)
            {
                AccidentOnHighway = accidentOnHighway.Clone();

                AccidentOnVillage = new AccidentOnVillage();
            }
            else
            {
                AccidentOnVillage = new AccidentOnVillage();
                AccidentOnHighway = new AccidentOnHighway();
            }

            AccidentOnHighway.ErrorInput += ShowErrorMessage;
            AccidentOnVillage.ErrorInput += ShowErrorMessage;

            RoadIndexAndNumber = AccidentOnHighway.HighwayIndexAndNumber;
            RoadBinding = AccidentOnHighway.Binding;
            Kilometer = AccidentOnHighway.Kilometer;
            Meter = AccidentOnHighway.Meter;
            VillageName = AccidentOnVillage.Name;
            VillageStreet = AccidentOnVillage.Street;
            VillageDistrict = AccidentOnVillage.District;
            VillageBinding = AccidentOnVillage.VillageBinding;
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
                    case "VillageName":
                        if (string.IsNullOrEmpty(VillageName))
                        {
                            _error = "Поле 'Название' не может быть пустым.";
                        }
                        else
                        {
                            AccidentOnVillage.Name = VillageName;
                        }
                        break;
                    case "VillageStreet":
                        if (string.IsNullOrEmpty(VillageStreet))
                        {
                            _error = "Поле 'Улица' не может быть пустым.";
                        }
                        else
                        {
                            AccidentOnVillage.Street = VillageStreet;
                        }
                        break;
                    case "VillageDistrict":
                        if (string.IsNullOrEmpty(VillageDistrict))
                        {
                            _error = "Поле 'Район' не может быть пустым.";
                        }
                        else
                        {
                            AccidentOnVillage.District = VillageDistrict;
                        }
                        break;
                    case "VillageBinding":
                        if (string.IsNullOrEmpty(VillageBinding))
                        {
                            _error = "Поле 'Привязка' не может быть пустым.";
                        }
                        else
                        {
                            AccidentOnVillage.VillageBinding = VillageBinding;
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

        public AccidentOnVillage AccidentOnVillage { get; set; }
        public AccidentOnHighway AccidentOnHighway { get; set; }
        public string RoadIndexAndNumber { get; set; }
        public string RoadBinding { get; set; }
        public string VillageName { get; set; }
        public string VillageStreet { get; set; }
        public string VillageDistrict { get; set; }
        public string VillageBinding { get; set; }
        public string Kilometer { get; set; }
        public string Meter { get; set; }

        public string Error => error;

        private void ShowErrorMessage(string msg)
        {
            MessageBox.Show(msg, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
