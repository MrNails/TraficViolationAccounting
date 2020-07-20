using AccountingOfTraficViolation.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using System.Text.RegularExpressions;

namespace AccountingOfTraficViolation.Models
{
    public partial class RoadCondition : MainTable
    {
        private byte surfaceType;
        private byte illumination;
        private byte artificialConstructions;
        private byte engineeringTranpsortEquipment;
        private byte weatherCondition;
        private byte incidentPlace;
        private string surfaceState;
        private string placeElement;
        private string technicalTool;
        private string roadDisadvantages;
        private Regex surfaceStateRegex;
        private Regex placeElementRegex;
        private Regex roadDisadvantagesRegex;
        private Regex technicalToolRegex;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RoadCondition()
        {
            Cases = new HashSet<Case>();

            surfaceStateRegex = new Regex(@"\d{1},\d{1}$");
            placeElementRegex = new Regex(@"\d{2},\d{2},\d{2}$");
            roadDisadvantagesRegex = new Regex(@"\d{2},\d{2},\d{2},\d{2},\d{2}$");
            technicalToolRegex = new Regex(@"\d{2},\d{2},\d{2},\d{2},\d{2}$");

            SurfaceState = "";
            PlaceElement = "";
            TechnicalTool = "";
            RoadDisadvantages = "";
        }

        public int Id { get; set; }

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
                    return;
                }

                if (surfaceStateRegex.IsMatch(value) || int.TryParse(value, out int st))
                {
                    surfaceState = value.GetStrWithoutSeparator(',');
                    OnPropertyChanged("SurfaceState");
                    errors["SurfaceState"] = null;
                }
                else
                {
                    errors["SurfaceState"] = "������ �� ������������� �� ������ �� ���� ������������� ��������:\n" +
                                             "\t- 0,0\n" +
                                             "\t- 00";
                }
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
                    errors["PlaceElement"] = "������� �� ����� ���� ������.";
                    return;
                }

                else if (placeElementRegex.IsMatch(value) || int.TryParse(value, out int pl))
                {
                    placeElement = value.GetStrWithoutSeparator(',');
                    OnPropertyChanged("PlaceElement");
                    errors["PlaceElement"] = null;
                }
                else
                {
                    errors["PlaceElement"] = "������ �� ������������� �� ������ �� ���� ������������� ��������:\n" +
                                             "\t- 00,00,00\n" +
                                             "\t- 000000";
                }
            }
        }

        public byte EngineeringTranpsortEquipment
        {
            get { return engineeringTranpsortEquipment; }
            set
            {
                if (value >= 0 && value < 10)
                {
                    engineeringTranpsortEquipment = value;
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
                    return;
                }

                if (technicalToolRegex.IsMatch(value) || int.TryParse(value, out int tt))
                {
                    technicalTool = value.GetStrWithoutSeparator(',');
                    OnPropertyChanged("TechnicalTool");
                    errors["TechnicalTool"] = null;
                }
                else
                {
                    errors["TechnicalTool"] = "������ �� ������������� �� ������ �� ���� ������������� ��������:\n" +
                                             "\t- 00,00,00,00,00\n" +
                                             "\t- 0000000000";
                }
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
                    return;
                }

                if (roadDisadvantagesRegex.IsMatch(value) || int.TryParse(value, out int rd))
                {
                    roadDisadvantages = value.GetStrWithoutSeparator(',');
                    OnPropertyChanged("RoadDisadvantages");
                    errors["RoadDisadvantages"] = null;
                }
                else
                {
                    errors["RoadDisadvantages"] = "������ �� ������������� �� ������ �� ���� ������������� ��������:\n" +
                                             "\t- 00,00,00,00,00\n" +
                                             "\t- 0000000000";
                }
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Case> Cases { get; set; }
    }
}
