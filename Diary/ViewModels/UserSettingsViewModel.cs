using Diary.Commands;
using Diary.Models.Wrappers;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Diary.ViewModels
{
    public class UserSettingsViewModel : ViewModelBase
    {
        //Zrobiłem 4 pola ze względu na to ,że nazwę servera bazy danych(instancję) można podać w sqlServerName np. w ten sposob: (local)/SQLEXPERSS
        public UserSettingsViewModel() 
        {
            ConfirmCommand = new AsyncRelayCommand(Confirm, CanConfirm);
            CloseCommand = new RelayCommand(Close);

            _sqlServerName = Properties.Settings.Default.SqlServerName;
            _sqlDatabaseName = Properties.Settings.Default.SqlDataBaseName;
            _sqlLogin = Properties.Settings.Default.SqlLogin;
            _sqlPassword = Properties.Settings.Default.SqlPassword;
        }

        public ICommand ConfirmCommand { get; set; }
        public ICommand CloseCommand { get; set; }

        private string _sqlServerName;

        public string SqlServerName
        {
            get { return _sqlServerName; }
            set
            {
                _sqlServerName = value;
                OnPropertyChanged();
            }
        }

        private string _sqlDatabaseName;

        public string SqlDatabaseName
        {
            get { return _sqlDatabaseName; }
            set
            {
                _sqlDatabaseName = value;
                OnPropertyChanged();
            }
        }

        private string _sqlLogin;

        public string SqlLogin
        {
            get { return _sqlLogin; }
            set
            {
                _sqlLogin = value;
                OnPropertyChanged();
            }
        }

        private string _sqlPassword;

        public string SqlPassword
        {
            get { return _sqlPassword; }
            set
            {
                _sqlPassword = value;
                OnPropertyChanged();
            }
        }

        private async Task  Confirm(object obj)
        {
            await UpdateSqlUserDataAsync();
            CloseWindow(obj as Window);
            Restart();
        }


        private bool CanConfirm(object obj)
        {
            return true;
        }

        private void Close(object obj)
        {
            CloseWindow(obj as Window);   
        }

        private void CloseWindow(Window window)
        {
            window.Close();
        }

        private async Task UpdateSqlUserDataAsync()
        {
            await Task.Run(() => {
                Properties.Settings.Default.SqlServerName = _sqlServerName;
                Properties.Settings.Default.SqlDataBaseName = _sqlDatabaseName;
                Properties.Settings.Default.SqlLogin = _sqlLogin;
                Properties.Settings.Default.SqlPassword = _sqlPassword;
                Properties.Settings.Default.Save();
            });
        }

       
        private void Restart()
        {
            Process.Start(Application.ResourceAssembly.Location);;
            Application.Current.Shutdown();
        }
    }
}
