using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccountingOfTraficViolation.Models;
using AccountingOfTraficViolation.Services;
using System.Diagnostics;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Threading;

namespace AccountingOfTraficViolation.ViewModels
{
    public class WordSaveActionArgs
    {
        private bool isSaved;
        private bool isAborted;

        private TimeSpan elapsedTime;

        public WordSaveActionArgs(TimeSpan elapsedTime) : this(elapsedTime, false)
        {}
        public WordSaveActionArgs(TimeSpan elapsedTime, bool isSaved) : this (elapsedTime, isSaved, false)
        {}
        public WordSaveActionArgs(TimeSpan elapsedTime, bool isSaved, bool isAborted)
        {
            this.elapsedTime = elapsedTime;
            this.isSaved = isSaved;
            this.isAborted = isAborted;
        }

        public bool IsSaved
        {
            get { return isSaved; }
        }
        public bool IsAborted
        {
            get { return isAborted; }


        }

        public TimeSpan ElapsedTime
        {
            get { return elapsedTime; }
        }
    }


    public delegate void WordSaveHandler(WordSaveActionArgs args);
    public delegate void WordExceptionHandler(Exception ex, WordSaveActionArgs args);

    public sealed class SaveCaseToWordVM : INotifyPropertyChanged, IDisposable
    {   
        private double savingProgress;
        private string saveFilePath;

        private WordSaver wordSaver;

        private Stopwatch stopwatch;
        private Dispatcher dispatcher;

        public event WordSaveHandler Saved;
        public event WordSaveHandler Canceled;
        public event WordExceptionHandler ExceptionCaptured;
        public event PropertyChangedEventHandler PropertyChanged;
        
        public SaveCaseToWordVM(string filePath) : this(filePath, null)
        {}
        public SaveCaseToWordVM(string filePath, string saveFilePath) : this (filePath, saveFilePath, null, null)
        { }
        public SaveCaseToWordVM(string filePath, string saveFilePath, Case _case, Officer officer)
        {
            FilePath = filePath;
            SaveFilePath = saveFilePath;

            Case = _case;
            Officer = officer;

            wordSaver = new WordSaver(filePath);

            stopwatch = new Stopwatch();

            dispatcher = Dispatcher.CurrentDispatcher;
        }

        public double SavingProgress
        {
            get { return savingProgress; }
            private set 
            { 
                savingProgress = value;
                OnPropertyChanged("SavingProgress");
            }
        }
        public string SaveFilePath
        {
            get { return saveFilePath; }
            set
            {
                saveFilePath = value;
                OnPropertyChanged("SaveFilePath");
            }
        }

        public string FilePath { get; private set; }

        public Case Case { get; set; }
        public Officer Officer { get; set; }


