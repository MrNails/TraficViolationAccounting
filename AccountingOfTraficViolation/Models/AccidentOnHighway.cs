using AccountingOfTraficViolation.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using System.Text.RegularExpressions;

namespace AccountingOfTraficViolation.Models
{
    public partial class AccidentOnHighway : MainTable
    {
        private string highwayIndexAndNumber;
        private string additionalInfo;
        private string kilometer;
        private string binding;
        private string meter;
        private Regex regex;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AccidentOnHighway()
        {
            CaseAccidentPlaces = new HashSet<CaseAccidentPlace>();

            regex = new Regex(@"[a-zA-Z]-\d{2}-\d{2}(-[0-9])?$");

            HighwayIndexAndNumber = "";
            AdditionalInfo = "";
            Kilometer = "";
            Binding = "";
            Meter = "";
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
                    errors["HighwayIndexAndNumber"] = "������ � ����� ������ �� ����� �������������.";
                    return;
                }

                if (regex.IsMatch(value) || int.TryParse(value, out int h))
                {
                    highwayIndexAndNumber = value.GetStrWithoutSeparator('-');
                    OnPropertyChanged("HighwayIndexAndNumber");
                    errors["HighwayIndexAndNumber"] = null;
                }
                else
                {
                    errors["HighwayIndexAndNumber"] = "������ �� ������������� �� ������ �� ���� ������������� ��������:\n" +
                                             "\t- 0-00-00-0*\n" +
                                             "\t- 000000*\n" +
                                             "* - �� ������������ �������";
                }
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
                    errors["AdditionalInfo"] = "���������� �������� � ���� � ����������� �� ����� ���� ������ 20.";
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
                    errors["Kilometer"] = "���� � ����������� ���������� �� ����� ���� ������.";
                    return;
                }

                if (value.Length <= 4)
                {
                    kilometer = value;
                    OnPropertyChanged("Kilometer");
                    errors["Kilometer"] = null;
                }
                else
                {
                    errors["Kilometer"] = "���������� �������� � ���� � ����������� �� ����� ���� ������ 4.";
                }
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
                    return;
                }

                if (value.Length <= 3)
                {
                    meter = value;
                    OnPropertyChanged("Meter");
                    errors["Meter"] = null;
                }
                else
                {
                    errors["Meter"] = "���������� �������� � ���� � ������� �� ����� ���� ������ 3.";
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
                    errors["Binding"] = "���� '��������' �� ����� ���� ������.";
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
                    errors["Binding"] = "���������� �������� � ���� '��������' �� ����� ���� ������ 47.";
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CaseAccidentPlace> CaseAccidentPlaces { get; set; }
    }
}
