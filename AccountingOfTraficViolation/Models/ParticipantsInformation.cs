using AccountingOfTraficViolation.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using System.Text.RegularExpressions;

namespace AccountingOfTraficViolation.Models
{
    public partial class ParticipantsInformation : MainTable
    {
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
        private Regex pddViolationRegex;

        public ParticipantsInformation()
        {
            Name = "";
            Surname = "";
            Address = "";
            Patronymic = "";
            Citizenship = "";
            PDDViolation = "";

            pddViolationRegex = new Regex(@"\d{2},\d{2}$");
        }

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
                    errors["Name"] = "��� �� ����� ���� ������.";
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
                    errors["Name"] = "���������� �������� � ����� �� ����� ���� ������ 15.";
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
                    errors["Surname"] = "������� �� ����� ���� ������.";
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
                    errors["Surname"] = "���������� �������� � ������� �� ����� ���� ������ 15.";
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
                    errors["Patronymic"] = "�������� �� ����� ���� ������.";
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
                    errors["Patronymic"] = "���������� �������� � �������� �� ����� ���� ������ 15.";
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
                    errors["Address"] = "����� �� ����� ���� ������.";
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
                    errors["Address"] = "���������� �������� � ������ �� ����� ���� ������ 50.";
                }
            }
        }

        public byte Qualification
        {
            get { return qualification; }
            set
            {
                qualification = value;
                OnPropertyChanged("Qualification");
            }
        }

        public byte Age
        {
            get { return age; }
            set
            {
                if (value <= 100)
                {
                    age = value;
                    OnPropertyChanged("Age");
                    errors["Age"] = null;
                }
                else
                {
                    errors["Age"] = "������� �� ����� ��������� 100 ���.";
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

        [Required]
        [StringLength(3)]
        public string Citizenship
        {
            get { return citizenship; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    errors["Citizenship"] = "����������� �� ����� ���� ������.";
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
                    errors["Citizenship"] = "���������� �������� � ����������� �� ����� ���� ������ 3.";
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
                    errors["PDDViolation"] = "��������� ��� �� ����� ���� ������.";
                    return;
                }

                if (pddViolationRegex.IsMatch(value) || int.TryParse(value, out int pdd))
                {
                    _PDDViolation = value.GetStrWithoutSeparator(',');
                    OnPropertyChanged("PDDViolation");
                    errors["PDDViolation"] = null;
                }
                else
                {
                    errors["PDDViolation"] = "������ �� ������������� �� ������ �� ���� ������������� ��������:\n" +
                         "- 00,00\n" +
                         "- 0000\n";
                }
            }
        }

        public int CaseId { get; set; }

        public virtual Case Case { get; set; }

        public override string ToString()
        {
            string str = "";

            str += "���: " + Name + Environment.NewLine;
            str += "�������: " + Surname + Environment.NewLine;
            str += "��������: " + Patronymic;

            return str;
        }
    }
}
