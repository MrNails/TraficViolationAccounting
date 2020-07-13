using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace AccountingOfTraficViolation.Models
{
    public partial class AccidentOnHighway : MainTable
    {
        private string highwayIndexAndNumber;
        private string additionalInfo;
        private string kilometer;
        private string binding;
        private string meter;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AccidentOnHighway()
        {
            CaseAccidentPlaces = new HashSet<CaseAccidentPlace>();
        }

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
                    OnErrorInput("Индекс и номер дороги не могут отсутствовать.");
                    return;
                }

                if (value.Length <= 6)
                {
                    highwayIndexAndNumber = value;
                }
                else
                {
                    highwayIndexAndNumber = value.Remove(6);
                    OnErrorInput("Количество символов индексе и номере дороги не может быть больше 6.");
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
                if (value.Length <= 20)
                {
                    additionalInfo = value;
                }
                else
                {
                    additionalInfo = value.Remove(20);
                    OnErrorInput("Количество символов в поле с километрами не может быть больше 20.");
                }

                OnPropertyChanged("AdditionalInfo");
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
                    OnErrorInput("Поле с количеством километров не может быть пустым.");

                    return;
                }

                if (value.Length <= 4)
                {
                    kilometer = value;
                }
                else
                {
                    kilometer = value.Remove(4);
                    OnErrorInput("Количество символов в поле с километрами не может быть больше 4.");
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
                    OnErrorInput("Поле с количеством метров не может быть пустым.");

                    return;
                }

                if (value.Length <= 3)
                {
                    meter = value;
                }
                else
                {
                    meter = value.Remove(3);
                    OnErrorInput("Количество символов в поле с метрами не может быть больше 3.");
                }

                OnPropertyChanged("Meter");
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
                    OnErrorInput("Привязка не может быть пустой");

                    return;
                }

                if (value.Length <= 47)
                {
                    binding = value;
                }
                else
                {
                    binding = value.Remove(47);
                    OnErrorInput("Количество символов в привязке не может быть больше 47");
                }

                OnPropertyChanged("Binding");
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CaseAccidentPlace> CaseAccidentPlaces { get; set; }
    }
}
