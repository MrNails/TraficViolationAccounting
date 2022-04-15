using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AccidentOnHighway()
        {
            CaseAccidentPlaces = new HashSet<CaseAccidentPlace>();

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

                highwayIndexAndNumber = value;
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
                }
                else if (value.Length > 20)
                {
                    errors["AdditionalInfo"] = "���������� �������� � ���� � ����������� �� ����� ���� ������ 20.";
                }
                else
                {
                    errors["AdditionalInfo"] = null;
                }

                additionalInfo = value;
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

                kilometer = value;
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

                meter = value;
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

                binding = value;
                OnPropertyChanged("Binding");
            }
        }

        [NotAssign]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CaseAccidentPlace> CaseAccidentPlaces { get; set; }
    }
}
