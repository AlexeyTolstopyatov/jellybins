using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using jellybins.Fluent.Models;

namespace jellybins.Fluent.ViewModels;

public sealed class AboutPageViewModel : INotifyPropertyChanged
{
    private string? _productCoreVersion;
    private string? _productJavaVersion;
    private string? _productName;
    private string? _productDescription;
    private string? _productVersion;
    private string? _productConsoleVersion;
    private string? _commonRuntimeVersion;

    public AboutPageViewModel()
    {
        AboutPageModel m = new();
        _productDescription = m.ProductDescription;
        _productName = m.ProductName;
        _productVersion = m.ProductVersion;
        _productConsoleVersion = m.ProductConsoleVersion;
        _productJavaVersion = m.ProductJavaVersion;
        _productCoreVersion = m.ProductCoreVersion;
        _commonRuntimeVersion = m.CommonRuntimeVersion;
    }
    
    public string? ProductConsoleVersion
    {
        get => _productConsoleVersion;
        set => SetField(ref _productConsoleVersion, value);
    }
    public string? CommonRuntimeVersion
    {
        get => _commonRuntimeVersion;
        set => SetField(ref _commonRuntimeVersion, value);
    }
    public string? ProductCoreVersion
    { 
        get => _productCoreVersion;
        set => SetField(ref _productCoreVersion, value);
    }
    public string? ProductJavaVersion
    {
        get => _productJavaVersion;
        set => SetField(ref _productJavaVersion, value);
    }
    public string? ProductVersion
    {
        get => _productVersion;
        set => SetField(ref _productVersion, value);
    }
    public string? ProductDescription
    {
        get => _productDescription;
        set => SetField(ref _productDescription, value);
    }
    public string? ProductName
    {
        get => _productName;
        set => SetField(ref _productName, value);
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