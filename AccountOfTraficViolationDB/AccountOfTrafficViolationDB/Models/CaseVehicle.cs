using System.ComponentModel.DataAnnotations;
using AccountingOfTrafficViolation;
using AccountOfTrafficViolationDB.Helpers;

namespace AccountOfTrafficViolationDB.Models;

public class CaseVehicle : MainTable
{
    private bool trailerAvailability;
    private string surname;
    private string plateNumber;
    private string frameNumber;
    private string chasisNumber;
    private string licenceNumber;
    private string licenceSeries;
    private string corruptionCode;
    private string technicalFaults;
    private string activityLicensingInfo;
    private Vehicle m_vehicle;

    public CaseVehicle()
    {
        DriverSurname = string.Empty;
        PlateNumber = string.Empty;
        FrameNumber = string.Empty;
        ChasisNumber = string.Empty;
        LicenceNumber = string.Empty;
        LicenceSeries = string.Empty;
        CorruptionCode = string.Empty;
        TechnicalFaults = string.Empty;
        ActivityLicensingInfo = string.Empty;
    }
    
    public int VehicleId { get; set; }
    
    public int CaseId { get; set; }

    [Required]
    [StringLength(8)]
    public string PlateNumber
    {
        get { return plateNumber; }
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                errors["PlateNumber"] = "Номерной знак машины не может быть пустым.";
            }
            else if (value.Length <= 8)
            {
                errors["PlateNumber"] = null;
            }
            else
            {
                errors["PlateNumber"] = "Количество символов в номерном знаке не может быть больше 8.";
            }

            plateNumber = value;
            OnPropertyChanged("PlateNumber");
        }
    }

    [Required]
    [StringLength(8)]
    public string FrameNumber
    {
        get { return frameNumber; }
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                errors["FrameNumber"] = "Номер рамы не может быть пустым.";
            }
            else if (value.Length <= 8)
            {
                errors["FrameNumber"] = null;
            }
            else
            {
                errors["FrameNumber"] = "Количество символов в номере рамы не может быть больше 8.";
            }

            frameNumber = value;
            OnPropertyChanged("FrameNumber");
        }
    }

    [Required]
    [StringLength(9)]
    public string ChasisNumber
    {
        get { return chasisNumber; }
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                errors["ChasisNumber"] = "Номер шасси не может быть пустым.";
            }
            else if (value.Length <= 9)
            {
                errors["ChasisNumber"] = null;
            }
            else
            {
                errors["ChasisNumber"] = "Количество символов в номере шасси не может быть больше 9.";
            }

            chasisNumber = value;
            OnPropertyChanged("ChasisNumber");
        }
    }

    public bool TrailerAvailability
    {
        get { return trailerAvailability; }
        set
        {
            trailerAvailability = value;
            OnPropertyChanged("TrailerAvailability");
        }
    }

    [Required]
    [StringLength(10)]
    public string DriverSurname
    {
        get { return surname; }
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                errors["Surname"] = "Фамилия водителя не может быть пустой.";
            }
            else if (value.Length > 10)
            {
                errors["Surname"] = "Количество символов в фамилии водителя не может быть больше 10.";
            }
            else
            {
                errors["Surname"] = null;
            }

            surname = value;
            OnPropertyChanged("Surname");
        }
    }

    [Required]
    [StringLength(3)]
    public string LicenceSeries
    {
        get { return licenceSeries; }
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                errors["LicenceSeries"] = "Серия удостоверения водителя не может быть пустой.";
            }
            else if (value.Length <= 3)
            {
                errors["LicenceSeries"] = null;
            }
            else
            {
                errors["LicenceSeries"] = "Количество символов в серии удостовирении водителя не может быть больше 3.";
            }

            licenceSeries = value;
            OnPropertyChanged("LicenceSeries");
        }
    }

    [Required]
    [StringLength(6)]
    public string LicenceNumber
    {
        get { return licenceNumber; }
        set
        {
            if (string.IsNullOrEmpty(value))
                errors["LicenceNumber"] = "Удостоверения водителя не может быть пустым.";
            else if (value.Length <= 6)
                errors["LicenceNumber"] = null;
            else
                errors["LicenceNumber"] = "Количество символов в удостовирении водителя не может быть больше 6.";

            licenceNumber = value;
            OnPropertyChanged("LicenceNumber");
        }
    }

    [Required]
    [StringLength(2)]
    public string TechnicalFaults
    {
        get { return technicalFaults; }
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                errors["TechnicalFaults"] = "Поле технические неисправности не может быть пустым.";
            }
            else if (value.Length > 2)
            {
                errors["TechnicalFaults"] =
                    "Количество символов в поле \"технические неисправности\" не может превышать 2.";
            }
            else
            {
                errors["TechnicalFaults"] = null;
            }

            technicalFaults = value;
            OnPropertyChanged("TechnicalFaults");
        }
    }

    [StringLength(8)]
    public string CorruptionCode
    {
        get { return corruptionCode; }
        set
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (value.Length > 8)
                {
                    errors["CorruptionCode"] = "Количество символов в поле \"код повреждения\" не может превышать 8.";
                }
                else
                {
                    errors["CorruptionCode"] = null;
                }
            }

            corruptionCode = value;
            OnPropertyChanged("CorruptionCode");
        }
    }

    [StringLength(2)]
    public string ActivityLicensingInfo
    {
        get { return activityLicensingInfo; }
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                activityLicensingInfo = null;
            }
            else if (value.Length > 2)
            {
                errors["ActivityLicensingInfo"] =
                    "Количество символов в ведомости о лицензировании водителя не может быть больше 2.";
            }
            else
            {
                errors["ActivityLicensingInfo"] = null;
            }

            activityLicensingInfo = value;
            OnPropertyChanged("ActivityLicensingInfo");
        }
    }
    
    [NotAssign] public virtual Case Case { get; set; }

    [NotAssign]
    public virtual Vehicle Vehicle
    {
        get => m_vehicle;
        set
        {
            m_vehicle = value;
            OnPropertyChanged();
        }
    }
}