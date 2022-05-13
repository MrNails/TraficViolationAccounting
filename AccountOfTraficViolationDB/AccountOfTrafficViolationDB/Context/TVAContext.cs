using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccountingOfTrafficViolation.Models;
using AccountOfTrafficViolationDB.Models;
using AccountOfTraficViolationDB.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace AccountOfTrafficViolationDB.Context
{
    public record Credential(string Login, string Password);
    
    public class TVAContext : DbContext
    {
        private readonly string m_connectionString;
        private readonly Credential m_credential;
        
        public TVAContext(string connectionString, Credential credential)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentException("Connection string cannot be empty.");

            if (credential == null)
                throw new ArgumentNullException(nameof(credential));
            
            m_connectionString = connectionString + $"{(connectionString.EndsWith(';') ? string.Empty : ";")}user id={credential.Login};password={credential.Password}";
            m_credential = credential;
        }

        public Credential Credential => m_credential;

        public virtual DbSet<AccidentOnHighway> AccidentOnHighways { get; set; }
        public virtual DbSet<AccidentOnVillage> AccidentOnVillages { get; set; }
        public virtual DbSet<Case> Cases { get; set; }
        public virtual DbSet<GeneralInfo> GeneralInfos { get; set; }
        public virtual DbSet<ParticipantsInformation> ParticipantsInformations { get; set; }
        public virtual DbSet<RoadCondition> RoadConditions { get; set; }
        public virtual DbSet<Officer> Officers { get; set; }
        public virtual DbSet<Vehicle> Vehicles { get; set; }
        public virtual DbSet<CaseVehicle> CaseVehicles { get; set; }
        public virtual DbSet<Victim> Victims { get; set; }
        public virtual DbSet<CaseAccidentPlace> CaseAccidentPlaces { get; set; }
        
        public virtual DbSet<Code> Codes { get; set; }
        public virtual DbSet<CodeBinding> CodeBindings { get; set; }

        public override int SaveChanges()
        {
            SetDateTime();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetDateTime();
            return base.SaveChangesAsync(cancellationToken);
        }

        public void CancelAllChanges()
        {
            var entries = ChangeTracker.Entries().Where(e => e.State == EntityState.Modified);

            foreach (var entry in entries)
                entry.Reload();
        }

        protected void SetDateTime()
        {
            var entries = ChangeTracker.Entries().
                                        Where(e => e.Entity is Case &&
                                                   (e.State == EntityState.Added ||
                                                   e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                ((Case)entry.Entity).Modified = DateTime.Now;

                if (entry.State == EntityState.Added)
                {
                    ((Case)entry.Entity).Created = DateTime.Now;
                }
            }
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(m_connectionString)
                .LogTo(str => Debug.WriteLine(str))
                .UseLazyLoadingProxies();
            
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TVAContext).Assembly);
        }
    }
}
