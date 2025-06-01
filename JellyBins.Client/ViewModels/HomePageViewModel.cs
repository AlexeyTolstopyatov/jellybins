using JellyBins.Abstractions;
using JellyBins.Core.Factories;
using Microsoft.Win32;
using System.Windows.Input;
using Wpf.Ui.Controls;
using Wpf.Ui.Input;

namespace JellyBins.Client.ViewModels;

public class HomePageViewModel : ViewModel
{
    public HomePageViewModel()
    {
        MakeInfoPage = new RelayCommand<Byte>(PrepareInfoPage);
        MakeDumpPage = new RelayCommand<Byte>(PrepareDumpPage);
        MakeAllPages = new RelayCommand<Byte>(PrepareAllPages);
    }
    public ICommand MakeInfoPage { get; }
    public ICommand MakeDumpPage { get; }
    public ICommand MakeAllPages { get; }
    
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

    private IDrawer? Collect(String path)
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

    private void PrepareAllPages(Byte unused)
    {
        String path = GetFilePath();
        IDrawer? drawer = Collect(path);
        
        if (drawer == null)
            return; // ignore

        // Relative Binding with MainWindowViewModel
        // Pages opening
        // Page remember
    }
    
    private void PrepareInfoPage(Byte unused)
    {
        String path = GetFilePath();
        IDrawer? drawer = Collect(path);
        
        if (drawer == null)
            return; // ignore

    }

    private void PrepareDumpPage(Byte unused)
    {
        String path = GetFilePath();
        IDrawer? drawer = Collect(path);
        
        if (drawer == null)
            return; // ignore
    }
}