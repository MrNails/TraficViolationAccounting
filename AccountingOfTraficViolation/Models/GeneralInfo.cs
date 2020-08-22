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
        private static Regex[] cardNumberRegexes;

        private byte �ardType;
        private byte incidentType;
        private byte dayOfWeek;
        private string cardNumber;
        private DateTime fillDate;
        private DateTime incidentDate;
        private TimeSpan fillTime;

        static GeneralInfo()
        {
            cardNumberRegexes = new Regex[] { new Regex(@"\d{2}-\d{7}(-[0-9])?$"), new Regex(@"\d{9}[0-9]?$") };
        }

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
                    errors["CardNumber"] = "����� ����� �� ����� ���� ������.";
                    cardNumber = null;
                    return;
                }

                foreach (var cardNumberRegex in cardNumberRegexes)
                {
                    if (cardNumberRegex.IsMatch(value))
                    {
                        cardNumber = value.GetStrWithoutSeparator('-');
                        errors["CardNumber"] = null;
                        return;
                    }
                    else
                    {
                        cardNumber = value;
                        errors["CardNumber"] = "������ �� ������������� �� ������ �� ���� ������������� ��������:\n" +
                                                 "- 00-0000000-0*\n" +
                                                 "- 0000000000*\n" +
                                                 "* - �� ������������ �������";
                    }
                }


                OnPropertyChanged("CardNumber");
            }
        }

        public byte CardType
        {
            get { return �ardType; }
            set
            {
                if (value <= 10)
                {
                    �ardType = value;
                    OnPropertyChanged("CardType");
                    errors["CardType"] = null;
                }
                else
                {
                    errors["CardType"] = "�� ���������� ���� ���� �����. �������� ������ ���������� � �������� �� 0 �� 10.";
                }
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
                    dayOfWeek = value;
                    errors["DayOfWeek"] = null;
                    OnPropertyChanged("DayOfWeek");
                }
                else
                {
                    errors["DayOfWeek"] = "�� ���������� ���� ��� ������. �������� ������ ���������� � �������� �� 1 �� 7 (1 - �����������, 7 - �����������).";
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
                    errors["IncidentType"] = "�� ���������� ���� ���� �����������. �������� ������ ���������� � �������� �� 0 �� 100.";
                }
            }
        }

        [NotAssign]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Case> Cases { get; set; }
    }
}
