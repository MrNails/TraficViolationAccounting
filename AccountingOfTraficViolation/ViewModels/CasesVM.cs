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
using AccountingOfTraficViolation.Models;
using AccountingOfTraficViolation.Services;
using AccountingOfTraficViolation.Views;
using AccountingOfTraficViolation.Views.AddInfoWindows;

namespace AccountingOfTraficViolation.ViewModels
{
    public class CasesVM : INotifyPropertyChanged, IDisposable
    {
        private readonly User user;

        private TVAContext TVAContext;
        private Case currentCase;
        private List<Case> cases;
        private RelayCommand showCaseInfo;
        private RelayCommand doubleClickCaseInfo;
        private bool caseChanged;

        private object locker = new object();

        public event PropertyChangedEventHandler PropertyChanged;

        public CasesVM(User user)
        {
            TVAContext = new TVAContext();
            this.user = user;

            CurrentGeneralInfo = new ObservableCollection<GeneralInfo>();
            CurrentRoadCondition = new ObservableCollection<RoadCondition>();
            CurrentParticipantsInformation = new ObservableCollection<ParticipantsInformation>();
            CurrentVehicles = new ObservableCollection<Vehicle>();
            CurrentVictims = new ObservableCollection<Victim>();
            CurrentAccidentOnHighway = new ObservableCollection<AccidentOnHighway>();
            CurrentAccidentOnVillage = new ObservableCollection<AccidentOnVillage>();

            CaseChanged = false;

            showCaseInfo = new RelayCommand(obj =>
            {
                if (obj != null && obj is Case)
                {
                    CurrentCase = (Case)obj;

                    ClearCollections();

                    CurrentGeneralInfo.Add(CurrentCase.GeneralInfo);
                    CurrentRoadCondition.Add(CurrentCase.RoadCondition);
                    CurrentParticipantsInformation.AddRange(CurrentCase.ParticipantsInformations);
                    CurrentVehicles.AddRange(CurrentCase.Vehicles);
                    CurrentVictims.AddRange(CurrentCase.Victims);

                    if (CurrentCase.CaseAccidentPlace.AccidentOnHighway != null)
                    {
                        CurrentAccidentOnHighway.Add(CurrentCase.CaseAccidentPlace.AccidentOnHighway);
                    }
                    else if (CurrentCase.CaseAccidentPlace.AccidentOnVillage != null)
                    {
                        CurrentAccidentOnVillage.Add(CurrentCase.CaseAccidentPlace.AccidentOnVillage);
                    }
                }
            });
            doubleClickCaseInfo = new RelayCommand(obj =>
            {
                if (obj == null)
                {
                    return;
                }

                if (obj is Case)
                {
                    if (currentCase == null)
                    {
                        currentCase = (Case)obj;
                    }

                    CaseReviewWindow caseReviewWindow = new CaseReviewWindow((Case)obj, user);
                    if (caseReviewWindow.ShowDialog() == true && caseReviewWindow.Case.State != "CLOSE")
                    {
                        currentCase.Assign(caseReviewWindow.Case);
                        CaseChanged = true;
                    }

                    return;
                }

                if (currentCase != null && currentCase.State == "CLOSE")
                {
                    MessageBox.Show("Подробности дела нельзя изменить после его закрытия.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (currentCase != null && currentCase.CreaterLogin != user.Login)
                {
                    MessageBox.Show("Вы не можете изменять подробности в чужом деле.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (obj is GeneralInfo)
                {
                    AddGeneralInfoWindow generalInfoWindow = new AddGeneralInfoWindow((GeneralInfo)obj);
                    if (generalInfoWindow.ShowDialog() == true)
                    {
                        GeneralInfo generalInfo = CurrentGeneralInfo.FirstOrDefault();

                        generalInfo.Assign(generalInfoWindow.GeneralInfo);

                        CaseChanged = true;
                    }
                }

                if (obj is RoadCondition)
                {
                    AddRoadConditionWindow roadConditionWindow = new AddRoadConditionWindow((RoadCondition)obj);
                    if (roadConditionWindow.ShowDialog() == true)
                    {
                        RoadCondition roadCondition = CurrentRoadCondition.FirstOrDefault();
                        roadCondition.Assign(roadConditionWindow.RoadCondition);
                        CaseChanged = true;
                    }
                }

                if (obj is AccidentOnHighway || obj is AccidentOnVillage)
                {
                    AddAccidentPlaceWindow accidentPlaceWindow = new AddAccidentPlaceWindow(currentCase.CaseAccidentPlace, false);
                    if (accidentPlaceWindow.ShowDialog() == true)
                    {
                        AccidentOnHighway accidentOnHighway = CurrentAccidentOnHighway.FirstOrDefault();
                        AccidentOnVillage accidentOnVillage = CurrentAccidentOnVillage.FirstOrDefault();

                        if (accidentOnVillage != null)
                        {
                            accidentOnVillage.Assign(accidentPlaceWindow.CaseAccidentPlace.AccidentOnVillage);
                        }
                        else if (accidentOnHighway != null)
                        {
                            accidentOnHighway.Assign(accidentPlaceWindow.CaseAccidentPlace.AccidentOnHighway);
                        }

                        CaseChanged = true;
                    }
                }

                if (obj is ParticipantsInformation)
                {
                    AddParticipantInfoWindow participantInfoWindow = new AddParticipantInfoWindow(new ObservableCollection<ParticipantsInformation> { (ParticipantsInformation)obj }, false);
                    if (participantInfoWindow.ShowDialog() == true)
                    {
                        ParticipantsInformation windowParticipantInfo = participantInfoWindow.ParticipantsInformations.FirstOrDefault();
                        ParticipantsInformation participantsInformation = CurrentParticipantsInformation.FirstOrDefault(p => p.Id == windowParticipantInfo.Id);

                        participantsInformation.Assign(windowParticipantInfo);
                        CaseChanged = true;
                    }
                }

                if (obj is Vehicle)
                {
                    AddVehiclesWindow vehiclesWindow = new AddVehiclesWindow(new ObservableCollection<Vehicle> { (Vehicle)obj }, false);
                    if (vehiclesWindow.ShowDialog() == true)
                    {
                        Vehicle windowVehicle = vehiclesWindow.Vehicles.FirstOrDefault();
                        Vehicle vehicle = CurrentVehicles.FirstOrDefault(p => p.Id == windowVehicle.Id);

                        vehicle.Assign(windowVehicle);
                        CaseChanged = true;
                    }
                }

                if (obj is Victim)
                {
                    AddVictimsWindow victimsWindow = new AddVictimsWindow(new ObservableCollection<Victim> { (Victim)obj }, false);
                    if (victimsWindow.ShowDialog() == true)
                    {
                        Victim windowVictim = victimsWindow.Victims.FirstOrDefault();
                        Victim victim = CurrentVictims.FirstOrDefault(p => p.Id == windowVictim.Id);

                        victim.Assign(windowVictim);
                        CaseChanged = true;
                    }
                }

                if (CaseChanged)
                {
                    TVAContext.Entry(CurrentCase).State = System.Data.Entity.EntityState.Modified;
                }
            });
        }

        public RelayCommand ShowCaseInfo => showCaseInfo;
        public RelayCommand DoubleClickCaseInfo => doubleClickCaseInfo;

        public List<Case> FoundCases
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
        public bool CaseChanged
        {
            get { return caseChanged; }
            private set
            {
                caseChanged = value;
                OnPropertyChanged("CaseChanged");
            }
        }

        public ObservableCollection<GeneralInfo> CurrentGeneralInfo { get; set; }
        public ObservableCollection<RoadCondition> CurrentRoadCondition { get; set; }
        public ObservableCollection<AccidentOnHighway> CurrentAccidentOnHighway { get; set; }
        public ObservableCollection<AccidentOnVillage> CurrentAccidentOnVillage { get; set; }
        public ObservableCollection<ParticipantsInformation> CurrentParticipantsInformation { get; set; }
        public ObservableCollection<Vehicle> CurrentVehicles { get; set; }
        public ObservableCollection<Victim> CurrentVictims { get; set; }

        public bool FindCase(Func<Case, bool> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }

            FoundCases = TVAContext.Cases.Where(predicate).ToList();

            return FoundCases.Count != 0;
        }

        public async Task<bool> FindCaseAsync(Func<Case, bool> predicate, CancellationToken cancellationToken)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }

            bool findRes;

            try
            {
                findRes = await Task.Run(() =>
                {
                    Func<Case, bool> cancellationPredicate = c =>
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        return predicate(c);
                    };

                    int count = 0;
                    var res = TVAContext.Cases.Where(cancellationPredicate).ToList();

                        lock (locker)
                        {
                            FoundCases = res;
                            count = res.Count;
                        }

                    return count != 0;
                }, cancellationToken);

            }
            catch (OperationCanceledException ex)
            {
                throw ex;
            }
            catch (Exception)
            {
                findRes = false;
            }

            return findRes;
        }
        public async void SaveChangesAsync()
        {
            await TVAContext.SaveChangesAsync();
            CaseChanged = false;
        }

        public void DiscardChanges()
        {
            TVAContext.CancelAllChanges();
            CaseChanged = false;
        }

        public void OnPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        public void Dispose()
        {
            TVAContext.Dispose();
        }

        private void ClearCollections()
        {
            CurrentGeneralInfo.Clear();
            CurrentRoadCondition.Clear();
            CurrentAccidentOnHighway.Clear();
            CurrentAccidentOnVillage.Clear();
            CurrentParticipantsInformation.Clear();
            CurrentVehicles.Clear();
            CurrentVictims.Clear();
        }
    }
}
