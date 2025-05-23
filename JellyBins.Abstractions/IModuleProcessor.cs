namespace JellyBins.Abstractions;

/// <summary>
/// Common API for Module processor:
/// Module processor checks known libraries
/// and tells, which API uses inside application
/// </summary>
public interface IModuleProcessor
{
    String[] TryToKnowUsedModules();
}