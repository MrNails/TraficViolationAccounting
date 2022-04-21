using System;

namespace AccountOfTrafficViolationDB.Helpers
{
    [System.AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class NotAssignAttribute : Attribute
    {
    }
}
