using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using AccountingOfTrafficViolation.Models;
using AccountingOfTrafficViolation.Services;
using AccountingOfTrafficViolation.Views;
using AccountingOfTrafficViolation.Views.AddInfoWindows;
using AccountOfTrafficViolationDB.Context;
using AccountOfTrafficViolationDB.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountingOfTrafficViolation.ViewModels
{
    public class CasesVM : INotifyPropertyChanged, IDisposable
    {
        private TVAContext m_TVAContext;
        private Case? m_currentCase;
        private Case m_oldCase;
        private List<Case> m_cases;
        private RelayCommand m_showCaseInfo;
        private RelayCommand m_doubleClickCaseInfo;

        private bool m_caseChanged;

        private SaveCaseToWordVM m_saveCaseToWordVM;

        public event PropertyChangedEventHandler PropertyChanged;

        public CasesVM()
        {
            m_TVAContext = new TVAContext(GlobalSettings.ConnectionStrings[Constants.DefaultDB], GlobalSettings.Credential);

            CurrentGeneralInfo = new ObservableCollection<GeneralInfo>();
            CurrentRoadCondition = new ObservableCollection<RoadCondition>();
            CurrentParticipantsInformation = new ObservableCollection<ParticipantsInformation>();
            CurrentVehicles = new ObservableCollection<CaseVehicle>();
            CurrentVictims = new ObservableCollection<Victim>();
            CurrentAccidentOnHighway = new ObservableCollection<AccidentOnHighway>();
            CurrentAccidentOnVillage = new ObservableCollection<AccidentOnVillage>();

            CaseChanged = false;

            m_showCaseInfo = new RelayCommand(ShowCaseInfoHandler, obj => obj != null && obj is Case);
            m_doubleClickCaseInfo = new RelayCommand(DoubleClickCaseInfoHandler, o => o != null);

            m_saveCaseToWordVM =
                new SaveCaseToWordVM($@"{Environment.CurrentDirectory}\WordPattern\Accounting form.docx");
            m_saveCaseToWordVM.Saved += SavedDocumentMessage;
            m_saveCaseToWordVM.ExceptionCaptured += ErrorDocumentMessage;
        }

        public RelayCommand ShowCaseInfo => m_showCaseInfo;
        public RelayCommand DoubleClickCaseInfo => m_doubleClickCaseInfo;
        public SaveCaseToWordVM SaveCaseToWordVm => m_saveCaseToWordVM;

        public List<Case> FoundCases
        {
            get { return m_cases; }
            private set
            {
                m_cases = value;

                OnPropertyChanged("FoundCases");
            }
        }

        public Case CurrentCase
        {
            get { return m_currentCase; }
            set
            {
                m_currentCase = value;
                OnPropertyChanged("CurrentCase");
            }
        }

        public bool CaseChanged
        {
            get { return m_caseChanged; }
            private set
            {
                m_caseChanged = value;
                OnPropertyChanged("CaseChanged");
            }
        }

        public ObservableCollection<GeneralInfo> CurrentGeneralInfo { get; set; }
        public ObservableCollection<RoadCondition> CurrentRoadCondition { get; set; }
        public ObservableCollection<AccidentOnHighway> CurrentAccidentOnHighway { get; set; }
        public ObservableCollection<AccidentOnVillage> CurrentAccidentOnVillage { get; set; }
        public ObservableCollection<ParticipantsInformation> CurrentParticipantsInformation { get; set; }
        public ObservableCollection<CaseVehicle> CurrentVehicles { get; set; }
        public ObservableCollection<Victim> CurrentVictims { get; set; }

        public async Task<bool> FillCasesAsync(Func<IQueryable<Case>, IQueryable<Case>> predicate,
            CancellationToken cancellationToken)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));
            
            var casesAndPlaces = await predicate(m_TVAContext.Cases)
                .Join(m_TVAContext.CaseAccidentPlaces, 
                      c => c.Id,
                      cpa => cpa.CaseId,
                      (c, cpa) => new { Case = c, CaseAccidentPlace = cpa })
                .ToListAsync(cancellationToken);

            FoundCases = casesAndPlaces.Select(h =>
            {
                h.Case.CaseAccidentPlace = h.CaseAccidentPlace;
                return h.Case;
            }).ToList();
            
            return FoundCases.Count != 0;
        }

        public async void SaveChangesAsync()
        {
            using (var transaction = await m_TVAContext.Database.BeginTransactionAsync())
            {
                try
                {
                    await m_TVAContext.SaveChangesAsync();
                    CaseChanged = false;
                    await transaction.CommitAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    MessageBox.Show("Не возможно изменить данные, так как они уже были изменены кем-то другим",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    await transaction.RollbackAsync();
                }
            }
        }

        public Task<Officer?> GetCurrentCaseCreatorAsync()
        {
            if (CurrentCase == null)
                throw new Exception("Выбранное дело не может быть пустым.");

            return m_TVAContext.Officers.AsNoTracking().FirstOrDefaultAsync(u => u.Id == CurrentCase.OfficerId);
        }

        public async Task SaveToDocument(string path, DocumentSaveType documentSaveType)
        {
            if (CurrentCase == null)
                throw new Exception("Выбранное дело не может быть пустым.");

            m_saveCaseToWordVM.SaveFilePath = path;
            m_saveCaseToWordVM.Case = CurrentCase;
            m_saveCaseToWordVM.Officer = GlobalSettings.ActiveOfficer;

            await m_saveCaseToWordVM.SaveAsync(documentSaveType);
        }

        public void DiscardChanges()
        {
            m_TVAContext.CancelModifiedChanges();
            CaseChanged = false;
        }

        private void ShowCaseInfoHandler(object obj)
        {
            if (CurrentCase != null && m_oldCase != null &&
                CurrentCase.Id == m_oldCase.Id ||
                CaseChanged &&
                MessageBox.Show("Данные были изменены. При просмотре другого дела " +
                                "данные будут возвращены в начальное состояние. " +
                                "Вы уверены, что хотите продолжить?", "Внимание",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
            {
                return;
            }

            CaseChanged = false;

            if (m_oldCase != null)
                m_TVAContext.Entry(m_oldCase).Reload();

            m_oldCase = CurrentCase;

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
            CurrentVehicles.AddRange(CurrentCase.CaseVehicles);
            CurrentVictims.AddRange(CurrentCase.Victims);

            if (CurrentCase.CaseAccidentPlace.AccidentOnHighway != null)
                CurrentAccidentOnHighway.Add(CurrentCase.CaseAccidentPlace.AccidentOnHighway);
            else if (CurrentCase.CaseAccidentPlace.AccidentOnVillage != null)
                CurrentAccidentOnVillage.Add(CurrentCase.CaseAccidentPlace.AccidentOnVillage);
        }

        private void DoubleClickCaseInfoHandler(object obj)
        {
            if (obj is Case)
            {
                if (m_currentCase == null)
                    m_currentCase = (Case)obj;


                var caseReviewWindow = new CaseReviewWindow((Case)obj, GlobalSettings.ActiveOfficer);
                if (caseReviewWindow.ShowDialog() == true)
                {
                    m_currentCase.Assign(caseReviewWindow.Case);
                    CaseChanged = true;
                }

                return;
            }

            if (m_currentCase != null && m_currentCase.State == "CLOSE")
            {
                MessageBox.Show("Подробности дела нельзя изменить после его закрытия.", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (m_currentCase != null && m_currentCase.OfficerId != GlobalSettings.ActiveOfficer.Id)
            {
                MessageBox.Show("Вы не можете изменять подробности в чужом деле.", "Ошибка", MessageBoxButton.OK,
                    MessageBoxImage.Error);
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
                    var roadCondition = CurrentRoadCondition.FirstOrDefault();
                    roadCondition.Assign(roadConditionWindow.RoadCondition);
                    CaseChanged = true;
                }
            }

            if (obj is AccidentOnHighway || obj is AccidentOnVillage)
            {
                AddAccidentPlaceWindow accidentPlaceWindow =
                    new AddAccidentPlaceWindow(m_currentCase.CaseAccidentPlace, false);
                if (accidentPlaceWindow.ShowDialog() == true)
                {
                    var accidentOnHighway = CurrentAccidentOnHighway.FirstOrDefault();
                    var accidentOnVillage = CurrentAccidentOnVillage.FirstOrDefault();

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
                var participantInfoWindow =
                    new AddParticipantInfoWindow(
                        new ObservableCollection<ParticipantsInformation> { (ParticipantsInformation)obj }, false);
                if (participantInfoWindow.ShowDialog() == true)
                {
                    ParticipantsInformation windowParticipantInfo =
                        participantInfoWindow.ParticipantsInformations.FirstOrDefault();
                    ParticipantsInformation participantsInformation =
                        CurrentParticipantsInformation.FirstOrDefault(p => p.Id == windowParticipantInfo.Id);

                    participantsInformation.Assign(windowParticipantInfo);
                    CaseChanged = true;
                }
            }

            if (obj is CaseVehicle)
            {
                AddVehiclesWindow vehiclesWindow =
                    new AddVehiclesWindow(new ObservableCollection<CaseVehicle> { (CaseVehicle)obj }, false);
                if (vehiclesWindow.ShowDialog() == true)
                {
                    var windowVehicle = vehiclesWindow.Vehicles.FirstOrDefault();
                    var vehicle = CurrentVehicles.FirstOrDefault(p => p.CaseId == windowVehicle.CaseId && p.VehicleId == windowVehicle.VehicleId);

                    vehicle.Assign(windowVehicle);
                    CaseChanged = true;
                }
            }

            if (obj is Victim)
            {
                AddVictimsWindow victimsWindow =
                    new AddVictimsWindow(new ObservableCollection<Victim> { (Victim)obj }, false);
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
                m_TVAContext.Entry(CurrentCase).State = EntityState.Modified;
            }
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
                m_TVAContext.Entry(entity).Reload();
            }
        }

        private void OnPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public void Dispose()
        {
            m_TVAContext.Dispose();
            m_saveCaseToWordVM.Dispose();
        }
    }
}