using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using jellybins.Core.Attributes;
using jellybins.Fluent.Models;
using jellybins.Fluent.Views;
using Microsoft.Win32;
using Wpf.Ui.Input;

namespace jellybins.Fluent.ViewModels;

public class MainWindowViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    private Page _frameContent;
    public MainWindowViewModel()
    {
        OpenFileCommand = new RelayCommand<string>(OpenFile!);
        _frameContent = new Page();
    }

    public RelayCommand<string> OpenFileCommand { get; }

    public Page FrameContent
    {
        get => _frameContent;
        set => SetField(ref _frameContent, value);
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
            DefaultExt = "All files|*.*",
            Multiselect = false
        };
        try
        {
            // fixme: Cancel result throws exception.
            if (!ofd.ShowDialog().HasValue) return; // throws exception.
            
            CommonPropertiesModel model = new(ofd.FileName);
            FrameContent = new CommonPropertiesPage
            {
                DataContext = new CommonPropertiesPageViewModel(model)
            };
        }
        catch (Exception e)
        {
            FrameContent = new ErrorPage
            {
                DataContext = new ErrorPageViewModel(new ErrorPageModel(e))
            };
        }
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
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