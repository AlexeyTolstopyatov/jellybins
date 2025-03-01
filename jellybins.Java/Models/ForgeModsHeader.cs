using System.Runtime.Serialization;

namespace jellybins.Java.Models;

public class ForgeModsHeader
{
    // [[mods]] header
    [DataMember(Name = "modId")]
    public string? ModId { get; set; }
    [DataMember(Name = "versionRange")]
    public string? ModVersion { get; set; }
    [DataMember(Name = "ordering")]
    public string? ModDisplayName { get; set; }
    [DataMember(Name = "displayURL")]
    public string? ModDisplayUrl { get; set; }
    [DataMember(Name = "authors")]
    public string? ModAuthors { get; set; }
    [DataMember(Name = "logoFile")]
    public string? ModLogoFile { get; set; }
}