using JellyBins.Abstractions;
using JellyBins.DosCommand.Models;
using JellyBins.LinearExecutable.Models;
using JellyBins.NewExecutable.Models;
using JellyBins.PortableExecutable.Models;

namespace JellyBins.Core.Factories;

public static class FileDumperFactory
{
    private static IFileDumper FindCommandSigns(String path)
    {
        if (File.ReadAllBytes(path).Length < 64 * 1024 * 8)
            return new ComFileDumper(path);

        throw new KeyNotFoundException("Not COMmand file");
    }
    
    /// <param name="path"> fileName </param>
    /// <returns> Type of segmentation </returns>
    private static Boolean TryFindMicrosoftIbmSigns(String path, out FileSegmentationType type)
    {
        using FileStream stream = new(path, FileMode.Open);
        using BinaryReader reader = new(stream);
        
        if (reader.ReadUInt16() != 0x5a4d)
        {
            type = FileSegmentationType.Unknown;
            return false;
        }

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
        return true;
    }
    /// <summary>
    /// Call this if you don't know your exe-type
    /// </summary>
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
        FileSegmentationType type;

        if (File.ReadAllBytes(path).Length < 64 * 1024 &&
            String.Equals(new FileInfo(path).Extension, ".COM", StringComparison.InvariantCultureIgnoreCase))
            return new ComFileDumper(path);

        if (TryFindMicrosoftIbmSigns(path, out type))
        {
            return type switch
            {
                FileSegmentationType.NewExecutable => new NeFileDumper(path),
                FileSegmentationType.LinearExecutable => new LeFileDumper(path),
                FileSegmentationType.PortableExecutable => new PeFileDumper(path),
                _ => throw new NotSupportedException()
            };
        }
        
        return type switch
        {
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