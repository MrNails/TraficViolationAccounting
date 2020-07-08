using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RoadCondition()
        {
            Cases = new HashSet<Case>();
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
                }
                else
                {
                    OnErrorInput("Не правилный ввод типа покрытия.");
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
                    OnErrorInput("Состояние дороги не может быть пустым");

                    return;
                }

                if (value.Length <= 2)
                {
                    surfaceState = value;
                }
                else
                {
                    surfaceState = value.Remove(2);
                    OnErrorInput("Количество символов в состоянии дороги не может быть больше 2");
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
                }
                else
                {
                    OnErrorInput("Не правилный ввод освещённости.");
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
                }
                else
                {
                    OnErrorInput("Не правилный ввод кода исскуственных сооружений.");
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
                    OnErrorInput("Элемент не может быть пустым");

                    return;
                }

                if (value.Length <= 6)
                {
                    placeElement = value;
                }
                else
                {
                    placeElement = value.Remove(6);
                    OnErrorInput("Количество символов в элементе дороги не может быть больше 6");
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
                }
                else
                {
                    OnErrorInput("Не правилный ввод кода инженерно-транспортного оборудования.");
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
                    OnErrorInput("Поле с техническими приспособлениями не может быть пустым");

                    return;
                }

                if (value.Length <= 10)
                {
                    technicalTool = value;
                }
                else
                {
                    technicalTool = value.Remove(10);
                    OnErrorInput("Количество символов в поле с техническими приспособлениями не может быть больше 10");
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
                }
                else
                {
                    OnErrorInput("Не правилный ввод кода погодных условий.");
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
                    OnErrorInput("Поле с неисправностями дороги не может быть пустым");

                    return;
                }

                if (value.Length <= 10)
                {
                    roadDisadvantages = value;
                }
                else
                {
                    roadDisadvantages = value.Remove(10);
                    OnErrorInput("Количество символов в поле с неисправностями дороги не может быть больше 10");
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
                    OnErrorInput("Не правилный ввод кода места концентрации ДТП.");
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Case> Cases { get; set; }
    }
}
