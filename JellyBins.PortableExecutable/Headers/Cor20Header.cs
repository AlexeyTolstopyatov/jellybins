using System.Runtime.InteropServices;

namespace JellyBins.PortableExecutable.Headers;

/// <summary>
/// Cor 2.0 Common Object Runtime header содержит
/// метаданные для приложений собранных с использованием
/// .NET
/// Компилятор/линковщик не создает в исполняемом файле
/// ничего подобного на <see cref="PeSection"/>
/// и ни о каких Import/Export/TLS/IAT речи не идет. Все хранится
/// в таблицах, которые цепляются к Cor20.
/// </summary>
[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
public struct Cor20Header
{
    public UInt32 SizeOfHead;
    public UInt16 MajorRuntimeVersion;
    public UInt16 MinorRuntimeVersion;
    public UInt64 MetaDataOffset;
    public UInt32 LinkerFlags;
    public UInt32 EntryPointRva;
    public UInt32 EntryPointToken;

    public PeDirectory Resources;
    public PeDirectory StrongName;
    public PeDirectory CodeManager;
    public PeDirectory VTableDirectory;
    public PeDirectory Exports;
    public PeDirectory ManagedNativeHeader;
}
/* COR 2.0 Header taken from Ghidra
 *    DWORD                   cb;                      // Size of the structure
 *    WORD                    MajorRuntimeVersion;     // Version of the CLR Runtime
 *    WORD                    MinorRuntimeVersion;     // Version of the CLR Runtime
 *
 *    // Symbol table and startup information
 *    IMAGE_DATA_DIRECTORY    MetaData;                // A Data Directory giving RVA and Size of MetaData
 *    DWORD                   Flags;
 *    union {
 *      DWORD                 EntryPointRVA;           // Points to the .NET native EntryPoint method
 *      DWORD                 EntryPointToken;         // Points to the .NET IL EntryPoint method
 *    };
 *    // Binding information
 *    IMAGE_DATA_DIRECTORY    Resources;               // A Data Directory for Resources, which are referenced in the MetaData
 *    IMAGE_DATA_DIRECTORY    StrongNameSignature;     // A Data Directory for unique .NET assembly signatures
 *
 *    // Regular fixup and binding information
 *    IMAGE_DATA_DIRECTORY    CodeManagerTable;        // Always 0
 *    IMAGE_DATA_DIRECTORY    VTableFixups;            // Not well documented VTable used by languages who don't follow the common type system runtime model
 *    IMAGE_DATA_DIRECTORY    ExportAddressTableJumps; // Always 0 in normal .NET assemblies, only present in native images
 *
 *    // Precompiled image info (internal use only - set to zero)
 *    IMAGE_DATA_DIRECTORY    ManagedNativeHeader;
 */