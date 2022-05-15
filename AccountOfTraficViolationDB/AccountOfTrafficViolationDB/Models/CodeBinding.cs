using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AccountingOfTrafficViolation.Models;
using AccountOfTrafficViolationDB.Models;

namespace AccountOfTraficViolationDB.Models;

[Table(name: "CodeBindings")]
public class CodeBinding : MainTable
{
    private string name;
    private string m_value;
    private string description;

    public CodeBinding()
    {
        Codes = new HashSet<Code>();
    }
    
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name
    {
        get { return name; }
        set
        {
            if (value == null)
            {
                errors["Name"] = "Поле \"Имя\" не может быть пустым";
            }
            else if (value.Length > 50)
            {
                errors["Name"] = "Количество символов в поле \"Имя\" не может быть больше 20";
            }
            else
            {
                errors["Name"] = null;
            }

            OnPropertyChanged("Name");
            name = value;
        }
    }

    [StringLength(500)]
    public string Description
    {
        get { return description; }
        set
        {
            if (value.Length > 500)
            {
                errors["Description"] = "Количество символов в поле \"Описание\" не может быть больше 500";
            }
            else
            {
                errors["Description"] = null;
            }

            OnPropertyChanged("Description");
            description = value;
        }
    }
    
    public virtual ICollection<Code> Codes { get; set; }
}