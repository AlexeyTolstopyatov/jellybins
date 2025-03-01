### JellyBins Console utility usage

### Interactive mode
You can use keys for quick results

```
./jbc --bin "D:\Анализ файлов\inst\PE\pwgen.exe" --list --head
D:\Анализ файлов\inst\PE\pwgen.exe
Machine                     0x8664
NumberOfSections            0xA
TimeDateStamp               0x667E8929
PointerToSymbolTable        0x0
NumberOfSymbols             0x0
SizeOfOptionalHeader        0xF0
Characteristics             0x22
Magic                       0x20B
MajorLinkerVersion          0xE
MinorLinkerVersion          0x24
SizeOfCode                  0x8200
SizeOfInitializedData       0x7A00
SizeOfUninitializedData     0x0
AddressOfEntryPoint         0x1127B
BaseOfCode                  0x1000
BaseOfData                  0x40000000
ImageBase                   0x20000001000
SectionAlignment            0x6
FileAlignment               0x0
MajorOperatingSystemVersion 0x6
MinorOperatingSystemVersion 0x0
MajorImageVersion           0x0
MinorImageVersion           0x0
MajorSubsystemVersion       0x6000
MinorSubsystemVersion       0x2
Win32VersionValue           0x400
SizeOfImage                 0x0
SizeOfHeaders               0x81600003
CheckSum                    0x100000
Subsystem                   0x0
DllCharacteristics          0x0
SizeOfStackReserve          0x1000
SizeOfStackCommit           0x100000
SizeOfHeapReserve           0x1000
SizeOfHeapCommit            0x1000000000
LoaderFlags                 0x0
NumberOfRvaAndSizes         0x0

ImageType              Application
Subsystem              Unknown
CpuArchitecture        Intel x86-64
OperatingSystem        Microsoft Windows
CpuWordLength          64
ImageVersion           14.36
OperatingSystemVersion 6.0
```

### Active mode

If you call application with no keys, it starts with shell-mode
You can query parts of binary in it.

```
binary_path> D:\Анализ файлов\inst\LE\FUJI9.DRv
JellyBins (C) Tolstopyatov Alexey 2024-2025
binary_part> plist

ImageType              Application
Subsystem              Win16Gui
CpuArchitecture        Intel i286
OperatingSystem        Microsoft Windows/286
CpuWordLength          16
ImageVersion           5.20
OperatingSystemVersion 3.0

binary_part> new
binary_path> D:\Анализ файлов\inst\LE\VNETWARE.386
binary_part> plist

ImageType              VirtualDriver
Subsystem              Native
CpuArchitecture        Intel i386
OperatingSystem        Microsoft Windows
CpuWordLength          16
ImageVersion           0.0
OperatingSystemVersion 3.0

binary_part>
```
