using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
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
        private Case oldCase;
        private List<Case> cases;
        private RelayCommand showCaseInfo;
        private RelayCommand doubleClickCaseInfo;

        private bool caseChanged;

        private SaveCaseToWordVM saveCaseToWordVM;

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
                if (CurrentCase != null && oldCase != null &&
                    CurrentCase.Id == oldCase.Id ||
                    CaseChanged &&
                    MessageBox.Show("Данные были изменены. При просмотре другого дела " +
                                    "данные будут возвращены в начальное состояние. " +
                                    "Вы уверены, что хотите продолжить?", "Внимание",
                                    MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                {
                    return;
                }


                CaseChanged = false;

                if (oldCase != null)
                {
                    TVAContext.Entry(oldCase).Reload();
                }

                oldCase = CurrentCase;

                ReloadCollection(CurrentGeneralInfo);
                ReloadCollection(CurrentRoadCondition);
                ReloadCollection(CurrentParticipantsInformation);
                ReloadCollection(CurrentVehicles);
                ReloadCollection(CurrentVictims);
                ReloadCollection(CurrentAccidentOnHighway);
                ReloadCollection(CurrentAccidentOnVillage);

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

            }, obj => obj != null && obj is Case);
            doubleClickCaseInfo = new RelayCommand(obj =>
            {
                if (obj is Case)
                {
                    if (currentCase == null)
                    {
                        currentCase = (Case)obj;
                    }

                    CaseReviewWindow caseReviewWindow = new CaseReviewWindow((Case)obj, user);
                    if (caseReviewWindow.ShowDialog() == true)
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
            }, (o) => o != null);

            saveCaseToWordVM = new SaveCaseToWordVM($@"{Environment.CurrentDirectory}\WordPattern\Accounting form.docx");
            saveCaseToWordVM.Saved += SavedDocumentMessage;
            saveCaseToWordVM.ExceptionCaptured += ErrorDocumentMessage;
        }

        public RelayCommand ShowCaseInfo => showCaseInfo;
        public RelayCommand DoubleClickCaseInfo => doubleClickCaseInfo;
        public SaveCaseToWordVM SaveCaseToWordVM => saveCaseToWordVM;

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
        public User GetCurrentCaseCreator()
        {
            if (CurrentCase == null)
            {
                throw new Exception("Выбранное дело не может быть пустым.");
            }

            return TVAContext.Users.Where(u => u.Login == CurrentCase.CreaterLogin)
                                   .AsNoTracking()
                                   .ToArray()
                                   .Where(u => u.Login == CurrentCase.CreaterLogin)
                                   .FirstOrDefault();

        }

        public async Task<bool> FindCaseAsync(Func<IQueryable<Case>, IQueryable<Case>> predicate, CancellationToken cancellationToken)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }

            FoundCases = await predicate(TVAContext.Cases).ToListAsync(cancellationToken);

            return FoundCases.Count != 0;
        }
        public async void SaveChangesAsync()
        {
            using (var transaction = TVAContext.Database.BeginTransaction())
            {
                try
                {
                    await TVAContext.SaveChangesAsync();
                    CaseChanged = false;
                    transaction.Commit();
                }
                catch (DbUpdateConcurrencyException)
                {
                    MessageBox.Show("Не возможно изменить данные, так как они уже были изменены кем-то другим", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    transaction.Rollback();
                }
            }
        }
        public async Task<User> GetCurrentCaseCreatorAsync()
        {
            if (CurrentCase == null)
            {
                throw new Exception("Выбранное дело не может быть пустым.");
            }

            return (await TVAContext.Users.Where(u => u.Login == CurrentCase.CreaterLogin)
                                          .AsNoTracking()
                                          .ToArrayAsync())
                                          .Where(u => u.Login == CurrentCase.CreaterLogin)
                                          .FirstOrDefault();

        }

        public Task SaveToDocument(string path, DocumentSaveType documentSaveType)
        {
            if (CurrentCase == null)
            {
                throw new Exception("Выбранное дело не может быть пустым.");
            }

            Task[] tasks = new Task[2];

            saveCaseToWordVM.SaveFilePath = path;
            saveCaseToWordVM.Case = CurrentCase;
            saveCaseToWordVM.User = GetCurrentCaseCreator();

            return saveCaseToWordVM.SaveAsync(documentSaveType);
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
            saveCaseToWordVM.Dispose();
        }

        private void SavedDocumentMessage(WordSaveActionArgs args)
        {
            MessageBox.Show("Файл успешно сохранён.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void ErrorDocumentMessage(Exception ex, WordSaveActionArgs args)
        {
            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void ReloadCollection<T>(IEnumerable<T> collection)
        {
            foreach (var entity in collection)
            {
                TVAContext.Entry(entity).Reload();
            }
        }
    }
}
