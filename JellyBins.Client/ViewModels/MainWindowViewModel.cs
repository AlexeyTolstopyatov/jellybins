using System.Configuration.Internal;
using System.Windows.Controls;
using System.Windows.Input;
using Wpf.Ui.Input;

namespace JellyBins.Client.ViewModels;

public class MainWindowViewModel : ViewModel
{
    private Page? _pageContainer;
    public MainWindowViewModel()
    {
        ShowHomePageCommand = new RelayCommand<Object>(ShowHomePage!);
    }
    
    public ICommand ShowHomePageCommand { get; }

    public Page PageContainer
    {
        get => _pageContainer!;
        set => SetField(ref _pageContainer, value);
    } 
    private void ShowHomePage(Object a)
    {
        
    }
}