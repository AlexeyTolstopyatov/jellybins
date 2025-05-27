# JellyBins.Console (`jbc`)

<img align="right" src="JellyBins.Assets/beans512.png" height="128" width="128">
Is a GUI-less application variant built for
binary static analysis. using F#

 Works correctly only with Low Endian segmented binaries

### Version

<img src="JellyBins.Assets/version.png">

### JellyBins `BaseDump<T>` structure

When you make dump of application, first table never be
an expected information. This table names - "BaseDump" 
Base dump structure describes Dumper's behaviour and
don't have main goal to show you raw bytes.

Let's see example of this table:

```powershell
PS D:\GitHub\JellyBins\JellyBins.Console\bin\Debug\net8.0> ./JellyBins.Console.exe dump "D:\Анализ файлов\inst\LE\DOOM.EXE" --sections

| Name                            | Address | Size | <-- JellyBins
|---------------------------------|---------|------|     BaseDump members
| LE Entry (JellyBins: IMAGE_SEC) | 3554    | 1C   | 
| LE Entry (JellyBins: IMAGE_SEC) | 3570    | 1C   |

    Name    -- Original name of structure, taken from 
            Software public API
    Address -- Position of file-pointer 
                "where a structure begins?" 
    Size    -- Safe (GCHandle) structure size
                without any unfixed Arrays
                and variable data segments
```

Next by the `BaseDump<T>` follows the `<T>` itself.
Table strongly depends on required Type `<T>`.
Because next segment in BaseDump is an `T Segmentation` field
which collects raw bytes to readable object

Let's see continue of previous example

```powershell
OS/2 LX ObjectSection contains next fields 
Object Sections in table = 2 (took from BaseDump)
So, prepared Table looks like this:

| VirtualAddress | #PageTable | #PageTableEntries | BaseRelocAddress | ObjectFlags |
|----------------|------------|-------------------|------------------|-------------|
| 114336         | 1C         | 0                 | 10000            | 2045        |
| 196608         | 0          | 10000             | 2043             | 1D          |

```

And last documented (fixed) field in BaseDump structure is
a `Characteristics` which represents array of strings
processed from raw byte flags. In example, Those sections in
`ObjectSectionsTable` from previous output
have `ObjectFlags` and those sections characteristics 
looks like this:

```
Position: 3554
IMAGE_OBJECT_ENTRY_READ
IMAGE_OBJECT_ENTRY_EXECUTE

Position: 3570
IMAGE_OBJECT_ENTRY_READ
IMAGE_OBJECT_ENTRY_WRITE
```

### Exports/Imports

Expoting entries or non-resident names translates to Markdown table too.
Because I find it more comfortable way than print longpost in 2 columns
like Microsoft GUI-less utilities.

Let's see an New executable segmented binary insights
together:

```powershell
PS D:\GitHub\JellyBins\JellyBins.Console\bin\Debug\net8.0> ./JellyBins.Console.exe dump "D:\Анализ файлов\inst\NE\GDI.EXE" --exports 
```

You really can paste processed data to markdown issue-message and
send or store it. 

| #  | Name                                        | Ordinal |
|----|---------------------------------------------|---------|
| 43 | Microsoft Windows Graphics Device Interface | @0      |
| 14 | GETWINDOWEXTEX                              | @474    |
| 10 | COMBINERGN                                  | @47     |
| 14 | SETWINDOWORGEX                              | @482    |
| 16 | SETVIEWPORTORGEX                            | @480    |
| 10 | QUERYABORT                                  | @155    |
| 16 | ENGINEDELETEFONT                            | @301    |
| .. | ...                                         | ...     |

Or a Portable executable importing names

```powershell
PS D:\GitHub\JellyBins\JellyBins.Console\bin\Debug\net8.0> ./JellyBins.Console.exe dump "D:\Анализ файлов\inst\PE\SMSS.EXE" --imports
| Name                                                 | Address | Size |
|------------------------------------------------------|---------|------|
| PE Imports (JellyBins: IMAGE_IMPORT_DIRECTORY_TABLE) | 0       | 0    |

| From      | Name                          | Ordinal | Hint | Address |
|-----------|-------------------------------|---------|------|---------|
| ntdll.dll | NtOpenKey                     | @0      | 9F   | 1360    |
| ntdll.dll | RtlSetDaclSecurityDescriptor  | @0      | 262  | 1190    |
| ntdll.dll | RtlCreateSecurityDescriptor   | @0      | 182  | 11B0    |
| ntdll.dll | RtlAddAccessAllowedAce        | @0      | 138  | 11CE    |
| ntdll.dll | RtlCreateAcl                  | @0      | 179  | 11E8    |
| ntdll.dll | RtlFreeSid                    | @0      | 1DA  | 11F8    |
| ntdll.dll | RtlAllocateAndInitializeSid   | @0      | 148  | 1206    |
| ntdll.dll | NtClose                       | @0      | 4C   | 1224    |
| ntdll.dll | NtSetSecurityObject           | @0      | 109  | 122E    |
| ntdll.dll | NtOpenFile                    | @0      | 9C   | 1244    |
| ntdll.dll | RtlInitUnicodeString          | @0      | 1FE  | 1252    |
| ntdll.dll | LdrUnloadDll                  | @0      | 2E   | 126A    |

```

### Suggested extern toolchains

Key `--uses` for `dump` operation allows program
to print comparisons between importing entities
and those goals.

In example: We all know, a `kernel32.dll` is
a one of necessary bridges between Client Applications
at Ring #3 (user's layer) and NT kernel modules at the Ring 0, 
So, if you use `--uses` or `-u` key, `jbc` tells you, what
your program required need to work correctly:

```powershell 
PS D:\GitHub\JellyBins\JellyBins.Console\bin\Debug\net8.0> ./JellyBins.Console.exe dump "D:\Анализ файлов\inst\PE\write.exe" --uses
| Category           | Module       |
|--------------------|--------------|
| Windows Win32 API  | SHELL32.dll  |
| Windows Win32 API  | KERNEL32.dll |
| Visual C++ Runtime | msvcrt.dll   |
```

Those names not hardcoded. They are contains at the isolated
dictionary `.json` files, which you can edit.

For every format of linking exists own dictionary file.
In example: for `PE` files storage of names will be:
```json
// JellyBins.PortableExecutable.Dictionary.json (*)
{
    "Dangerous Software modules": [
        // Set your own patterns
        "lib", 
        "lib2", 
        "/etc/"
    ]
}
```

### Shorten Information
If you need only shorten information set (i.e. uses and characteristics of program), you also can call `dump` procedure

```powershell
PS D:\GitHub\JellyBins\JellyBins.Console\bin\Debug\net8.0> ./JellyBins.Console.exe dump "D:\Анализ файлов\inst\PE\write.exe" --uses --info
| Category           | Module       |
|--------------------|--------------|
| Windows Win32 API  | SHELL32.dll  |
| Windows Win32 API  | KERNEL32.dll |
| Visual C++ Runtime | msvcrt.dll   |

FileName        write.exe
FilePath        D:\Анализ файлов\inst\PE\write.exe
BirthDay        10/28/2012 23:38:01
Target CPU      Amd64
Max. WORD       64 BIT
Target OS       Microsoft Windows
Target OS ver.  10.0
Minimum OS ver. 10.0
FileType        Application
ExtType         Application
```

If you don't have dictionary files, program gives you
empty table... and at all.
You can make those files yourself but you must follow
next rules:

1) Path to the `jbc.exe`
2) Naming: `JellyBins.<Segmentation Format>.Dictionary.json`
3) Structure like in `(*)`

