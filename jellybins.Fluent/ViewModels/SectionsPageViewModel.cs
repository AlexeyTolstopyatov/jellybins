using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using jellybins.Core.Models;
using jellybins.Fluent.Models;

namespace jellybins.Fluent.ViewModels;

public class SectionsPageViewModel : INotifyPropertyChanged
{
    private SectionsProperties[]? _sections;
    private string _description;
    public SectionsPageViewModel() {}
    public SectionsPageViewModel(SectionsPageModel model)
    {
        _sections = model.Sections;
        _description = @"This page exists for every type of object or executable file,
because all files has determined physical partitions of code, resources, and other
sections of data. Microsoft COFFs starts from New executable type already have
relocations tables and import/export tables.";
    }

    public string Description => _description;
    
    public SectionsProperties[] Sections
    {
        get => _sections!;
        set => SetField(ref _sections, value);
    }
    
    #region INotifyPropertyChanged
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
    #endregion
}