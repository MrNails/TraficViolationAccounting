using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;



namespace AccountingOfTraficViolation.Models
{
    public partial class Case : MainTable
    {
        private int generalInfoId;
        private int roadConditionId;
        private string createrLogin;
        private string state;
        private DateTime openAt;
        private DateTime closeAt;
        private DateTime createdAt;
        private DateTime updatedAt;
        private GeneralInfo generalInfo;
        private RoadCondition roadCondition;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Case()
        {
            ParticipantsInformations = new HashSet<ParticipantsInformation>();
            Vehicles = new HashSet<Vehicle>();
            Victims = new HashSet<Victim>();
        }

        public int Id { get; set; }

        public int GeneralInfoId { get; set; }

        public int RoadConditionId { get; set; }

        public DateTime OpenAt
        {
            get { return openAt; }
            set
            {
                openAt = value;
                OnPropertyChanged("OpenAt");
            }
        }

        public DateTime CloseAt
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
        public string State { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public virtual CaseAccidentPlace CaseAccidentPlace { get; set; }

        public virtual GeneralInfo GeneralInfo { get; set; }

        public virtual RoadCondition RoadCondition { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParticipantsInformation> ParticipantsInformations { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Vehicle> Vehicles { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Victim> Victims { get; set; }
    }
}
