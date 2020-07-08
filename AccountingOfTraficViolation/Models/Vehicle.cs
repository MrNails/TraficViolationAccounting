using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

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
                    OnErrorInput("Номерной знак машины не может быть пустым");

                    return;
                }

                if (value.Length <= 8)
                {
                    plateNumber = value;
                }
                else
                {
                    plateNumber = value.Remove(8);
                    OnErrorInput("Количество символов в номерном знаке не может быть больше 8");
                }

                OnPropertyChanged("PlateNumber");
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
                    OnErrorInput("Номер рамы не может быть пустым");

                    return;
                }

                if (value.Length <= 8)
                {
                    frameNumber = value;
                }
                else
                {
                    frameNumber = value.Remove(8);
                    OnErrorInput("Количество символов в номере рамы не может быть больше 8");
                }

                OnPropertyChanged("FrameNumber");
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
                    OnErrorInput("Номер шасси не может быть пустым");

                    return;
                }

                if (value.Length <= 9)
                {
                    chasisNumber = value;
                }
                else
                {
                    chasisNumber = value.Remove(9);
                    OnErrorInput("Количество символов в номере шасси не может быть больше 9");
                }

                OnPropertyChanged("ChasisNumber");
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
                    OnErrorInput("Марка машини не может быть пустой");

                    return;
                }

                if (value.Length <= 10)
                {
                    make = value;
                }
                else
                {
                    make = value.Remove(10);
                    OnErrorInput("Количество символов в марке машины не может быть больше 10");
                }

                OnPropertyChanged("Make");
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
                    OnErrorInput("Модель машины не может быть пустой");

                    return;
                }

                if (value.Length <= 10)
                {
                    model = value;
                }
                else
                {
                    model = value.Remove(10);
                    OnErrorInput("Количество символов в моделе машины не может быть больше 10");
                }

                OnPropertyChanged("Model");
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
                }
                else
                {
                    OnErrorInput("Не правилный ввод типа.");
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
                    OnErrorInput("Cерия свидетельства о регистрации не может быть пустой.");
                    return;
                }

                if (value.Length <= 3)
                {
                    seriesOfRegistrationSertificate = value;
                }
                else
                {
                    seriesOfRegistrationSertificate = value.Remove(3);
                    OnErrorInput("Количество символов в серии свидетельства о регистрации не может быть больше 3");
                }

                OnPropertyChanged("SeriesOfRegistrationSertificate");
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
                    OnErrorInput("Cвидетельство о регистрации не может быть пустым");

                    return;
                }

                if (value.Length <= 6)
                {
                    registrationSertificate = value;
                }
                else
                {
                    registrationSertificate = value.Remove(6);
                    OnErrorInput("Количество символов в свидетельстве о регистрации не может быть больше 6");
                }

                OnPropertyChanged("RegistrationSertificate");
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
                }
                else
                {
                    OnErrorInput("Не правилный ввод кода страховки.");
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
                    OnErrorInput("Серия полиса не может быть пустым.");

                    return;
                }

                if (value.Length <= 3)
                {
                    policySeries = value;
                }
                else
                {
                    policySeries = value.Remove(3);
                    OnErrorInput("Количество символов в серии полиса не может быть больше 3.");
                }

                OnPropertyChanged("PolicySeries");
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
                    OnErrorInput("Полис не может быть пустым");

                    return;
                }

                if (value.Length <= 10)
                {
                    policyNumber = value;
                }
                else
                {
                    policyNumber = value.Remove(10);
                    OnErrorInput("Количество символов в полисе не может быть больше 10");
                }

                OnPropertyChanged("PolicyNumber");
            }
        }

        [Column(TypeName = "date")]
        public DateTime PolicyEndDate
        {
            get { return policyEndDate; }
            set
            {
                policyEndDate = value;
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
                    OnErrorInput("Фамилия водителя не может быть пустой");

                    return;
                }

                if (value.Length <= 15)
                {
                    surname = value;
                }
                else
                {
                    surname = value.Remove(15);
                    OnErrorInput("Количество символов в фамилии водителя не может быть больше 15");
                }

                OnPropertyChanged("Surname");
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
                    OnErrorInput("Серия удостоверения водителя не может быть пустой.");

                    return;
                }

                if (value.Length <= 3)
                {
                    licenceSeries = value;
                }
                else
                {
                    licenceSeries = value.Remove(3);
                    OnErrorInput("Количество символов в серии удостовирении водителя не может быть больше 3.");
                }

                OnPropertyChanged("LicenceNumber");
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
                    OnErrorInput("Удостоверения водителя не может быть пустым.");

                    return;
                }

                if (value.Length <= 6)
                {
                    licenceNumber = value;
                }
                else
                {
                    licenceNumber = value.Remove(6);
                    OnErrorInput("Количество символов в удостовирении водителя не может быть больше 6");
                }

                OnPropertyChanged("LicenceNumber");
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
                    OnErrorInput("Владелец не может быть пустым");

                    return;
                }

                if (value.Length <= 20)
                {
                    owner = value;
                }
                else
                {
                    owner = value.Remove(20);
                    OnErrorInput("Количество символов в владельце не может быть больше 20");
                }

                OnPropertyChanged("Owner");
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
                    OnErrorInput("Поле технические неисправности не может быть пустым");

                    return;
                }

                if (value.Length <= 2)
                {
                    technicalFaults = value;
                }
                else
                {
                    technicalFaults = value.Remove(2);
                    OnErrorInput("Количество символов в поле технические неисправности не может быть больше 2");
                }

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
                    OnErrorInput("Код ЕДРПОУ не может быть пустым");

                    return;
                }

                if (value.Length <= 10)
                {
                    _EDRPOU_Code = value;
                }
                else
                {
                    _EDRPOU_Code = value.Remove(10);
                    OnErrorInput("Количество символов в коде ЕДРПОУ не может быть больше 10");
                }

                OnPropertyChanged("EDRPOU_Code");
            }
        }

        [StringLength(10)]
        public string CorruptionCode
        {
            get { return corruptionCode; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    corruptionCode = null;
                    return;
                }

                if (value.Length <= 10)
                {
                    corruptionCode = value;
                }
                else
                {
                    corruptionCode = value.Remove(10);
                    OnErrorInput("Количество символов в коде повреждения ТЗ не может быть больше 10");
                }

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
                }
                else
                {
                    activityLicensingInfo = value.Remove(2);
                    OnErrorInput("Количество символов в ведомости о лицензировании водителя не может быть больше 2");
                }

                OnPropertyChanged("ActivityLicensingInfo");
            }
        }

        public int CaseId { get; set; }

        public virtual Case Case { get; set; }
    }
}
