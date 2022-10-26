using Thoughts.UI.MAUI.ViewModels.Base;

namespace Thoughts.UI.MAUI.ViewModels
{
    public class MainViewModel : ViewModel
    {
        private string _title = "Thoughts";

        public string Title { get => _title; set => Set(ref _title, value); }


        #region Constructors

        public MainViewModel()
        {

        } 

        #endregion
    }
}
