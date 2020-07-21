using AccountingOfTraficViolation.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using System.Text.RegularExpressions;

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
        private Regex cardNumberRegex;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GeneralInfo()
        {
            Cases = new HashSet<Case>();

            cardNumberRegex = new Regex(@"\d{2}-\d{7}(-[0-9])?$");

            CardNumber = "";
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
                    errors["CardNumber"] = "Номер карты не может быть пустым.";
                    cardNumber = null;
                    return;
                }

                if (cardNumberRegex.IsMatch(value) || int.TryParse(value, out int cn))
                {
                    cardNumber = value.GetStrWithoutSeparator('-');
                    errors["CardNumber"] = null;
                }
                else
                {
                    cardNumber = value;
                    errors["CardNumber"] = "Строка не соответствует ни одному из ниже перечисленных форматов:\n" +
                                             "- 00-0000000-0*\n" +
                                             "- 0000000000*\n" +
                                             "* - не обязательный элемент";
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
                    errors["CardType"] = null;
                }
                else
                {
                    errors["CardType"] = "Не правильный ввод типа карты. Значение должно находиться в пределах от 0 до 10.";
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
                    errors["DayOfWeek"] = null;
                    OnPropertyChanged("DayOfWeek");
                }
                else
                {
                    errors["DayOfWeek"] = "Не правильный ввод дня недели. Значение должно находиться в пределах от 1 до 7 (1 - Понедельник, 7 - Воскресенье).";
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
                    errors["IncidentType"] = null;
                    OnPropertyChanged("IncidentType");
                }
                else
                {
                    errors["IncidentType"] = "Не правильный ввод типа проишествия. Значение должно находиться в пределах от 0 до 100.";
                }
            }
        }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Case> Cases { get; set; }
    }
}
