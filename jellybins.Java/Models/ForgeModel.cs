using System.Runtime.Serialization;

namespace jellybins.Java.Models;

public class ForgeModel
{
    // main header
    [DataMember(Name = "modLoader")]
    public string? Loader { get; set; }
    
    [DataMember(Name = "loaderVersion")]
    public string? LoaderVersion { get; set; }
    
    [DataMember(Name = "issueTrackerURL")]
    public string? IssueTrackerUrl { get; set; }
    
    [DataMember(Name = "license")]
    public string? License { get; set; }
    
    //[[mods]] header
    [DataMember(Name = "[[mods]]")]
    public ForgeModsHeader? Mods { get; set; }
    
    // [[dependencies.{name}]] headers
    public ForgeDependency[] Dependencies { get; set; } = Array.Empty<ForgeDependency>();
}