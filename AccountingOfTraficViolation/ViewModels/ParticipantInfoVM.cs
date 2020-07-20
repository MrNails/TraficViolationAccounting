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
    public class ParticipantInfoVM : INotifyPropertyChanged
    {
        private int currentIndex;
        private ParticipantsInformation currentParticipantsInformation;
        private RelayCommand addCommand;
        private RelayCommand removeCommand;

        public ParticipantInfoVM() : this(null)
        { }
        public ParticipantInfoVM(ObservableCollection<ParticipantsInformation> participantsInfo)
        {
            if (participantsInfo != null)
            {
                ParticipantsInformations = participantsInfo;
                CurrentParticipantsInformation = ParticipantsInformations.FirstOrDefault();
            }
            else
            {
                CurrentParticipantsInformation = new ParticipantsInformation();
                ParticipantsInformations = new ObservableCollection<ParticipantsInformation>() { CurrentParticipantsInformation };
            }

            addCommand = new RelayCommand(pInfo =>
            {
                CurrentParticipantsInformation = new ParticipantsInformation();
                ParticipantsInformations.Add(CurrentParticipantsInformation);
            });
            removeCommand = new RelayCommand(pInfo =>
            {
                if (pInfo != null && pInfo is ParticipantsInformation)
                {
                    ParticipantsInformations.Remove((ParticipantsInformation)pInfo);
                }
            });
        }


        public int CurrentIndex
        {
            get { return currentIndex; }
            set
            {
                if (value >= 0 && value < ParticipantsInformations.Count)
                {
                    currentIndex = value;
                }
                else
                {
                    currentIndex = 0;
                }
                CurrentParticipantsInformation = ParticipantsInformations[currentIndex];
            }
        }

        public ParticipantsInformation CurrentParticipantsInformation
        {
            get { return currentParticipantsInformation; }
            set
            {
                currentParticipantsInformation = value;
                OnPropertyChanged("CurrentParticipantsInformation");
            }
        }

        public RelayCommand AddCommand => addCommand;
        public RelayCommand RemoveCommand => removeCommand;

        public ObservableCollection<ParticipantsInformation> ParticipantsInformations { get; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
