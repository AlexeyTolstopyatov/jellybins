//
// Created by MagicBook on 30.09.2024.
//
// ELF (Executable and Linking Format) — это формат исполняемых двоичных файлов,
// используемый во многих современных UNIX-подобных операционных системах,
// таких как FreeBSD, Linux, Solaris и др.
//
// Стандарт формата ELF различает несколько типов файлов:
//
//  1) Перемещаемый файл.
//  Хранит инструкции и данные, которые могут быть связаны с другими объектными файлами.
//
//  2) Совместно используемый объектный файл. (Shared Object *.so)
//  Также содержит инструкции и данные
//  и может быть связан с другими перемещаемыми
//  файлами и совместно используемыми объектными файлами.
//
//  3) Исполняемый файл. (Program .o)
//  Содержит полное описание,
//  позволяющее системе создать образ процесса.
//
// .без расширения;
// .axf, .bin, .elf, .o, .out, .prx, .puff, .ko, .mod, .so
//

#ifndef JBC_ELFMODEL_H
#define JBC_ELFMODEL_H

# include <stdint.h>

typedef uint16_t elf32_half_t;          // Unsigned half int
typedef uint32_t elf32_uoffset_t;	    // Unsigned offset
typedef uint32_t elf32_address_t;       // Unsigned address
typedef uint32_t elf32_word_t;          // Unsigned int
typedef int32_t  elf32_sword_t;         // Signed int

typedef uint32_t elf64_half_t;
typedef uint64_t elf64_uoffset_t;
typedef uint64_t elf64_address_t;
typedef uint64_t elf64_word_t;
typedef int64_t  elf_sword_t;

// Переопределить типы данных
// для 32 разрядных архитектур
struct ELF32_HEADER {
    uint32_t        p_type;
    elf32_uoffset_t p_offset;
    elf32_address_t p_vaddr;
    elf32_address_t p_paddr;
    uint32_t        p_filesz;
    uint32_t        p_memsz;
    uint32_t        p_flags;
    uint32_t        p_align;
};


// Переопределить типы данных
// для 64 разрядных архитектур
struct ELF64_HEADER {
    uint32_t        p_type;
    uint32_t        p_flags;
    elf64_uoffset_t p_offset;
    elf64_address_t p_vaddr;
    elf64_address_t p_paddr;
    uint64_t        p_filesz;
    uint64_t        p_memsz;
    uint64_t        p_align;
};



#endif //JBC_ELFMODEL_H
