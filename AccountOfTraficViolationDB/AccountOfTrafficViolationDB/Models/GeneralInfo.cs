using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AccountingOfTrafficViolation;
using AccountOfTrafficViolationDB.Helpers;

namespace AccountOfTrafficViolationDB.Models
{
    public partial class GeneralInfo : MainTable
    {
        private byte cardType;
        private byte incidentType;
        private byte dayOfWeek;
        private string cardNumber;
        private DateTime fillDate;
        private DateTime incidentDate;
        private TimeSpan fillTime;
        
        public GeneralInfo()
        {
            CardNumber = string.Empty;

            fillDate = DateTime.Now;
            incidentDate = DateTime.Now;
        }

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
        
        public int CaseId { get; set; }
        [NotAssign] public virtual Case Case { get; set; }
    }
}
