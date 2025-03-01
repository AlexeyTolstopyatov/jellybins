namespace jellybins.Java.Models;

/// <summary>
/// Represents Minecraft modification shorten model
///  
/// </summary>
public class McModificationProperties
{
    public string? Name { get; set; }
    public string? Path { get; set; }
    public string? LoaderId { get; set; }
    public string? LoaderVersion { get; init; }
    public string? ModId { get; init; }
    public string? ModVersion { get; init; }
    public string? ModDescription { get; init; } = "not provided";
    public string? ModLicense { get; init; } = "none";
    public Dictionary<string, string> ModDependencies { get; set; } = new();
}