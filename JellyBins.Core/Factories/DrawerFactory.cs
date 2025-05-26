using JellyBins.Abstractions;
using JellyBins.Core.Drawers;
using JellyBins.DosCommand.Models;
using JellyBins.LinearExecutable.Models;
using JellyBins.NewExecutable.Models;
using JellyBins.PortableExecutable.Models;

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
        return dumper.SegmentationType switch
        {
            FileSegmentationType.Dos1Command => new ComDrawer((dumper as ComFileDumper)!),
            FileSegmentationType.NewExecutable => new NewExecutableDrawer((dumper as NeFileDumper)!),
            FileSegmentationType.LinearExecutable => new LinearExecutableDrawer((dumper as LeFileDumper)!),
            FileSegmentationType.PortableExecutable => new PortableExecutableDrawer((dumper as PeFileDumper)!),
            FileSegmentationType.Dos2MarkZbikowski => throw new NotSupportedException(),
            FileSegmentationType.AssemblerOutput => throw new NotImplementedException(),
            FileSegmentationType.ExecutableLinkable => throw new NotImplementedException(),
            FileSegmentationType.MachObject => throw new NotImplementedException(),
            FileSegmentationType.FatMachObject => throw new NotImplementedException(),
            FileSegmentationType.Unknown => throw new NotSupportedException(),
            _ => throw new NotSupportedException()
        };
    }
}