using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using System.Text.RegularExpressions;
using AccountingOfTraficViolation.Services;

namespace AccountingOfTraficViolation.Models
{
    public partial class AccidentOnHighway : MainTable
    {
        private string highwayIndexAndNumber;
        private string additionalInfo;
        private string kilometer;
        private string binding;
        private string meter;
        private Regex[] regexes;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AccidentOnHighway()
        {
            CaseAccidentPlaces = new HashSet<CaseAccidentPlace>();
            regexes = new Regex[]
            {
                new Regex(@"[а-яА-Яa-zA-Z]-\d{2}-\d{2}(-[0-9])?$"),
                new Regex(@"[а-яА-Яa-zA-Z]\d{4}[0-9]?$")
            };

            HighwayIndexAndNumber = "";
            AdditionalInfo = "";
            Kilometer = "";
            Binding = "";
            Meter = "";
        }

        [NotAssign]
        public int Id { get; set; }

        [Required]
        [StringLength(6)]
        public string HighwayIndexAndNumber
        {
            get { return highwayIndexAndNumber; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    errors["HighwayIndexAndNumber"] = "Индекс и номер дороги не могут отсутствовать.";
                    highwayIndexAndNumber = null;
                    return;
                }

                foreach (var regex in regexes)
                {
                    if (regex.IsMatch(value))
                    {
                        errors["HighwayIndexAndNumber"] = null;
                        highwayIndexAndNumber = value.GetStrWithoutSeparator('-');
                        break;
                    }
                    else
                    {
                        highwayIndexAndNumber = value;
                        errors["HighwayIndexAndNumber"] = "Строка не соответствует ни одному из ниже перечисленных форматов:\n" +
                                                 "\t- A-00-00-0*\n" +
                                                 "\t- A00000*\n" +
                                                 "* - не обязательный элемент";
                    }
                }

                OnPropertyChanged("HighwayIndexAndNumber");
            }
        }

        [StringLength(20)]
        public string AdditionalInfo
        {
            get { return additionalInfo; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    additionalInfo = null;
                    return;
                }

                if (value.Length <= 20)
                {
                    additionalInfo = value;
                    OnPropertyChanged("AdditionalInfo");
                    errors["AdditionalInfo"] = null;
                }
                else
                {
                    errors["AdditionalInfo"] = "Количество символов в поле с километрами не может быть больше 20.";
                }
            }
        }

        [Required]
        [StringLength(4)]
        public string Kilometer
        {
            get { return kilometer; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    errors["Kilometer"] = "Поле с количеством километров не может быть пустым.";
                    kilometer = null;
                }
                else if (int.TryParse(value, out int km))
                {
                    errors["Kilometer"] = $"Невозможно преобразовать значение '{value}'.";
                }
                else if (value.Length <= 4)
                {
                    kilometer = value;
                    errors["Kilometer"] = null;
                }
                else
                {
                    errors["Kilometer"] = "Количество символов в поле с километрами не может быть больше 4.";
                }

                OnPropertyChanged("Kilometer");
            }
        }

        [Required]
        [StringLength(3)]
        public string Meter
        {
            get { return meter; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    errors["Meter"] = "Поле с количеством метров не может быть пустым.";
                }
                else if (int.TryParse(value, out int m))
                {
                    errors["Kilometer"] = $"Невозможно преобразовать значение '{value}'.";
                }
                else if (value.Length <= 3)
                {
                    meter = value;
                    OnPropertyChanged("Meter");
                    errors["Meter"] = null;
                }
                else
                {
                    errors["Meter"] = "Количество символов в поле с метрами не может быть больше 3.";
                }
            }
        }

        [Required]
        [StringLength(47)]
        public string Binding
        {
            get { return binding; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    errors["Binding"] = "Поле 'Привязка' не может быть пустым.";
                    binding = null;
                    return;
                }

                if (value.Length <= 47)
                {
                    binding = value;
                    OnPropertyChanged("Binding");
                    errors["Binding"] = null;
                }
                else
                {
                    errors["Binding"] = "Количество символов в поле 'Привязка' не может быть больше 47.";
                }
            }
        }

        [NotAssign]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CaseAccidentPlace> CaseAccidentPlaces { get; set; }
    }
}
