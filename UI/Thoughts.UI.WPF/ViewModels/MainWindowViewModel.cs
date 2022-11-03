using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Microsoft.Extensions.Logging;

using Thoughts.UI.WPF.Infrastructure.Commands;
using Thoughts.UI.WPF.ViewModels.Base;
using Thoughts.UI.WPF.Views;
using Thoughts.WebAPI.Clients.Identity;

namespace Thoughts.UI.WPF.ViewModels
{
    internal class MainWindowViewModel: ViewModel
    {
        public static string Title => "Hello";
        private RecordsView _recordsView;
        private FilesView _filesView;
        private UsersView _usersView;
        private object _CurrrentView;
        private AccountsView _AccountView;


        public object CurrentView
        {
            get => _CurrrentView;
            set
            {
                _CurrrentView = value;
                OnPropertyChanged();
            }
        }

        #region Commands

        private ICommand? _RecordsButtonCheckedCommand;
        public ICommand RecordButtonCheckedCommand => _RecordsButtonCheckedCommand ?? new RelayCommand(OnRecordsButtonCheckedCommand, CanRecordsButtonCheckedCommandExecute);

        private bool CanRecordsButtonCheckedCommandExecute(object? p) => true;

        private void OnRecordsButtonCheckedCommand(object? p) => CurrentView = _recordsView;

        private ICommand? _FilesButtonCheckedCommand;
        public ICommand FilesButtonCheckedCommand => _FilesButtonCheckedCommand ?? new RelayCommand(OnFilesButtonCheckedCommand, CanFilesButtonCheckedCommandExecute);

        private bool CanFilesButtonCheckedCommandExecute(object? p) => true;

        private void OnFilesButtonCheckedCommand(object? p) => CurrentView = _filesView;


        private ICommand? _UsersButtonCheckedCommand;
        public ICommand UsersButtonCheckedCommand => _UsersButtonCheckedCommand ?? new RelayCommand(OnUsersButtonCheckedCommand, CanUsersButtonCheckedCommandExecute);
        private bool CanUsersButtonCheckedCommandExecute(object? p) => true;
        private void OnUsersButtonCheckedCommand(object? p) => CurrentView = _usersView;


        private ICommand? _AccountsButtonCheckedCommand;
        public ICommand AccountsButtonCheckedCommand => _AccountsButtonCheckedCommand ?? new RelayCommand(OnAccountsButtonCheckedCommand, CanAccountsButtonCheckedCommandExecute);
        private bool CanAccountsButtonCheckedCommandExecute(object? p) => true;
        private void OnAccountsButtonCheckedCommand(object? p) => CurrentView = _AccountView;

        #endregion


        public MainWindowViewModel()
        {
            _recordsView = new RecordsView();
            _filesView = new FilesView();
            _usersView = new UsersView();
            _CurrrentView = new RecordsView();
            _AccountView = new AccountsView();
        }
    }
}
