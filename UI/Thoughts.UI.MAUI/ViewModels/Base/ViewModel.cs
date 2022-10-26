#nullable enable
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Thoughts.UI.MAUI.ViewModels.Base
{
    public abstract class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? proppertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(proppertyName));
        }

        /// <summary>
        /// Universal method for applying changes to ViewModels fields. 
        /// </summary>
        /// <typeparam name="T">Any type of field.</typeparam>
        /// <param name="field">Reference to the current field.</param>
        /// <param name="value">New value for field.</param>
        /// <param name="propertyName">Name of property, that has called this method.</param>
        /// <returns></returns>
        protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (Equals(field, value)) 
                return false;

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        #region Bindable properties

        private string _title;

        public string Title
        {
            get => _title;

            set => Set(ref _title, value);
        }

        private bool _isBusy;

        public bool IsBusy
        {
            get => _isBusy;

            set
            {
                if (_isBusy == value)
                    return;

                _isBusy = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(IsNotBusy));
            }
        } 

        public bool IsNotBusy => !IsBusy;

        #endregion
    }
}
