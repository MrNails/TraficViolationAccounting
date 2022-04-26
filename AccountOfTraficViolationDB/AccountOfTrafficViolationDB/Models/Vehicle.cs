using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AccountOfTrafficViolationDB.Helpers;

namespace AccountOfTrafficViolationDB.Models
{
    public partial class Vehicle : MainTable
    {
        private short m_type;
        private short m_insurerCode;
        private string m_make;
        private string m_model;
        private string m_owner;
        private string m_policySeries;
        private string m_policyNumber;
        private string m_registrationSertificate;
        private string m_seriesOfRegistrationSertificate;
        private string m_EDRPOU_Code;
        private DateTime m_policyEndDate;
        
        public Vehicle()
        {
            m_make = string.Empty;
            m_model = string.Empty;
            m_owner = string.Empty;
            m_policySeries = string.Empty;
            m_policyNumber = string.Empty;
            m_EDRPOU_Code = string.Empty;
            m_registrationSertificate = string.Empty;
            m_seriesOfRegistrationSertificate = string.Empty;

            m_policyEndDate = DateTime.Now;
            
            CaseVehicles = new HashSet<CaseVehicle>();
        }

        [NotAssign, Column("VehicleId")]
        public int Id { get; set; }

        [Required]
        [StringLength(10)]
        public string Make
        {
            get { return m_make; }
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

                m_make = value;
                OnPropertyChanged("Make");
            }
        }

        [Required]
        [StringLength(10)]
        public string Model
        {
            get { return m_model; }
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

                m_model = value;
                OnPropertyChanged("Model");
            }
        }

        public short Type
        {
            get { return m_type; }
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

                m_type = value;
                OnPropertyChanged("Type");
            }
        }

        [Required]
        [StringLength(3)]
        public string SeriesOfRegistrationSertificate
        {
            get { return m_seriesOfRegistrationSertificate; }
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

                m_seriesOfRegistrationSertificate = value;
                OnPropertyChanged("SeriesOfRegistrationSertificate");
            }
        }

        [Required]
        [StringLength(6)]
        public string RegistrationSertificate
        {
            get { return m_registrationSertificate; }
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

                m_registrationSertificate = value;
                OnPropertyChanged("RegistrationSertificate");
            }
        }

        public short InsurerCode
        {
            get { return m_insurerCode; }
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

                m_insurerCode = value;
                OnPropertyChanged("InsurerCode");
            }
        }

        [Required]
        [StringLength(3)]
        public string PolicySeries
        {
            get { return m_policySeries; }
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

                m_policySeries = value;
                OnPropertyChanged("PolicySeries");
            }
        }

        [Required]
        [StringLength(10)]
        public string PolicyNumber
        {
            get { return m_policyNumber; }
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

                m_policyNumber = value;
                OnPropertyChanged("PolicyNumber");
            }
        }

        [Column(TypeName = "date")]
        public DateTime PolicyEndDate
        {
            get { return m_policyEndDate; }
            set
            {
                if (value < MinimumDate)
                {
                    m_policyEndDate = MinimumDate;
                }
                else
                {
                    m_policyEndDate = value;
                }
                OnPropertyChanged("PolicyEndDate");
            }
        }

        [Required]
        [StringLength(20)]
        public string Owner
        {
            get { return m_owner; }
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

                m_owner = value;
                OnPropertyChanged("Owner");
            }
        }
        
        [Required]
        [StringLength(10)]
        public string EDRPOU_Code
        {
            get { return m_EDRPOU_Code; }
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

                m_EDRPOU_Code = value;
                OnPropertyChanged("EDRPOU_Code");
            }
        }
        
        [NotAssign] public virtual ICollection<CaseVehicle> CaseVehicles { get; set; }
    }
}
