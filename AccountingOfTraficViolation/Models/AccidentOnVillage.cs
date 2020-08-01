using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using AccountingOfTraficViolation.Services;

namespace AccountingOfTraficViolation.Models
{
    public partial class AccidentOnVillage : MainTable
    {
        private byte status;
        private short reginalCodeOfName;
        private short reginalCodeOfDistrict;
        private short reginalCodeOfStreet;
        private short reginalCodeOfBinding;
        private string name;
        private string district;
        private string street;
        private string binding;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AccidentOnVillage()
        {
            CaseAccidentPlaces = new HashSet<CaseAccidentPlace>();

            Name = "";
            District = "";
            Street = "";
            VillageBinding = "";
        }

        [NotAssing]
        public int Id { get; set; }

        public byte Status
        {
            get { return status; }
            set
            {
                if (value <= 10)
                {
                    status = value;
                    OnPropertyChanged("Status");
                    errors["Status"] = null;
                }
                else
                {
                    errors["Status"] = "������ ����� �������";
                }
            }
        }

        [Required]
        [StringLength(22)]
        public string Name
        {
            get { return name; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    errors["Name"] = "�������� ���������� ������ �� ����� ���� ������.";
                    name = null;
                    return;
                }

                if (value.Length <= 22)
                {
                    name = value;
                    errors["Name"] = null;
                    OnPropertyChanged("Name");
                }
                else
                {
                    errors["Name"] = "���������� �������� � �������� ���������� ������ �� ����� ���� ������ 22.";
                }
            }
        }

        public short ReginalCodeOfName
        {
            get { return reginalCodeOfName; }
            set
            {
                if (value >= 0 && value <= 10000)
                {
                    reginalCodeOfName = value;
                    errors["ReginalCodeOfName"] = null;
                    OnPropertyChanged("ReginalCodeOfName");
                }
                else
                {

                    errors["ReginalCodeOfName"] = "������ ����� �������������� ����.";
                }
            }
        }

        [Required]
        [StringLength(22)]
        public string District
        {
            get { return district; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    errors["District"] = "�������� ������ �� ����� ���� ������.";
                    district = null;
                    return;
                }

                if (value.Length <= 22)
                {
                    district = value;
                    OnPropertyChanged("District");
                    errors["District"] = null;
                }
                else
                {
                    errors["District"] = "���������� �������� � �������� ������ �� ����� ���� ������ 22.";
                }
            }
        }

        public short ReginalCodeOfDistrict
        {
            get { return reginalCodeOfDistrict; }
            set
            {
                if (value >= 0 && value <= 10000)
                {
                    reginalCodeOfDistrict = value;
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
            get { return street; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    errors["Street"] = "�������� ����� �� ����� ���� ������.";
                    street = null;
                    return;
                }

                if (value.Length <= 22)
                {
                    street = value;
                    OnPropertyChanged("Street");
                    errors["Street"] = null;
                }
                else
                {
                    errors["Street"] = "���������� �������� � �������� ����� �� ����� ���� ������ 22.";
                }
            }
        }

        public short ReginalCodeOfStreet
        {
            get { return reginalCodeOfStreet; }
            set
            {
                if (value >= 0 && value <= 10000)
                {
                    reginalCodeOfStreet = value;
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
        [Column(name:"Binding")]
        public string VillageBinding
        {
            get { return binding; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    errors["VillageBinding"] = "�������� �� ����� ���� ������.";
                    binding = null;
                    return;
                }

                if (value.Length <= 47)
                {
                    binding = value;
                    OnPropertyChanged("VillageBinding");
                    errors["VillageBinding"] = null;
                }
                else
                {
                    errors["VillageBinding"] = "���������� �������� � �������� �� ����� ���� ������ 47.";
                }
            }
        }

        public short ReginalCodeOfBinding
        {
            get { return reginalCodeOfBinding; }
            set
            {
                if (value >= 0 && value <= 10000)
                {
                    reginalCodeOfBinding = value;
                    errors["ReginalCodeOfBinding"] = null;
                    OnPropertyChanged("ReginalCodeOfBinding");
                }
                else
                {
                    errors["ReginalCodeOfBinding"] = "������ ����� �������������� ����.";
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CaseAccidentPlace> CaseAccidentPlaces { get; set; }
    }
}
