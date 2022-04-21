using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using AccountingOfTrafficViolation;
using AccountingOfTrafficViolation.Models;
using AccountOfTrafficViolationDB.Helpers;

namespace AccountOfTrafficViolationDB.Models
{
    [Table("Cases")]
    public partial class Case : MainTable
    {
        private DateTime m_openAt;
        private DateTime? m_closeAt;
        private DateTime m_modified;
        
        public Case()
        {
            ParticipantsInformations = new HashSet<ParticipantsInformation>();
            CaseVehicles = new HashSet<CaseVehicle>();
            Victims = new HashSet<Victim>();

            OpenAt = DateTime.Now;
        }

        [NotAssign]
        [Column("CaseId")]
        public int Id { get; init; }
        
        public int OfficerId { get; set; }
        
        public string State { get; set; }

        public DateTime OpenAt
        {
            get { return m_openAt; }
            set
            {
                if (value < MinimumDate)
                {
                    m_openAt = MinimumDate;
                }
                else
                {
                    m_openAt = value;
                }
                OnPropertyChanged("OpenAt");
            }
        }

        public DateTime? CloseAt
        {
            get { return m_closeAt; }
            set
            {
                m_closeAt = value;
                OnPropertyChanged("CloseAt");
            }
        }

        public DateTime Created { get; set; }

        public DateTime Modified
        {
            get { return m_modified; }
            set
            {
                m_modified = value;
                OnPropertyChanged("UpdatedAt");
            }
        }
        
        [NotAssign] public virtual Officer Officer { get; set; }
        [NotAssign] public virtual GeneralInfo GeneralInfo { get; set; }
        [NotAssign] public virtual RoadCondition RoadCondition { get; set; }
        [NotAssign] public virtual ICollection<CaseVehicle> CaseVehicles { get; set; }
        [NotAssign] public virtual ICollection<ParticipantsInformation> ParticipantsInformations { get; set; }
        [NotAssign] public virtual ICollection<Victim> Victims { get; set; }
        [NotAssign] public virtual CaseAccidentPlace CaseAccidentPlace { get; set; }
    }
}
