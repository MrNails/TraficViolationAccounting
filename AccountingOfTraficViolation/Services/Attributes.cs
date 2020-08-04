using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingOfTraficViolation.Services
{
    [System.AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple =false)]
    sealed class NotAssignAttribute : Attribute
    {
    }
}
