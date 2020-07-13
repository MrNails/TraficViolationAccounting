using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace AccountingOfTraficViolation
{

    public abstract class MainTable : INotifyPropertyChanged
    {

        public delegate void ErrorHandler(string errorMessage);

        public event ErrorHandler ErrorInput;
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        public void OnErrorInput(string errorMessage)
        {
            ErrorInput?.Invoke(errorMessage);
        }
    }
}
