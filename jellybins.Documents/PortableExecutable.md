# Microsoft Object Format
### Portable Executable (PE) Format: A Detailed Overview
The **Portable Executable (PE)** format is a file format used for executables, object code, and DLLs (Dynamic Link Libraries) in Windows operating systems. It is a data structure that encapsulates the information necessary for the Windows OS loader to manage the executable code. The PE format is derived from the earlier **COFF (Common Object File Format)** and is used for 32-bit (PE32) and 64-bit (PE32+) executables.

### 1. **Structure of a PE File**
A PE file is divided into several sections, each serving a specific purpose. The main components of a PE file are:

#### 1.1 **DOS Header**
- Located at the beginning of the file.
- Starts with the **"MZ" signature** (0x4D 0x5A), which identifies the file as a DOS executable.
- Contains a stub program that runs in DOS mode, typically displaying a message like "This program cannot be run in DOS mode."
- The **e_lfanew** field in the DOS header points to the start of the PE header. I suggest, `e_lfanew` means "Long file address to..." something new. Many years ago, first Microsoft COFF was "New Executable", and `e_lfanew` was the pointer to `NE` signature.

#### 1.2 **PE Header**
- Begins with the **"PE\0\0" signature** (0x50 0x45 0x00 0x00).
- Contains the **COFF Header**(or `IMAGE_HEADER`) and the **Optional Header** (or `IMAGE_OPTIONAL_HEADER`).

##### **COFF Header**
- Contains metadata about the file, such as:
  - **Machine**: The target architecture (e.g., 0x014C for x86, 0x8664 for x64).
  - **NumberOfSections**: The number of sections in the file.
  - **TimeDateStamp**: The time the file was created.
  - **PointerToSymbolTable** and **NumberOfSymbols**: Used for debugging (rarely used in PE files).
  - **SizeOfOptionalHeader**: The size of the Optional Header.
  - **Characteristics**: Flags indicating attributes of the file (e.g., executable, DLL, etc.).

##### **Optional Header**
- Contains critical information for the OS loader, such as:
  - **Magic**: Identifies the type of PE file (0x10B for PE32, 0x20B for PE32+).
  - **AddressOfEntryPoint**: The RVA (Relative Virtual Address) of the entry point.
  - **ImageBase**: The preferred base address for loading the executable.
  - **SectionAlignment** and **FileAlignment**: Alignment of sections in memory and on disk.
  - **SizeOfImage**: The total size of the image in memory.
  - **Subsystem**: Specifies the subsystem required to run the executable (e.g., Windows GUI, console, etc.).
  - **DataDirectories**: An array of 16 directories, each pointing to important structures like the Import Table, Export Table, Resource Table, etc.

#### 1.3 **Section Headers**
- Each section in the PE file has a corresponding section header.
- Sections contain the actual data, such as code, resources, and imports/exports.
- Common sections include:
  - **.text**: Contains executable code.
  - **.data**: Contains initialized data.
  - **.rdata**: Contains read-only data.
  - **.rsrc**: Contains resources (e.g., icons, strings, etc.).
  - **.reloc**: Contains relocation information.
- Each section header includes:
  - **Name**: The name of the section (8 bytes).
  - **VirtualSize**: The size of the section in memory.
  - **VirtualAddress**: The RVA of the section.
  - **SizeOfRawData**: The size of the section on disk.
  - **PointerToRawData**: The file offset to the section's data.
  - **Characteristics**: Flags indicating section attributes (e.g., executable, writable, etc.).

#### 1.4 **Sections**
- The actual data for each section follows the section headers.
- Sections are aligned according to the **FileAlignment** and **SectionAlignment** values in the Optional Header.


### 2. **Key Data Directories**
The **DataDirectories** array in the Optional Header points to important structures used by the OS loader. Some of the most important directories include:

#### 2.1 **Export Table**
- Contains information about functions and data exported by the executable (used primarily in DLLs).
- Includes:
  - **Name**: The name of the DLL.
  - **Base**: The starting ordinal number for exports.
  - **NumberOfFunctions**: The number of exported functions.
  - **AddressOfFunctions**: An array of RVAs pointing to the exported functions.
  - **AddressOfNames**: An array of RVAs pointing to the names of the exported functions.
  - **AddressOfNameOrdinals**: An array of ordinals corresponding to the names.

#### 2.2 **Import Table**
- Contains information about functions and data imported from other DLLs.
- Includes:
  - **OriginalFirstThunk**: Points to the Import Lookup Table (ILT).
  - **TimeDateStamp**: Used for binding.
  - **ForwarderChain**: Used for forwarding.
  - **Name**: The name of the DLL being imported from.
  - **FirstThunk**: Points to the Import Address Table (IAT).

#### 2.3 **Resource Table**
- Contains resources such as icons, strings, dialogs, and version information.
- Resources are organized in a hierarchical directory structure.

#### 2.4 **Base Relocation Table**
- Contains information for relocating the image if it cannot be loaded at its preferred base address.
- Relocation entries specify offsets that need to be adjusted.

#### 2.5 **Debug Directory**
- Contains debugging information, such as the location of the PDB (Program Database) file.

#### 2.6 **TLS (Thread Local Storage) Table**
- Contains information for thread-local storage, used for variables that are unique to each thread.

#### 2.7 **Load Configuration Directory**
- Contains security-related information, such as SEH (Structured Exception Handling) and Guard CF (Control Flow Guard) settings.

### 3. **Loading and Execution**
When a PE file is executed, the Windows loader performs the following steps:
1. **Read the DOS Header**: Verify the "MZ" signature and locate the PE header.
2. **Read the PE Header**: Verify the "PE" signature and parse the COFF and Optional Headers.
3. **Map the Image into Memory**: Allocate memory and map the sections according to their RVAs.
4. **Resolve Imports**: Load required DLLs and resolve imported functions.
5. **Apply Relocations**: Adjust addresses if the image cannot be loaded at its preferred base address.
6. **Execute the Entry Point**: Transfer control to the address specified in **AddressOfEntryPoint**.

### 4. **Tools for Analyzing PE Files**
- **PEView**: A lightweight tool for viewing PE file structures.
- **CFF Explorer**: A powerful tool for exploring and editing PE files.
- **Dependency Walker**: Analyzes dependencies and imports/exports of PE files.
- **IDA Pro**: A disassembler and debugger for reverse engineering PE files.

### 5. **Common Uses of the PE Format**
- **Executables**: `.exe` files for Windows applications.
- **Dynamic Link Libraries**: `.dll` files containing shared code and resources.
- **Device Drivers**: `.sys` files for kernel-mode drivers.
- **Object Files**: `.obj` files produced by compilers.

### 6. **Security Considerations**
- **Malware Analysis**: PE files are often analyzed for malicious behavior.
- **ASLR (Address Space Layout Randomization)**: A security feature that randomizes the base address of loaded images.
- **Digital Signatures**: PE files can be signed to verify their authenticity and integrity.

### Conclusion
The Portable Executable (PE) format is a fundamental part of the Windows operating system, providing a standardized way to package and execute code. Understanding its structure is essential for software development, debugging, and security analysis. Whether you're writing a program, analyzing malware, or reverse engineering, a solid grasp of the PE format is invaluable.