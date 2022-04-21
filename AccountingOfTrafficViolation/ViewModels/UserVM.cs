using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AccountingOfTrafficViolation.Models;
using AccountingOfTrafficViolation.Services;
using AccountOfTrafficViolationDB.Context;
using AccountOfTrafficViolationDB.Models;
using AccountOfTrafficViolationDB.ProxyModels;
using Dapper;
using Microsoft.EntityFrameworkCore;

namespace AccountingOfTrafficViolation.ViewModels
{
    public class UserVM : IDisposable, INotifyPropertyChanged
    {
        private TVAContext m_TVAContext;
        private UserInfo m_currentFindOfficer;

        public event PropertyChangedEventHandler PropertyChanged;

        public UserVM()
        {
            m_TVAContext = new TVAContext(GlobalSettings.ConnectionStrings[Constants.DefaultDB]);
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

        public UserInfo? CurrentOfficer
        {
            get { return m_currentFindOfficer; }
            private set
            {
                m_currentFindOfficer = value;
                OnPropertyChanged("CurrentUser");
            }
        }

        //TODO: Fix user finding
        public async Task<bool> CheckIfCurrenUserLoginExistAsync()
        {
            var res = await m_TVAContext.Officers.FromSqlRaw(@"
            SELECT * FROM AccountOfTrafficViolation.dbo.Officers o (nolock) 
            WHERE OfficerId IN (SELECT OfficerId FROM AccountOfTrafficViolation.dbo.ProfileInfo pi (nolock) WHERE o.OfficerId = pi.OfficerId AND pi.Login = {0})", CurrentOfficer.UserProfile)
                .FirstOrDefaultAsync();
            
            return res != null;
        }

        public async Task ConfirmChangeAsync()
        {
            if (CurrentOfficer != null)
            {
                var officer = await m_TVAContext.Officers.FirstOrDefaultAsync(u => u.Id == CurrentOfficer.OfficerId);

                if (officer == null)
                {
                    var salt = RandomNumberGenerator.GetBytes(16);
                    var pwd = CryptoHelper.EncryptData(Encoding.UTF8.GetBytes(CurrentOfficer.UserProfile.Password), salt);
                    
                    await m_TVAContext.Database.ExecuteSqlRawAsync(
                        "exec AccountOfTrafficViolation.dbo.CreateOfficer {0}, {1}, {2}, {3}, {4}, {5}", 
                        CurrentOfficer.UserProfile.Login, Encoding.UTF8.GetString(pwd), Encoding.UTF8.GetString(salt), 
                        CurrentOfficer.Role, CurrentOfficer.Name, CurrentOfficer.Surname);
                }
            }

            await m_TVAContext.SaveChangesAsync();
        }
        public void DiscardChange()
        {
            m_TVAContext.CancelAllChanges();
        }
        public Task DeleteCurrentUserAsync()
        {
            if (CurrentOfficer == null)
                return Task.CompletedTask;

            CurrentOfficer = null;

            return m_TVAContext.Database.ExecuteSqlRawAsync("exec AccountOfTrafficViolation.dbo.DeleteUser {0}", CurrentOfficer.OfficerId);
        }
        public void AddNewUser(byte role = (byte)UserRole.User)
        {
            CurrentOfficer = new UserInfo
            {
                Role = role
            };
        }
        public async Task SetCurrentUserAsync(string login)
        {
            if (string.IsNullOrEmpty(login))
                throw new ArgumentException("Логин не может отсутствовать.", nameof(login));

            CurrentOfficer = (await m_TVAContext.Database.GetDbConnection().QueryAsync<UserInfo?>(@"
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
