using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AccountOfTrafficViolationDB.Helpers;

namespace AccountOfTrafficViolationDB.Models
{
    public partial class Vehicle : MainTable
    {
        private short type;
        private short insurerCode;
        private string make;
        private string model;
        private string owner;
        private string policySeries;
        private string policyNumber;
        private string registrationSertificate;
        private string seriesOfRegistrationSertificate;
        private string _EDRPOU_Code;
        private DateTime policyEndDate;
        
        public Vehicle()
        {
            Make = string.Empty;
            Model = string.Empty;
            Owner = string.Empty;
            PolicySeries = string.Empty;
            PolicyNumber = string.Empty;
            EDRPOU_Code = string.Empty;
            RegistrationSertificate = string.Empty;
            SeriesOfRegistrationSertificate = string.Empty;

            policyEndDate = DateTime.Now;
            
            CaseVehicles = new HashSet<CaseVehicle>();
        }

        [NotAssign, Column("VehicleId")]
        public int Id { get; init; }

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
                }
                else if (value.Length <= 10)
                {
                    errors["Make"] = null;
                }
                else
                {
                    errors["Make"] = "Количество символов в марке машины не может быть больше 10.";
                }

                make = value;
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
                    errors["Model"] = "Модель машины не может быть пустой.";
                }
                else if (value.Length <= 10)
                {
                    errors["Model"] = null;
                }
                else
                {
                    errors["Model"] = "Количество символов в моделе машины не может быть больше 10.";
                }

                model = value;
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
                    errors["Type"] = null;
                }
                else
                {
                    errors["Type"] = "Не правилный ввод типа.";
                }

                type = value;
                OnPropertyChanged("Type");
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
                }
                else if (value.Length <= 3)
                {
                    errors["SeriesOfRegistrationSertificate"] = null;
                }
                else
                {
                    errors["SeriesOfRegistrationSertificate"] = "Количество символов в серии свидетельства о регистрации не может быть больше 3.";
                }

                seriesOfRegistrationSertificate = value;
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
                    errors["RegistrationSertificate"] = "Cвидетельство о регистрации не может быть пустым.";
                }
                else if (value.Length <= 6)
                {
                    errors["RegistrationSertificate"] = null;
                }
                else
                {
                    errors["RegistrationSertificate"] = "Количество символов в свидетельстве о регистрации не может быть больше 6.";
                }

                registrationSertificate = value;
                OnPropertyChanged("RegistrationSertificate");
            }
        }

        public short InsurerCode
        {
            get { return insurerCode; }
            set
            {
                if (value >= 0 && value < 1000)
                {
                    errors["InsurerCode"] = null;
                }
                else
                {
                    errors["InsurerCode"] = "Не правилный ввод кода страховки.";
                }

                insurerCode = value;
                OnPropertyChanged("InsurerCode");
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
                }
                else if (value.Length <= 3)
                {
                    errors["PolicySeries"] = null;
                }
                else
                {
                    errors["PolicySeries"] = "Количество символов в серии полиса не может быть больше 3.";
                }

                policySeries = value;
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
                    errors["PolicyNumber"] = "Полис не может быть пустым.";
                }
                else if (value.Length <= 10)
                {
                    errors["PolicyNumber"] = null;
                }
                else
                {
                    errors["PolicyNumber"] = "Количество символов в полисе не может быть больше 10.d";
                }

                policyNumber = value;
                OnPropertyChanged("PolicyNumber");
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
        [StringLength(20)]
        public string Owner
        {
            get { return owner; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    errors["Owner"] = "Владелец не может быть пустым.";
                }
                else if (value.Length <= 20)
                {
                    errors["Owner"] = null;
                }
                else
                {
                    errors["Owner"] = "Количество символов в поле \"владелец\" не может быть больше 20.";
                }

                owner = value;
                OnPropertyChanged("Owner");
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
        
        [NotAssign] public virtual ICollection<CaseVehicle> CaseVehicles { get; set; }
    }
}
