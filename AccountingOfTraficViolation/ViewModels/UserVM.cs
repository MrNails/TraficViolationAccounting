using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccountingOfTraficViolation.Models;
using AccountingOfTraficViolation.Services;

namespace AccountingOfTraficViolation.ViewModels
{
    public class UserVM : IDisposable, INotifyPropertyChanged
    {
        private TVAContext TVAContext;
        private User currentFindUser;

        public event PropertyChangedEventHandler PropertyChanged;

        public UserVM()
        {
            TVAContext = new TVAContext();
        }

        public bool IsCurrentUserChanged
        {
            get 
            {
                if (CurrentUser == null)
                {
                    return false;
                }

                System.Data.Entity.EntityState state = TVAContext.Entry(CurrentUser).State;

                return state == System.Data.Entity.EntityState.Modified ||
                       state == System.Data.Entity.EntityState.Added; 
            }
        } 

        public User CurrentUser
        {
            get { return currentFindUser; }
            private set
            {
                currentFindUser = value;
                OnPropertyChanged("CurrentUser");
            }
        }

        public bool CheckIfCurrenUserLoginExist()
        {
            var res = TVAContext.Users.
                                     Where(u => u.Login == CurrentUser.Login).
                                     AsEnumerable().
                                     Where(u => u.Login == CurrentUser.Login).
                                     ToArray().
                                     FirstOrDefault();
            return res != null;
        }

        public void ConfirmChange()
        {
            if (CurrentUser != null)
            {
                User user = TVAContext.Users.FirstOrDefault(u => u.Id == CurrentUser.Id);

                if (user == null)
                {
                    TVAContext.Users.Add(CurrentUser);
                }
            }

            TVAContext.SaveChanges();
        }
        public void DiscardChange()
        {
            TVAContext.CancelAllChanges();
        }
        public void DeleteCurrentUser()
        {
            if (CurrentUser == null)
            {
                return;
            }

            TVAContext.Users.Remove(CurrentUser);
            CurrentUser = null;
        }
        public void AddNewUser(byte role = (byte)UserRole.User)
        {
            CurrentUser = new User
            {
                Role = role
            };

            TVAContext.Users.Add(CurrentUser);
        }
        public void SetCurrentUser(string login)
        {
            if (string.IsNullOrEmpty(login))
            {
                throw new ArgumentException("Логин не может отсутствовать.", "login");
            }

            CurrentUser = TVAContext.Users.
                                     Where(u => u.Login == login).
                                     AsEnumerable().
                                     Where(u => u.Login == login).
                                     ToArray().
                                     FirstOrDefault();
        }

        public void Dispose()
        {
            TVAContext.Dispose();
        }

        private void OnPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
