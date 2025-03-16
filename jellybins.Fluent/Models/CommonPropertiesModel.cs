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

namespace jellybins.Fluent.Models;

public sealed class CommonPropertiesModel : INotifyPropertyChanged
{
    public CommonPropertiesModel(string path)
    {
        ImagePath = path;
        ImageName = new FileInfo(path).Name;
        
        // TIME TO TOTALLY READING
        _ = BuildModelAsync();
        _ = BuildFlagsAsync();
        BuildImportsAsync();
    }

    private Task BuildImportsAsync()
    {
        Dictionary<string, string> result = new ReaderFactory(ImagePath)
            .CreateReader()
            .GetImports();

        foreach (var i in result.Values)
        {
            Console.Write(i);
        }
        return Task.CompletedTask;
    }

    private Task BuildFlagsAsync()
    {
        Dictionary<string, string[]>.ValueCollection result =
            new ReaderFactory(ImagePath)
                .CreateReader()
                .GetFlags()
                .Values;

        // ignore keys.
        _flagList = result
            .SelectMany(category => category)
            .ToArray();
        
        return Task.CompletedTask;
    }
    
    private Task BuildModelAsync()
    {
        var readerFactory = new ReaderFactory(ImagePath);
        CommonProperties model = readerFactory
            .CreateReader()
            .GetProperties();
        
        ImageBoxedSign = readerFactory.SignatureWord.ToString();
        OperatingSystemString = model.OperatingSystem;
        OperatingSystemVersionString = model.OperatingSystemVersion;
        CpuArchitectureString = model.CpuArchitecture;
        CpuWordLengthString = model.CpuWordLength;
        ImageSubsystemString = model.Subsystem;
        ImageVersionString = model.LinkerVersion;
        ImageTypeString = model.ImageType;
        ImageRuntime = model.RuntimeWord;
        
        CommonProperties reference =
            new ReaderFactory(@"C:\Windows\explorer.exe")
                .CreateReader()
                .GetProperties();
        
        _refImageCpuArchitectureString = reference.CpuArchitecture;
        _refImageOperatingSystemString = reference.OperatingSystem;
        _refImageOperatingSystemVersionString = reference.OperatingSystemVersion;
        
        return Task.CompletedTask;
    }

    private string? _imageBoxedSign;
    private string? _imageName;
    private string? _imagePath;
    private string? _operatingSystemString;
    private string? _operatingSystemVersionString;
    private string? _cpuArchitectureString;
    private string? _cpuWordLengthString;
    private string? _imageVersionString;
    private string? _imageTypeString;
    private string? _imageSubsystemString;
    private string[] _flagList = new string[1];
    private string? _imageRuntime;
    
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

    public string? ImageRuntime
    {
        get => _imageRuntime;
        set => SetField(ref _imageRuntime, value);
    }

    public string? ImageBoxedSign
    {
        get => _imageBoxedSign;
        private set => SetField(ref _imageBoxedSign, value);
    }
    public string OperatingSystemString
    {
        get => _operatingSystemString;
        private set => SetField(ref _operatingSystemString, value);
    }

    public string OperatingSystemVersionString
    {
        get => _operatingSystemVersionString;
        private set => SetField(ref _operatingSystemVersionString, value);
    }

    public string CpuArchitectureString
    {
        get => _cpuArchitectureString;
        private set => SetField(ref _cpuArchitectureString, value);
    }

    public string CpuWordLengthString
    {
        get => _cpuWordLengthString;
        private set => SetField(ref _cpuWordLengthString, value);
    }

    public string ImageTypeString
    {
        get => _imageTypeString;
        private set => SetField(ref _imageTypeString, value);
    }

    public string ImageSubsystemString
    {
        get => _imageSubsystemString;
        private set => SetField(ref _imageSubsystemString, value);
    }

    public string ImageVersionString
    {
        get => _imageVersionString;
        private set => SetField(ref _imageVersionString, value);
    }

    public string ImageName
    {
        get => _imageName;
        private set => SetField(ref _imageName, value);
    }

    public string ImagePath
    {
        get => _imagePath;
        private set => SetField(ref _imagePath, value);
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