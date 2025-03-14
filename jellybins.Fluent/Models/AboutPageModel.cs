using System;
using System.Diagnostics;
using System.Reflection;

namespace jellybins.Fluent.Models;

public class AboutPageModel
{
    public AboutPageModel()
    {
        FileVersionInfo @this = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
        FileVersionInfo core = FileVersionInfo.GetVersionInfo(Assembly.LoadFrom("jellybins.Core.dll").Location);
        FileVersionInfo java = FileVersionInfo.GetVersionInfo(Assembly.LoadFrom("jellybins.Java.dll").Location);
        FileVersionInfo cons = FileVersionInfo.GetVersionInfo(Assembly.LoadFrom("jellybins.Console.dll").Location);
        ProductName = @this.ProductName;
        ProductDescription = @"
JellyBins makes little report about selected binary and shows you more about.
In this build, JellyBins supports Microsoft COFFs and Unix object files,
reads information from structures in researching assembly without modifying it.
New, Linear, binaries from dark black past of IBM and Microsoft supported too.
Unix Assembler-output format specification supported, but ELFs doesn't yet.
Fat and Universal macOS binaries doesn't supported. It will be in future releases.
I want to make it.";
        ProductVersion = @this.FileVersion;
        ProductJavaVersion = java.FileVersion;
        ProductCoreVersion = core.FileVersion;
        ProductConsoleVersion = cons.FileVersion;
        CommonRuntimeVersion = Environment.Version.ToString();
    }
    
    public string? ProductVersion { get; init; }
    public string? ProductJavaVersion { get; init; }
    public string? ProductCoreVersion { get; init; }
    public string? ProductConsoleVersion { get; init; }
    public string? ProductName { get; init; }
    public string? ProductDescription { get; init; }
    public string? CommonRuntimeVersion { get; init; }
}