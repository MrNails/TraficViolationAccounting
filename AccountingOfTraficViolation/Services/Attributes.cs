using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingOfTraficViolation.Services
{
    [System.AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    sealed class NotAssingAttribute : Attribute
    { }
}
