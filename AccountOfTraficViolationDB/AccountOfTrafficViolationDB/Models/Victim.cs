using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AccountOfTrafficViolationDB.Helpers;

namespace AccountOfTrafficViolationDB.Models
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
            Name = string.Empty;
            IsDied = string.Empty;
            Surname = string.Empty;
            Patronymic = string.Empty;
            Citizenship = string.Empty;
        }

        [NotAssign, Column("VictimId")]
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
                    errors["IsDied"] = "�� ��������� ���� ���� �����(������������).";
                }
                else if (value.Length <= 2)
                {
                    errors["IsDied"] = null;
                }
                else
                {
                    errors["IsDied"] = "���������� �������� � ���� �����(������������) �� ����� ���� ������ 2.";
                }

                isDied = value;
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
                    errors["Category"] = null;
                }
                else
                {
                    errors["Category"] = "�� ��������� ���� ���������.";
                }

                category = value;
                OnPropertyChanged("Category");
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
                    errors["TORSerialNumber"] = null;
                }
                else
                {
                    errors["TORSerialNumber"] = "�� ��������� ���� ����������� ������ ��.";
                }

                _TORSerialNumber = value;
                OnPropertyChanged("TORSerialNumber");
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
                    errors["MedicalResult"] = null;
                }
                else
                {
                    errors["MedicalResult"] = "�� ��������� ���� ����������� ���. ������������.";
                }

                medicalResult = value;
                OnPropertyChanged("MedicalResult");
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
                    errors["Name"] = "��� �� ����� �������������.";
                }
                else if (value.Length <= 15)
                {
                    errors["Name"] = null;
                }
                else
                {
                    errors["Name"] = "���������� �������� � ����� �� ����� ���� ������ 15.";
                }

                name = value;
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
                    errors["Surname"] = "������� �� ����� �������������.";
                }
                else if (value.Length <= 15)
                {
                    errors["Surname"] = null;
                }
                else
                {
                    errors["Surname"] = "���������� �������� � ������� �� ����� ���� ������ 15.";
                }

                surname = value;
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
                    errors["Patronymic"] = "�������� �� ����� �������������.";
                }
                else if (value.Length <= 15)
                {
                    errors["Patronymic"] = null;
                }
                else
                {
                    errors["Patronymic"] = "���������� �������� � �������� �� ����� ���� ������ 15.";
                }

                patronymic = value;
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
                    errors["Citizenship"] = "����������� �� ����� �������������.";
                }
                else if (value.Length <= 3)
                {
                    errors["Citizenship"] = null;
                }
                else
                {
                    errors["Citizenship"] = "���������� �������� � ����������� �� ����� ���� ������ 3.";
                }

                citizenship = value;
                OnPropertyChanged("Citizenship");
            }
        }

        public int CaseId { get; set; }

        [NotAssign]
        public virtual Case Case { get; set; }
    }
}
