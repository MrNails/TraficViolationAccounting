using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using AccountingOfTraficViolation.Services;

namespace AccountingOfTraficViolation.Models
{
    [Table("GeneralInfos")]
    public partial class GeneralInfo : MainTable
    {
        private byte cardType;
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
                    errors["CardNumber"] = "����� ����� �� ����� ���� ������.";
                }
                else if (value.Length > 10)
                {
                    errors["CardNumber"] = "����� ������ ����� �� ����� ��������� 10 ��������.";
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
            get { return cardType; }
            set
            {
                if (value <= 10)
                {
                    errors["CardType"] = null;
                }
                else
                {
                    errors["CardType"] = "�� ���������� ���� ���� �����. �������� ������ ���������� � �������� �� 0 �� 11.";
                }

                cardType = value;
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
                    errors["DayOfWeek"] = "�� ���������� ���� ��� ������. �������� ������ ���������� � �������� �� 1 �� 7 (1 - �����������, 7 - �����������).";
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
                    errors["IncidentType"] = "�� ���������� ���� ���� �����������. �������� ������ ���������� � �������� �� 0 �� 100.";
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
