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
                    OnErrorInput("�� ��������� ���������� �� ����");

                    return;
                }

                if (value != Case.Id)
                {
                    Case = null;
                }

                caseId = value;

                OnPropertyChanged("CaseId");
            }
        }

        public int? AccidentOnHighwayId
        {
            get { return accidentOnHighwayId; }
            set
            {
                if (value < 0)
                {
                    OnErrorInput("�� ��������� ���������� �� ��� �� ����������");

                    return;
                }

                if (AccidentOnVillageId != null && value != null)
                {
                    AccidentOnVillageId = null;
                }
                else if (AccidentOnHighwayId == null && value == null)
                {
                    OnErrorInput("�� �� ������ ������� �� ����� ��� � ��������� ������ ������, ���� ������ �� ����� ��� �� ����������");

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
                }
                else
                {

                    OnErrorInput("�� ����������� �� ���������� �� ����� ���� ������ 0");
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
                    OnErrorInput("�� ��������� ���������� �� ��� � ��������� ������");

                    return;
                }

                if (AccidentOnHighwayId != null && value != null)
                {
                    AccidentOnHighwayId = null;
                }
                else if (AccidentOnHighwayId == null && value == null)
                {
                    OnErrorInput("�� �� ������ ������� �� ����� ��� �� ���������� ������, ���� ������ �� ����� ��� � ��������� ������");

                    return;
                }


                if (value.HasValue && AccidentOnVillage != null && value.Value != AccidentOnVillage.Id)
                {
                    AccidentOnVillage = null;
                }

                if (value.Value >= 0)
                {

                    accidentOnVillageId = value;
                    OnPropertyChanged("AccidentOnVillageId");
                }
                else
                {

                    OnErrorInput("�� ����������� � ��������� ������ �� ����� ���� ������ 0");
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
                    OnErrorInput("�� �� ������ ������� ����� ��� �� ���������� ������, ���� ������ ����� ��� � ��������� ������");

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
                    OnErrorInput("�� �� ������ ������� ����� ��� � ��������� ������ ������, ���� ������ ����� ��� �� ����������");

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
            }
        }

        public virtual Case Case
        {
            get { return _case; }
            set
            {
                if (value == null)
                {
                    OnErrorInput("���� �� ����� �������������");

                    return;
                }
                else
                {
                    CaseId = value.Id;
                }


                _case = value;
                OnPropertyChanged("Case");
            }
        }
    }
}
