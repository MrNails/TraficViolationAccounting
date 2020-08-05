using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using AccountingOfTraficViolation.Services;

namespace AccountingOfTraficViolation.Models
{
    public partial class Case : MainTable
    {
        private string state;
        private DateTime openAt;
        private DateTime? closeAt;
        private DateTime updatedAt;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Case()
        {
            ParticipantsInformations = new HashSet<ParticipantsInformation>();
            Vehicles = new HashSet<Vehicle>();
            Victims = new HashSet<Victim>();

            OpenAt = DateTime.Now;
        }

        [NotAssign]
        public int Id { get; set; }

        public int GeneralInfoId { get; set; }

        public int RoadConditionId { get; set; }

        public DateTime OpenAt
        {
            get { return openAt; }
            set
            {
                if (value < MinimumDate)
                {
                    openAt = MinimumDate;
                }
                else
                {
                    openAt = value;
                }
                OnPropertyChanged("OpenAt");
            }
        }

        public DateTime? CloseAt
        {
            get { return closeAt; }
            set
            {
                closeAt = value;
                OnPropertyChanged("CloseAt");
            }
        }

        [Required]
        [StringLength(20)]
        public string CreaterLogin { get; set; }

        [Required]
        [StringLength(12)]
        public string State
        {
            get { return state; }
            set
            {
                state = value;
                OnPropertyChanged("State");
            }
        }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt
        {
            get { return updatedAt; }
            set
            {
                updatedAt = value;
                OnPropertyChanged("UpdatedAt");
            }
        }

        [NotAssign]
        public virtual CaseAccidentPlace CaseAccidentPlace { get; set; }

        [NotAssign]
        public virtual GeneralInfo GeneralInfo { get; set; }

        [NotAssign]
        public virtual RoadCondition RoadCondition { get; set; }

        [NotAssign]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParticipantsInformation> ParticipantsInformations { get; set; }

        [NotAssign]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Vehicle> Vehicles { get; set; }

        [NotAssign]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Victim> Victims { get; set; }
    }
}
