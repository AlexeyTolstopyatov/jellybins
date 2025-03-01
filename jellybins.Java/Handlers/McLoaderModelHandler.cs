using jellybins.Java.Models;

namespace jellybins.Java.Handlers;

public static class McLoaderModelHandler
{
    /// <summary>
    /// Returns normalized common Minecraft
    /// modification model.
    /// </summary>
    /// <param name="model"></param>
    public static McModificationProperties Normalize(ForgeModel model)
    {
        McModificationProperties m = new() // NullReference
        {
            ModLicense = model.License,
            LoaderId = "Forge",
            LoaderVersion = model.LoaderVersion,
            ModId = model.Mods!.ModId,
            ModDescription = model.Mods.ModDisplayName,
            ModVersion = model.Mods.ModVersion,
        };
        
        var deps = 
            from i in model.Dependencies
            select new KeyValuePair<string?, string?>(i.ModId, i.VersionRange);

        m.ModDependencies = new Dictionary<string, string>(deps);
        
        return m;
    }
    /// <summary>
    /// Returns normalized common Minecraft
    /// modification model
    /// </summary>
    /// <param name="model"></param>
    public static McModificationProperties Normalize(FabricModel model)
    {
        // fuck it.
        string[] versions = model.Version!.Split('-');
        if (versions.Length < 2)
        {
            versions = model.Version!.Split('+');
            if (versions.Length < 2)
                versions = new string[2] {model.Version, "(*)"};
        }
            
        if (model.Depends.Count == 0) 
        {
            model.Depends = new Dictionary<string, string>()
            {
                { "independent", "0.0" }
            };
        }
        McModificationProperties m = new()
        {
            LoaderVersion = versions[0],
            ModId = model.Id,
            ModVersion = versions[1],
            ModLicense = model.License,
            ModDescription = model.Description
        };
        m.LoaderId = "Fabric";
        m.ModDependencies = model.Depends;

        return m;
    }
}