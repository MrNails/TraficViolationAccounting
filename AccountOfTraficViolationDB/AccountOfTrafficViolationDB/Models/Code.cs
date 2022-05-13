using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AccountOfTrafficViolationDB.Helpers;
using AccountOfTrafficViolationDB.Models;
using AccountOfTraficViolationDB.Models;

namespace AccountingOfTrafficViolation.Models
{
    [Table("Codes")]
    public class Code : MainTable
    {
        private string name;
        private string m_value;
        private string description;

        [NotAssign]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name
        {
            get { return name; }
            set 
            {
                if (value == null)
                {
                    errors["Name"] = "Поле \"Имя\" не может быть пустым";
                }
                else if (value.Length > 50)
                {
                    errors["Name"] = "Количество символов в поле \"Имя\" не может быть больше 20";
                }
                else
                {
                    errors["Name"] = null;
                }

                OnPropertyChanged("Name");
                name = value; 
            }
        }

        [Required]
        [StringLength(50)]
        public string Value
        {
            get { return m_value; }
            set
            {
                if (value == null)
                {
                    errors["Value"] = "Поле \"Код\" не может быть пустым";
                }
                else if (value.Length > 50)
                {
                    errors["Value"] = "Количество символов в поле \"Код\" не может быть больше 15";
                }
                else
                {
                    errors["Value"] = null;
                }

                OnPropertyChanged();
                m_value = value;
            }
        }
        
        [StringLength(500)]
        public string Description
        {
            get { return description; }
            set
            {
                if (value.Length > 500)
                {
                    errors["Description"] = "Количество символов в поле \"Описание\" не может быть больше 500";
                }
                else
                {
                    errors["Description"] = null;
                }

                OnPropertyChanged("Description");
                description = value;
            }
        }
        
        public int CodeBindingId { get; set; }
        public virtual CodeBinding CodeBinding { get; set; }
    }
}
