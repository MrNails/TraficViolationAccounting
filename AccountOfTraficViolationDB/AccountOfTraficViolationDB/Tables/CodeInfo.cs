using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountingOfTraficViolation.Models
{
    [Table("CodeInformations")]
    public class CodeInfo : MainTable
    {
        private string name;
        private string code;
        private string description;

        
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string Name
        {
            get { return name; }
            set 
            {
                if (value == null)
                {
                    errors["Name"] = "Поле \"Имя\" не может быть пустым";
                }
                else if (value.Length > 20)
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
        [StringLength(15)]
        public string Code
        {
            get { return code; }
            set
            {
                if (value == null)
                {
                    errors["Code"] = "Поле \"Код\" не может быть пустым";
                }
                else if (value.Length > 15)
                {
                    errors["Code"] = "Количество символов в поле \"Код\" не может быть больше 15";
                }
                else
                {
                    errors["Code"] = null;
                }

                OnPropertyChanged("Code");
                code = value;
            }
        }

        [StringLength(100)]
        public string Description
        {
            get { return description; }
            set
            {
                if (value.Length > 100)
                {
                    errors["Description"] = "Количество символов в поле \"Описание\" не может быть больше 100";
                }
                else
                {
                    errors["Description"] = null;
                }

                OnPropertyChanged("Description");
                description = value;
            }
        }

    }
}
