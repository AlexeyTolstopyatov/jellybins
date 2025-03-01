# Unix New Object Format
### ELF (Executable and Linkable Format): A Detailed Overview
The **ELF (Executable and Linkable Format)** is a common standard file format for executables, object code, shared libraries, and core dumps in Unix-like operating systems. It was designed to be flexible, extensible, and platform-independent, making it suitable for a wide range of applications and architectures.

### 1. **Structure of an ELF File**
An ELF file consists of several key components, each serving a specific purpose. The main parts of an ELF file are:

#### 1.1 **ELF Header**
- The **ELF header** is located at the beginning of the file and provides a roadmap to the rest of the file.
- It includes the following fields:
  - **Magic Number**: Identifies the file as an ELF file (0x7F, 'E', 'L', 'F').
  - **Class**: Specifies the architecture (32-bit or 64-bit).
  - **Data Encoding**: Specifies the byte order (little-endian or big-endian).
  - **ELF Version**: The version of the ELF format.
  - **OS/ABI**: Identifies the operating system and ABI (Application Binary Interface).
  - **Type**: Specifies the type of the file (e.g., executable, object, shared library).
  - **Machine**: Specifies the target architecture (e.g., x86, ARM).
  - **Entry Point**: The virtual address where execution begins.
  - **Program Header Offset**: The file offset to the program header table.
  - **Section Header Offset**: The file offset to the section header table.
  - **Flags**: Processor-specific flags.
  - **Header Size**: The size of the ELF header.
  - **Program Header Entry Size**: The size of an entry in the program header table.
  - **Number of Program Headers**: The number of entries in the program header table.
  - **Section Header Entry Size**: The size of an entry in the section header table.
  - **Number of Section Headers**: The number of entries in the section header table.
  - **Section Name Index**: The index of the section header string table.

#### 1.2 **Program Header Table**
- The **program header table** describes the segments of the executable that need to be loaded into memory.
- Each entry in the program header table includes:
  - **Type**: The type of the segment (e.g., loadable, dynamic).
  - **Offset**: The file offset to the segment.
  - **Virtual Address**: The virtual address where the segment should be loaded.
  - **Physical Address**: The physical address (usually the same as the virtual address).
  - **File Size**: The size of the segment in the file.
  - **Memory Size**: The size of the segment in memory.
  - **Flags**: Permissions (e.g., read, write, execute).
  - **Alignment**: The alignment of the segment.

#### 1.3 **Section Header Table**
- The **section header table** describes the sections of the file, which are used for linking and debugging.
- Each entry in the section header table includes:
  - **Name**: The name of the section (index into the section header string table).
  - **Type**: The type of the section (e.g., program data, symbol table).
  - **Flags**: Attributes of the section (e.g., writable, executable).
  - **Address**: The virtual address of the section.
  - **Offset**: The file offset to the section.
  - **Size**: The size of the section.
  - **Link**: A section header table index link.
  - **Info**: Additional information.
  - **Alignment**: The alignment of the section.
  - **Entry Size**: The size of each entry in the section (if applicable).

#### 1.4 **Sections**
- The actual data for each section follows the section header table.
- Common sections include:
  - **.text**: Contains executable code.
  - **.data**: Contains initialized data.
  - **.bss**: Contains uninitialized data.
  - **.rodata**: Contains read-only data.
  - **.symtab**: Contains the symbol table.
  - **.strtab**: Contains the string table.
  - **.rel.text**: Contains relocation information for the text section.
  - **.rel.data**: Contains relocation information for the data section.
  - **.dynamic**: Contains information for dynamic linking.
  - **.plt**: Contains the Procedure Linkage Table (PLT).
  - **.got**: Contains the Global Offset Table (GOT).

#### 1.5 **Segments**
- Segments are groups of sections that are loaded into memory together.
- The program header table describes how segments should be loaded.


### 2. **Key Features of the ELF Format**
- **Flexibility**: ELF supports a wide range of architectures and operating systems.
- **Dynamic Linking**: ELF supports dynamic linking, allowing shared libraries to be loaded at runtime.
- **Position-Independent Code (PIC)**: ELF supports position-independent code, which is essential for shared libraries.
- **Extensibility**: ELF is designed to be extensible, allowing new features to be added without breaking existing tools.


### 3. **Loading and Execution**
When an ELF file is executed, the operating system loader performs the following steps:
1. **Read the ELF Header**: Verify the magic number and parse the header information.
2. **Load Segments**: Allocate memory and load the segments according to the program header table.
3. **Resolve Dynamic Dependencies**: Load required shared libraries and resolve imported functions.
4. **Apply Relocations**: Adjust addresses in the code and data sections using the relocation tables.
5. **Execute the Entry Point**: Transfer control to the entry point specified in the ELF header.

### 4. **Comparison with Other Formats**
- **a.out**: ELF is more complex and flexible than a.out, supporting features like dynamic linking and position-independent code.
- **PE**: The Portable Executable (PE) format used in Windows is similar in complexity but has different design goals and features.

### 5. **Common Uses of the ELF Format**
- **Executables**: `.exe` files for Unix-like systems.
- **Shared Libraries**: `.so` files for dynamic linking.
- **Object Files**: `.o` files produced by compilers.
- **Core Dumps**: Files containing the memory state of a crashed program.
