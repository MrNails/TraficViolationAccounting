using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using AccountingOfTraficViolation.Services;

namespace AccountingOfTraficViolation.Models
{
    public partial class Victim : MainTable
    {
        private bool gender;
        private bool seatBelt;
        private byte category;
        private byte age;
        private byte _TORSerialNumber;
        private byte medicalResult;
        private string name;
        private string isDied;
        private string surname;
        private string patronymic;
        private string citizenship;

        public Victim()
        {
            Name = "";
            IsDied = "";
            Surname = "";
            Patronymic = "";
            Citizenship = "";
        }

        [NotAssing]
        public int Id { get; set; }

        [Required]
        [StringLength(2)]
        public string IsDied
        {
            get { return isDied; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    errors["IsDied"] = "Не правльный ввод поля погиб(травмироварн).";
                    isDied = null;
                    return;
                }

                if (value.Length <= 2)
                {
                    isDied = value;
                    OnPropertyChanged("IsDied");
                    errors["IsDied"] = null;
                }
                else
                {
                    errors["IsDied"] = "Количество символов в поле погиб(травмироварн) не может быть больше 2.";
                }
            }
        }

        public byte Category
        {
            get { return category; }
            set
            {
                if (value >= 0 && value < 100)
                {
                    category = value;
                    OnPropertyChanged("Category");
                    errors["Category"] = null;
                }
                else
                {
                    errors["Category"] = "Не правилный ввод категории.";
                }
            }
        }

        public bool Gender
        {
            get { return gender; }
            set
            {
                gender = value;
                OnPropertyChanged("Gender");
            }
        }

        public byte Age
        {
            get { return age; }
            set
            {
                age = value;
                OnPropertyChanged("Age");
            }
        }

        public byte TORSerialNumber
        {
            get { return _TORSerialNumber; }
            set
            {
                if (value >= 0 && value < 100)
                {
                    _TORSerialNumber = value;
                    OnPropertyChanged("TORSerialNumber");
                    errors["TORSerialNumber"] = null;
                }
                else
                {
                    errors["TORSerialNumber"] = "Не правилный ввод порядкового номера ТЗ.";
                }
            }
        }

        public bool SeatBelt
        {
            get { return seatBelt; }
            set
            {
                seatBelt = value;
                OnPropertyChanged("SeatBelt");
            }
        }

        public byte MedicalResult
        {
            get { return medicalResult; }
            set
            {
                if (value >= 0 && value < 10)
                {
                    medicalResult = value;
                    OnPropertyChanged("MedicalResult");
                    errors["MedicalResult"] = null;
                }
                else
                {
                    errors["MedicalResult"] = "Не правилный ввод результатов мет. обследования.";
                }
            }
        }

        [Required]
        [StringLength(15)]
        public string Name
        {
            get { return name; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    errors["Name"] = "Имя не может отсутствовать.";
                    name = null;
                    return;
                }

                if (value.Length <= 15)
                {
                    name = value;
                    OnPropertyChanged("Name");
                    errors["Name"] = null;
                }
                else
                {
                    errors["Name"] = "Количество символов в имени не может быть больше 15.";
                }
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
                    errors["Surname"] = "Фамилия не может отсутствовать.";
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
                    errors["Surname"] = "Количество символов в фамилии не может быть больше 15.";
                }
            }
        }

        [Required]
        [StringLength(15)]
        public string Patronymic
        {
            get { return patronymic; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    errors["Patronymic"] = "Отчество не может отсутствовать.";
                    patronymic = null;
                    return;
                }

                if (value.Length <= 15)
                {
                    patronymic = value;
                    OnPropertyChanged("Patronymic");
                    errors["Patronymic"] = null;
                }
                else
                {
                    errors["Patronymic"] = "Количество символов в отчестве не может быть больше 15.";
                }
            }
        }

        [Required]
        [StringLength(3)]
        public string Citizenship
        {
            get { return citizenship; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    errors["Citizenship"] = "Гражданство не может отсутствовать.";
                    citizenship = null;
                    return;
                }

                if (value.Length <= 3)
                {
                    citizenship = value;
                    OnPropertyChanged("Citizenship");
                    errors["Citizenship"] = null;
                }
                else
                {
                    errors["Citizenship"] = "Количество символов в гражданстве не может быть больше 3.";
                }
            }
        }

        public int CaseId { get; set; }

        public virtual Case Case { get; set; }
    }
}
