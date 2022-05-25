using Thoughts.UI.WPF.ViewModels.Base;

namespace Thoughts.UI.WPF.ViewModels;

/// <summary>
/// ViewModel главного окна
/// </summary>
public class MainWindowViewModel:ViewModel
{
    private string _title;
    public string Title { get => _title; set => Set(ref _title, value); }

    public MainWindowViewModel()
    {
        Title = "Thoughts";
    }
}
