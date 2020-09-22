using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using System.Text.RegularExpressions;
using AccountingOfTraficViolation.Services;

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

            CardNumber = "";

            fillDate = DateTime.Now;
            incidentDate = DateTime.Now;
        }

        [NotAssign]
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
                    errors["CardNumber"] = "Номер карты не может быть пустым.";
                }
                else if (value.Length > 10)
                {
                    errors["CardNumber"] = "Длина номера карты не может превышать 10 символов.";
                }
                else
                {
                    errors["CardNumber"] = null;
                }

                cardNumber = value;
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
                    errors["CardType"] = null;
                }
                else
                {
                    errors["CardType"] = "Не правильный ввод типа карты. Значение должно находиться в пределах от 0 до 11.";
                }

                сardType = value;
                OnPropertyChanged("CardType");
            }
        }

        [Column(TypeName = "date")]
        public DateTime FillDate
        {
            get { return fillDate; }
            set
            {
                if (value < MinimumDate)
                {
                    fillDate = MinimumDate;
                } 
                else
                {
                    fillDate = value;
                }
                OnPropertyChanged("FillDate");
            }
        }

        [Column(TypeName = "date")]
        public DateTime IncidentDate
        {
            get { return incidentDate; }
            set
            {
                if (value < MinimumDate)
                {
                    incidentDate = MinimumDate;
                }
                else
                {
                    incidentDate = value;
                }
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
                    errors["DayOfWeek"] = null;
                }
                else
                {
                    errors["DayOfWeek"] = "Не правильный ввод дня недели. Значение должно находиться в пределах от 1 до 7 (1 - Понедельник, 7 - Воскресенье).";
                }

                dayOfWeek = value;
                OnPropertyChanged("DayOfWeek");
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
                    errors["IncidentType"] = null;
                }
                else
                {
                    errors["IncidentType"] = "Не правильный ввод типа проишествия. Значение должно находиться в пределах от 0 до 100.";
                }

                incidentType = value;
                OnPropertyChanged("IncidentType");
            }
        }

        [NotAssign]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Case> Cases { get; set; }
    }
}
