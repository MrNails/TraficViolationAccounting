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
            get { return CurrentUser != null && TVAContext.Entry(CurrentUser).State == System.Data.Entity.EntityState.Modified; }
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
            if (CurrentUser == null)
            {
                return;
            }

            User user = TVAContext.Users.FirstOrDefault(u => u.Id == CurrentUser.Id);

            if (user == null)
            {
                TVAContext.Users.Add(CurrentUser);
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
            TVAContext.SaveChanges();
        }
        public void AddNewUser(byte role = 1)
        {
            CurrentUser = new User();
            CurrentUser.Role = role;
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
