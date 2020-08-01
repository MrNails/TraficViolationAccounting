using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Reflection;
using AccountingOfTraficViolation.Services;

namespace AccountingOfTraficViolation
{

    public abstract class MainTable : IDataErrorInfo, INotifyPropertyChanged
    {
        public static readonly DateTime MinimumDate;

        protected string error;
        protected Dictionary<string, string> errors;

        static MainTable()
        {
            MinimumDate = new DateTime(1990, 1, 1);
        }

        public MainTable()
        {
            errors = new Dictionary<string, string>();
        }

        public string Error => error;

        public string this[string columnName] => errors.ContainsKey(columnName) ? errors[columnName] : null;

        public delegate void ErrorHandler(string errorMessage);

        public event PropertyChangedEventHandler PropertyChanged;

        public void Assign(MainTable entity)
        {
            Type newObject = entity.GetType();
            Type currentObject = this.GetType();

            foreach (var property in currentObject.GetProperties())
            {
                if (property.CanWrite && property.GetCustomAttribute(typeof(NotAssingAttribute), false) == null)
                {
                    property.SetValue(this, newObject.GetProperty(property.Name).GetValue(entity));
                }
            }
        }

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
