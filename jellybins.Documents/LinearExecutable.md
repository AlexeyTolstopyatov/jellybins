# Microsoft Object Format
### Linear Executable (LX) Format: A Detailed Overview

The **Linear Executable (LX)** format is a 32-bit executable file format used primarily in OS/2 2.0 and later versions, as well as in some other operating systems like **Windows VxD** (Virtual Device Drivers). It was designed to take advantage of the 32-bit flat memory model, which simplifies memory management compared to the segmented model used in 16-bit systems.

### 1. **Structure of an LX File**
An LX file consists of several key components, each serving a specific purpose. The main parts of an LX file are:

#### 1.1 **DOS Stub**
- Like the NE and PE formats, LX files begin with a **DOS stub**.
- The stub starts with the **"MZ" signature** (0x4D 0x5A) and contains a simple DOS program.
- This program usually displays a message like "This program cannot be run in DOS mode."
- The DOS stub is followed by a pointer to the LX header.

#### 1.2 **LX Header**
- The LX header begins with the **"LX" signature** (0x4C 0x58).
- It contains essential information about the executable, such as:
  - **Module Format Level**: The version of the LX format.
  - **CPU Type**: The target CPU architecture (e.g., 80386, 80486).
  - **Module Flags**: Flags indicating attributes of the module (e.g., DLL, executable, etc.).
  - **Number of Pages**: The number of memory pages used by the module.
  - **Object Table Offset**: The offset to the object table.
  - **Resource Table Offset**: The offset to the resource table.
  - **Resident Name Table Offset**: The offset to the resident name table.
  - **Entry Table Offset**: The offset to the entry table.
  - **Module Reference Table Offset**: The offset to the module reference table.
  - **Import Name Table Offset**: The offset to the import name table.

#### 1.3 **Object Table**
- The object table describes the objects (`.text` and `.data`) in the executable.
- Each object entry includes:
  - **Virtual Size**: The size of the object in memory.
  - **Relocation Base Address**: The base address for relocations.
  - **Object Flags**: Attributes of the object (e.g., executable, readable, writable, etc.).
  - **Page Table Index**: The index into the page table for this object.

#### 1.4 **Page Table**
- The page table contains information about the memory pages used by the executable.
- Each page table entry includes:
  - **Page Flags**: Attributes of the page (e.g., present, readable, writable, etc.).
  - **Data Offset**: The file offset to the page data.

#### 1.5 **Resource Table**
- The resource table contains information about resources such as icons, menus, dialogs, and strings.
- Resources are organized in a hierarchical structure, with each resource identified by a type, name, and language.

#### 1.6 **Entry Table**
- The entry table contains information about exported functions and entry points.
- Each entry includes:
  - **Object Number**: The object containing the function.
  - **Offset**: The offset within the object to the function.

#### 1.7 **Resident Name Table**
- The resident name table contains names of exported functions and other symbols that are always loaded into memory.

#### 1.8 **Non-Resident Name Table**
- The non-resident name table contains names of exported functions and symbols that are not always loaded into memory.

#### 1.9 **Module Reference Table**
- The module reference table contains references to other modules (DLLs) that this executable depends on.

#### 1.10 **Import Name Table**
- The import name table contains the names of functions imported from other modules.

### 2. **Key Features of the LX Format**
- **32-bit Flat Memory Model**: LX files are designed for 32-bit flat memory architecture, which simplifies memory management compared to the segmented model used in NE files.
- **Multiple Objects**: LX files can have multiple objects (code and data), allowing for larger and more complex programs.
- **Resource Management**: LX files support a rich set of resources, which are stored in a separate section and can be loaded on demand.
- **Dynamic Linking**: LX files support dynamic linking through the use of DLLs and import/export tables.

### 3. **Loading and Execution**
When an LX file is executed, the OS/2 loader performs the following steps:
1. **Read the DOS Stub**: Verify the "MZ" signature and locate the LX header.
2. **Read the LX Header**: Verify the "LX" signature and parse the header information.
3. **Load Objects**: Allocate memory and load the objects according to the object table.
4. **Resolve Imports**: Load required DLLs and resolve imported functions.
5. **Initialize Resources**: Load resources as needed.
6. **Execute the Entry Point**: Transfer control to the entry point specified in the LX header.

### 4. **Comparison with NE and PE Formats**
- **Architecture**: LX is designed for 32-bit flat memory architecture, while NE is designed for 16-bit segmented architecture and PE for 32-bit/64-bit flat memory architecture.
- **Complexity**: LX is more complex than NE but less complex than PE, which supports advanced features like ASLR (Address Space Layout Randomization) and digital signatures.
- **Compatibility**: LX files are primarily used in OS/2 and some versions of Windows (e.g., VxD), while PE files are used in modern Windows systems.

### 5. **Common Uses of the LX Format**
- **OS/2 Applications**: `.exe` files for OS/2 2.0 and later versions.
- **Windows VxD**: `.386` or `.vxd` Virtual Device Drivers for Windows 9x.