using Diary.Commands;
using Diary.Models;
using Diary.Models.Domains;
using Diary.Models.Wrappers;
using Diary.Views;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Diary.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private Repository _repository = new Repository();
        public  MainViewModel()
        {

            AddStudentCommand = new RelayCommand(AddEditStudent);
            EditStudentCommand = new RelayCommand(AddEditStudent, CanEditStudent);
            DeleteStudentCommand = new AsyncRelayCommand(DeleteStudent, CanDeleteStudent);
            RefreshStudentsCommand = new RelayCommand(RefreshStudents);
            SettingsUserCommand = new RelayCommand(SettingsUser);
            if(ConnectSqlStatus())
            {
                RefreshDiary();
                InitGroups();
            }
            else
            {
                SqlConnectionError(Application.Current.MainWindow);
            }

            
        }

        public ICommand AddStudentCommand { get; set; }
        public ICommand EditStudentCommand { get; set; }
        public ICommand DeleteStudentCommand { get; set; }
        public ICommand RefreshStudentsCommand { get; set; }
        public ICommand SettingsUserCommand { get; set; }

        private StudentWrapper _selectedStudent;

        public StudentWrapper SelectedStudent
        {
            get { return _selectedStudent; }
            set
            {
                _selectedStudent = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<StudentWrapper> _students;

        public ObservableCollection<StudentWrapper> Students
        {
            get { return _students; }
            set
            {
                _students = value;
                OnPropertyChanged();
            }
        }

        private int _selectedGroupId;

        public int SelectedGroupId
        {
            get { return _selectedGroupId; }
            set
            {
                _selectedGroupId = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Group> _groups;

        public ObservableCollection<Group> Groups
        {
            get { return _groups; }
            set
            {
                _groups = value;
                OnPropertyChanged();
            }
        }
        private void AddEditStudent(object obj)
        {
            var addEdditStudentWindow = new AddEditStudentView(obj as StudentWrapper);
            addEdditStudentWindow.Closed += addEdditStudentWindow_Closed;
            addEdditStudentWindow.ShowDialog();
        }

        private void addEdditStudentWindow_Closed(object sender, EventArgs e)
        {
            RefreshDiary();
        }

        private bool CanEditStudent(object obj)
        {
            return SelectedStudent != null;
        }


        private async Task DeleteStudent(object obj)
        {
            var metroWindow = Application.Current.MainWindow as MetroWindow;
            var dialog = await metroWindow.ShowMessageAsync("Usuwanie ucznia", $"Czy chcesz usunąć ucznia {SelectedStudent.FirstName} {SelectedStudent.LastName}?", MessageDialogStyle.AffirmativeAndNegative);

            if (dialog != MessageDialogResult.Affirmative)
                return;

            _repository.DeleteStudent(SelectedStudent.Id);

            RefreshDiary();
        }

        private bool CanDeleteStudent(object obj)
        {
            return SelectedStudent != null;
        }

        private void RefreshStudents(object obj)
        {
            RefreshDiary();
        }

        private void InitGroups()
        {

            var groups = _repository.GetGroups();
            groups.Insert(0, new Group { Id = 0, Name = "Wszystkie" });
            Groups = new ObservableCollection<Group>(groups);

            SelectedGroupId = 0;
        }

        private void RefreshDiary()
        {
            Students = new ObservableCollection<StudentWrapper>(_repository.GetStudents(SelectedGroupId));
        }

        private void SettingsUser(object obj)
        {
            var userSettingsWindow = new UserSettingsView();
            userSettingsWindow.ShowDialog();
        }

        private bool ConnectSqlStatus()
        {
            using (var context = new ApplicationDbContext())
            {
                try
                {
                    context.Database.Connection.Open();
                    context.Database.Connection.Close();
                }
                catch (SqlException)
                {
                    return false;
                }
                return true;

            }
            
        }

        private  async void SqlConnectionError(Window window)
        {
            var metroWindow = Application.Current.MainWindow as MetroWindow;
            var dialog = await metroWindow.ShowMessageAsync("Błąd", "Błąd połączenia z bazą SQL. Czy chcesz zmienić dane logowania SQL?", MessageDialogStyle.AffirmativeAndNegative);

            if(dialog == MessageDialogResult.Affirmative)
            {
                var userSettingsWindow = new UserSettingsView();
                userSettingsWindow.ShowDialog();
            }
            else
            {
                window.Close();
            }

        }

        
    }
}
