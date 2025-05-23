using System.Linq.Expressions;
using JellyBins.Abstractions;
using JellyBins.DosCommand.Models;
using JellyBins.LinearExecutable.Models;
using JellyBins.NewExecutable.Models;
using JellyBins.PortableExecutable.Models;

namespace JellyBins.Core.Factories;

public static class FileDumperFactory
{
    /// <param name="path"> fileName </param>
    /// <returns> Type of segmentation </returns>
    private static FileSegmentationType FindMicrosoftIbmSigns(String path)
    {
        using FileStream stream = new(path, FileMode.Open);
        using BinaryReader reader = new(stream);
        FileSegmentationType type = FileSegmentationType.Unknown;

        if (reader.ReadUInt16() != 0x5a4d)
            return type;

        stream.Position = 0x3C;
        UInt32 signPointer = reader.ReadUInt32();

        stream.Position = signPointer;
        type = reader.ReadUInt16() switch
        {
            0x4550 or 0x5045 => FileSegmentationType.PortableExecutable,
            0x454e or 0x5e45 => FileSegmentationType.NewExecutable,
            0x454c or 0x584c => FileSegmentationType.LinearExecutable,
            _ => FileSegmentationType.Dos2MarkZbikowski
        };
        reader.Close();
        return type;
    }
    /// <summary> Call this if you don't know your exe-type </summary>
    /// <param name="path"> your path to file </param>
    /// <returns> <see cref="IFileDumper"/> instance </returns>
    /// <exception cref="NotSupportedException">
    /// Throws if function doesn't know type of what
    /// type of <see cref="IFileDumper"/> instance needed
    /// </exception>
    /// <exception cref="NotImplementedException">
    /// Throws if function knows what you mean but
    /// this type in progress and unavailable from public Factory
    /// </exception>
    public static IFileDumper CreateInstance(String path)
    {
        FileSegmentationType type = FindMicrosoftIbmSigns(path);
        
        return type switch
        {
            // old format OSes
            FileSegmentationType.Dos1Command => new ComFileDumper(path),
            FileSegmentationType.Dos2MarkZbikowski => throw new NotImplementedException(),
            FileSegmentationType.NewExecutable => new NeFileDumper(path),
            FileSegmentationType.LinearExecutable => new LeFileDumper(path),
            FileSegmentationType.PortableExecutable => new PeFileDumper(path),
            // UNIX
            FileSegmentationType.AssemblerOutput => throw new NotImplementedException(),
            FileSegmentationType.ExecutableLinkable => throw new NotImplementedException(),
            // XNU
            FileSegmentationType.FatMachObject => throw new NotImplementedException(),
            FileSegmentationType.MachObject => throw new NotImplementedException(),
            _ => throw new NotSupportedException()
        };
    }
}