using JellyBins.Abstractions;
using JellyBins.Core.Drawers;
using JellyBins.DosCommand.Models;

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
        dumper.Dump(); // ub?
        // FIXME: null forgive denied 
        return dumper.SegmentationType switch
        {
            FileSegmentationType.Dos1Command => new ComDrawer((dumper as ComFileDumper)!),
            
            _ => throw new NotSupportedException()
        };
    }
}