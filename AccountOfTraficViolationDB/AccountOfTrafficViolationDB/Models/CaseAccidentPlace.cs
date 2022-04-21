using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AccountingOfTrafficViolation;
using AccountOfTrafficViolationDB.Helpers;

namespace AccountOfTrafficViolationDB.Models
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
        
        [NotAssign]
        public int CaseId
        {
            get { return caseId; }
            set
            {
                if (value < 0)
                {
                    errors["CaseId"] = "�� ��������� ���������� �� ����.";
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
                    errors["AccidentOnHighwayId"] = "�� ��������� ���������� Id ��� �� ����������.";
                    return;
                }

                if (AccidentOnVillageId != null && value != null)
                {
                    AccidentOnVillageId = null;
                }
                else if (AccidentOnVillageId == null && value == null)
                {
                    errors["AccidentOnHighwayId"] = "�� �� ������ ������� Id ����� ��� � ��������� ������ ������, ���� ������ Id ����� ��� �� ����������.";
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
                    errors["AccidentOnHighwayId"] = "�� ����������� �� ���������� �� ����� ���� ������ 0.";
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
                    errors["AccidentOnVillageId"] = "�� ��������� ���������� Id ��� � ��������� ������.";
                    return;
                }

                if (AccidentOnHighwayId != null && value != null)
                {
                    AccidentOnHighwayId = null;
                }
                else if (AccidentOnHighwayId == null && value == null)
                {
                    errors["AccidentOnVillageId"] = "�� �� ������ ������� Id ����� ��� �� ���������� ������, ���� ������ Id ����� ��� � ��������� ������.";
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
                    errors["AccidentOnVillageId"] = "�� ����������� � ��������� ������ �� ����� ���� ������ 0.";
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
                    errors["AccidentOnHighway"] = "�� �� ������ ������� ����� ��� �� ���������� ������, ���� ������ ����� ��� � ��������� ������.";
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
                    errors["AccidentOnVillage"] = "�� �� ������ ������� ����� ��� � ��������� ������ ������, ���� ������ ����� ��� �� ����������.";
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

        [NotAssign] public virtual Case Case
        {
            get { return _case; }
            set
            {
                if (value == null)
                {
                    errors["Case"] = "���� �� ����� �������������.";
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
