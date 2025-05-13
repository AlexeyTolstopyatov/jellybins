namespace JellyBins.Abstractions;

public interface IFileSaver
{
    public Task WriteAsync();
}