using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using System.Text.RegularExpressions;
using AccountingOfTraficViolation.Services;

namespace AccountingOfTraficViolation.Models
{
    public partial class ParticipantsInformation : MainTable
    {
        private static Regex[] pddViolationRegexes;

        private bool gender;
        private byte age;
        private byte qualification;
        private byte driveExpirience;
        private byte drivingTimeBeforeAccident;
        private string name;
        private string surname;
        private string address;
        private string patronymic;
        private string citizenship;
        private string _PDDViolation;

        static ParticipantsInformation()
        {
            pddViolationRegexes = new Regex[] { new Regex(@"\d{2},\d{2}$"), new Regex(@"\d{4}$") };
        }

        public ParticipantsInformation()
        {
            Name = "";
            Surname = "";
            Address = "";
            Patronymic = "";
            Citizenship = "";
            PDDViolation = "";
            Qualification = 0;
            Age = 0;
        }

        [NotAssign]
        public int Id { get; set; }

        [Required]
        [StringLength(15)]
        public string Name
        {
            get { return name; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    errors["Name"] = "Имя не может быть пустым.";
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
                    errors["Surname"] = "Фамилия не может быть пустой.";
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
                    errors["Patronymic"] = "Отчество не может быть пустым.";
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
        [StringLength(50)]
        public string Address
        {
            get { return address; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    errors["Address"] = "Адрес не может быть пустым.";
                    address = null;
                    return;
                }

                if (value.Length <= 50)
                {
                    address = value;
                    OnPropertyChanged("Address");
                    errors["Address"] = null;
                }
                else
                {
                    errors["Address"] = "Количество символов в адресе не может быть больше 50.";
                }
            }
        }

        public byte Qualification
        {
            get { return qualification; }
            set
            {
                if (value < 1)
                {
                    errors["Qualification"] = "Квалификация не может быть 0.";
                }
                else
                {
                    errors["Qualification"] = null;
                }

                qualification = value;
                OnPropertyChanged("Qualification");
            }
        }

        public byte Age
        {
            get { return age; }
            set
            {
                if (value > 0 && value <= 100)
                {
                    errors["Age"] = null;
                }
                else
                {
                    errors["Age"] = "Возраст не может быть меньше 1 года и превышать 100 лет.";
                }

                age = value;
                OnPropertyChanged("Age");
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

        [Required]
        [StringLength(3)]
        public string Citizenship
        {
            get { return citizenship; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    errors["Citizenship"] = "Гражданство не может быть пустым.";
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

        public byte DriveExpirience
        {
            get { return driveExpirience; }
            set
            {
                driveExpirience = value;
                OnPropertyChanged("DriveExpirience");
            }
        }

        public byte DrivingTimeBeforeAccident
        {
            get { return drivingTimeBeforeAccident; }
            set
            {
                drivingTimeBeforeAccident = value;
                OnPropertyChanged("DrivingTimeBeforeAccident");
            }
        }

        [Required]
        [StringLength(4)]
        public string PDDViolation
        {
            get { return _PDDViolation; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    errors["PDDViolation"] = "Нарушение ПДР не может быть пустым.";
                    _PDDViolation = null;
                    return;
                }

                foreach (var pddViolationRegex in pddViolationRegexes)
                {
                    if (pddViolationRegex.IsMatch(value))
                    {
                        _PDDViolation = value.GetStrWithoutSeparator(',');
                        errors["PDDViolation"] = null;
                        break;
                    }
                    else
                    {
                        _PDDViolation = value;
                        errors["PDDViolation"] = "Строка не соответствует ни одному из ниже перечисленных форматов:\n" +
                             "- 00,00\n" +
                             "- 0000\n";
                    }
                }


                OnPropertyChanged("PDDViolation");
            }
        }

        public int CaseId { get; set; }

        [NotAssign]
        public virtual Case Case { get; set; }

        public override string ToString()
        {
            string str = "";

            str += "Имя: " + Name + Environment.NewLine;
            str += "Фамилия: " + Surname + Environment.NewLine;
            str += "Отчество: " + Patronymic;

            return str;
        }
    }
}
