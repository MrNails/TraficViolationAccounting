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
                    errors["PlateNumber"] = "Номерной знак машины не может быть пустым.";
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
                    errors["PlateNumber"] = "Количество символов в номерном знаке не может быть больше 8.";
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
                    errors["FrameNumber"] = "Номер рамы не может быть пустым.";
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
                    errors["FrameNumber"] = "Количество символов в номере рамы не может быть больше 8.";
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
                    errors["ChasisNumber"] = "Номер шасси не может быть пустым.";
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
                    errors["ChasisNumber"] = "Количество символов в номере шасси не может быть больше 9.";
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
                    errors["Make"] = "Марка машини не может быть пустой.";
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
                    errors["Make"] = "Количество символов в марке машины не может быть больше 10.";
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
                    errors["Model"] = "Модель машины не может быть пустой.";
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
                    errors["Model"] = "Количество символов в моделе машины не может быть больше 10.";
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
                    errors["Type"] = "Не правилный ввод типа.";
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
                    errors["SeriesOfRegistrationSertificate"] = "Cерия свидетельства о регистрации не может быть пустой.";
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
                    errors["SeriesOfRegistrationSertificate"] = "Количество символов в серии свидетельства о регистрации не может быть больше 3.";
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
                    errors["RegistrationSertificate"] = "Cвидетельство о регистрации не может быть пустым.";
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
                    errors["RegistrationSertificate"] = "Количество символов в свидетельстве о регистрации не может быть больше 6.";
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
                    errors["InsurerCode"] = "Не правилный ввод кода страховки.";
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
                    errors["PolicySeries"] = "Серия полиса не может быть пустым.";
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
                    errors["PolicySeries"] = "Количество символов в серии полиса не может быть больше 3.";
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
                    errors["PolicyNumber"] = "Полис не может быть пустым.";
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
                    errors["PolicyNumber"] = "Количество символов в полисе не может быть больше 10.d";
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
                    errors["Surname"] = "Фамилия водителя не может быть пустой.";
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
                    errors["Surname"] = "Количество символов в фамилии водителя не может быть больше 15.";
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
                    errors["LicenceSeries"] = "Серия удостоверения водителя не может быть пустой.";
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
                    errors["LicenceSeries"] = "Количество символов в серии удостовирении водителя не может быть больше 3.";
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
                    errors["LicenceNumber"] = "Удостоверения водителя не может быть пустым.";
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
                    errors["LicenceNumber"] = "Количество символов в удостовирении водителя не может быть больше 6.";
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
                    errors["Owner"] = "Владелец не может быть пустым.";
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
                    errors["Owner"] = "Количество символов в поле \"владелец\" не может быть больше 20.";
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
                    errors["TechnicalFaults"] = "Поле технические неисправности не может быть пустым.";
                }
                else if (value.Length > 2)
                {
                    errors["TechnicalFaults"] = "Количество символов в поле \"технические неисправности\" не может превышать 2.";
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
                    errors["EDRPOU_Code"] = "Код ЕДРПОУ не может быть пустым.";
                }
                else if (value.Length > 10)
                {
                    errors["EDRPOU_Code"] = "Количество символов в поле \"код ЕДРПОУ\" не может превышать 10.";
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
                        errors["CorruptionCode"] = "Количество символов в поле \"код повреждения\" не может превышать 10.";
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
                    errors["ActivityLicensingInfo"] = "Количество символов в ведомости о лицензировании водителя не может быть больше 2.";
                }
            }
        }

        public int CaseId { get; set; }

        [NotAssign]
        public virtual Case Case { get; set; }
    }
}
