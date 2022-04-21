using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccountOfTrafficViolationDB.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountOfTrafficViolationDB.Context
{
    public class TVAContext : DbContext
    {
        private readonly string m_connectionString;
        
        public TVAContext(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentException("Connection string cannot be empty.");
            
            m_connectionString = connectionString;
        }

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
        // public virtual DbSet<CodeInfo> CodeInfos { get; set; }

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
            optionsBuilder.UseSqlServer(m_connectionString);
            optionsBuilder.LogTo(str => Debug.WriteLine(str));
            
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TVAContext).Assembly);
        }
    }
}
