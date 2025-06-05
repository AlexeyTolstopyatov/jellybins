using System.Windows.Controls;
using System.Windows.Input;
using JellyBins.Abstractions;
using JellyBins.Client.Services;
using JellyBins.Client.Views;
using JellyBins.Core.Factories;
using Microsoft.Win32;
using Wpf.Ui.Controls;

namespace JellyBins.Client.ViewModels;

public class MainWindowViewModel : ViewModel
{
    public static MainWindowViewModel Instance { get; private set; } = new();
    private Page? _pageContainer;
    private IDrawer? _dataStorage;
    
    public MainWindowViewModel()
    {
        ShowHomePageCommand = new Command(ShowHomePage);
        MakeAllCommand = new Command(PrepareAllPages);
        MakeDumpCommand = new Command(PrepareDumpPage);
        MakeInfoCommand = new Command(PrepareAllPages);
        
        Instance = this;
    }
    public ICommand ShowHomePageCommand { get; }
    public ICommand MakeInfoCommand { get; }
    public ICommand MakeDumpCommand { get; }
    public ICommand MakeAllCommand { get; }
    public IDrawer? DataStorage
    {
        get => _dataStorage;
        set => SetField(ref _dataStorage, value);
    }
    
    public Page PageContainer
    {
        get => _pageContainer!;
        private set => SetField(ref _pageContainer, value);
    }

    private void ShowHomePage()
    {
        PageContainer = new MainWindowMenuPage();
    }
    
    private String GetFilePath()
    {
        OpenFileDialog dialog = new()
        {
            Filter = "Old Format (.*exe *.dll *.sys)|*.exe *.dll *.sys *.scr *.fon *.drv|New Format (*.o *.so *.d)|*.*|All Files|*.*",
            Title = "Select file path",
            Multiselect = false
        };

        dialog.ShowDialog();
        return !String.IsNullOrEmpty(dialog.FileName) 
            ? dialog.FileName 
            : "%windir%\\explorer.exe";
    }
    private IDrawer? GetDrawer(String path)
    {
        try
        {
            IFileDumper dumper = FileDumperFactory.CreateInstance(path);
            IDrawer drawer = DrawerFactory.CreateInstance(dumper);

            return drawer;
        }
        catch (Exception e)
        {
            new MessageBox
            {
                Title = e.Message,
                Content = e.ToString()
            }.ShowDialogAsync();
        }

        return null;
    }
    private void PrepareAllPages()
    {
        String path = GetFilePath();
        IDrawer? drawer = GetDrawer(path);
        
        if (drawer == null)
            return; // ignore

        DataStorage = drawer;

        PageContainer = new FileInfoPage()
        {
            DataContext = new FileInfoPageViewModel(drawer)
        };
    }

    private void PrepareDumpPage()
    {
        String path = GetFilePath();
        IDrawer? drawer = GetDrawer(path);
        
        if (drawer == null)
            return; // ignore

        DataStorage = drawer;

        PageContainer = new FileDumpPage()
        {
            DataContext = new FileDumpPageViewModel(drawer)
        };
    }
}