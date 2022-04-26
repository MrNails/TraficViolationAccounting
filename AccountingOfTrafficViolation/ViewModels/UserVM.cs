using System;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AccountingOfTrafficViolation.Services;
using AccountOfTrafficViolationDB.Context;
using AccountOfTrafficViolationDB.Models;
using Dapper;
using Microsoft.EntityFrameworkCore;

namespace AccountingOfTrafficViolation.ViewModels
{
    public class UserVM : IDisposable, INotifyPropertyChanged
    {
        private TVAContext m_TVAContext;
        private Officer m_currentFindOfficer;

        public event PropertyChangedEventHandler PropertyChanged;

        public UserVM()
        {
            m_TVAContext = new TVAContext(GlobalSettings.ConnectionStrings[Constants.DefaultDB], GlobalSettings.Credential);
        }

        public bool IsCurrentUserChanged
        {
            get 
            {
                if (CurrentOfficer == null)
                {
                    return false;
                }

                var state = m_TVAContext.Entry(CurrentOfficer).State;

                return state == EntityState.Modified ||
                       state == EntityState.Added; 
            }
        } 

        public Officer? CurrentOfficer
        {
            get { return m_currentFindOfficer; }
            private set
            {
                m_currentFindOfficer = value;
                OnPropertyChanged("CurrentUser");
            }
        }
        
        public async Task<bool> CheckIfCurrenUserLoginExistAsync()
        {
            var res = await m_TVAContext.Officers.FirstOrDefaultAsync(o => o.Id == CurrentOfficer.Id);
            
            return res != null;
        }

        public async Task ConfirmChangeAsync()
        {
            if (CurrentOfficer != null)
            {
                var officer = await m_TVAContext.Officers.FirstOrDefaultAsync(u => u.Id == CurrentOfficer.Id);

                // if (officer == null)
                // {
                //     var salt = RandomNumberGenerator.GetBytes(16);
                //     var pwd = CryptoHelper.EncryptData(Encoding.UTF8.GetBytes(CurrentOfficer.UserProfile.Password), salt);
                //     
                //     await m_TVAContext.Database.ExecuteSqlRawAsync(
                //         "exec AccountOfTrafficViolation.dbo.CreateOfficer {0}, {1}, {2}, {3}, {4}, {5}", 
                //         CurrentOfficer.UserProfile.Login, Encoding.UTF8.GetString(pwd), Encoding.UTF8.GetString(salt), 
                //         CurrentOfficer.Role, CurrentOfficer.Name, CurrentOfficer.Surname);
                // }
            }

            await m_TVAContext.SaveChangesAsync();
        }
        public void DiscardChange()
        {
            m_TVAContext.CancelAllChanges();
        }
        public async Task DeleteCurrentUserAsync()
        {
            if (CurrentOfficer == null)
                return;

            CurrentOfficer = null;

            m_TVAContext.Officers.Remove(await m_TVAContext.Officers.FirstOrDefaultAsync(o => o.Id == CurrentOfficer.Id));

            await m_TVAContext.SaveChangesAsync();
        }
        public void AddNewUser(byte role = (byte)UserRole.User)
        {
            CurrentOfficer = new Officer();
            //Add new officer to context
        }
        public async Task SetCurrentUserAsync(string login)
        {
            if (string.IsNullOrEmpty(login))
                throw new ArgumentException("Логин не может отсутствовать.", nameof(login));

            CurrentOfficer = (await m_TVAContext.Database.GetDbConnection().QueryAsync<Officer?>(@"
            SELECT o.OfficerId, o.Name, o.Surname, o.Role, pi.Login 
            FROM AccountOfTrafficViolation.dbo.Officers o (nolock) 
                INNER JOIN
            AccountOfTrafficViolation.dbo.ProfileInfo pi (nolock) ON o.OfficerId = pi.OfficerId
            WHERE pi.Login = @login AND o.IsActive = 1", new { Login = login }))
                .FirstOrDefault();
        }

        public void Dispose()
        {
            m_TVAContext.Dispose();
        }

        private void OnPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
