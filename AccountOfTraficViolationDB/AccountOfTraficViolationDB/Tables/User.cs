using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using AccountingOfTraficViolation.Services;

namespace AccountingOfTraficViolation.Models
{
    public partial class User : MainTable
    {
        private static Regex phoneNumberRegex;

        private string login;
        private string password;
        private string name;
        private string surname;
        private string phone;

        static User()
        {
            phoneNumberRegex = new Regex(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$");
        }

        public User()
        {
            Login = "";
            Password = "";
            Name = "";
            Surname = "";
        }

        [NotAssign]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string Login
        {
            get { return login; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    errors["Login"] = "Поле \"логин\" не может отсутствовать.";
                }
                else if (value.Length > 20)
                {
                    errors["Login"] = "Количество символов в поле \"логин\" не может превышать 20.";
                }
                else
                {
                    errors["Login"] = null;
                }

                login = value;
                OnPropertyChanged("Login");
            }
        }

        [Required]
        [StringLength(20)]
        public string Password
        {
            get { return password; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    errors["Password"] = "Поле \"пароль\" не может отсутствовать.";
                }
                else if (value.Length > 20)
                {
                    errors["Password"] = "Количество символов в поле \"пароль\" не может превышать 20.";
                }
                else
                {
                    errors["Password"] = null;
                }

                password = value;
                OnPropertyChanged("Password");
            }
        }

        public byte Role { get; set; }

        [StringLength(15)]
        public string Name
        {
            get { return name; }
            set
            {
                if (value?.Length > 20)
                {
                    errors["Name"] = "Количество символов в поле \"имя\" не может превышать 15.";
                }
                else
                {
                    errors["Name"] = null;
                }

                name = value;
                OnPropertyChanged("Name");
            }
        }

        [StringLength(15)]
        public string Surname
        {
            get { return surname; }
            set
            {
                if (value?.Length > 20)
                {
                    errors["Surname"] = "Количество символов в поле \"фамилия\" не может превышать 15.";
                }
                else
                {
                    errors["Surname"] = null;
                }

                surname = value;
                OnPropertyChanged("Surname");
            }
        }

        [StringLength(10)]
        public string Phone
        {
            get { return phone; }
            set
            {
                if (value != null && !phoneNumberRegex.IsMatch(value))
                {
                    errors["Phone"] = "Номер телефона должен соответствовать одному из ниже перечисленных форматов:\n" +
                                      "\t0123456789" +
                                      "\t012-345-6789" +
                                      "\t(012)-345-6789" +
                                      "\t(012)3456789" +
                                      "\t012 3456789" +
                                      "\t012 345 6789" +
                                      "\t012 345-6789" +
                                      "\t(012) 345-6789" +
                                      "\t012.345.6789";
                }
                else
                {
                    errors["Phone"] = null;
                }

                phone = value;
                OnPropertyChanged("Phone");
            }
        }

    }
}
