using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using jellybins.Core.Attributes;
using jellybins.Fluent.Models;
using jellybins.Fluent.Views;
using jellybins.Java.Exceptions;
using jellybins.Java.Handlers;
using jellybins.Java.Models;
using Microsoft.Win32;
using Wpf.Ui.Input;

namespace jellybins.Fluent.ViewModels;

public sealed class MainWindowViewModel : INotifyPropertyChanged
{
    private struct FilledPages
    {
        public Page ProgramGeneralPage;
        public Page ProgramHeadersPage;
        public Page ProgramImportsPage;
    }

    private FilledPages _pagesCollection;

    public event PropertyChangedEventHandler? PropertyChanged;
    private Page _frameContent;
    private string? _programPath;

    // Possibility Flags
    private bool _allowProgramHeadersPage = true;
    private bool _allowProgramImportsPage;
    private bool _allowSaveResultsButton;
    private bool _allowCommonResultsPage;

    public bool AllowCommonResultsPage
    {
        get => _allowCommonResultsPage;
        set => SetField(ref _allowCommonResultsPage, value);
    }

    public bool AllowProgramHeadersPage
    {
        get => _allowProgramHeadersPage;
        set => SetField(ref _allowProgramHeadersPage, value);
    }

    public bool AllowProgramImportsPage
    {
        get => _allowProgramImportsPage;
        set => SetField(ref _allowProgramImportsPage, value);
    }

    public bool AllowSaveResultsButton
    {
        get => _allowSaveResultsButton;
        set => SetField(ref _allowSaveResultsButton, value);
    }

    public MainWindowViewModel()
    {
        OpenFileCommand = new RelayCommand<string>(OpenFile!);
        ProgramHeadersPageCommand = new RelayCommand<string>(s => ShowPage(PagesCollection.ProgramHeadersPage));
        ProgramGeneralPageCommand = new RelayCommand<string>(s => ShowPage(PagesCollection.ProgramGeneralPage));
        _frameContent = new Page();
        AllowProgramHeadersPage = 
            AllowSaveResultsButton =
                AllowProgramImportsPage = false;
    }

    private FilledPages PagesCollection
    {
        get => _pagesCollection;
        set => SetField(ref _pagesCollection, value);
    }

    public ICommand OpenFileCommand { get; }
    public ICommand ProgramHeadersPageCommand { get; set; }
    public ICommand ProgramGeneralPageCommand { get; set; }

    public Page FrameContent
    {
        get => _frameContent;
        set => SetField(ref _frameContent, value);
    }

    private void CreateJavaAppletPage()
    {
        ExtractedFileHandler hMod = new(_programPath!);
        
        if (hMod.LoaderType == MetadataType.Forge)
        {
            ForgeModel forge = new();
            hMod.DeserializeLoaderManifest(ref forge);
            McModificationProperties model = new(); 
            model = McLoaderModelHandler.Normalize(forge);
            model.Name = new FileInfo(_programPath!).Name;
            model.Path = _programPath;
            _pagesCollection.ProgramGeneralPage = new McModificationPage()
            {
                DataContext = new McModificationPageViewModel(model, hMod.ReadManifest(), hMod.ReadLoaderManifest())
            };
        }
        else
        {
            FabricModel fabric = new();
            hMod.DeserializeLoaderManifest(ref fabric);
            McModificationProperties model = McLoaderModelHandler.Normalize(fabric);
            model.LoaderId = hMod.LoaderType.ToString();
            model.Name = new FileInfo(_programPath!).Name;
            model.Path = _programPath;
            _pagesCollection.ProgramGeneralPage = new McModificationPage()
            {
                DataContext = new McModificationPageViewModel(model, hMod.ReadManifest(), hMod.ReadLoaderManifest())
            };
            FrameContent = _pagesCollection.ProgramGeneralPage;
        }
    }
    private void CreateProgramGeneralPage()
    {
        CommonPropertiesModel model = new(_programPath!);
        _pagesCollection.ProgramGeneralPage = new CommonPropertiesPage
        {
            DataContext = new CommonPropertiesPageViewModel(model)
        };
    }
    
    private void CreateProgramHeadersPage()
    {
        _pagesCollection.ProgramHeadersPage = new ProgramHeadersPage
        {
            DataContext = new ProgramHeaderPageViewModel(new ProgramHeaderPageModel(_programPath!))
        };
    }
    
    private void ShowPage(Page page)
    {
        FrameContent = page;
    }

    [UnderConstruction("Cancellation throws exceptions.")]
    private void OpenFile(string path)
    {
        // Creates Model instance
        OpenFileDialog ofd = new()
        {
            Filter = "Old-Format Modules (*.exe, *.dll *.sys *.drv)|*.exe;*.dll;*.drv;*.sys;*.scr|" +
                     "New-Format Modules (*.o *.so)|*.so;*.o;*.aout;*.a.out;*.out|" + 
                     "All files|*.*",
            Title = "Select object file",
            DefaultExt = "All files",
            Multiselect = false,
        };
        try
        {
            // fixme: Cancel result throws exception.
            if (!ofd.ShowDialog().HasValue) return; // throws exception.
            _programPath = ofd.FileName;
            AllowProgramHeadersPage = true;
            AllowSaveResultsButton = true;

            if (new FileInfo(ofd.FileName).Extension == ".jar")
            {
                CreateJavaAppletPage();
                return;
            }
            
            // create page -> call page
            CreateProgramGeneralPage();
            CreateProgramHeadersPage();
            ShowPage(PagesCollection.ProgramGeneralPage);
        }
        catch (Exception e)
        {
            FrameContent = new ErrorPage
            {
                DataContext = new ErrorPageViewModel(new ErrorPageModel(e))
            };
        }
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}