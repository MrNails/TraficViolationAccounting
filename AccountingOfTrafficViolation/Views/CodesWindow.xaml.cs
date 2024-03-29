﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Windows.Data;
using System.Collections.ObjectModel;
using System.Data;
using AccountingOfTrafficViolation.Models;
using AccountingOfTrafficViolation.Services;
using AccountingOfTrafficViolation.Views.UserControls;
using AccountOfTrafficViolationDB.Context;
using AccountOfTrafficViolationDB.Models;
using AccountOfTraficViolationDB.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace AccountingOfTrafficViolation.Views
{
    /// <summary>
    /// Логика взаимодействия для CodesWindow.xaml
    /// </summary>
    public partial class CodesWindow : Window
    {
        private readonly string m_fromWindow;

        private TVAContext m_TVAContext;
        private Code m_unchangedCode;
        private ObservableCollection<Code> m_codeInfos;
        private Code? m_code;

        private bool m_isAddingMode;
        private bool m_isEditingMode;
        private bool m_isChanged;
        private bool m_isAnyObjDeleted;

        private Button AddButton;
        private Button EditButton;
        private Button DeleteButton;
        private Button SelectButton;
        private Button SaveButton;
        private Button DiscardButton;

        public string Code { get { return m_code.Value; } }

        public CodesWindow() : this(string.Empty) { }
        public CodesWindow(string? fromWindow = "")
        {
            InitializeComponent();

            m_fromWindow = fromWindow;
            
            m_code = null;
            m_isAddingMode = false;
            m_isEditingMode = false;
            m_isChanged = false;
        }

        private bool AddCode()
        {
            if (CodeGroupBox.CheckIfExistValidationError())
            {
                MessageBox.Show("Есть поля, которые заполены не правильно.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (CodeBindingComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите привязку к окну.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            var code = (Code)DataContext;
            var cb = (CodeBinding)CodeBindingComboBox.SelectedItem;

            code.CodeBindingId = cb.Id;
            code.CodeBinding = cb;
            
            m_codeInfos.Add(code);

            m_isChanged = true;

            SaveButton.IsEnabled = true;
            DiscardButton.IsEnabled = true;

            FilterCollection();

            return true;
        }
        private bool EditCode()
        {
            if (DataContext is not Models.Code)
            {
                return false;
            }

            if (CodeGroupBox.CheckIfExistValidationError())
            {
                MessageBox.Show("Есть поля, которые заполены не правильно.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            m_unchangedCode.Assign((Code)DataContext);
            m_isChanged = true;

            SaveButton.IsEnabled = true;
            DiscardButton.IsEnabled = true;

            return true;
        }
        private bool RemoveCode()
        {
            if (m_code == null)
                return false;

            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить выбранный код?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Information);

            if (result != MessageBoxResult.Yes)
                return false;

            m_codeInfos.Remove(m_code);
            
            m_code = null;
            DataContext = null;
            
            m_isChanged = true;

            m_isAnyObjDeleted = true;

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
            CodeBindingComboBox.IsEnabled = string.IsNullOrEmpty(m_fromWindow);


            EditButton.IsEnabled = false;
        }
        private void OffMode()
        {
            AddButton.Content = "Добавить";
            DeleteButton.Content = "Удалить";

            CodeNameTB.IsEnabled = false;
            CodeValueTB.IsEnabled = false;
            DescriptionTB.IsEnabled = false;
            CodeBindingComboBox.IsEnabled = false;

            EditButton.IsEnabled = true;
        }

        private void OnAddingMode()
        {
            var code = new Code { Id = 1 };
            
            if (m_codeInfos.Count > 0)
                code.Id = m_codeInfos[m_codeInfos.Count - 1].Id + 1;

            DataContext = code;
            
            CodeValueTB.Text = string.Empty;
            CodeNameTB.Text = string.Empty;

            m_isAddingMode = true;

            OnMode();
        }
        private void OffAddingMode()
        {
            m_isAddingMode = false;

            OffMode();
        }

        private void OnEditingMode()
        {
            if (!(DataContext is Code))
            {
                return;
            }

            m_unchangedCode = (Code)DataContext;

            if (m_code == null)
            {
                m_code = new Code();
            }

            m_code.Assign(m_unchangedCode);

            DataContext = m_code;

            m_isEditingMode = true;

            OnMode();
        }
        private void OffEditingMode()
        {
            m_isEditingMode = false;

            OffMode();
        }

        private void FilterCollection()
        {
            if (string.IsNullOrEmpty(CodeNameFiltrTB.Text) && string.IsNullOrEmpty(CodeValueFiltrTB.Text))
            {
                CodeGrid.ItemsSource = m_codeInfos;
                return;
            }

            try
            {
                CodeGrid.ItemsSource = m_codeInfos.Where(c =>
                                                  (string.IsNullOrEmpty(CodeValueFiltrTB.Text) || c.Value == CodeValueFiltrTB.Text || c.Value.Contains(CodeValueFiltrTB.Text)) &&
                                                  (string.IsNullOrEmpty(CodeNameFiltrTB.Text) || c.Name == CodeNameFiltrTB.Text || c.Name.Contains(CodeNameFiltrTB.Text)));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        private async Task LoadContext(Action<Exception> action = null)
        {
            var loadView = new LoadView();

            try
            {
                Grid.SetRowSpan(loadView, 3);

                var widthBinding = new Binding
                {
                    Source = this,
                    Path = new PropertyPath("ActualWidth")
                };
                var heightBinding = new Binding
                {
                    Source = this,
                    Path = new PropertyPath("ActualHeight")
                };
                
                loadView.SetBinding(WidthProperty, widthBinding);
                loadView.SetBinding(HeightProperty, heightBinding);

                MainField.Children.Add(loadView);

                m_TVAContext = await Task.Run(() => new TVAContext(GlobalSettings.ConnectionStrings[Constants.DefaultDB], GlobalSettings.Credential));

                await (from c in m_TVAContext.Codes.DefaultIfEmpty()
                    join cb in m_TVAContext.CodeBindings.DefaultIfEmpty() on c.CodeBindingId equals cb.Id
                    where cb.Name == m_fromWindow
                    select c)
                    .LoadAsync();

                m_codeInfos = m_TVAContext.Codes.Local.ToObservableCollection();
                    
                CodeGrid.ItemsSource = m_codeInfos;
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
            if (m_isAddingMode && AddCode())
                OffAddingMode();
            else if (m_isEditingMode && EditCode())
                OffEditingMode();
            else if (!m_isAddingMode && !m_isEditingMode)
                OnAddingMode();
        }
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (!m_isEditingMode)
                OnEditingMode();
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (m_isAddingMode)
                OffAddingMode();
            else if (m_isEditingMode)
                OffEditingMode();
            else
                RemoveCode();
        }
        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var maxCodeId = 1;
            
            if (await m_TVAContext.Codes.AnyAsync())
                maxCodeId = await m_TVAContext.Codes.MaxAsync(c => c.Id) + 1;

            foreach (var code in m_codeInfos)
                if (m_TVAContext.Entry(code).State == EntityState.Added)
                    code.Id = maxCodeId++;

            await m_TVAContext.SaveChangesAsync();
            m_isChanged = false;

            SaveButton.IsEnabled = false;
            DiscardButton.IsEnabled = false;

            MessageBox.Show("Все изменения сохранены в бд.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void DiscardButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var entry in m_TVAContext.ChangeTracker.Entries())
                entry.Reload();

            if (m_isAnyObjDeleted)
                m_codeInfos = m_TVAContext.Codes.Local.ToObservableCollection();

            CodeGrid.ItemsSource = m_codeInfos;

            m_isChanged = false;

            SaveButton.IsEnabled = false;
            DiscardButton.IsEnabled = false;

            m_isAnyObjDeleted = false;
            
            FilterCollection();

            MessageBox.Show("Все изменения отменены.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            if (m_code == null && DataContext != null)
                m_code = (Code)DataContext;
            else if (DataContext == null)
                m_code = new Code { Value = string.Empty };

            this.DialogResult = true;
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (m_isChanged &&
                MessageBox.Show("Есть изменённые данные, вы уверены, что хотите выйти?.", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Information) != MessageBoxResult.Yes)
            {
                return;
            }

            if (m_TVAContext != null)
            {
                m_TVAContext.Dispose();
            }
        }

        private void CodeGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is DataGrid dataGrid)
            {
                DataContext = dataGrid.CurrentItem;

                if (dataGrid.CurrentItem is Code code)
                    m_code = code;
            }
        }

        private void FilterCodes_Click(object sender, RoutedEventArgs e)
        {
            FilterCollection();
        }

        private void ClearFindButton_OnClick(object sender, RoutedEventArgs e)
        {
            CodeNameFiltrTB.Text = string.Empty;
            CodeValueFiltrTB.Text = string.Empty;
            
            FilterCollection();
        }
        
        private async void CodesWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            await LoadContext(ex => MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error));
            
            SelectButton = new Button();
            DiscardButton = new Button();

            SelectButton.Content = "Выбрать";
            DiscardButton.Content = "Отменить";

            var isAdm = new SqlParameter { ParameterName = "@result", Direction = ParameterDirection.Output, DbType = DbType.Boolean};

            await m_TVAContext.Database.ExecuteSqlRawAsync("AccountOfTrafficViolation.dbo.proc_IAmAdmin @result out", isAdm);

            if ((bool)isAdm.Value) 
            {
                AddButton = new Button();
                EditButton = new Button();
                DeleteButton = new Button();
                SaveButton = new Button();
            
                AddButton.Content = "Добавить";
                EditButton.Content = "Изменить";
                DeleteButton.Content = "Удалить";
                SaveButton.Content = "Сохранить в БД";
            
                SaveButton.IsEnabled = false;
                DiscardButton.IsEnabled = false;
            
                AddButton.Click += AddButton_Click;
                EditButton.Click += EditButton_Click;
                DeleteButton.Click += DeleteButton_Click;
                SaveButton.Click += SaveButton_Click;
                DiscardButton.Click += DiscardButton_Click;
            
                ButtonFieldStackPanel.Children.Add(AddButton);
                ButtonFieldStackPanel.Children.Add(EditButton);
                ButtonFieldStackPanel.Children.Add(DeleteButton);
                ButtonFieldStackPanel.Children.Add(SaveButton);

                CodeBindingCol.Visibility = Visibility.Visible;
                CodeBindingTB.Visibility = Visibility.Visible;
                CodeBindingComboBox.Visibility = Visibility.Visible;

                var codeBindings = await (string.IsNullOrEmpty(m_fromWindow) ? 
                        m_TVAContext.CodeBindings : 
                        m_TVAContext.CodeBindings.Where(cb => cb.Name == m_fromWindow))
                    .ToListAsync();
                
                CodeBindingComboBox.ItemsSource = codeBindings;
                CodeBindingComboBox.SelectedIndex = codeBindings.Count == 0 ? -1 : 0;
            }
            else
            {
                DiscardButton.Click += CancelButton_Click;
                
                CodeBindingCol.Visibility = Visibility.Collapsed;
                CodeBindingTB.Visibility = Visibility.Collapsed;
                CodeBindingComboBox.Visibility = Visibility.Collapsed;
            }

            SelectButton.Click += SelectButton_Click;

            ButtonFieldStackPanel.Children.Add(SelectButton);
            ButtonFieldStackPanel.Children.Add(DiscardButton);
        }
    }
}
