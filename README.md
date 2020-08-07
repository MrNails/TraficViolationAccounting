# TraficViolationAccounting

This program is for recording traffic violations and is created for the test. The database is not perfect and needs some work (if not complete rework). In this program I have used everything I know.

# Dependencies
This program contains such dependencies:
 - Newsofton
 - EntityFramework
 - Microsoft XAML Behaviours
 
# Services
Services contains help classes such as extension classes, converter classes (for XAML), command class, and logger.
 
 # Models
This folder contains all models and some basic business logic. There is also a DataBase context (TVAContext) and an abstract class for some common things like: 
 - errors property for visual check
 - implements the INotifyPropertyChanged interface

# ViewModels
Has three classes:
 - AccidentObjectVM - required for windows with multiple selection
 - CaseVM - for find case and get info about found case
 - UserVM - for management user model
 
# Views 
Contains all windows (except main window)