using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diary.Models
{
    public class UserSettings : IDataErrorInfo
    {
        public UserSettings() 
        {
            SqlServerName = Properties.Settings.Default.SqlServerName;
            SqlDatabaseName = Properties.Settings.Default.SqlDataBaseName;
            SqlLogin = Properties.Settings.Default.SqlLogin;
            SqlPassword = Properties.Settings.Default.SqlPassword;
        }
        private bool _isSqlServerNameValid;
        private bool _isSqlDatabaseNameValid;
        private bool _isSqlLoginValid;
        private bool _isSqlPasswordValid;

        public string SqlServerName { get; set; }
        public string SqlDatabaseName { get; set; }
        public string SqlLogin { get; set; }
        public string SqlPassword { get; set; }

        public void AddToSettings(UserSettings uS)
        {
            Properties.Settings.Default.SqlServerName = uS.SqlServerName;
            Properties.Settings.Default.SqlDataBaseName = uS.SqlDatabaseName;
            Properties.Settings.Default.SqlLogin = uS.SqlLogin;
            Properties.Settings.Default.SqlPassword = uS.SqlPassword;
            Properties.Settings.Default.Save();
        }


        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case nameof(SqlServerName):
                        if (string.IsNullOrWhiteSpace(SqlServerName))
                        {
                            Error = "Pole Sql Server jest wymagane";
                            _isSqlServerNameValid = false;
                        }
                        else
                        {
                            Error = string.Empty;
                            _isSqlServerNameValid = true;
                        }
                        break;
                    case nameof(SqlDatabaseName):
                        if (string.IsNullOrWhiteSpace(SqlDatabaseName))
                        {
                            Error = "Pole Database Name jest wymagane";
                            _isSqlDatabaseNameValid = false;
                        }
                        else
                        {
                            Error = string.Empty;
                            _isSqlDatabaseNameValid = true;
                        }
                        break;
                    case nameof(SqlLogin):
                        if (string.IsNullOrWhiteSpace(SqlLogin))
                        {
                            Error = "Pole Login jest wymagane";
                            _isSqlLoginValid = false;
                        }
                        else
                        {
                            Error = string.Empty;
                            _isSqlLoginValid = true;
                        }
                        break;
                    //case nameof(SqlPassword):
                    //    if (string.IsNullOrWhiteSpace(SqlPassword))
                    //    {
                    //        Error = "Pole Hasło jest wymagane";
                    //        _isSqlPasswordValid = false;
                    //    }
                    //    else
                    //    {
                    //        Error = string.Empty;
                    //        _isSqlPasswordValid = true;
                    //    }
                    //    break;
                    default:
                        break;
                }

                return Error;
            }
        }
        public string Error { get; set; }

        public bool IsValid
        {
            get
            {
                return _isSqlDatabaseNameValid && _isSqlServerNameValid && _isSqlLoginValid; //&& _isSqlPasswordValid;
            }

        }
    }

}

