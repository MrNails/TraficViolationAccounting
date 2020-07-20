using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace AccountingOfTraficViolation
{

    public abstract class MainTable : IDataErrorInfo, INotifyPropertyChanged
    {
        protected string error;
        protected Dictionary<string, string> errors;

        public MainTable()
        {
            errors = new Dictionary<string, string>();
        }

        public string Error => error;

        public string this[string columnName] => errors.ContainsKey(columnName) ? errors[columnName] : null;

        public delegate void ErrorHandler(string errorMessage);

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
