using JellyBins.Abstractions;
using JellyBins.Client.Models;

namespace JellyBins.Client.ViewModels;

public class FileInfoPageViewModel() : ViewModel
{
    public FileInfoPageViewModel(IDrawer drawer) : this()
    {
        Model = new FileInfoModel(drawer);
    }
    
    public FileInfoModel? Model { get; private set; }
}