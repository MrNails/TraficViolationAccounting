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
        private string m_state;

        public Case()
        {
            ParticipantsInformations = new HashSet<ParticipantsInformation>();
            CaseVehicles = new HashSet<CaseVehicle>();
            Victims = new HashSet<Victim>();

            OpenAt = DateTime.Now;
        }

        [NotAssign]
        [Column("CaseId")]
        public int Id { get; set; }
        
        public string OfficerId { get; set; }

        public string State
        {
            get => m_state;
            set
            {
                if (string.IsNullOrEmpty(value))
                    errors["State"] = "Состояние не может быть пустым";
                else
                    errors["State"] = null;
                
                m_state = value;
                OnPropertyChanged();
            }
        }

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
        [NotAssign, NotMapped] public virtual CaseAccidentPlace CaseAccidentPlace { get; set; }
    }
}
