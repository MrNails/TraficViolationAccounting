using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using AccountOfTrafficViolationDB.Helpers;

namespace AccountOfTrafficViolationDB.Models
{
    public partial class Officer : MainTable
    {
        private static readonly Regex phoneNumberRegex;

        private string login;
        private string password;
        private string name;
        private string surname;
        private string phone;

        static Officer()
        {
            phoneNumberRegex = new Regex(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$");
        }

        public Officer()
        {
            Name = string.Empty;
            Surname = string.Empty;
            
            Cases = new HashSet<Case>();
        }

        [NotAssign, Column("OfficerId")]
        public string Id { get; init; }
        
        public string Name
        {
            get { return name; }
            set
            {
                if (value?.Length > 100)
                {
                    errors["Name"] = "Количество символов в поле \"имя\" не может превышать 100.";
                }
                else
                {
                    errors["Name"] = null;
                }

                name = value;
                OnPropertyChanged("Name");
            }
        }
        
        public string Surname
        {
            get { return surname; }
            set
            {
                if (value?.Length > 100)
                {
                    errors["Surname"] = "Количество символов в поле \"фамилия\" не может превышать 100.";
                }
                else
                {
                    errors["Surname"] = null;
                }

                surname = value;
                OnPropertyChanged("Surname");
            }
        }

        [StringLength(13)]
        public string Phone
        {
            get { return phone; }
            set
            {
                if (value != null && !phoneNumberRegex.IsMatch(value))
                {
                    errors["Phone"] = "Номер телефона должен соответствовать формату:\n" +
                                      "\t0123456789\n" +
                                      "\t(012) 345-67-89";
                }
                else
                {
                    errors["Phone"] = null;
                }

                phone = value;
                OnPropertyChanged("Phone");
            }
        }

        public bool IsActive { get; set; }
        
        [NotAssign]
        public virtual ICollection<Case> Cases { get; set; }
    }
}
