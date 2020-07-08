using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace AccountingOfTraficViolation.Models
{
    [Table("GeneralInfos")]
    public partial class GeneralInfo : MainTable
    {
        private byte сardType;
        private byte incidentType;
        private byte dayOfWeek;
        private string cardNumber;
        private DateTime fillDate;
        private DateTime incidentDate;
        private TimeSpan fillTime;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GeneralInfo()
        {
            Cases = new HashSet<Case>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(10)]
        public string CardNumber
        {
            get { return cardNumber; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    OnErrorInput("Номер карты не может быть пустым");

                    return;
                }

                if (value.Length <= 10)
                {
                    cardNumber = value;
                }
                else
                {
                    cardNumber = value.Remove(10);
                    OnErrorInput("Количество символов в номере карты не может быть больше 10");
                }


                OnPropertyChanged("CardNumber");
            }
        }

        public byte CardType
        {
            get { return сardType; }
            set
            {
                if (value <= 10)
                {
                    сardType = value;

                    OnPropertyChanged("CardType");
                }
                else
                {
                    OnErrorInput("Не правильный ввод типа карты");

                }
            }
        }

        [Column(TypeName = "date")]
        public DateTime FillDate
        {
            get { return fillDate; }
            set
            {
                fillDate = value;

                OnPropertyChanged("FillDate");
            }
        }

        [Column(TypeName = "date")]
        public DateTime IncidentDate
        {
            get { return incidentDate; }
            set
            {
                incidentDate = value;

                OnPropertyChanged("IncidentDate");
            }
        }

        public byte DayOfWeek
        {
            get { return dayOfWeek; }
            set
            {
                if (value > 0 && value < 8 )
                {
                    dayOfWeek = value;

                    OnPropertyChanged("DayOfWeek");
                }
                else
                {
                    OnErrorInput("Не правильный ввод дня недели. Введите номер от 1 до 7 (1 - Понедельник, 7 - Воскресенье)");
                }
            }
        }

        public TimeSpan FillTime
        {
            get { return fillTime; }
            set
            {
                fillTime = value;

                OnPropertyChanged("FillTime");
            }
        }

        public byte IncidentType
        {
            get { return incidentType; }
            set
            {
                if (value < 100)
                {
                    incidentType = value;

                    OnPropertyChanged("IncidentType");
                }
                else
                {
                    OnErrorInput("Не правильный ввод типа проишествия");

                }
            }
        }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Case> Cases { get; set; }
    }
}
