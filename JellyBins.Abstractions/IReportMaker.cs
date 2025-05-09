namespace JellyBins.Abstractions;

public interface IReportMaker
{
    Task MakeAsync(String path);
}