using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Xaml.Behaviors;
using AccountingOfTraficViolation.Models;

namespace AccountingOfTraficViolation.ViewModels
{
    public class CasesVM : INotifyPropertyChanged, IDisposable
    {
        private TVAContext TVAContext;
        private Case currentCase;
        private IEnumerable<GeneralInfo> currentGeneralInfo;
        private IEnumerable<RoadCondition> currentRoadCondition;
        private IEnumerable<AccidentOnHighway> currentaccidentOnHighways;
        private IEnumerable<AccidentOnVillage> currentAccidentOnVillage;
        private IEnumerable<ParticipantsInformation> currentParticipantsInformation;
        private IEnumerable<Vehicle> currentVehicles;
        private IEnumerable<Victim> currentVictims;
        private IEnumerable<Case> cases;
        private RelayCommand showCaseInfo;
        private RelayCommand doubleClickCaseInfo;

        public event PropertyChangedEventHandler PropertyChanged;

        public CasesVM()
        {
            TVAContext = new TVAContext();
            showCaseInfo = new RelayCommand(obj =>
            {
                if (obj != null && obj is Case)
                {
                    Case _case = (Case)obj;

                    CurrentGeneralInfo = new GeneralInfo[] { _case.GeneralInfo };
                    CurrentRoadCondition = new RoadCondition[] { _case.RoadCondition };
                    CurrentParticipantsInformation = _case.ParticipantsInformations;
                    CurrentVehicles = _case.Vehicles;
                    CurrentVictims = _case.Victims;

                    if (_case.CaseAccidentPlace.AccidentOnHighway != null)
                    {
                        CurrentAccidentOnHighway = new AccidentOnHighway[] { _case.CaseAccidentPlace.AccidentOnHighway };
                        CurrentAccidentOnVillage = null;
                    }
                    else if (_case.CaseAccidentPlace.AccidentOnVillage != null)
                    {
                        CurrentAccidentOnVillage = new AccidentOnVillage[] { _case.CaseAccidentPlace.AccidentOnVillage };
                        CurrentAccidentOnHighway = null;
                    }
                }
            });
            doubleClickCaseInfo = new RelayCommand(obj =>
            {
                MessageBox.Show("+");
            });
        }

        public RelayCommand ShowCaseInfo => showCaseInfo;
        public RelayCommand DoubleClickCaseInfo => doubleClickCaseInfo;

        public IEnumerable<Case> FoundCases
        {
            get { return cases; }
            private set
            {
                cases = value;
                OnPropertyChanged("FoundCases");
            }
        }

        public Case CurrentCase
        {
            get { return currentCase; }
            set
            {
                currentCase = value;
                OnPropertyChanged("CurrentCase");
            }
        }
        public IEnumerable<GeneralInfo> CurrentGeneralInfo
        {
            get { return currentGeneralInfo; }
            private set
            {
                currentGeneralInfo = value;
                OnPropertyChanged("CurrentGeneralInfo");
            }
        }
        public IEnumerable<RoadCondition> CurrentRoadCondition
        {
            get { return currentRoadCondition; }
            private set
            {
                currentRoadCondition = value;
                OnPropertyChanged("CurrentRoadCondition");
            }
        }
        public IEnumerable<AccidentOnHighway> CurrentAccidentOnHighway
        {
            get { return currentaccidentOnHighways; }
            private set
            {
                currentaccidentOnHighways = value;
                OnPropertyChanged("CurrentAccidentOnHighway");
            }
        }
        public IEnumerable<AccidentOnVillage> CurrentAccidentOnVillage
        {
            get { return currentAccidentOnVillage; }
            private set
            {
                currentAccidentOnVillage = value;
                OnPropertyChanged("CurrentAccidentOnVillage");
            }
        }
        public IEnumerable<ParticipantsInformation> CurrentParticipantsInformation
        {
            get { return currentParticipantsInformation; }
            private set
            {
                currentParticipantsInformation = value;
                OnPropertyChanged("CurrentParticipantsInformation");
            }
        }
        public IEnumerable<Vehicle> CurrentVehicles
        {
            get { return currentVehicles; }
            private set
            {
                currentVehicles = value;
                OnPropertyChanged("CurrentVehicles");
            }
        }
        public IEnumerable<Victim> CurrentVictims
        {
            get { return currentVictims; }
            private set
            {
                currentVictims = value;
                OnPropertyChanged("CurrentVictims");
            }
        }

        public bool FindCase(Func<TVAContext, IEnumerable<Case>> findFunc)
        {
            if (findFunc == null)
            {
                throw new ArgumentNullException("findFunc");
            }

            FoundCases = findFunc(TVAContext);

            return FoundCases.Count() != 0;
        }
        public async Task<bool> FindCaseAsync(Func<TVAContext, IQueryable<Case>> findFunc, CancellationToken cancellationToken)
        {
            if (findFunc == null)
            {
                throw new ArgumentNullException("findFunc");
            }

            bool findRes;

            try
            {
                findRes = await Task.Run(async () =>
                {
                    var res = await findFunc(TVAContext).ToArrayAsync(cancellationToken);
                    FoundCases = res;
                    return FoundCases.Count() != 0;
                });
            }
            catch
            {
                findRes = false;
            }
            
            return findRes;
        }

        public void OnPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        public void Dispose()
        {
            TVAContext.Dispose();
        }
    }
}
