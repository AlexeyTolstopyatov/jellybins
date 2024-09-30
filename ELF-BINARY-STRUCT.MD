### Executable Linkable binary's Header

```c
typedef struct {
    uint32_t   p_type;
    Elf32_Off  p_offset;
    Elf32_Addr p_vaddr;
    Elf32_Addr p_paddr;
    uint32_t   p_filesz;
    uint32_t   p_memsz;
    uint32_t   p_flags;
    uint32_t   p_align;
} Elf32_Phdr;
```

```c
typedef struct {
    uint32_t   p_type;
    uint32_t   p_flags;
    Elf64_Off  p_offset;
    Elf64_Addr p_vaddr;
    Elf64_Addr p_paddr;
    uint64_t   p_filesz;
    uint64_t   p_memsz;
    uint64_t   p_align;
} Elf64_Phdr;
```

The main difference between the 32-bit and the 64-bit program
header lies in the location of the p_flags member in the total struct.

### ELF Types

```c
# include <stdint.h>

typedef uint16_t Elf32_Half; // Unsigned half int
typedef uint32_t Elf32_Off;	  //Unsigned offset
typedef uint32_t Elf32_Addr; // Unsigned address
typedef uint32_t Elf32_Word; // Unsigned int
typedef int32_t  Elf32_Sword;// Signed int
```