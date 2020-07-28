using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace AccountingOfTraficViolation.Models
{
    [Table("CaseAccidentPlace")]
    public partial class CaseAccidentPlace : MainTable
    {
        private int caseId;
        private int? accidentOnHighwayId;
        private int? accidentOnVillageId;
        private Case _case;
        private AccidentOnHighway accidentOnHighway;
        private AccidentOnVillage accidentOnVillage;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CaseId
        {
            get { return caseId; }
            set
            {
                if (value < 0)
                {
                    errors["CaseId"] = "Не правильно установлен ид дела.";
                    return;
                }

                if (Case != null && value != Case.Id)
                {
                    Case = null;
                }

                caseId = value;
                OnPropertyChanged("CaseId");
                errors["CaseId"] = null;
            }
        }

        public int? AccidentOnHighwayId
        {
            get { return accidentOnHighwayId; }
            set
            {
                if (value < 0)
                {
                    errors["AccidentOnHighwayId"] = "Не правильно установлен Id ДТП на автодороге.";
                    return;
                }

                if (AccidentOnVillageId != null && value != null)
                {
                    AccidentOnVillageId = null;
                }
                else if (AccidentOnVillageId == null && value == null)
                {
                    errors["AccidentOnHighwayId"] = "Вы не можете сделать Id места ДТП в населённом пункте пустым, пока пустой Id места ДТП на автодороге.";
                    return;
                }

                if (value.HasValue && AccidentOnHighway != null && value.Value != AccidentOnHighway.Id)
                {
                    AccidentOnHighway = null;
                }

                if (value >= 0)
                {
                    accidentOnHighwayId = value;
                    OnPropertyChanged("AccidentOnHighwayId");
                    errors["AccidentOnHighwayId"] = null;
                }
                else
                {
                    errors["AccidentOnHighwayId"] = "Ид проишествия на автодороге не может быть меньше 0.";
                }
            }
        }
        public int? AccidentOnVillageId
        {
            get { return accidentOnVillageId; }
            set
            {
                if (value < 0)
                {
                    errors["AccidentOnVillageId"] = "Не правильно установлен Id ДТП в населённом пункте.";
                    return;
                }

                if (AccidentOnHighwayId != null && value != null)
                {
                    AccidentOnHighwayId = null;
                }
                else if (AccidentOnHighwayId == null && value == null)
                {
                    errors["AccidentOnVillageId"] = "Вы не можете сделать Id места ДТП на автодороге пустым, пока пустой Id места ДТП в населённом пункте.";
                    return;
                }


                if (value.HasValue && AccidentOnVillage != null && value.Value != AccidentOnVillage.Id)
                {
                    AccidentOnVillage = null;
                }

                if (value >= 0)
                {
                    accidentOnVillageId = value;
                    OnPropertyChanged("AccidentOnVillageId");
                    errors["AccidentOnVillageId"] = null;
                }
                else
                {
                    errors["AccidentOnVillageId"] = "Ид проишествия в населённом пункте не может быть меньше 0.";
                }
            }
        }

        public virtual AccidentOnHighway AccidentOnHighway
        {
            get { return accidentOnHighway; }
            set
            {
                if (AccidentOnVillage != null && value != null)
                {
                    AccidentOnVillage = null;
                }
                else if (AccidentOnVillage == null && value == null)
                {
                    errors["AccidentOnHighway"] = "Вы не можете сделать место ДТП на автодороге пустым, пока пустое место ДТП в населённом пункте.";
                    return;
                }

                if (value == null)
                {
                    AccidentOnHighwayId = null;
                }
                else
                {
                    AccidentOnHighwayId = value.Id;
                }

                accidentOnHighway = value;
                OnPropertyChanged("AccidentOnHighway");
                errors["AccidentOnHighway"] = null;
            }
        }
        public virtual AccidentOnVillage AccidentOnVillage
        {
            get { return accidentOnVillage; }
            set
            {
                if (AccidentOnHighway != null && value != null)
                {
                    AccidentOnHighway = null;
                }
                else if (AccidentOnHighway == null && value == null)
                {
                    errors["AccidentOnVillage"] = "Вы не можете сделать место ДТП в населённом пункте пустым, пока пустое место ДТП на автодороге.";
                    return;
                }

                if (value == null)
                {
                    AccidentOnVillageId = null;
                }
                else
                {
                    AccidentOnVillageId = value.Id;
                }


                accidentOnVillage = value;
                OnPropertyChanged("AccidentOnVillage");
                errors["AccidentOnVillage"] = null;
            }
        }

        public virtual Case Case
        {
            get { return _case; }
            set
            {
                if (value == null)
                {
                    errors["Case"] = "Дело не может отсутствовать.";
                    return;
                }
                else
                {
                    _case = value;
                    CaseId = value.Id;
                }

                OnPropertyChanged("Case");
                errors["Case"] = null;
            }
        }
    }
}
