# JellyBins

<img src="JellyBins.Assets/beans512.png" height="128" width="128" align="right"/>

JellyBins is a little software, which represents functional
for dumping binaries. This is not debugger, enough. 
Best comparison for this is a universal `CFF Explorer` or other utility
for seeing map of binary.

## Supported Segmentations

| Format Name                       | Where find                                                                                           | State |
|-----------------------------------|------------------------------------------------------------------------------------------------------|-------|
| `COM`                             | `CP/M` `BW-DOS` `PC-DOS` `MS-DOS` and other Disk operating system                                    | [x]   |
| `NE` "New Executable"             | Microsoft Windows 3x, and files, OS/2 system internals  (Ring 2 Drivers `.DRV`) Fonts `.fon`         | [x]   |
| `LE` and `LX` "Linear Executable" | Microsoft Windows 9x VxD drivers, IBM OS/2 at all (`.EXE`, `.DLL`, `.SYS`).                          | [x]   |
| `PE` "Portable Executable"        | Microsoft Windows 9x, NT at all (`.DLL`, `.EXE`, `.SYS`, `.OCX`) and EFI bytecode                    | [x]   |
| "Assembler-Output"  objects       | Early `UNIX`-like Operating Systems Files don't have extension or have `.o`,  `.so`, `.d` at the end | []    |
| `ELF` "Executable Linkable"       | Modern `UNIX`-like Operating Systems. Files don't have extension or have `.o`,  `.so`, `.d`          | []    |
| `Mach-O` "Mach Object"            | Apple `macOS`/`OS X`/`MacOS X` has special segmentation format for objects and programs.             | []    |

### Core insights

A `JellyBins.Core.dll` contains generics or common Entities/Methods. Those internals like a bridge between user and library, needs to automate
recognition of segmentation's type.

At the moment of `README(s)` updates, avaliable to find and deserialize next structures

  - Program's Headers
  - Sections
  - Static Import data
  - Export entries/Non-resident names
  - Runtime Headers
  - Suggested usings of external modules (libraries)

### Specially for every segment's dump

All dumps have *special* JellyBins structure,
which describes Dumper's behaviour.

```csharp
class BaseDump<T> {
   /// <summary> Name of Dump (orig. struct name) </summary>
   public String Name {get;set;}
   /// <summary>Start of struct raw Pointer</summary>
   public UInt64 Address {get;set;}
   /// <summary>Strongly size of struct</summary>
   public UInt64 Size {get;set;}
   /// <summary>Required object</summary>
   public T Segmentation {get;set;}
}
```

### Abstractions insights

A `JellyBins.Abstractions.dll` contains templates and 
interfaces for project's internals. (e.g. `IFileDumper` or `IDrawer`)

### Documentation and Sources

 - [Usage of JellyBins Console application](README_JBC.md)
 - [NE Format (segmentation details)](JellyBins.Documents/Microsoft%20NE%20Segmentation.pdf)
 - [LE Format (OS/2 16-32 bit binary model)](JellyBins.Documents/IBM%20LE%20Segments.html.pdf)
 - [LX Format (OS/2 OMF Segmentation)](JellyBins.Documents/OS2_OMF_and_LX_Object_Formats_Revision_8_199406.pdf)
 - [Visual Basic Virtual Machine metadata](JellyBins.Documents/Visual%20Basic%20Compiled%20PE%20Internals.pdf)
 - [Semi VB Decompiler PCode4.bas](JellyBins.Documents/modPCode4.bas)
 - [.NET Runtime metadata](JellyBins.Documents/The%20.NET%20File%20Format.pdf)

### Icons
Taken from [icon8](https://icons8.com/), licensed for free usage.
