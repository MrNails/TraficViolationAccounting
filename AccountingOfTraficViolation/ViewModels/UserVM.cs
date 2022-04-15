using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccountingOfTraficViolation.Models;
using AccountingOfTraficViolation.Services;
using Microsoft.EntityFrameworkCore;

namespace AccountingOfTraficViolation.ViewModels
{
    public class UserVM : IDisposable, INotifyPropertyChanged
    {
        private TVAContext TVAContext;
        private Officer currentFindOfficer;

        public event PropertyChangedEventHandler PropertyChanged;

        public UserVM()
        {
            TVAContext = new TVAContext(GlobalSettings.ConnectionStrings[Constants.DefaultDB]);
        }

        public bool IsCurrentUserChanged
        {
            get 
            {
                if (CurrentOfficer == null)
                {
                    return false;
                }

                var state = TVAContext.Entry(CurrentOfficer).State;

                return state == EntityState.Modified ||
                       state == EntityState.Added; 
            }
        } 

        public Officer CurrentOfficer
        {
            get { return currentFindOfficer; }
            private set
            {
                currentFindOfficer = value;
                OnPropertyChanged("CurrentUser");
            }
        }

        public bool CheckIfCurrenUserLoginExist()
        {
            var res = TVAContext.Officers.
                                     Where(u => u.Login == CurrentOfficer.Login).
                                     AsEnumerable().
                                     Where(u => u.Login == CurrentOfficer.Login).
                                     ToArray().
                                     FirstOrDefault();
            return res != null;
        }

        public void ConfirmChange()
        {
            if (CurrentOfficer != null)
            {
                Officer officer = TVAContext.Officers.FirstOrDefault(u => u.Id == CurrentOfficer.Id);

                if (officer == null)
                {
                    TVAContext.Officers.Add(CurrentOfficer);
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
            if (CurrentOfficer == null)
            {
                return;
            }

            TVAContext.Officers.Remove(CurrentOfficer);
            CurrentOfficer = null;
        }
        public void AddNewUser(byte role = (byte)UserRole.User)
        {
            CurrentOfficer = new Officer
            {
                Role = role
            };

            TVAContext.Officers.Add(CurrentOfficer);
        }
        public void SetCurrentUser(string login)
        {
            if (string.IsNullOrEmpty(login))
            {
                throw new ArgumentException("Логин не может отсутствовать.", "login");
            }

            CurrentOfficer = TVAContext.Officers.
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
