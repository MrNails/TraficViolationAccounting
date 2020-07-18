using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using AccountingOfTraficViolation.Models;
using AccountingOfTraficViolation.Services;

namespace AccountingOfTraficViolation.ViewModels
{
    public class AccidentOnVillageVM : IDataErrorInfo
    {
        private string error;

        public AccidentOnVillageVM() : this(null)
        { }
        public AccidentOnVillageVM(AccidentOnVillage accidentOnVillage)
        {
            if (accidentOnVillage != null)
            {
                AccidentOnVillage = accidentOnVillage.Clone();
            }
            else
            {
                AccidentOnVillage = new AccidentOnVillage();
            }

            AccidentOnVillage.ErrorInput += ShowErrorMessage;

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

                switch (columnName)
                {
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
                    default:
                        break;
                }

                return _error;
            }
        }

        public AccidentOnVillage AccidentOnVillage { get; set; }
        public string VillageName { get; set; }
        public string VillageStreet { get; set; }
        public string VillageDistrict { get; set; }
        public string VillageBinding { get; set; }

        public string Error => error;

        private void ShowErrorMessage(string msg)
        {
            MessageBox.Show(msg, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
