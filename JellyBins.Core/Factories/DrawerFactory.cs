using JellyBins.Abstractions;
using JellyBins.Core.Drawers;
using JellyBins.DosCommand.Models;
using JellyBins.LinearExecutable.Models;
using JellyBins.NewExecutable.Models;

namespace JellyBins.Core.Factories;

public class DrawerFactory
{
    /// <summary>
    /// Calls <c>Dump()</c> method itself.
    /// </summary>
    /// <param name="dumper"></param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"> If some what you want not in public access </exception>
    public static IDrawer CreateInstance(IFileDumper dumper)
    {
        dumper.Dump();
        // TODO: Characteristics output!
        return dumper.SegmentationType switch
        {
            FileSegmentationType.Dos1Command => new ComDrawer((dumper as ComFileDumper)!),
            FileSegmentationType.NewExecutable => new NewExecutableDrawer((dumper as NeFileDumper)!),
            FileSegmentationType.LinearExecutable => new LinearExecutableDrawer((dumper as LeFileDumper)!),
            _ => throw new NotSupportedException()
        };
    }
}