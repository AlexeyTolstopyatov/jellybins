using System.Windows.Input;

namespace JellyBins.Client.Services;

public class Command(Action execute) : ICommand
{
    public Boolean CanExecute(Object? parameter) => true;
    
    public void Execute(Object? parameter) => execute();
    
    public event EventHandler? CanExecuteChanged;
}