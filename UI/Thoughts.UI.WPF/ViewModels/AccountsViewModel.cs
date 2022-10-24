using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Thoughts.DAL.Entities.Idetity;
using Thoughts.UI.WPF.ViewModels.Base;

using static Thoughts.UI.WPF.ViewModels.FilesViewModel;

namespace Thoughts.UI.WPF.ViewModels
{
    internal class AccountsViewModel : ViewModel
    {
        private static ObservableCollection<IdentUser> _identUserCollection = new ObservableCollection<IdentUser>();
        private static ObservableCollection<IdentRole> _identRoleCollection = new ObservableCollection<IdentRole>();
        public static ObservableCollection<IdentUser> IdentUserCollection
        {
            get => _identUserCollection;
            set 
            { 
                _identUserCollection = value;
            }
        }
        public static ObservableCollection<IdentRole> IdentRoleCollection 
        {
            get => _identRoleCollection;
            set
            {
                _identRoleCollection = value;
            }
        }

        /// <summary>
        /// Login
        /// </summary>
        public ICommand Login
        {
            get
            {
                return new DelegateCommand((p) =>
                {

                }, (p) => true);
            }
        }
    }
}
