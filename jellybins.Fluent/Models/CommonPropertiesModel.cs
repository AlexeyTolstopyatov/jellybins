using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using jellybins.Core.Interfaces;
using jellybins.Core.Models;
using jellybins.Core.Readers.Factory;
using jellybins.Core.Strings;
using jellybins.Fluent.ViewModels;

namespace jellybins.Fluent.Models;

public sealed class CommonPropertiesModel : INotifyPropertyChanged
{
    public CommonPropertiesModel(string path)
    {
        ImagePath = path;
        ImageName = new FileInfo(path).Name;
        
        BuildModelAsync();
        BuildFlagsAsync();
    }

    private Task BuildFlagsAsync()
    {
        Dictionary<string, string[]>.ValueCollection result =
            new ReaderFactory(ImagePath)
                .CreateReader()
                .GetFlags()
                .Values;

        _flagList = result.SelectMany(category => category).ToArray();
        
        return Task.CompletedTask;
    }
    
    private Task BuildModelAsync()
    {
        CommonProperties model = 
            new ReaderFactory(ImagePath)
                .CreateReader()
                .GetProperties();
        OperatingSystemString = model.OperatingSystem;
        OperatingSystemVersionString = model.OperatingSystemVersion;
        CpuArchitectureString = model.CpuArchitecture;
        CpuWordLengthString = model.CpuWordLength;
        ImageSubsystemString = model.Subsystem;
        ImageVersionString = model.LinkerVersion;
        ImageTypeString = model.ImageType;
        
        CommonProperties reference =
            new ReaderFactory(@"C:\Windows\explorer.exe")
                .CreateReader()
                .GetProperties();
        
        _refImageCpuArchitectureString = reference.CpuArchitecture;
        _refImageOperatingSystemString = reference.OperatingSystem;
        _refImageOperatingSystemVersionString = reference.OperatingSystemVersion;
        
        return Task.CompletedTask;
    }
    private string _imageName = nameof(ArgumentNullException);
    private string _imagePath = nameof(ArgumentNullException);
    private string _operatingSystemString = nameof(ArgumentNullException);
    private string _operatingSystemVersionString = nameof(ArgumentNullException);
    private string _cpuArchitectureString = nameof(ArgumentNullException);
    private string _cpuWordLengthString = nameof(ArgumentNullException);
    private string _imageVersionString = nameof(ArgumentNullException);
    private string _imageTypeString = nameof(ArgumentNullException);
    private string _imageSubsystemString = nameof(ArgumentNullException);
    private string[] _flagList = new string[1];
    
    private string _refImageCpuArchitectureString = null!;
    private string _refImageOperatingSystemVersionString = null!;
    private string _refImageOperatingSystemString = null!;
    public event PropertyChangedEventHandler? PropertyChanged;

    public string ReferenceOperatingSystem 
        => _refImageOperatingSystemString;
    public string ReferenceCpuArchitecture => 
        _refImageCpuArchitectureString;
    public string ReferenceOperatingSystemVersionString => 
        _refImageOperatingSystemVersionString;

    public string[] ApplicationFlagsArray 
        => _flagList;
    
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
}