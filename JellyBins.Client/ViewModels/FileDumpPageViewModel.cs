using JellyBins.Abstractions;
using JellyBins.Client.Models;

namespace JellyBins.Client.ViewModels;

public class FileDumpPageViewModel() : ViewModel
{
    public FileDumpModel? Model { get; private set; }

    public FileDumpPageViewModel(IDrawer drawer) : this()
    {
        Model = new FileDumpModel(drawer);
    }
}