using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Windows.Input;

using Thoughts.DAL.Entities.Idetity;
using Thoughts.UI.WPF.Infrastructure.Commands;
using Thoughts.UI.WPF.ViewModels.Base;
using Thoughts.WebAPI.Clients.Identity;

namespace Thoughts.UI.WPF.ViewModels
{
    public class AccountsViewModel : ViewModel
    {
        private static HttpClient http = new HttpClient
        {
            BaseAddress = new("https://localhost:5011")
        };
        private static AccountClient account_client = new AccountClient(http);

        private ObservableCollection<IdentUser> _identUserCollection = new ObservableCollection<IdentUser>();
        private ObservableCollection<IdentRole> _identRoleCollection = new ObservableCollection<IdentRole>();

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

        /// <summary>
        /// Login
        /// </summary>
        public ICommand Login
        {
            get
            {
                return new RelayCommand(async (p) =>
                {
                    await account_client.LoginAsync("Admin", "AdPAss_123");
                }, (p) => true);
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
                    await account_client.LogoutAsync();
                }, (p) => true);
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
                    var temp = await account_client.GetAllRolessAsync();
                    if (temp is not null)
                    {
                        IdentRoleCollection = new ObservableCollection<IdentRole>(temp);
                    }
                    else 
                    {
                        IdentRoleCollection = new ObservableCollection<IdentRole>(); 
                    }
                }, (p) => true);
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
                    var temp = await account_client.GetAllUsersAsync();
                    if (temp is not null)
                    {
                        IdentUserCollection = new ObservableCollection<IdentUser>(temp);
                    }
                    else
                    {
                        IdentUserCollection = new ObservableCollection<IdentUser>();
                    }
                }, (p) => true);
            }
        }
    }
}
