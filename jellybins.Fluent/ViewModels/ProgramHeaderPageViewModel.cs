using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media.Animation;
using jellybins.Fluent.Models;

namespace jellybins.Fluent.ViewModels;
public sealed class ProgramHeaderPageViewModel : INotifyPropertyChanged
{
    // Boolean fields initializes with 0 by default.... ok.
    private bool _allowProgramHeader;
    private bool _allowRuntimeHeader;
    private Dictionary<string, string> _programHeader;
    private Dictionary<string, string> _runtimeHeader;
    private string _programHeaderExpanderHeader;
    private string _runtimeHeaderExpanderHeader;

    public ProgramHeaderPageViewModel(ProgramHeaderPageModel model) : this()
    {
        // factory trick
        // next: call BuildDetectedHeadersAsync
        _ = BuildDetectedHeadersAsync(model);
    }

    public ProgramHeaderPageViewModel()
    {
        // (un)safe trick
        _programHeader =
        _runtimeHeader = new();
        _programHeaderExpanderHeader = "Program Header";
        _runtimeHeaderExpanderHeader = "Runtime Header";
        _allowProgramHeader = 
        _allowRuntimeHeader = false;
    }

    private Task BuildDetectedHeadersAsync(ProgramHeaderPageModel model)
    {
        ProgramHeader = model.ProgramHeader;
        RuntimeHeader = model.RuntimeHeader ?? new(); // f_ you :3
        AllowProgramHeader = model.ProgramHeaderExists;
        AllowRuntimeHeader = model.RuntimeHeaderExists;
        // next: imagine, renaming of expander need. Some ideas?
        
        return Task.CompletedTask;
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;

    public bool AllowProgramHeader
    {
        get => _allowProgramHeader;
        set => SetField(ref _allowProgramHeader, value);
    }

    public string RuntimeHeaderExpanderHeader
    {
        get => _runtimeHeaderExpanderHeader;
        set => SetField(ref _runtimeHeaderExpanderHeader, value);
    }

    public string ProgramHeaderExpanderHeader
    {
        get => _programHeaderExpanderHeader;
        set => SetField(ref _programHeaderExpanderHeader, value);
    }
    
    public bool AllowRuntimeHeader
    {
        get => _allowRuntimeHeader;
        set => SetField(ref _allowRuntimeHeader, value);
    }

    public Dictionary<string, string> ProgramHeader
    {
        get => _programHeader;
        set => SetField(ref _programHeader, value);
    }

    public Dictionary<string, string> RuntimeHeader
    {
        get => _runtimeHeader;
        set => SetField(ref _runtimeHeader, value);
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