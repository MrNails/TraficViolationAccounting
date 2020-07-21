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
                    errors["SurfaceType"] = "Не правилный ввод типа покрытия.";
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
                    errors["SurfaceState"] = "Состояние дороги не может быть пустым.";
                    surfaceState = null;
                    return;
                }

                if (surfaceStateRegex.IsMatch(value) || int.TryParse(value, out int st))
                {
                    surfaceState = value.GetStrWithoutSeparator(',');
                    errors["SurfaceState"] = null;
                }
                else
                {
                    surfaceState = value;
                    errors["SurfaceState"] = "Строка не соответствует ни одному из ниже перечисленных форматов:\n" +
                                             "\t- 0,0\n" +
                                             "\t- 00";
                }

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
                    errors["Illumination"] = "Не правилный ввод освещённости.";
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
                    errors["ArtificialConstructions"] = "Не правилный ввод кода исскуственных сооружений.";
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
                    errors["PlaceElement"] = "Элемент не может быть пустым.";
                    placeElement = null;
                }
                else if (placeElementRegex.IsMatch(value) || int.TryParse(value, out int pl))
                {
                    placeElement = value.GetStrWithoutSeparator(',');
                    errors["PlaceElement"] = null;
                }
                else
                {
                    placeElement = value;
                    errors["PlaceElement"] = "Строка не соответствует ни одному из ниже перечисленных форматов:\n" +
                                             "\t- 00,00,00\n" +
                                             "\t- 000000";
                }

                OnPropertyChanged("PlaceElement");
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
                    errors["EngineeringTranpsortEquipment"] = "Не правилный ввод кода инженерно-транспортного оборудования.";
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
                    errors["TechnicalTool"] = "Поле с техническими приспособлениями не может быть пустым.";
                    technicalTool = null;
                }
                else if (technicalToolRegex.IsMatch(value) || int.TryParse(value, out int tt))
                {
                    technicalTool = value.GetStrWithoutSeparator(',');
                    errors["TechnicalTool"] = null;
                }
                else
                {
                    technicalTool = value;
                    errors["TechnicalTool"] = "Строка не соответствует ни одному из ниже перечисленных форматов:\n" +
                                             "\t- 00,00,00,00,00\n" +
                                             "\t- 0000000000";
                }

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
                    errors["WeatherCondition"] = "Не правилный ввод кода погодных условий.";
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
                    errors["RoadDisadvantages"] = "Поле с неисправностями дороги не может быть пустым.";
                    roadDisadvantages = null;
                }
                else if (roadDisadvantagesRegex.IsMatch(value) || int.TryParse(value, out int rd))
                {
                    roadDisadvantages = value.GetStrWithoutSeparator(',');
                    errors["RoadDisadvantages"] = null;
                }
                else
                {
                    roadDisadvantages = value;
                    errors["RoadDisadvantages"] = "Строка не соответствует ни одному из ниже перечисленных форматов:\n" +
                                             "\t- 00,00,00,00,00\n" +
                                             "\t- 0000000000";
                }

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
                    errors["IncidentPlace"] = "Не правилный ввод кода места концентрации ДТП.";
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Case> Cases { get; set; }
    }
}
