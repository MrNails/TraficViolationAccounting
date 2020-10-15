using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using AccountingOfTraficViolation.Models;
using AccountingOfTraficViolation.Services;

namespace AccountingOfTraficViolation.ViewModels
{
    public class AccidentObjectsVM<T> : INotifyPropertyChanged
        where T : MainTable, new()
    {
        private int currentIndex;
        private T currentAccidentObject;
        private RelayCommand addCommand;
        private RelayCommand removeCommand;

        public AccidentObjectsVM() : this(null)
        { }
        public AccidentObjectsVM(ObservableCollection<T> participantsInfo)
        {
            if (participantsInfo != null)
            {
                AccidentObjects = participantsInfo.Clone();
                CurrentAccidentObject = AccidentObjects.FirstOrDefault();
            }
            else
            {
                CurrentAccidentObject = new T();
                AccidentObjects = new ObservableCollection<T>() { CurrentAccidentObject };
            }

            addCommand = new RelayCommand(obj =>
            {
                CurrentAccidentObject = new T();
                AccidentObjects.Add(CurrentAccidentObject);
            }, obj => AccidentObjects.Count < 5);
            removeCommand = new RelayCommand(obj =>
            {
                if (obj is T)
                {
                    AccidentObjects.Remove((T)obj);
                }
                else if (obj.IsIntegerNumber() && Convert.ToInt32(obj) >= 0)
                {
                    AccidentObjects.RemoveAt(Convert.ToInt32(obj));
                }
                


            }, (obj => obj != null && AccidentObjects.Count > 1));
        }


        public int CurrentIndex
        {
            get { return currentIndex; }
            set
            {
                if (value >= 0 && value < AccidentObjects.Count)
                {
                    currentIndex = value;
                }
                else
                {
                    currentIndex = 0;
                }
                CurrentAccidentObject = AccidentObjects[currentIndex];
            }
        }

        public T CurrentAccidentObject
        {
            get { return currentAccidentObject; }
            set
            {
                currentAccidentObject = value;
                OnPropertyChanged("CurrentAccidentObject");
            }
        }

        public RelayCommand AddCommand => addCommand;
        public RelayCommand RemoveCommand => removeCommand;

        public ObservableCollection<T> AccidentObjects { get; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
