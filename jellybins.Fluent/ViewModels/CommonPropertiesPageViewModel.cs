using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Accessibility;
using jellybins.Core.Readers.Factory;
using jellybins.Fluent.Models;

namespace jellybins.Fluent.ViewModels;

public class CommonPropertiesPageViewModel : INotifyPropertyChanged
{
    private const string DEV_OS = "IBM OS/2";
    private const string DEV_CPU = "Intel i286";
    private const string DEV_OS_VER = "3.0";
    private const string DEV_REF_OS = "Microsoft Windows";
    private const string DEV_REF_OS_VER = "6.5"; // Windows 9
    private const string DEV_REF_CPU = "Intel x86-64";
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
    }

    private string _refImageCpuArchitectureString = DEV_REF_CPU;
    private string _refImageOperatingSystemVersionString = DEV_REF_OS_VER;
    private string _refImageOperatingSystemString = DEV_REF_OS;
    
    private string _imageName = nameof(ArgumentNullException);
    private string _imagePath = nameof(ArgumentNullException);
    private string _operatingSystemString = DEV_OS;
    private string _operatingSystemVersionString = DEV_OS_VER;
    private string _cpuArchitectureString = DEV_CPU;
    private string _cpuWordLengthString = nameof(ArgumentNullException);
    private string _imageVersionString = nameof(ArgumentNullException);
    private string _imageTypeString = nameof(ArgumentNullException);
    private string _imageSubsystemString = nameof(ArgumentNullException);
    
    public event PropertyChangedEventHandler? PropertyChanged;

    public string ReferenceOperatingSystem => _refImageOperatingSystemString;
    public string ReferenceCpuArchitecture => _refImageCpuArchitectureString;
    public string ReferenceOperatingSystemVersionString => _refImageOperatingSystemVersionString;
    public string[] ApplicationFlagsArray { get; }

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