using System;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;

namespace AccountingOfTraficViolation.Models
{
    class ContextInitializer : CreateDatabaseIfNotExists<TVAContext>
    {
        protected override void Seed(TVAContext context)
        {
            User user = new User()
            {
                Login = "admin",
                Password = "12345",
                Role = 0
            };

            context.Users.Add(user);
            context.SaveChanges();
        }
    }

    public partial class TVAContext : DbContext
    {
        static TVAContext()
        {
            Database.SetInitializer<TVAContext>(new ContextInitializer());
        }

        public TVAContext()
            : base("name=TraficViolationAccounting")
        {
            Database.CreateIfNotExists();
        }

        public virtual DbSet<AccidentOnHighway> AccidentOnHighways { get; set; }
        public virtual DbSet<AccidentOnVillage> AccidentOnVillages { get; set; }
        public virtual DbSet<Case> Cases { get; set; }
        public virtual DbSet<GeneralInfo> GeneralInfos { get; set; }
        public virtual DbSet<ParticipantsInformation> ParticipantsInformations { get; set; }
        public virtual DbSet<RoadCondition> RoadConditions { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Vehicle> Vehicles { get; set; }
        public virtual DbSet<Victim> Victims { get; set; }
        public virtual DbSet<CaseAccidentPlace> CaseAccidentPlaces { get; set; }
        public virtual DbSet<CodeInfo> CodeInfos { get; set; }

        public override int SaveChanges()
        {
            SetDateTime();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync()
        {
            SetDateTime();
            return base.SaveChangesAsync();
        }

        public void CancelAllChanges()
        {
            var entries = ChangeTracker.Entries().Where(e => e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                entry.Reload();
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Case>()
                .HasOptional(e => e.CaseAccidentPlace)
                .WithRequired(e => e.Case);

            modelBuilder.Entity<Case>()
                .HasMany(e => e.ParticipantsInformations)
                .WithRequired(e => e.Case)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Case>()
                .HasMany(e => e.Vehicles)
                .WithRequired(e => e.Case)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Case>()
                .HasMany(e => e.Victims)
                .WithRequired(e => e.Case)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<GeneralInfo>()
                .Property(e => e.FillTime)
                .HasPrecision(0);

            modelBuilder.Entity<GeneralInfo>()
                .HasMany(e => e.Cases)
                .WithRequired(e => e.GeneralInfo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<RoadCondition>()
                .HasMany(e => e.Cases)
                .WithRequired(e => e.RoadCondition)
                .WillCascadeOnDelete(false);
        }

        protected void SetDateTime()
        {
            var entries = ChangeTracker.Entries().
                                        Where(e => e.Entity is Case &&
                                                   (e.State == EntityState.Added ||
                                                   e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                ((Case)entry.Entity).UpdatedAt = DateTime.Now;

                if (entry.State == EntityState.Added)
                {
                    ((Case)entry.Entity).CreatedAt = DateTime.Now;
                }
            }
        }
    }
}
