using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

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
        }

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
                }
                else
                {
                    OnErrorInput("������ ����� �������");

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
                    OnErrorInput("�������� ���������� ������ �� ����� ���� ������");

                    return;
                }

                if (value.Length <= 22)
                {
                    name = value;
                }
                else
                {
                    name = value.Remove(22);
                    OnErrorInput("���������� �������� � �������� ���������� ������ �� ����� ���� ������ 22");
                }

                OnPropertyChanged("Name");
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

                    OnPropertyChanged("ReginalCodeOfName");
                }
                else
                {

                    OnErrorInput("������ ����� �������������� ����");
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
                    OnErrorInput("�������� ������ �� ����� ���� ������");

                    return;
                }

                if (value.Length <= 22)
                {
                    district = value;
                }
                else
                {
                    district = value.Remove(22);
                    OnErrorInput("���������� �������� � �������� ������ �� ����� ���� ������ 22");
                }

                OnPropertyChanged("District");
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

                    OnPropertyChanged("ReginalCodeOfDistrict");
                }
                else
                {

                    OnErrorInput("������ ����� �������������� ����");
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
                    OnErrorInput("�������� ����� �� ����� ���� ������");

                    return;
                }

                if (value.Length <= 22)
                {
                    street = value;
                }
                else
                {
                    street = value.Remove(22);
                    OnErrorInput("���������� �������� � �������� ����� �� ����� ���� ������ 22");
                }

                OnPropertyChanged("Street");
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

                    OnPropertyChanged("ReginalCodeOfStreet");
                }
                else
                {

                    OnErrorInput("������ ����� �������������� ����");
                }
            }
        }

        [Required]
        [StringLength(47)]
        public string Binding
        {
            get { return binding; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    OnErrorInput("�������� �� ����� ���� ������");

                    return;
                }

                if (value.Length <= 47)
                {
                    binding = value;
                }
                else
                {
                    binding = value.Remove(47);
                    OnErrorInput("���������� �������� � �������� �� ����� ���� ������ 47");
                }

                OnPropertyChanged("Binding");
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

                    OnPropertyChanged("ReginalCodeOfBinding");
                }
                else
                {

                    OnErrorInput("������ ����� �������������� ����");
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CaseAccidentPlace> CaseAccidentPlaces { get; set; }
    }
}
