using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

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
                    OnErrorInput("��� �� ����� ���� ������");

                    return;
                }

                if (value.Length <= 15)
                {
                    name = value;
                }
                else
                {
                    name = value.Remove(15);
                    OnErrorInput("���������� �������� � ����� �� ����� ���� ������ 15");
                }

                OnPropertyChanged("Name");
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
                    OnErrorInput("������� �� ����� ���� ������");

                    return;
                }

                if (value.Length <= 15)
                {
                    surname = value;
                }
                else
                {
                    surname = value.Remove(15);
                    OnErrorInput("���������� �������� � ������� �� ����� ���� ������ 15");
                }

                OnPropertyChanged("Surname");
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
                    OnErrorInput("�������� �� ����� ���� ������");

                    return;
                }

                if (value.Length <= 15)
                {
                    patronymic = value;
                }
                else
                {
                    patronymic = value.Remove(15);
                    OnErrorInput("���������� �������� � �������� �� ����� ���� ������ 15");
                }

                OnPropertyChanged("Patronymic");
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
                    OnErrorInput("����� �� ����� ���� ������");

                    return;
                }

                if (value.Length <= 50)
                {
                    address = value;
                }
                else
                {
                    address = value.Remove(50);
                    OnErrorInput("���������� �������� � ������ �� ����� ���� ������ 50");
                }

                OnPropertyChanged("Address");
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
                }
                else
                {
                    OnErrorInput("������� �� ����� ��������� 100 ���");
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
                    OnErrorInput("����������� �� ����� ���� ������");

                    return;
                }

                if (value.Length <= 3)
                {
                    citizenship = value;
                }
                else
                {
                    citizenship = value.Remove(3);
                    OnErrorInput("���������� �������� � ����������� �� ����� ���� ������ 3");
                }

                OnPropertyChanged("Citizenship");
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
                    OnErrorInput("��������� ��� �� ����� ���� ������");

                    return;
                }

                if (value.Length <= 4)
                {
                    _PDDViolation = value;
                }
                else
                {
                    _PDDViolation = value.Remove(4);
                    OnErrorInput("���������� �������� � ��������� ��� �� ����� ���� ������ 4");
                }

                OnPropertyChanged("PDDViolation");
            }
        }

        public int CaseId { get; set; }

        public virtual Case Case { get; set; }
    }
}