        #region WordSaver Region
        public Task SaveAsync(DocumentSaveType documentSaveType)
        {
            if (Case == null)
            {
                throw new Exception("Дело для сохранения не может отсутствовать.");
            }

            stopwatch.Start();

            bool savingResult = false;
            int numberOfSavingThing = 25;
            int numberOfSavedThing = 0;

            GeneralInfo generalInfo = Case.GeneralInfo;
            CaseAccidentPlace caseAccidentPlace = Case.CaseAccidentPlace;
            RoadCondition roadCondition = Case.RoadCondition;
            ICollection<ParticipantsInformation> participantsInformations = Case.ParticipantsInformations;
            ICollection<Vehicle> vehicles = Case.Vehicles;
            ICollection<Victim> victims = Case.Victims;

            return Task.Run(() =>
            {
                try
                {
                    if (generalInfo == null || caseAccidentPlace == null || roadCondition == null ||
                        participantsInformations == null || vehicles == null || victims == null)
                    {
                        throw new Exception("Дело для сохранения не может отсутствовать.");
                    }

                    wordSaver.OpenDocument();

                    wordSaver.Replace<GeneralInfo>(generalInfo, Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne,
                                                   propName => $"%{propName}%",
                                                   (propName, propValue) =>
                                                   {
                                                       if (propValue == null)
                                                       {
                                                           return "";
                                                       }

                                                       string value = ConverToString(propValue);

                                                       if (propName == "CardNumber")
                                                       {
                                                           value.AddSeparator('-', 2, 10);
                                                       }

                                                       return WrapEachSbmlInVerticalLine(value);
                                                   });

                    dispatcher.Invoke(() => SavingProgress = ((double)(++numberOfSavedThing) / numberOfSavingThing) * 100);

                    if (caseAccidentPlace.AccidentOnVillage != null)
                    {
                        wordSaver.Replace<AccidentOnVillage>(caseAccidentPlace.AccidentOnVillage,
                                                             Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne,
                                                             propName => $"%{propName}%",
                                                             (propName, propValue) =>
                                                             {
                                                                 if (propValue == null)
                                                                 {
                                                                     return "";
                                                                 }

                                                                 string value = propValue.ToString();

                                                                 if (propName.Contains("Code"))
                                                                 {
                                                                     value = value.AddZeroBeforeText(4 - value.Length);
                                                                 }
                                                                 else
                                                                 {
                                                                     int maxLength = GetAttributeMaxLength(caseAccidentPlace.AccidentOnVillage, propName);

                                                                     if (maxLength != -1)
                                                                     {
                                                                         value = value.AddSymbols('_', maxLength - value.Length);
                                                                     }
                                                                 }

                                                                 return WrapEachSbmlInVerticalLine(value);
                                                             });

                        wordSaver.Replace("%HighwayIndexAndNumber%", "|__|-|__|__|-|__|__|-|__|", Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace("%AdditionalInfo%", "_______________________________________________________", Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace("%Kilometer%", "|__|__|__|__|", Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace("%Meter%", "|__|__|__|", Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace("%Binding%", "_______________________________________________________" +
                                                        " _______________________________________________________", Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                    }
                    else if (caseAccidentPlace.AccidentOnHighway != null)
                    {
                        wordSaver.Replace<AccidentOnVillage>(new AccidentOnVillage(),
                                                             Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne,
                                                             propName => $"%{propName}%",
                                                             (propName, propValue) =>
                                                             {
                                                                 if (propName == "Status")
                                                                 {
                                                                     return "|__|";
                                                                 }
                                                                 else if (propName.Contains("Code"))
                                                                 {
                                                                     return "|__|__|__|__|";
                                                                 }
                                                                 else if (propName == "VillageBinding")
                                                                 {
                                                                     return "|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|" +
                                                                            " |__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|";
                                                                 }
                                                                 else
                                                                 {
                                                                     return "|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|";
                                                                 }
                                                             });

                        wordSaver.Replace<AccidentOnHighway>(caseAccidentPlace.AccidentOnHighway,
                                                             Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne,
                                                             propName => $"%{propName}%",
                                                             (propName, propValue) =>
                                                             {
                                                                 if (propValue == null)
                                                                 {
                                                                     return "";
                                                                 }

                                                                 string value = propValue.ToString();

                                                                 if (propName == "AdditionalInfo" || propName == "Binding")
                                                                 {
                                                                     return value;
                                                                 }

                                                                 if (propName == "HighwayIndexAndNumber")
                                                                 {
                                                                     value = value.AddSeparator('-', 1, 4, 7);
                                                                 }

                                                                 return WrapEachSbmlInVerticalLine(value);
                                                             });

                    }

                    dispatcher.Invoke(() => SavingProgress = ((double)(++numberOfSavedThing) / numberOfSavingThing) * 100);

                    wordSaver.Replace<RoadCondition>(roadCondition,
                                                     Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne,
                                                     propName =>
                                                     {
                                                         StringBuilder propNameBuilder = new StringBuilder();

                                                         propNameBuilder.Append('%');
                                                         propNameBuilder.Append(propName.GetStringWithUpperSymbols());
                                                         propNameBuilder.Append('%');

                                                         return propNameBuilder.ToString();
                                                     },
                                                     (propName, propValue) =>
                                                     {
                                                         if (propValue == null)
                                                         {
                                                             return "";
                                                         }

                                                         string value = propValue.ToString();

                                                         if (propName == "SurfaceState")
                                                         {
                                                             value = value.AddSeparator(',', 1);
                                                         }
                                                         else if (propName == "PlaceElement")
                                                         {
                                                             value = value.AddSeparator(',', 2, 5);
                                                         }
                                                         else if (propName == "TechnicalTool" || propName == "RoadDisadvantages")
                                                         {
                                                             value = value.AddSeparator(',', 2, 5, 8, 11);
                                                         }

                                                         return WrapEachSbmlInVerticalLine(value);
                                                     });

                    dispatcher.Invoke(() => SavingProgress = ((double)(++numberOfSavedThing) / numberOfSavingThing) * 100);

                    List<ParticipantsInformation> tempList = null;

                    if (participantsInformations.Count < 5)
                    {
                        tempList = new List<ParticipantsInformation>();

                        tempList.AddRange(participantsInformations);
                        ParticipantsInformation participantsInformation = new ParticipantsInformation() { Id = -1 };

                        for (int i = tempList.Count; i < 5; i++)
                        {
                            tempList.Add(participantsInformation);
                        }
                    }

                    string qualification = null;
                    string age = null;
                    string gender = null;
                    string citizenship = null;
                    string driveExpirience = null;
                    string drivingTimeBeforeAccident = null;
                    string pddViolation = null;

                    for (int i = 0; i < tempList.Count; i++)
                    {
                        dispatcher.Invoke(() => SavingProgress = ((double)(++numberOfSavedThing) / numberOfSavingThing) * 100);

                        if (tempList[i].Id != -1)
                        {
                            qualification = tempList[i].Qualification.ToString();
                            age = tempList[i].Age.ToString();
                            gender = tempList[i].Gender.ToString();
                            citizenship = tempList[i].Citizenship.ToString();
                            driveExpirience = tempList[i].DriveExpirience.ToString();
                            drivingTimeBeforeAccident = tempList[i].DrivingTimeBeforeAccident.ToString();
                            pddViolation = tempList[i].PDDViolation.AddSeparator(',', 2);

                            qualification = WrapEachSbmlInVerticalLine(qualification.AddZeroBeforeText(2 - qualification.Length));
                            age = WrapEachSbmlInVerticalLine(age.AddZeroBeforeText(3 - age.Length));
                            citizenship = WrapEachSbmlInVerticalLine(citizenship.AddZeroBeforeText(GetAttributeMaxLength(tempList[i], "Citizenship") - citizenship.Length));
                            driveExpirience = WrapEachSbmlInVerticalLine(driveExpirience.AddZeroBeforeText(2 - driveExpirience.Length));
                            drivingTimeBeforeAccident = WrapEachSbmlInVerticalLine(drivingTimeBeforeAccident.AddZeroBeforeText(2 - drivingTimeBeforeAccident.Length));
                            pddViolation = WrapEachSbmlInVerticalLine(pddViolation);

                            if (gender != "True")
                            {
                                gender = "| М |";
                            }
                            else
                            {
                                gender = "| Ж |";
                            }
                        }
                        else
                        {
                            qualification = "|__|__|";
                            age = "|__|__|__|";
                            gender = "|__|";
                            citizenship = age;
                            driveExpirience = qualification;
                            drivingTimeBeforeAccident = qualification;
                            pddViolation = "|__|__|,|__|__|";
                        }

                        wordSaver.Replace($"%Surname{i + 1}%", tempList[i].Surname, Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%Name{i + 1}%", tempList[i].Name, Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%Patronymic{i + 1}%", tempList[i].Patronymic, Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%Address{i + 1}%", tempList[i].Address, Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%Qualification{i + 1}%", qualification, Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%Age{i + 1}%", age, Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%Gender{i + 1}%", gender, Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%Citizenship{i + 1}%", citizenship, Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%DriveExpirience{i + 1}%", driveExpirience, Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%DTBA{i + 1}%", drivingTimeBeforeAccident, Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%PDDViolation{i + 1}%", pddViolation, Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                    }


                    for (int i = 0; i < vehicles.Count; i++)
                    {
                        dispatcher.Invoke(() => SavingProgress = ((double)(++numberOfSavedThing) / numberOfSavingThing) * 100);

                        wordSaver.Replace<Vehicle>(vehicles.ElementAt(i),
                                                   Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne,
                                                         propName =>
                                                         {
                                                             StringBuilder propNameBuilder = new StringBuilder();

                                                             propNameBuilder.Append('%');

                                                             if (propName == "SeriesOfRegistrationSertificate" ||
                                                                 propName == "RegistrationSertificate" ||
                                                                 propName == "TrailerAvailability" ||
                                                                 propName == "LicenceSeries" ||
                                                                 propName == "LicenceNumber")
                                                             {
                                                                 propNameBuilder.Append(propName.GetStringWithUpperSymbols() + (i + 1));
                                                             }
                                                             else if (propName == "Surname")
                                                             {
                                                                 propNameBuilder.Append("v" + propName + (i + 1));
                                                             }
                                                             else
                                                             {
                                                                 propNameBuilder.Append(propName + (i + 1));
                                                             }

                                                             propNameBuilder.Append('%');

                                                             return propNameBuilder.ToString();
                                                         },
                                                         (propName, propValue) =>
                                                         {
                                                             if (propValue == null)
                                                             {
                                                                 return "";
                                                             }

                                                             string value = ConverToString(propValue);

                                                             if (propName == "TechnicalFaults")
                                                             {
                                                                 value = value.AddSeparator(',', 1);
                                                             }
                                                             else if (propName == "EDRPOU_Code1")
                                                             {
                                                                 value = value.AddSeparator('-', 7);
                                                             }
                                                             else if (propName == "CorruptionCode")
                                                             {
                                                                 value = value.AddSeparator(',', 2, 5, 8);
                                                             }

                                                             int maxLength = GetAttributeMaxLength<Vehicle>(vehicles.ElementAt(i), propName);

                                                             if (maxLength != -1)
                                                             {
                                                                 value = value.AddSymbols('_', maxLength - value.Length);
                                                             }

                                                             value = WrapEachSbmlInVerticalLine(value);

                                                             if (propName == "CorruptionCode")
                                                             {
                                                                 for (int j = 0; j < 3; j++)
                                                                 {
                                                                     value = value.Replace(" , ", ",");
                                                                 }
                                                             }

                                                             return value;
                                                         });
                    }

                    Vehicle vehicle = new Vehicle();

                    for (int i = vehicles.Count; i <= 5; i++)
                    {
                        dispatcher.Invoke(() => SavingProgress = ((double)(++numberOfSavedThing) / numberOfSavingThing) * 100);

                        wordSaver.Replace($"%PlateNumber{i}%", "|__|__|__|__|__|__|__|__|", Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%FrameNumber{i}%", "|__|__|__|__|__|__|__|__|", Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%ChasisNumber{i}%", "|__|__|__|__|__|__|__|__|__|", Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%Make{i}%", "|__|__|__|__|__|__|__|__|__|__|", Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%Model{i}%", "|__|__|__|__|__|__|__|__|__|__|", Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%Type{i}%", "|__|__|__|", Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%SORS{i}%", "|__|__|__|", Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%RS{i}%", "|__|__|__|__|__|__|", Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%TA{i}%", "|__|", Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%InsurerCode{i}%", "|__||__||__|", Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%PolicySeries{i}%", "|__|__|__|__|__|__|__|__|__||__|", Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%PolicyNumber{i}%", "|__|__|__|__|__|__|__|__|__||__|", Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%PolicyEndDate{i}%", "|__|__|-|__|__|-|__|__|__|__|", Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%vSurname{i}%", "|__|__|__|__|__|__|__|__|__|__|", Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%LS{i}%", "|__|__|__|", Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%LN{i}%", "|__|__|__|__|__|__|", Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%Owner{i}%", "|__|__|__|__|__|__|__|__|__|__| |__|__|__|__|__|__|__|__|__|__|", Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%TechnicalFaults{i}%", "|__|,|__|", Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%EDRPOU_Code{i}%", "|__|__|__|__|__|__|__| -|__|__|__|", Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%CorruptionCode{i}%", "|__|__|,|__|__|,|__|__|,|__|__|", Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%ActivityLicensingInfo{i}%", "|__||__|", Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                    }

                    List<Victim> victimTempList = null;

                    if (victims.Count < 10)
                    {
                        victimTempList = new List<Victim>();

                        victimTempList.AddRange(victims);
                        Victim victim = new Victim() { Id = -1 };

                        for (int i = victimTempList.Count; i < 10; i++)
                        {
                            victimTempList.Add(victim);
                        }
                    }

                    string victimIsDied = null;
                    string victimCategory = null;
                    string victimGender = null;
                    string victimAge = null;
                    string victimTORSerialNumber = null;
                    string victimSeatBelt = null;
                    string victimMedicalResult = null;
                    string victimCitizenship = null;

                    for (int i = 0; i < victimTempList.Count; i++)
                    {
                        dispatcher.Invoke(() => SavingProgress = ((double)(++numberOfSavedThing) / numberOfSavingThing) * 100);

                        if (victimTempList[i].Id != -1)
                        {
                            victimIsDied = victimTempList[i].IsDied.AddSymbols('_', GetAttributeMaxLength(victims.ElementAt(i), "IsDied") - victims.ElementAt(i).IsDied.Length);
                            victimCategory = victimTempList[i].Category.ToString();
                            victimGender = victimTempList[i].Gender.ToString();
                            victimAge = victimTempList[i].Age.ToString();
                            victimTORSerialNumber = victimTempList[i].TORSerialNumber.ToString();
                            victimSeatBelt = ConverToString(victimTempList[i].SeatBelt);
                            victimMedicalResult = victimTempList[i].MedicalResult.ToString();
                            victimCitizenship = victimTempList[i].Citizenship;

                            victimIsDied = WrapEachSbmlInVerticalLine(victimIsDied);
                            victimCategory = WrapEachSbmlInVerticalLine(victimCategory.AddZeroBeforeText(2 - victimCategory.Length));
                            victimAge = WrapEachSbmlInVerticalLine(victimAge.AddZeroBeforeText(3 - victimAge.Length));
                            victimTORSerialNumber = WrapEachSbmlInVerticalLine(victimTORSerialNumber.AddZeroBeforeText(2 - victimTORSerialNumber.Length));
                            victimSeatBelt = WrapEachSbmlInVerticalLine(victimSeatBelt);
                            victimMedicalResult = WrapEachSbmlInVerticalLine(victimMedicalResult);
                            victimCitizenship = WrapEachSbmlInVerticalLine(victimCitizenship.AddZeroBeforeText(GetAttributeMaxLength(victims.ElementAt(i), "Citizenship") - victimCitizenship.Length));

                            if (victimGender != "True")
                            {
                                victimGender = "| М |";
                            }
                            else
                            {
                                victimGender = "| Ж |";
                            }
                        }
                        else
                        {
                            victimIsDied = "|__|__|";
                            victimCategory = "|__|__|";
                            victimGender = "|__|";
                            victimAge = "|__|__|__|";
                            victimTORSerialNumber = "|__|__|";
                            victimSeatBelt = "|__|";
                            victimMedicalResult = "|__|";
                            victimCitizenship = "|__|__|__|";
                        }

                        wordSaver.Replace($"%IsDied{i + 1}%", victimIsDied, Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%Cy{i + 1}%", victimCategory, Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%Age{i + 1}%", victimAge, Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%TOR{i + 1}%", victimTORSerialNumber, Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%G{i + 1}%", victimGender, Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%SB{i + 1}%", victimSeatBelt, Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%MR{i + 1}%", victimMedicalResult, Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%S{i + 1}%", victimTempList[i].Surname, Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%N{i + 1}%", victimTempList[i].Name, Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%P{i + 1}%", victimTempList[i].Patronymic, Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace($"%C{i + 1}%", victimCitizenship, Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                    }

                    string allVictims = victims.Count.ToString();
                    allVictims = WrapEachSbmlInVerticalLine(allVictims.AddZeroBeforeText(3 - allVictims.Length));

                    wordSaver.Replace($"%AV%", allVictims, Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);

                    if (Officer != null)
                    {
                        wordSaver.Replace("%UserFullName%", $"{Officer.Name} {Officer.Surname}", Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        wordSaver.Replace("%UserPhone%", Officer.Phone, Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                    }

                    dispatcher.Invoke(() => SavingProgress = ((double)(++numberOfSavedThing) / numberOfSavingThing) * 100);

                    if (SaveFilePath != null && SaveFilePath != FilePath)
                    {
                        wordSaver.SaveDocumentAs(SaveFilePath, documentSaveType);
                    }
                    else
                    {
                        wordSaver.SaveDocument();
                    }

                    dispatcher.Invoke(() => SavingProgress = ((double)(++numberOfSavedThing) / numberOfSavingThing) * 100);

                    dispatcher.Invoke(() =>
                    {
                        savingResult = true;
                        stopwatch.Stop();
                        OnProgressSaved(stopwatch.Elapsed, savingResult);
                        SavingProgress = 0;
                    });
                }
                catch (Exception ex)
                {
                    dispatcher.Invoke(() => 
                    {
                        stopwatch.Stop();
                        OnExceptionCaptured(ex, stopwatch.Elapsed, savingResult);
                    });
                }
                finally
                {
                    wordSaver.CloseDocument();
                }
            });
        }

        private string WrapEachSbmlInVerticalLine(string textToWrap)
        {
            StringBuilder wrapedString = new StringBuilder();

            foreach (var smbl in textToWrap)
            {
                wrapedString.Append("| ");
                wrapedString.Append(smbl);
                wrapedString.Append(' ');
            }

            wrapedString.Append('|');

            return wrapedString.ToString();
        }

        private string ConverToString(object _object)
        {
            string formattedValue = null;

            switch (_object.GetType().Name)
            {
                case "DateTime":
                    formattedValue = ((DateTime)_object).ToString("d").GetStrWithoutSeparator('.').AddSeparator('-', 2, 5);
                    break;
                case "TimeSpan":
                    formattedValue = ((TimeSpan)_object).ToString(@"hh\:mm").Remove(2, 1).AddSeparator('-', 2);
                    break;
                case "Boolean":
                    if ((bool)_object)
                    {
                        formattedValue = "+";
                    }
                    else
                    {
                        formattedValue = "-";
                    }
                    break;
                default:
                    formattedValue = _object.ToString();
                    break;
            }

            return formattedValue;
        }

        private int GetAttributeMaxLength<T>(T _object, string propertyName)
        {
            int length = -1;

            if (_object == null || string.IsNullOrEmpty(propertyName))
            {
                return length;
            }

            Type objectType = _object.GetType();

            Attribute attr = objectType.GetProperty(propertyName).GetCustomAttribute(typeof(System.ComponentModel.DataAnnotations.StringLengthAttribute), false);

            if (attr != null)
            {
                length = ((System.ComponentModel.DataAnnotations.StringLengthAttribute)attr).MaximumLength;
            }

            return length;
        }
        #endregion

        private void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        private void OnProgressSaved(TimeSpan elapsedTime, bool isSaved)
        {
            Saved?.Invoke(new WordSaveActionArgs(elapsedTime, isSaved));
        }
        private void OnProgressCanceled(TimeSpan elapsedTime, bool isSaved)
        {
            Canceled?.Invoke(new WordSaveActionArgs(elapsedTime, isSaved, true));
        }
        private void OnExceptionCaptured(Exception ex, TimeSpan elapsedTime, bool isSaved)
        {
            ExceptionCaptured?.Invoke(ex, new WordSaveActionArgs(elapsedTime, isSaved, true));
        }

        public void Dispose()
        {
            wordSaver.Dispose();
        }
    }
}
