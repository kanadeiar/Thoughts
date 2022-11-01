using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

using Thoughts.DAL.Entities.Idetity;
using Thoughts.UI.WPF.Infrastructure.Commands;
using Thoughts.UI.WPF.ViewModels.Base;
using Thoughts.UI.WPF.Views;
using Thoughts.WebAPI.Clients.Identity;

namespace Thoughts.UI.WPF.ViewModels
{
    public class AccountsViewModel : ViewModel
    {
        public AccountsViewModel(AccountClient account_client)
        {
            _account_client = account_client;
        }

        private static AccountClient _account_client;

        private ObservableCollection<IdentUser> _identUserCollection = new ObservableCollection<IdentUser>();
        private ObservableCollection<IdentRole> _identRoleCollection = new ObservableCollection<IdentRole>();
        private string _title;
        private string _userName;      // Admin  // AdPAss_123
        private bool _isAuthorization;


        public ObservableCollection<IdentUser> IdentUserCollection
        {
            get => _identUserCollection;
            set
            {
                _identUserCollection = value;
                OnPropertyChanged("IdentUserCollection");
            }
        }

        public ObservableCollection<IdentRole> IdentRoleCollection
        {
            get => _identRoleCollection;
            set
            {
                _identRoleCollection = value;
                OnPropertyChanged("IdentRoleCollection");
            }
        }
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged("Title");
            }
        }

        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                OnPropertyChanged("UserName");
            }
        }

        /// <summary>
        /// Login
        /// </summary>
        public ICommand Login
        {
            get
            {
                return new RelayCommand(async (p) =>
                {
                    try
                    {
                        var password = ((PasswordBox)p).Password;
                        await _account_client.LoginAsync(_userName, password);
                        ChangeTitle("Admin");
                        _isAuthorization = true;
                        MessageBox.Show($"{_userName} добро пожаловать в систему.");
                    }
                    catch (System.InvalidOperationException)
                    {
                        MessageBox.Show("Авторизация не выполнена.");
                    }


                }, (p) => !_isAuthorization);
            }
        }

        /// <summary>
        /// Login
        /// </summary>
        public ICommand Logout
        {
            get
            {
                return new RelayCommand(async (p) =>
                {
                    await _account_client.LogoutAsync();
                    ChangeTitle();
                    _isAuthorization = false;
                    MessageBox.Show($"{_userName} вышел из системы.");
                }, (p) => _isAuthorization);
            }
        }

        /// <summary>
        /// Login
        /// </summary>
        public ICommand GetAllRoless
        {
            get
            {
                return new RelayCommand(async (p) =>
                {
                    var temp = await _account_client.GetAllRolessAsync();
                    if (temp is not null)
                    {
                        IdentRoleCollection = new ObservableCollection<IdentRole>(temp);
                    }
                    else
                    {
                        IdentRoleCollection = new ObservableCollection<IdentRole>();
                    }
                }, (p) => _isAuthorization);
            }
        }

        /// <summary>
        /// Login
        /// </summary>
        public ICommand GetAllUsers
        {
            get
            {
                return new RelayCommand(async (p) =>
                {
                    var temp = await _account_client.GetAllUsersAsync();
                    if (temp is not null)
                    {
                        IdentUserCollection = new ObservableCollection<IdentUser>(temp);
                    }
                    else
                    {
                        IdentUserCollection = new ObservableCollection<IdentUser>();
                    }
                }, (p) => _isAuthorization);
            }
        }

        private static void ChangeTitle(string name = "")
        {
            foreach (Window window in App.Current.Windows)
            {
                if (window is MainWindow)
                {
                    if (name != "")
                        window.Title = $"Hello {name}";
                    else
                        window.Title = "Hello";
                }
            }
        }


    }
}
