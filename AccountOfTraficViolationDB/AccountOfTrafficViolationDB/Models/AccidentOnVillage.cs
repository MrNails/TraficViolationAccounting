using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AccountOfTrafficViolationDB.Helpers;

namespace AccountOfTrafficViolationDB.Models
{
    public partial class AccidentOnVillage : MainTable
    {
        private byte m_status;
        private short m_regionalCodeOfName;
        private short m_regionalCodeOfDistrict;
        private short m_regionalCodeOfStreet;
        private short m_regionalCodeOfBinding;
        private string m_name;
        private string m_district;
        private string m_street;
        private string m_binding;
        
        public AccidentOnVillage()
        {
            Name = string.Empty;
            District = string.Empty;
            Street = string.Empty;
            VillageBinding = string.Empty;
        }

        [NotAssign, Column("AccidentOnVillageId")]
        public int Id { get; set; }

        public byte Status
        {
            get { return m_status; }
            set
            {
                if (value > 10)
                {
                    errors["Status"] = "������ ����� �������. ������ �� ����� ���� ������ 10.";
                }
                else
                {
                    errors["Status"] = null;
                }

                m_status = value;
                OnPropertyChanged("Status");
            }
        }

        [Required]
        [StringLength(22)]
        public string Name
        {
            get { return m_name; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    errors["Name"] = "�������� ���������� ������ �� ����� ���� ������.";
                    m_name = null;
                    return;
                }
                else if (value.Length > 22)
                {
                    errors["Name"] = "���������� �������� � �������� ���������� ������ �� ����� ���� ������ 22.";
                }
                else
                {
                    errors["Name"] = null;
                }

                m_name = value;
                OnPropertyChanged("Name");
            }
        }

        public short RegionalCodeOfName
        {
            get { return m_regionalCodeOfName; }
            set
            {
                if (value < 0 && value > 10000)
                {
                    errors["ReginalCodeOfName"] = "������ ����� �������������� ����.";
                }
                else
                {
                    errors["ReginalCodeOfName"] = null;
                }

                m_regionalCodeOfName = value;
                OnPropertyChanged("ReginalCodeOfName");
            }
        }

        [Required]
        [StringLength(22)]
        public string District
        {
            get { return m_district; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    errors["District"] = "�������� ������ �� ����� ���� ������.";
                    m_district = null;
                    return;
                }

                if (value.Length <= 22)
                {
                    m_district = value;
                    OnPropertyChanged("District");
                    errors["District"] = null;
                }
                else
                {
                    errors["District"] = "���������� �������� � �������� ������ �� ����� ���� ������ 22.";
                }
            }
        }

        public short RegionalCodeOfDistrict
        {
            get { return m_regionalCodeOfDistrict; }
            set
            {
                if (value >= 0 && value <= 10000)
                {
                    m_regionalCodeOfDistrict = value;
                    errors["ReginalCodeOfDistrict"] = null;
                    OnPropertyChanged("ReginalCodeOfDistrict");
                }
                else
                {
                    errors["ReginalCodeOfDistrict"] = "������ ����� �������������� ����.";
                }
            }
        }

        [Required]
        [StringLength(22)]
        public string Street
        {
            get { return m_street; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    errors["Street"] = "�������� ����� �� ����� ���� ������.";
                    m_street = null;
                    return;
                }

                if (value.Length <= 22)
                {
                    m_street = value;
                    OnPropertyChanged("Street");
                    errors["Street"] = null;
                }
                else
                {
                    errors["Street"] = "���������� �������� � �������� ����� �� ����� ���� ������ 22.";
                }
            }
        }

        public short RegionalCodeOfStreet
        {
            get { return m_regionalCodeOfStreet; }
            set
            {
                if (value >= 0 && value <= 10000)
                {
                    m_regionalCodeOfStreet = value;
                    errors["ReginalCodeOfStreet"] = null;
                    OnPropertyChanged("ReginalCodeOfStreet");
                }
                else
                {
                    errors["ReginalCodeOfStreet"] = "������ ����� �������������� ����.";
                }
            }
        }

        [Required]
        [StringLength(47)]
        public string VillageBinding
        {
            get { return m_binding; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    errors["VillageBinding"] = "�������� �� ����� ���� ������.";
                    m_binding = null;
                    return;
                }

                if (value.Length <= 47)
                {
                    m_binding = value;
                    OnPropertyChanged("VillageBinding");
                    errors["VillageBinding"] = null;
                }
                else
                {
                    errors["VillageBinding"] = "���������� �������� � �������� �� ����� ���� ������ 47.";
                }
            }
        }

        public short RegionalCodeOfBinding
        {
            get { return m_regionalCodeOfBinding; }
            set
            {
                if (value >= 0 && value <= 10000)
                {
                    m_regionalCodeOfBinding = value;
                    errors["ReginalCodeOfBinding"] = null;
                    OnPropertyChanged("ReginalCodeOfBinding");
                }
                else
                {
                    errors["ReginalCodeOfBinding"] = "������ ����� �������������� ����.";
                }
            }
        }
    }
}
