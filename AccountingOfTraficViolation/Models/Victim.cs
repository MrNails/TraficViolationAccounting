using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

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
                    OnErrorInput("�� ��������� ���� ���� �����(������������).");

                    return;
                }

                if (value.Length <= 2)
                {
                    isDied = value;
                }
                else
                {
                    isDied = value.Remove(2);
                    OnErrorInput("���������� �������� � ���� �����(������������) �� ����� ���� ������ 2.");
                }

                OnPropertyChanged("IsDied");
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
                }
                else
                {
                    OnErrorInput("�� ��������� ���� ���������.");
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
                }
                else
                {
                    OnErrorInput("�� ��������� ���� ����������� ������ ��.");
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
                }
                else
                {
                    OnErrorInput("�� ��������� ���� ����������� ���. ������������.");
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
                    OnErrorInput("��� �� ����� �������������.");

                    return;
                }

                if (value.Length <= 15)
                {
                    name = value;
                }
                else
                {
                    name = value.Remove(15);
                    OnErrorInput("���������� �������� � ����� �� ����� ���� ������ 15.");
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
                    OnErrorInput("������� �� ����� �������������.");

                    return;
                }

                if (value.Length <= 15)
                {
                    surname = value;
                }
                else
                {
                    surname = value.Remove(15);
                    OnErrorInput("���������� �������� � ������� �� ����� ���� ������ 15.");
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
                    OnErrorInput("�������� �� ����� �������������.");

                    return;
                }

                if (value.Length <= 15)
                {
                    patronymic = value;
                }
                else
                {
                    patronymic = value.Remove(15);
                    OnErrorInput("���������� �������� � �������� �� ����� ���� ������ 15.");
                }

                OnPropertyChanged("Patronymic");
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
                    OnErrorInput("����������� �� ����� �������������.");

                    return;
                }

                if (value.Length <= 3)
                {
                    citizenship = value;
                }
                else
                {
                    citizenship = value.Remove(3);
                    OnErrorInput("���������� �������� � ����������� �� ����� ���� ������ 3.");
                }

                OnPropertyChanged("Citizenship");
            }
        }

        public int CaseId { get; set; }

        public virtual Case Case { get; set; }
    }
}
