using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AccountOfTrafficViolationDB.Helpers;

namespace AccountOfTrafficViolationDB.Models
{
    public partial class RoadCondition : MainTable
    {
        private byte surfaceType;
        private byte illumination;
        private byte artificialConstructions;
        private byte engineeringTransportEquipment;
        private byte weatherCondition;
        private byte incidentPlace;
        private string surfaceState;
        private string placeElement;
        private string technicalTool;
        private string roadDisadvantages;
        
        public RoadCondition()
        {
            SurfaceState = string.Empty;
            PlaceElement = string.Empty;
            TechnicalTool = string.Empty;
            RoadDisadvantages = string.Empty;
        }

        [NotAssign, Column("RoadConditionId")]
        public int Id { get; init; }

        public byte SurfaceType
        {
            get { return surfaceType; }
            set
            {
                if (value >= 0 && value < 10)
                {
                    surfaceType = value;
                    OnPropertyChanged("SurfaceType");
                    errors["SurfaceType"] = null;
                }
                else
                {
                    errors["SurfaceType"] = "�� ��������� ���� ���� ��������.";
                }
            }
        }

        [Required]
        [StringLength(2)]
        public string SurfaceState
        {
            get { return surfaceState; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    errors["SurfaceState"] = "��������� ������ �� ����� ���� ������.";
                }
                else if (value.Length > 2)
                {
                    errors["SurfaceState"] = "���������� �������� � ��������� ������ �� ����� ��������� 2.";
                }
                else
                {
                    errors["SurfaceState"] = null;
                }

                surfaceState = value;
                OnPropertyChanged("SurfaceState");
            }
        }

        public byte Illumination
        {
            get { return illumination; }
            set
            {
                if (value >= 0 && value < 10)
                {
                    illumination = value;
                    OnPropertyChanged("Illumination");
                    errors["Illumination"] = null;
                }
                else
                {
                    errors["Illumination"] = "�� ��������� ���� ������������.";
                }
            }
        }

        public byte ArtificialConstructions
        {
            get { return artificialConstructions; }
            set
            {
                if (value >= 0 && value < 10)
                {
                    artificialConstructions = value;
                    OnPropertyChanged("ArtificialConstructions");
                    errors["ArtificialConstructions"] = null;
                }
                else
                {
                    errors["ArtificialConstructions"] = "�� ��������� ���� ���� ������������� ����������.";
                }
            }
        }

        [Required]
        [StringLength(6)]
        public string PlaceElement
        {
            get { return placeElement; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    errors["PlaceElement"] = "������� ��������� �� ����� ���� ������.";
                }
                else if (value.Length > 6)
                {
                    errors["PlaceElement"] = "���������� �������� � �������� ��������� �� ����� ��������� 6.";
                }
                else
                {
                    errors["PlaceElement"] = null;
                }

                placeElement = value;
                OnPropertyChanged("PlaceElement");
            }
        }

        public byte EngineeringTransportEquipment
        {
            get { return engineeringTransportEquipment; }
            set
            {
                if (value >= 0 && value < 10)
                {
                    engineeringTransportEquipment = value;
                    OnPropertyChanged("EngineeringTranpsortEquipment");
                    errors["EngineeringTranpsortEquipment"] = null;
                }
                else
                {
                    errors["EngineeringTranpsortEquipment"] = "�� ��������� ���� ���� ���������-������������� ������������.";
                }
            }
        }

        [Required]
        [StringLength(10)]
        public string TechnicalTool
        {
            get { return technicalTool; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    errors["TechnicalTool"] = "���� � ������������ ���������������� �� ����� ���� ������.";
                }
                else if (value.Length > 10)
                {
                    errors["TechnicalTool"] = "���������� �������� � ���� ������������ ���������������� �� ����� ��������� 10.";
                }
                else
                {
                    errors["TechnicalTool"] = null;
                }

                technicalTool = value;
                OnPropertyChanged("TechnicalTool");
            }
        }

        public byte WeatherCondition
        {
            get { return weatherCondition; }
            set
            {
                if (value >= 0 && value < 10)
                {
                    weatherCondition = value;
                    OnPropertyChanged("WeatherCondition");
                    errors["WeatherCondition"] = null;
                }
                else
                {
                    errors["WeatherCondition"] = "�� ��������� ���� ���� �������� �������.";
                }
            }
        }

        [Required]
        [StringLength(10)]
        public string RoadDisadvantages
        {
            get { return roadDisadvantages; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    errors["RoadDisadvantages"] = "���� � ��������������� ������ �� ����� ���� ������.";
                }
                else if (value.Length > 10)
                {
                    errors["RoadDisadvantages"] = "���������� �������� � ���� � ��������������� ������ �� ����� ��������� 10.";
                }
                else
                {
                    errors["RoadDisadvantages"] = null;
                }

                roadDisadvantages = value;
                OnPropertyChanged("RoadDisadvantages");
            }
        }

        public byte IncidentPlace
        {
            get { return incidentPlace; }
            set
            {
                if (value >= 0 && value < 10)
                {
                    incidentPlace = value;
                    OnPropertyChanged("IncidentPlace");
                }
                else
                {
                    errors["IncidentPlace"] = "�� ��������� ���� ���� ����� ������������ ���.";
                }
            }
        }

        [NotAssign] public int CaseId { get; set; }
        [NotAssign] public virtual Case Case { get; set; }
    }
}
