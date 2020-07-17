using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AccountingOfTraficViolation.Models;
using AccountingOfTraficViolation.Services;

namespace AccountingOfTraficViolation.ViewModels
{
    public class RoadConditionVM : IDataErrorInfo
    {
        public RoadConditionVM() : this(null)
        { }
        public RoadConditionVM(RoadCondition roadCondition)
        {
            if (roadCondition != null)
            {
                RoadCondition = roadCondition.Clone();
            }
            else
            {
                RoadCondition = new RoadCondition();
            }

            SurfaceState = RoadCondition.SurfaceState;
            PlaceElement = RoadCondition.PlaceElement;
            TechnicalTool = RoadCondition.TechnicalTool;
            RoadDisadvantages = RoadCondition.RoadDisadvantages;
        }

        public string this[string columnName]
        {
            get
            {
                string _error = null;
                Regex regex;

                switch (columnName)
                {
                    case "SurfaceState":
                        regex = new Regex(@"\d{1},\d{1}$");
                        if (int.TryParse(SurfaceState, out int st))
                        {
                            SurfaceState = SurfaceState.AddSeparator(',', 1);
                        }

                        if (string.IsNullOrEmpty(SurfaceState))
                        {
                            _error = "Строка 'Состояние покрытия' не может быть пустой.";
                        }
                        else if (!regex.IsMatch(SurfaceState))
                        {
                            _error = "Строка не соответствует шаблону:\n\t0,0";
                        }
                        else
                        {
                            RoadCondition.SurfaceState = SurfaceState;
                        }
                        break;
                    case "PlaceElement":
                        regex = new Regex(@"\d{2},\d{2},\d{2}$");
                        if (int.TryParse(PlaceElement, out int pl))
                        {
                            PlaceElement = PlaceElement.AddSeparator(',', 2, 5);
                        }

                        if (string.IsNullOrEmpty(PlaceElement))
                        {
                            _error = "Строка 'Элементы участка' не может быть пустой.";
                        }
                        else if (!regex.IsMatch(PlaceElement))
                        {
                            _error = "Строка не соответствует шаблону:\n\t00,00,00";
                        }
                        else
                        {
                            RoadCondition.PlaceElement = PlaceElement;
                        }
                        break;
                    case "TechnicalTool":
                        regex = new Regex(@"\d{2},\d{2},\d{2},\d{2},\d{2}$");
                        if (long.TryParse(TechnicalTool, out long tl))
                        {
                            TechnicalTool = TechnicalTool.AddSeparator(',', 2, 5, 8, 11);
                        }

                        if (string.IsNullOrEmpty(TechnicalTool))
                        {
                            _error = "Строка 'Технические способы организации дорожного движения' не может быть пустой.";
                        }
                        else if (!regex.IsMatch(TechnicalTool))
                        {
                            _error = "Строка не соответствует шаблону:\n\t00,00,00,00,00";
                        }
                        else
                        {
                            RoadCondition.TechnicalTool = TechnicalTool;
                        }
                        break;
                    case "RoadDisadvantages":
                        regex = new Regex(@"\d{2},\d{2},\d{2},\d{2},\d{2}$");
                        if (long.TryParse(RoadDisadvantages, out long rd))
                        {
                            RoadDisadvantages = RoadDisadvantages.AddSeparator(',', 2, 5, 8, 11);
                        }

                        if (string.IsNullOrEmpty(RoadDisadvantages))
                        {
                            _error = "Строка 'Имеющиеся недостатки в содержании дороги (улицы)' не может быть пустой.";
                        }
                        else if (!regex.IsMatch(RoadDisadvantages))
                        {
                            _error = "Строка не соответствует шаблону:\n\t00,00,00,00,00";
                        }
                        else
                        {
                            RoadCondition.RoadDisadvantages = RoadDisadvantages;
                        }
                        break;
                    default:
                        break;
                }

                return _error;
            }
        }

        public RoadCondition RoadCondition { get; set; }
        public string SurfaceState { get; set; }
        public string PlaceElement { get; set; }
        public string TechnicalTool { get; set; }
        public string RoadDisadvantages { get; set; }

        public string Error => throw new NotImplementedException();
    }
}
