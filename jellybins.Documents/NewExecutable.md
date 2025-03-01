# Microsoft Object Format
### New Executable (NE) Format: A Detailed Overview
The **New Executable (NE)** format is a file format used for 16-bit Windows executables and dynamic link libraries (DLLs). It was introduced with Windows 1.0 and was the standard format for executables until the introduction of the 32-bit Portable Executable (PE) format with Windows NT. The NE format is designed to support segmented architecture, which was common in 16-bit systems.

### 1. **Structure of an NE File**
An NE file consists of several key components, each serving a specific purpose. The main parts of an NE file are:

#### 1.1 **DOS Stub**
- Like the PE format, NE files begin with a **DOS stub**.
- The stub starts with the **"MZ" signature** (0x4D 0x5A) and contains a simple DOS program.
- This program usually displays a message like "This program requires Microsoft Windows."
- The DOS stub is followed by a pointer to the NE header.

#### 1.2 **NE Header**
- The NE header begins with the **"NE" signature** (0x4E 0x45).
- It contains essential information about the executable, such as:
  - **Linker Version**: The version of the linker used to create the file.
  - **Entry Table Offset**: The offset to the entry table, which contains information about exported functions.
  - **Resource Table Offset**: The offset to the resource table.
  - **Resident Name Table Offset**: The offset to the resident name table.
  - **Module Reference Table Offset**: The offset to the module reference table (used for DLL imports).
  - **Segment Table Offset**: The offset to the segment table.
  - **Import Name Table Offset**: The offset to the import name table.

#### 1.3 **Segment Table**
- The segment table describes the segments (code and data) in the executable.
- Each segment entry includes:
  - **Offset**: The file offset to the segment data.
  - **Length**: The size of the segment in the file.
  - **Flags**: Attributes of the segment (e.g., code, data, readable, writable, etc.).
  - **Allocation Size**: The size of the segment when loaded into memory.

#### 1.4 **Resource Table**
- The resource table contains information about resources such as icons, menus, dialogs, and strings.
- Resources are organized in a hierarchical structure, with each resource identified by a type, name, and language.

#### 1.5 **Entry Table**
- The entry table contains information about exported functions and entry points.
- Each entry includes:
  - **Segment Number**: The segment containing the function.
  - **Offset**: The offset within the segment to the function.

#### 1.6 **Resident Name Table**
- The resident name table contains names of exported functions and other symbols that are always loaded into memory.

#### 1.7 **Non-Resident Name Table**
- The non-resident name table contains names of exported functions and symbols that are not always loaded into memory.

#### 1.8 **Module Reference Table**
- The module reference table contains references to other modules (DLLs) that this executable depends on.

#### 1.9 **Import Name Table**
- The import name table contains the names of functions imported from other modules.

### 2. **Key Features of the NE Format**
- **Segmented Architecture**: NE files are designed for 16-bit segmented memory architecture, where memory is divided into segments of up to 64 KB.
- **Multiple Data and Code Segments**: NE files can have multiple code and data segments, allowing for larger programs.
- **Resource Management**: NE files support a rich set of resources, which are stored in a separate section and can be loaded on demand.
- **Dynamic Linking**: NE files support dynamic linking through the use of DLLs and import/export tables.

### 3. **Loading and Execution**
When an NE file is executed, the Windows loader performs the following steps:
1. **Read the DOS Stub**: Verify the "MZ" signature and locate the NE header.
2. **Read the NE Header**: Verify the "NE" signature and parse the header information.
3. **Load Segments**: Allocate memory and load the segments according to the segment table.
4. **Resolve Imports**: Load required DLLs and resolve imported functions.
5. **Initialize Resources**: Load resources as needed.
6. **Execute the Entry Point**: Transfer control to the entry point specified in the NE header.

### 4. **Comparison with PE Format**
- **Architecture**: NE is designed for 16-bit segmented architecture, while PE is designed for 32-bit and 64-bit flat memory architecture.
- **Complexity**: PE is more complex and flexible, supporting features like ASLR (Address Space Layout Randomization) and digital signatures.
- **Compatibility**: NE files are not compatible with modern 64-bit Windows systems, while PE files are.