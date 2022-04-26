using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Windows.Data;
using System.Collections.ObjectModel;
using AccountingOfTrafficViolation.Models;
using AccountingOfTrafficViolation.Services;
using AccountingOfTrafficViolation.Views.UserControls;
using AccountOfTrafficViolationDB.Context;
using AccountOfTrafficViolationDB.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountingOfTrafficViolation.Views
{
    /// <summary>
    /// Логика взаимодействия для CodesWindow.xaml
    /// </summary>
    public partial class CodesWindow : Window
    {
        private TVAContext _TVAContext;
        private Officer officer;
        private CodeInfo unchengedCodeInfo;
        private ObservableCollection<CodeInfo> codeInfos;
        private CodeInfo CodeInfo;

        private bool isAddingMode;
        private bool isEditingMode;
        private bool isChanged;

        private Button AddButton;
        private Button EditButton;
        private Button DeleteButton;
        private Button SelectButton;
        private Button SaveButton;
        private Button DiscardButton;

        public string Code { get { return CodeInfo.Code; } }

        public CodesWindow() : this(null)
        { }
        public CodesWindow(Officer officer)
        {
            InitializeComponent();

            this.officer = officer;

            SelectButton = new Button();
            DiscardButton = new Button();

            SelectButton.Content = "Выбрать";
            DiscardButton.Content = "Отменить";

            // if (officer != null && (officer.Role == (byte)UserRole.Debug || officer.Role == (byte)UserRole.Admin)) 
            // {
            //     AddButton = new Button();
            //     EditButton = new Button();
            //     DeleteButton = new Button();
            //     SaveButton = new Button();
            //
            //     AddButton.Content = "Добавить";
            //     EditButton.Content = "Изменить";
            //     DeleteButton.Content = "Удалить";
            //     SaveButton.Content = "Сохранить в БД";
            //
            //     SaveButton.IsEnabled = false;
            //     DiscardButton.IsEnabled = false;
            //
            //     AddButton.Click += AddButton_Click;
            //     EditButton.Click += EditButton_Click;
            //     DeleteButton.Click += DeleteButton_Click;
            //     SaveButton.Click += SaveButton_Click;
            //     DiscardButton.Click += DiscardButton_Click;
            //
            //     ButtonFieldStackPanel.Children.Add(AddButton);
            //     ButtonFieldStackPanel.Children.Add(EditButton);
            //     ButtonFieldStackPanel.Children.Add(DeleteButton);
            //     ButtonFieldStackPanel.Children.Add(SaveButton);
            // }
            // else
            // {
                DiscardButton.Click += CancelButton_Click;
            // }

            SelectButton.Click += SelectButton_Click;

            ButtonFieldStackPanel.Children.Add(SelectButton);
            ButtonFieldStackPanel.Children.Add(DiscardButton);

            CodeInfo = null;
            isAddingMode = false;
            isEditingMode = false;
            isChanged = false;

            LoadContext(ex => MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error));

            GC.Collect();
        }

        private bool AddCode()
        {
            if (CodeGroupBox.CheckIfExistValidationError())
            {
                MessageBox.Show("Есть поля, которые заполены не правильно.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            codeInfos.Add((CodeInfo)DataContext);
            // _TVAContext.CodeInfos.Add((CodeInfo)DataContext);

            isChanged = true;

            SaveButton.IsEnabled = true;
            DiscardButton.IsEnabled = true;

            FilterCollection();

            return true;
        }
        private bool EditCode()
        {
            // if (!(DataContext is this.CodeInfo))
            // {
            //     return false;
            // }

            if (CodeGroupBox.CheckIfExistValidationError())
            {
                MessageBox.Show("Есть поля, которые заполены не правильно.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            unchengedCodeInfo.Assign((CodeInfo)DataContext);
            isChanged = true;

            SaveButton.IsEnabled = true;
            DiscardButton.IsEnabled = true;

            return true;
        }
        private bool RemoveCode()
        {
            if (CodeInfo == null)
            {
                return false;
            }

            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить выбранный код?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Information);

            if (result != MessageBoxResult.Yes)
            {
                return false;
            }

            // _TVAContext.CodeInfos.Remove(CodeInfo);
            codeInfos.Remove(CodeInfo);
            isChanged = true;

            SaveButton.IsEnabled = true;
            DiscardButton.IsEnabled = true;
            return true;
        }

        private void OnMode()
        {
            AddButton.Content = "Сохранить";
            DeleteButton.Content = "Отменить";

            CodeNameTB.IsEnabled = true;
            CodeValueTB.IsEnabled = true;
            DescriptionTB.IsEnabled = true;

            EditButton.IsEnabled = false;
        }
        private void OffMode()
        {
            AddButton.Content = "Добавить";
            DeleteButton.Content = "Удалить";

            CodeNameTB.IsEnabled = false;
            CodeValueTB.IsEnabled = false;
            DescriptionTB.IsEnabled = false;

            EditButton.IsEnabled = true;
        }

        private void OnAddingMode()
        {
            CodeInfo codeInfo = new CodeInfo();
            if (codeInfos.Count > 0)
            {
                codeInfo.Id = codeInfos[codeInfos.Count - 1].Id;
            }

            DataContext = codeInfo;


            isAddingMode = true;

            OnMode();
        }
        private void OffAddingMode()
        {
            isAddingMode = false;

            OffMode();
        }

        private void OnEditingMode()
        {
            if (!(DataContext is CodeInfo))
            {
                return;
            }

            unchengedCodeInfo = (CodeInfo)DataContext;

            if (CodeInfo == null)
            {
                CodeInfo = new CodeInfo();
            }

            CodeInfo.Assign(unchengedCodeInfo);

            DataContext = CodeInfo;

            isEditingMode = true;

            OnMode();
        }
        private void OffEditingMode()
        {
            isEditingMode = false;

            OffMode();
        }

        private void FilterCollection()
        {
            if (string.IsNullOrEmpty(CodeNameFiltrTB.Text) && string.IsNullOrEmpty(CodeValueFiltrTB.Text))
            {
                CodeGrid.ItemsSource = codeInfos;
                return;
            }

            try
            {
                CodeGrid.ItemsSource = codeInfos.Where(c =>
                                                  (string.IsNullOrEmpty(CodeValueFiltrTB.Text) || c.Code == CodeValueFiltrTB.Text || c.Code.Contains(CodeValueFiltrTB.Text)) &&
                                                  (string.IsNullOrEmpty(CodeNameFiltrTB.Text) || c.Name == CodeNameFiltrTB.Text || c.Name.Contains(CodeNameFiltrTB.Text)));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        private async void LoadContext(Action<Exception> action = null)
        {
            LoadView loadView = new LoadView();

            try
            {
                Grid.SetRowSpan(loadView, 3);

                Binding widthBinding = new Binding()
                {
                    Source = this,
                    Path = new PropertyPath("ActualWidth")
                };
                Binding heightBinding = new Binding()
                {
                    Source = this,
                    Path = new PropertyPath("ActualHeight")
                };
                loadView.SetBinding(WidthProperty, widthBinding);
                loadView.SetBinding(HeightProperty, heightBinding);

                MainField.Children.Add(loadView);

                _TVAContext = await Task.Run<TVAContext>(() =>
                {
                    TVAContext dbContext = new TVAContext(GlobalSettings.ConnectionStrings[Constants.DefaultDB], GlobalSettings.Credential);
                    return dbContext;
                });

                // List<CodeInfo> listRes = await _TVAContext.CodeInfos.ToListAsync();

                // codeInfos = await Task.Run(() =>
                // {
                //     return new ObservableCollection<CodeInfo>(listRes);
                // });

                CodeGrid.ItemsSource = codeInfos;
            }
            catch (Exception ex) when (action != null)
            {
                action(ex);
                this.Close();
            }
            finally
            {
                MainField.Children.Remove(loadView);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (isAddingMode && AddCode())
            {
                OffAddingMode();
            }
            else if (isEditingMode && EditCode())
            {
                OffEditingMode();
            }
            else if (!isAddingMode && !isEditingMode)
            {
                OnAddingMode();
            }

        }
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (!isEditingMode)
            {
                OnEditingMode();
            }
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (isAddingMode)
            {
                OffAddingMode();
            }
            else if (isEditingMode)
            {
                OffEditingMode();
            }
            else
            {
                RemoveCode();
            }
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _TVAContext.SaveChanges();
            isChanged = false;

            SaveButton.IsEnabled = false;
            DiscardButton.IsEnabled = false;

            MessageBox.Show("Все изменения сохранены в бд.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void DiscardButton_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < codeInfos.Count; i++)
            {
                if (_TVAContext.Entry(codeInfos[i]).State == EntityState.Added)
                {
                    codeInfos.RemoveAt(i);
                    i--;
                }
            }

            _TVAContext.CancelAllChanges();
            isChanged = false;

            SaveButton.IsEnabled = false;
            DiscardButton.IsEnabled = false;

            FilterCollection();

            MessageBox.Show("Все изменения отменены.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            if (CodeInfo == null && DataContext != null)
            {
                CodeInfo = (CodeInfo)DataContext;
            }
            else if (DataContext == null)
            {
                CodeInfo = new CodeInfo() { Code = null };
            }

            this.DialogResult = true;
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (isChanged &&
                MessageBox.Show("Есть изменённые данные, вы уверены, что хотите выйти?.", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Information) != MessageBoxResult.Yes)
            {
                return;
            }

            if (_TVAContext != null)
            {
                _TVAContext.Dispose();
            }
        }

        private void CodeGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is DataGrid dataGrid)
            {
                DataContext = dataGrid.CurrentItem;
            }
        }

        private void FilterCodes_Click(object sender, RoutedEventArgs e)
        {
            FilterCollection();
        }
    }
}
