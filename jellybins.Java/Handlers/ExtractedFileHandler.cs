using System.Diagnostics;
using System.Text.Json;
using jellybins.Java.Exceptions;
using jellybins.Java.Models;
using SharpCompress.Archives;
using SharpCompress.Common;
using SharpCompress.Readers;
using Tomlyn;
using Tomlyn.Model;
using Tomlyn.Syntax;

namespace jellybins.Java.Handlers;

/// <summary>
/// Represents common entity for deserialize loader-manifest
/// </summary>
public class ExtractedFileHandler
{
    public string? ManifestText { get; private set; }
    public string? LoaderManifestText { get; private set; }
    private string? ModName { get; set; }
    private string? ModPath { get; set; }
    
    private string Root { get; set; } = AppDomain.CurrentDomain.FriendlyName + "\\";

    public MetadataType LoaderType { get; private set; }
    
    /// <param name="fileName">
    /// Full path of targeted jar
    /// </param>
    public ExtractedFileHandler(string fileName)
    {
        ModPath = fileName;
        ModName = new FileInfo(fileName).Name + "\\";
        ProcessTarget();
        ReadManifest();
        ReadLoaderManifest();
    }

    /// <summary>
    /// Processes targeted java applet
    /// extracts manifests and icon to new modification
    /// catalog
    /// </summary>
    private void ProcessTarget()
    {
        using IArchive aFactory = ArchiveFactory.Open(ModPath!);
        Directory.CreateDirectory(Root + ModName);
        
        foreach (IArchiveEntry entry in aFactory.Entries)
        {
            switch (entry.Key)
            {
                case "META-INF/mods.toml":
                case "META-INF/forge/mods.toml":
                    entry.WriteToFile(Root + ModName + "C_METADATA.MF");
                    break;
                case "META-INF/MANIFEST.MF":
                    entry.WriteToFile(Root + ModName + "MANIFEST.MF");
                    break;
                case "quilt.mod.json":
                    entry.WriteToFile(Root + ModName + "Q_METADATA.MF");
                    break;
                case "fabric.mod.json":
                    entry.WriteToFile(Root + ModName + "F_METADATA.MF");
                    break;
            }
        }
    }
    /// <summary>
    /// Reads MANIFEST.MF in java archive.
    /// </summary>
    public string ReadManifest()
    {
        return File.ReadAllText(
            Root +
            ModName +
            "MANIFEST.MF"
        );
    }
    /// <summary>
    /// Reads loader manifest file
    /// (depends on loader)
    /// </summary>
    public string ReadLoaderManifest()
    {
        try
        {
            if (File.Exists(Root + ModName + "Q_METADATA.MF"))
            {
                LoaderType = MetadataType.Quilt;
                return File.ReadAllText(Root + ModName + "Q_METADATA");
            }
            if (File.Exists(Root + ModName + "C_METADATA.MF"))
            {
                LoaderType = MetadataType.Forge;
                return File.ReadAllText(Root + ModName + "C_METADATA.MF");
            }
            if (File.Exists(Root + ModName + "F_METADATA.MF"))
            {
                LoaderType = MetadataType.Fabric;
                return File.ReadAllText(Root + ModName + "F_METADATA.MF");
            }
        }
        catch
        {
            // ignat
        }

        return string.Empty; // spooky dookie
    }
    /// <summary>
    /// Trying to deserialize Fabric with model.
    /// </summary>
    /// <param name="model"></param>
    /// <exception cref="JavaAppletTypeException"/>
    public ExtractedFileHandler DeserializeLoaderManifest(ref FabricModel model)
    {
        try
        {
            string content = File.Exists(Root + ModName +  "F_METADATA.MF") 
                ? File.ReadAllText(Root + ModName + "F_METADATA.MF") 
                : File.ReadAllText(Root + ModName + "Q_METADATA.MF");
            
            model = JsonSerializer.Deserialize<FabricModel>(content)!;
        }
        catch (JsonException)
        {
            // top 1 funny moments //
            throw new JavaAppletTypeException();
        }
        return this;
    }

    /// <summary>
    /// Trying to deserialize Forge model file
    /// </summary>
    /// <param name="forge"></param>
    /// <returns></returns>
    /// <exception cref="JavaAppletTypeException"/>
    public ExtractedFileHandler DeserializeLoaderManifest(ref ForgeModel forge)
    {
        TomlTable model = Toml.ToModel(ReadLoaderManifest());

        // Преобразуем TomlTable в ваш класс
        ForgeModel metadata = new()
        {
            Loader = model["modLoader"] as string,
            LoaderVersion = model["loaderVersion"] as string,
            IssueTrackerUrl = model["issueTrackerURL"] as string,
            License = model["license"] as string
        };

        if (!model.TryGetValue("mods", out object modsTable) || modsTable is not TomlTableArray modsArray) 
            return this;
        
        foreach (TomlTable modTable in modsArray)
        {
            ForgeModsHeader mod = new()
            {
                ModId = modTable["modId"] as string,
                ModVersion = modTable["version"] as string,
                ModDisplayName = modTable["displayName"] as string,
                ModDisplayUrl = modTable["displayURL"] as string,
                ModAuthors = modTable["authors"] as string,
                ModLogoFile = modTable["logoFile"] as string
            };
            // [[dependencies.modId]] 2-3 items
            // Why 0????
            if (modTable.TryGetValue($"dependencies.{mod.ModId}", out object depsTable) && depsTable is TomlTableArray depsArray)
            {
                foreach (TomlTable depTable in depsArray)
                {
                    string depName = depTable.Keys.First();
                    
                    if (depTable[depName] is not TomlTableArray depItems) continue;
                    var depList = depItems
                        .Select(item => new ForgeDependency {
                            ModId = item["modId"] as string, 
                            VersionRange = item["versionRange"] as string, 
                            Ordering = item["ordering"] as string, 
                            Side = item["side"] as string
                        })
                        .ToList();
                    metadata.Dependencies = depList.ToArray();
                }
            }
            metadata.Mods = mod;
        }
        forge = metadata;
        return this;
    }

    /// <summary>
    /// Removes temporary catalog with extracted jar members.
    /// </summary>
    /// <returns></returns>
    public ExtractedFileHandler DeleteExtractedFiles()
    {
        if (Directory.Exists(Root + ModName))
            Directory.Delete(Root + ModName);
        
        return this;
    }
}