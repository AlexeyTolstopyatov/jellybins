using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using jellybins.Java.Exceptions;
using jellybins.Java.Models;

namespace jellybins.Fluent.ViewModels;

public sealed class McModificationPageViewModel : INotifyPropertyChanged
{
    private McModificationProperties _properties;
    private string _manifestText;
    private string _loaderText;

    public McModificationPageViewModel(McModificationProperties model, string manifestText, string loaderText)
    {
        _properties = model;
        _manifestText = manifestText;
        _loaderText = loaderText;
    }

    public McModificationPageViewModel()
    {
        
    }

    public McModificationProperties Properties
    {
        get => _properties;
        set => SetField(ref _properties, value);
    }

    public string ManifestText
    {
        get => _manifestText;
        set => SetField(ref _manifestText, value);
    }

    public string LoaderText
    {
        get => _loaderText;
        set => SetField(ref _loaderText, value);
    }
    
    #region INotifyPropertyChanged
    
    public event PropertyChangedEventHandler? PropertyChanged;

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
    #endregion
}