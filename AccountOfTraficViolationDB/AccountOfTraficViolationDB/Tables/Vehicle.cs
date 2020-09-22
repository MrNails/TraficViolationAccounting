using AccountingOfTraficViolation.Services;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace AccountingOfTraficViolation.Models
{
    public partial class Vehicle : MainTable
    {
        private bool trailerAvailability;
        private short type;
        private short insurerCode;
        private string make;
        private string model;
        private string owner;
        private string surname;
        private string plateNumber;
        private string frameNumber;
        private string chasisNumber;
        private string policySeries;
        private string policyNumber;
        private string _EDRPOU_Code;
        private string licenceNumber;
        private string licenceSeries;
        private string corruptionCode;
        private string technicalFaults;
        private string activityLicensingInfo;
        private string registrationSertificate;
        private string seriesOfRegistrationSertificate;
        private DateTime policyEndDate;
        public Vehicle()
        {
            Make = "";
            Model = "";
            Owner = "";
            Surname = "";
            PlateNumber = "";
            FrameNumber = "";
            ChasisNumber = "";
            PolicySeries = "";
            PolicyNumber = "";
            EDRPOU_Code = "";
            LicenceNumber = "";
            LicenceSeries = "";
            CorruptionCode = "";
            TechnicalFaults = "";
            ActivityLicensingInfo = "";
            RegistrationSertificate = "";
            SeriesOfRegistrationSertificate = "";

            policyEndDate = DateTime.Now;
        }

        [NotAssign]
        public int Id { get; set; }

        [Required]
        [StringLength(8)]
        public string PlateNumber
        {
            get { return plateNumber; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    errors["PlateNumber"] = "�������� ���� ������ �� ����� ���� ������.";
                    plateNumber = null;
                    return;
                }

                if (value.Length <= 8)
                {
                    plateNumber = value;
                    OnPropertyChanged("PlateNumber");
                    errors["PlateNumber"] = null;
                }
                else
                {
                    errors["PlateNumber"] = "���������� �������� � �������� ����� �� ����� ���� ������ 8.";
                }
            }
        }

        [Required]
        [StringLength(8)]
        public string FrameNumber
        {
            get { return frameNumber; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    errors["FrameNumber"] = "����� ���� �� ����� ���� ������.";
                    frameNumber = null;
                    return;
                }

                if (value.Length <= 8)
                {
                    frameNumber = value;
                    OnPropertyChanged("FrameNumber");
                    errors["FrameNumber"] = null;
                }
                else
                {
                    errors["FrameNumber"] = "���������� �������� � ������ ���� �� ����� ���� ������ 8.";
                }
            }
        }

        [Required]
        [StringLength(9)]
        public string ChasisNumber
        {
            get { return chasisNumber; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    errors["ChasisNumber"] = "����� ����� �� ����� ���� ������.";
                    chasisNumber = null;
                    return;
                }

                if (value.Length <= 9)
                {
                    chasisNumber = value;
                    OnPropertyChanged("ChasisNumber");
                    errors["ChasisNumber"] = null;
                }
                else
                {
                    errors["ChasisNumber"] = "���������� �������� � ������ ����� �� ����� ���� ������ 9.";
                }
            }
        }

        [Required]
        [StringLength(10)]
        public string Make
        {
            get { return make; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    errors["Make"] = "����� ������ �� ����� ���� ������.";
                    make = null;
                    return;
                }

                if (value.Length <= 10)
                {
                    make = value;
                    OnPropertyChanged("Make");
                    errors["Make"] = null;
                }
                else
                {
                    errors["Make"] = "���������� �������� � ����� ������ �� ����� ���� ������ 10.";
                }
            }
        }

        [Required]
        [StringLength(10)]
        public string Model
        {
            get { return model; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    errors["Model"] = "������ ������ �� ����� ���� ������.";
                    model = null;
                    return;
                }

                if (value.Length <= 10)
                {
                    model = value;
                    OnPropertyChanged("Model");
                    errors["Model"] = null;
                }
                else
                {
                    errors["Model"] = "���������� �������� � ������ ������ �� ����� ���� ������ 10.";
                }
            }
        }

        public short Type
        {
            get { return type; }
            set
            {
                if (value >= 0 && value < 1000)
                {
                    type = value;
                    OnPropertyChanged("Type");
                    errors["Type"] = null;
                }
                else
                {
                    errors["Type"] = "�� ��������� ���� ����.";
                }
            }
        }

        [Required]
        [StringLength(3)]
        public string SeriesOfRegistrationSertificate
        {
            get { return seriesOfRegistrationSertificate; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    errors["SeriesOfRegistrationSertificate"] = "C���� ������������� � ����������� �� ����� ���� ������.";
                    seriesOfRegistrationSertificate = null;
                    return;
                }

                if (value.Length <= 3)
                {
                    seriesOfRegistrationSertificate = value;
                    OnPropertyChanged("SeriesOfRegistrationSertificate");
                    errors["SeriesOfRegistrationSertificate"] = null;
                }
                else
                {
                    errors["SeriesOfRegistrationSertificate"] = "���������� �������� � ����� ������������� � ����������� �� ����� ���� ������ 3.";
                }
            }
        }

        [Required]
        [StringLength(6)]
        public string RegistrationSertificate
        {
            get { return registrationSertificate; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    errors["RegistrationSertificate"] = "C������������ � ����������� �� ����� ���� ������.";
                    registrationSertificate = null;
                    return;
                }

                if (value.Length <= 6)
                {
                    registrationSertificate = value;
                    OnPropertyChanged("RegistrationSertificate");
                    errors["RegistrationSertificate"] = null;
                }
                else
                {
                    errors["RegistrationSertificate"] = "���������� �������� � ������������� � ����������� �� ����� ���� ������ 6.";
                }
            }
        }

        public bool TrailerAvailability
        {
            get { return trailerAvailability; }
            set
            {
                trailerAvailability = value;
                OnPropertyChanged("TrailerAvailability");
            }
        }

        public short InsurerCode
        {
            get { return insurerCode; }
            set
            {
                if (value >= 0 && value < 1000)
                {
                    insurerCode = value;
                    OnPropertyChanged("InsurerCode");
                    errors["InsurerCode"] = null;
                }
                else
                {
                    errors["InsurerCode"] = "�� ��������� ���� ���� ���������.";
                }
            }
        }

        [Required]
        [StringLength(3)]
        public string PolicySeries
        {
            get { return policySeries; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    errors["PolicySeries"] = "����� ������ �� ����� ���� ������.";
                    policySeries = null;
                    return;
                }

                if (value.Length <= 3)
                {
                    policySeries = value;
                    OnPropertyChanged("PolicySeries");
                    errors["PolicySeries"] = null;
                }
                else
                {
                    errors["PolicySeries"] = "���������� �������� � ����� ������ �� ����� ���� ������ 3.";
                }
            }
        }

        [Required]
        [StringLength(10)]
        public string PolicyNumber
        {
            get { return policyNumber; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    errors["PolicyNumber"] = "����� �� ����� ���� ������.";
                    policyNumber = null;
                    return;
                }

                if (value.Length <= 10)
                {
                    policyNumber = value;
                    OnPropertyChanged("PolicyNumber");
                    errors["PolicyNumber"] = null;
                }
                else
                {
                    errors["PolicyNumber"] = "���������� �������� � ������ �� ����� ���� ������ 10.d";
                }
            }
        }

        [Column(TypeName = "date")]
        public DateTime PolicyEndDate
        {
            get { return policyEndDate; }
            set
            {
                if (value < MinimumDate)
                {
                    policyEndDate = MinimumDate;
                }
                else
                {
                    policyEndDate = value;
                }
                OnPropertyChanged("PolicyEndDate");
            }
        }

        [Required]
        [StringLength(15)]
        public string Surname
        {
            get { return surname; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    errors["Surname"] = "������� �������� �� ����� ���� ������.";
                    surname = null;
                    return;
                }

                if (value.Length <= 15)
                {
                    surname = value;
                    OnPropertyChanged("Surname");
                    errors["Surname"] = null;
                }
                else
                {
                    errors["Surname"] = "���������� �������� � ������� �������� �� ����� ���� ������ 15.";
                }
            }
        }

        [Required]
        [StringLength(3)]
        public string LicenceSeries
        {
            get { return licenceSeries; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    errors["LicenceSeries"] = "����� ������������� �������� �� ����� ���� ������.";
                    licenceSeries = null;
                    return;
                }

                if (value.Length <= 3)
                {
                    licenceSeries = value;
                    OnPropertyChanged("LicenceSeries");
                    errors["LicenceSeries"] = null;
                }
                else
                {
                    errors["LicenceSeries"] = "���������� �������� � ����� ������������� �������� �� ����� ���� ������ 3.";
                }
            }
        }

        [Required]
        [StringLength(6)]
        public string LicenceNumber
        {
            get { return licenceNumber; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    errors["LicenceNumber"] = "������������� �������� �� ����� ���� ������.";
                    licenceNumber = null;
                    return;
                }

                if (value.Length <= 6)
                {
                    licenceNumber = value;
                    OnPropertyChanged("LicenceNumber");
                    errors["LicenceNumber"] = null;
                }
                else
                {
                    errors["LicenceNumber"] = "���������� �������� � ������������� �������� �� ����� ���� ������ 6.";
                }
            }
        }

        [Required]
        [StringLength(20)]
        public string Owner
        {
            get { return owner; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    errors["Owner"] = "�������� �� ����� ���� ������.";
                    owner = null;
                    return;
                }

                if (value.Length <= 20)
                {
                    owner = value;
                    OnPropertyChanged("Owner");
                    errors["Owner"] = null;
                }
                else
                {
                    errors["Owner"] = "���������� �������� � ���� \"��������\" �� ����� ���� ������ 20.";
                }
            }
        }

        [Required]
        [StringLength(2)]
        public string TechnicalFaults
        {
            get { return technicalFaults; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    errors["TechnicalFaults"] = "���� ����������� ������������� �� ����� ���� ������.";
                }
                else if (value.Length > 2)
                {
                    errors["TechnicalFaults"] = "���������� �������� � ���� \"����������� �������������\" �� ����� ��������� 2.";
                }
                else
                {
                    errors["TechnicalFaults"] = null;
                }

                technicalFaults = value;
                OnPropertyChanged("TechnicalFaults");
            }
        }

        [Required]
        [StringLength(10)]
        public string EDRPOU_Code
        {
            get { return _EDRPOU_Code; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    errors["EDRPOU_Code"] = "��� ������ �� ����� ���� ������.";
                }
                else if (value.Length > 10)
                {
                    errors["EDRPOU_Code"] = "���������� �������� � ���� \"��� ������\" �� ����� ��������� 10.";
                }
                else
                {
                    errors["EDRPOU_Code"] = null;
                }

                _EDRPOU_Code = value;
                OnPropertyChanged("EDRPOU_Code");
            }
        }

        [StringLength(8)]
        public string CorruptionCode
        {
            get { return corruptionCode; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    if (value.Length > 10)
                    {
                        errors["CorruptionCode"] = "���������� �������� � ���� \"��� �����������\" �� ����� ��������� 10.";
                    }
                    else
                    {
                        errors["CorruptionCode"] = null;
                    }
                }

                corruptionCode = value;
                OnPropertyChanged("CorruptionCode");
            }
        }

        [StringLength(2)]
        public string ActivityLicensingInfo
        {
            get { return activityLicensingInfo; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    activityLicensingInfo = null;
                    return;
                }

                if (value.Length <= 2)
                {
                    activityLicensingInfo = value;
                    OnPropertyChanged("ActivityLicensingInfo");
                    errors["ActivityLicensingInfo"] = null;
                }
                else
                {
                    errors["ActivityLicensingInfo"] = "���������� �������� � ��������� � �������������� �������� �� ����� ���� ������ 2.";
                }
            }
        }

        public int CaseId { get; set; }

        [NotAssign]
        public virtual Case Case { get; set; }
    }
}
