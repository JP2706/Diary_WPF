using Diary.Commands;
using Diary.Models;
using Diary.Models.Domains;
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
            ConfirmCommand = new AsyncRelayCommand(Confirm);
            CloseCommand = new RelayCommand(Close);
            UserSettings = new UserSettings();
            //_sqlServerName = userSetting.SqlServerName;
            //_sqlDatabaseName = userSetting.SqlDatabaseName;
            //_sqlLogin = userSetting.SqlLogin;
            //_sqlPassword = userSetting.SqlPassword;
        }

        public ICommand ConfirmCommand { get; set; }
        public ICommand CloseCommand { get; set; }

        private UserSettings _userSettings;

        public UserSettings UserSettings
        {
            get { return _userSettings; }
            set
            {
                _userSettings = value;
                OnPropertyChanged();
            }
        }

        //private string _sqlServerName;

        //public string SqlServerName
        //{
        //    get { return _sqlServerName; }
        //    set
        //    {
        //        _sqlServerName = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private string _sqlDatabaseName;

        //public string SqlDatabaseName
        //{
        //    get { return _sqlDatabaseName; }
        //    set
        //    {
        //        _sqlDatabaseName = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private string _sqlLogin;

        //public string SqlLogin
        //{
        //    get { return _sqlLogin; }
        //    set
        //    {
        //        _sqlLogin = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private string _sqlPassword;

        //public string SqlPassword
        //{
        //    get { return _sqlPassword; }
        //    set
        //    {
        //        _sqlPassword = value;
        //        OnPropertyChanged();
        //    }
        //}

        private async Task  Confirm(object obj)
        {
            if (!_userSettings.IsValid)
                return;
            await UpdateSqlUserDataAsync();
            CloseWindow(obj as Window);
            Restart();
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
                _userSettings.AddToSettings(_userSettings);
            });
        }

       
        private void Restart()
        {
            Process.Start(Application.ResourceAssembly.Location);;
            Application.Current.Shutdown();
        }
    }
}
