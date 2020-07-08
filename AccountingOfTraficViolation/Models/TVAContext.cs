using System;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace AccountingOfTraficViolation.Models
{

    public partial class TVAContext : DbContext
    {
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
    }
}
