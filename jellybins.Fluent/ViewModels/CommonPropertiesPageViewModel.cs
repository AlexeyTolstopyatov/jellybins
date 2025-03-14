using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using jellybins.Fluent.Models;

namespace jellybins.Fluent.ViewModels;

public sealed class CommonPropertiesPageViewModel : INotifyPropertyChanged
{
    private const string DevOs = "IBM OS/2";
    private const string DevCpu = "Intel i286";
    private const string DevOsVer = "3.0";
    private const string DevRefOs = "Microsoft Windows";
    private const string DevRefOsVer = "6.5"; // Windows 9
    private const string DevRefCpu = "Intel x86-64";
    public CommonPropertiesPageViewModel()
    {
        
    }

    public CommonPropertiesPageViewModel(CommonPropertiesModel model)
    {
        ImageName = model.ImageName;
        ImagePath = model.ImagePath;
        ImageTypeString = model.ImageTypeString;
        ImageSubsystemString = model.ImageSubsystemString;
        ImageVersionString = model.ImageVersionString;
        OperatingSystemString = model.OperatingSystemString;
        OperatingSystemVersionString = model.OperatingSystemVersionString;
        CpuArchitectureString = model.CpuArchitectureString;
        CpuWordLengthString = model.CpuWordLengthString;

        _refImageOperatingSystemString = model.ReferenceOperatingSystem;
        _refImageOperatingSystemVersionString = model.ReferenceOperatingSystemVersionString;
        _refImageCpuArchitectureString = model.ReferenceCpuArchitecture;
        ApplicationFlagsArray = model.ApplicationFlagsArray;
        SpecialRuntimeWord = model.ImageRuntime!;
    }
    #region Information Storage
    private string _refImageCpuArchitectureString = DevRefCpu;
    private string _refImageOperatingSystemVersionString = DevRefOsVer;
    private string _refImageOperatingSystemString = DevRefOs;
    
    private string _imageName = "Image's Name";
    private string _imagePath = "Image's Path";
    private string _operatingSystemString = DevOs;
    private string _operatingSystemVersionString = DevOsVer;
    private string _cpuArchitectureString = DevCpu;
    private string _cpuWordLengthString = "Maximum CPU Word";
    private string _imageVersionString = "1.1.2.3";
    private string _imageTypeString = "Dynamic Linked Library module";
    private string _imageSubsystemString = "Windows CE";
    private string _imageSpecialRuntimeWord = "Windows API";
    #endregion
    
    #region INotifyPropertyChanged
    public event PropertyChangedEventHandler? PropertyChanged;

    public string ReferenceOperatingSystem => _refImageOperatingSystemString;
    public string ReferenceCpuArchitecture => _refImageCpuArchitectureString;
    public string ReferenceOperatingSystemVersionString => _refImageOperatingSystemVersionString;
    public string[] ApplicationFlagsArray { get; }

    public string SpecialRuntimeWord
    {
        get => _imageSpecialRuntimeWord;
        set => SetField(ref _imageSpecialRuntimeWord, value);
    }
    public string OperatingSystemString
    {
        get => _operatingSystemString;
        set => SetField(ref _operatingSystemString, value);
    }

    public string OperatingSystemVersionString
    {
        get => _operatingSystemVersionString;
        set => SetField(ref _operatingSystemVersionString, value);
    }

    public string CpuArchitectureString
    {
        get => _cpuArchitectureString;
        set => SetField(ref _cpuArchitectureString, value);
    }

    public string CpuWordLengthString
    {
        get => _cpuWordLengthString;
        set => SetField(ref _cpuWordLengthString, value);
    }

    public string ImageTypeString
    {
        get => _imageTypeString;
        set => SetField(ref _imageTypeString, value);
    }

    public string ImageSubsystemString
    {
        get => _imageSubsystemString;
        set => SetField(ref _imageSubsystemString, value);
    }

    public string ImageVersionString
    {
        get => _imageVersionString;
        set => SetField(ref _imageVersionString, value);
    }

    public string ImageName
    {
        get => _imageName;
        set => SetField(ref _imageName, value);
    }

    public string ImagePath
    {
        get => _imagePath;
        set => SetField(ref _imagePath, value);
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
    #endregion
}