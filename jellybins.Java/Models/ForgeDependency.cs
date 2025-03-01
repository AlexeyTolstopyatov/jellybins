using System.Runtime.Serialization;

namespace jellybins.Java.Models;

public class ForgeDependency
{
    [DataMember(Name = "modId")]
    public string? ModId;
    [DataMember(Name = "mandatory")]
    public bool Mandatory;
    [DataMember(Name = "versionRange")]
    public string? VersionRange { get; set; }
    [DataMember(Name = "ordering")]
    public string? Ordering { get; set; }
    [DataMember(Name = "side")]
    public string? Side { get; set; }
}