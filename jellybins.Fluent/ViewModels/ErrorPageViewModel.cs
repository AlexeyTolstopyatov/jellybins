using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using jellybins.Fluent.Models;

namespace jellybins.Fluent.ViewModels;

public class ErrorPageViewModel : INotifyPropertyChanged
{
    public ErrorPageViewModel()
    {
        
    }

    public ErrorPageViewModel(ErrorPageModel model)
    {
        SetField(ref _message, model.ShortenMessage);
        SetField(ref _tree, model.ExceptionRawTree);
    }

    private string? _message;
    private string? _tree;

    public string? Message => _message;
    public string? Tree => _tree;
    
    public event PropertyChangedEventHandler? PropertyChanged;

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