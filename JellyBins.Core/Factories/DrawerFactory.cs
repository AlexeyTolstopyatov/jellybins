using JellyBins.Abstractions;
using JellyBins.Core.Drawers;
using JellyBins.DosCommand.Models;
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
        
        if (dumper.SegmentationType == FileSegmentationType.Dos1Command)
            return new ComDrawer((dumper as ComFileDumper)!); // always true
        
        if (dumper.SegmentationType == FileSegmentationType.NewExecutable)
            return new NewExecutableDrawer((dumper as NeFileDumper)!); // not
        
        throw new NotImplementedException();
    }
}