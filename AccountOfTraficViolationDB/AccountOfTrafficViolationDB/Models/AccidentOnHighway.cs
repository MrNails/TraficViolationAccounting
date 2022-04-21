using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AccountingOfTrafficViolation;
using AccountingOfTrafficViolation.Models;
using AccountOfTrafficViolationDB.Helpers;

namespace AccountOfTrafficViolationDB.Models
{
    public partial class AccidentOnHighway : MainTable
    {
        private string m_highwayIndexAndNumber;
        private string m_additionalInfo;
        private string m_kilometer;
        private string m_highwayBinding;
        private string m_meter;
        
        public AccidentOnHighway()
        {
            HighwayIndexAndNumber = string.Empty;
            AdditionalInfo = string.Empty;
            Kilometer = string.Empty;
            HighwayBinding = string.Empty;
            Meter = string.Empty;
        }

        [NotAssign, Column("AccidentOnHighwayId")]
        public int Id { get; init; }

        [Required]
        [StringLength(6)]
        public string HighwayIndexAndNumber
        {
            get { return m_highwayIndexAndNumber; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    errors["HighwayIndexAndNumber"] = "������ � ����� ������ �� ����� �������������.";
                    
                }
                else if (value.Length > 6)
                {
                    errors["HighwayIndexAndNumber"] = "���������� �������� � ������� � ������ ������ �� ����� ��������� 6.";
                }
                else
                {
                    errors["HighwayIndexAndNumber"] = null;
                }

                m_highwayIndexAndNumber = value;
                OnPropertyChanged("HighwayIndexAndNumber");
            }
        }

        [StringLength(20)]
        public string AdditionalInfo
        {
            get { return m_additionalInfo; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    m_additionalInfo = null;
                }
                else if (value.Length > 20)
                {
                    errors["AdditionalInfo"] = "���������� �������� � ���� � ����������� �� ����� ���� ������ 20.";
                }
                else
                {
                    errors["AdditionalInfo"] = null;
                }

                m_additionalInfo = value;
                OnPropertyChanged("AdditionalInfo");
            }
        }

        [Required]
        [StringLength(4)]
        public string Kilometer
        {
            get { return m_kilometer; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    errors["Kilometer"] = "���� � ����������� ���������� �� ����� ���� ������.";
                }
                else if (!int.TryParse(value, out int km))
                {
                    errors["Kilometer"] = $"���������� ������������� �������� '{value}'.";
                }
                else if (value.Length > 4)
                {
                    errors["Kilometer"] = "���������� �������� � ���� � ����������� �� ����� ���� ������ 4.";
                }
                else
                {
                    errors["Kilometer"] = null;
                }

                m_kilometer = value;
                OnPropertyChanged("Kilometer");
            }
        }

        [Required]
        [StringLength(3)]
        public string Meter
        {
            get { return m_meter; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    errors["Meter"] = "���� � ����������� ������ �� ����� ���� ������.";
                }
                else if (!int.TryParse(value, out int m))
                {
                    errors["Kilometer"] = $"���������� ������������� �������� '{value}'.";
                }
                else if (value.Length > 3)
                {
                    errors["Meter"] = "���������� �������� � ���� � ������� �� ����� ���� ������ 3.";
                }
                else
                {
                    errors["Meter"] = null;
                }

                m_meter = value;
                OnPropertyChanged("Meter");
            }
        }

        [Required]
        [StringLength(47)]
        public string HighwayBinding
        {
            get { return m_highwayBinding; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    errors["Binding"] = "���� '��������' �� ����� ���� ������.";
                }
                else if (value.Length > 47)
                {
                    errors["Binding"] = "���������� �������� � ���� '��������' �� ����� ���� ������ 47.";
                }
                else
                {
                    errors["Binding"] = null;
                }

                m_highwayBinding = value;
                OnPropertyChanged("Binding");
            }
        }
    }
}
