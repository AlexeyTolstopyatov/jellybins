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
        public Page ProgramSectionsPage;
        public Page AboutPage;
    }

    private FilledPages _pagesCollection;

    public event PropertyChangedEventHandler? PropertyChanged;
    private Page _frameContent;
    private string? _programPath;

    // Possibility Flags
    private bool _allowProgramHeadersPage = true;
    private bool _allowProgramSectionsPage;
    private bool _allowSaveResultsButton;
    private bool _allowCommonResultsPage;
    private bool _expandOperatorsBlock;

    public bool ExpandOperatorsBlock
    {
        get => _expandOperatorsBlock;
        set => SetField(ref _expandOperatorsBlock, value);
    }
    
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

    public bool AllowProgramSectionsPage
    {
        get => _allowProgramSectionsPage;
        set => SetField(ref _allowProgramSectionsPage, value);
    }

    public bool AllowSaveResultsButton
    {
        get => _allowSaveResultsButton;
        set => SetField(ref _allowSaveResultsButton, value);
    }

    public MainWindowViewModel()
    {
        // frame startup bindings
        _frameContent = new AboutPage { DataContext = new AboutPageViewModel(/*Automatically calls Model builder*/) };
        _pagesCollection.AboutPage = FrameContent;
        // command bindings
        OpenFileCommand = new RelayCommand<string>(OpenFile!);
        ProgramHeadersPageCommand = new RelayCommand<string>(s => ShowPage(PagesCollection.ProgramHeadersPage));
        ProgramGeneralPageCommand = new RelayCommand<string>(s => ShowPage(PagesCollection.ProgramGeneralPage));
        OpenAboutPageCommand = new RelayCommand<string>(x => ShowPage(PagesCollection.AboutPage));
        OpenSectionsTablePageCommand = new RelayCommand<string>(x => ShowPage(PagesCollection.ProgramSectionsPage));
        // toggles startup bindings
        AllowProgramHeadersPage = 
        AllowSaveResultsButton =
        AllowProgramSectionsPage = false;
    }

    private FilledPages PagesCollection
    {
        get => _pagesCollection;
        set => SetField(ref _pagesCollection, value);
    }

    public ICommand OpenFileCommand { get; }
    public ICommand ProgramHeadersPageCommand { get; set; }
    public ICommand ProgramGeneralPageCommand { get; set; }
    public ICommand OpenAboutPageCommand { get; set; }
    public ICommand OpenSectionsTablePageCommand { get; set; }
    
    public Page FrameContent
    {
        get => _frameContent;
        private set => SetField(ref _frameContent, value);
    }

    private void CreateJavaAppletPage()
    {
        ExtractedFileHandler hMod = new(_programPath!);
        
        if (hMod.LoaderType == MetadataType.Forge)
        {
            ForgeModel forge = new();
            hMod.DeserializeLoaderManifest(ref forge);
            McModificationProperties model = McLoaderModelHandler.Normalize(forge);
            model.Name = new FileInfo(_programPath!).Name;
            model.Path = _programPath;
            _pagesCollection.ProgramGeneralPage = new McModificationPage()
            {
                DataContext = new McModificationPageViewModel(model, hMod.ReadManifest(), hMod.ReadLoaderManifest())
            };
            FrameContent = PagesCollection.ProgramGeneralPage;
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
        CreateProgramSectionsHeaderPage(model.ImageBoxedSign!);
    }
    private void CreateProgramHeadersPage()
    {
        _pagesCollection.ProgramHeadersPage = new ProgramHeadersPage
        {
            DataContext = new ProgramHeaderPageViewModel(new ProgramHeaderPageModel(_programPath!))
        };
    }
    private void CreateProgramSectionsHeaderPage(string sign)
    {
        SectionsPageModel model = new(_programPath!, ushort.Parse(sign));
        _pagesCollection.ProgramSectionsPage = new SectionsPage()
        {
            DataContext = new SectionsPageViewModel(model)
        };
        AllowProgramSectionsPage = true;
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
            DefaultExt = "*.*",
            Multiselect = false,
        };
        try
        {
            ofd.ShowDialog();
            
            if (!File.Exists(ofd.FileName))
                return;
            
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
            ExpandOperatorsBlock = true;
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