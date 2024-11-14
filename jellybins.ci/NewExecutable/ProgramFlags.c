//
// Created by MagicBook on 11.11.2024.
//

#include "ProgramFlags.h"

void ne_GetCpu(uint8_t cpu) {
    printf("Target CPU\t");
    switch (cpu) {
        case NEW_CPU_8086:
            printf("Intel 8086\n");
            break;
        case NEW_CPU_i286:
            printf("Intel i286\n");
            break;
        case NEW_CPU_i386 :
            printf("Intel i386\n");
            break;
        case NEW_CPU_8087:
            printf("Intel 8087");
        default:
            printf("Unknown Targeted CPU 0x%x\n", cpu);
            break;
    }
}

void ne_GetOperatingSystem(uint8_t os) {
    printf("Tagret OS\t");
    switch (os) {
        case NEW_OS_OS2:
            printf("IBM OS/2\n");
            break;
        case NEW_OS_WIN:
            printf("Microsoft Windows\n");
            break;
        case NEW_OS_DOS:
            printf("Microsoft DOS\n");
            break;
        case NEW_OS_WIN386:
            printf("Microsoft Windows/386");
            break;
        case NEW_OS_BOSS:
            printf("Borland OS Services");
            break;
        default:
            printf("Unknown OS 0x%x\n", os);
            break;
    }
}

void ne_GetApplicationFlags(uint8_t flags) {
    if ((flags & ne_app_fullscrn) != 0) printf("Windowing\tFull Screen\n");
    if ((flags & ne_app_pmcompat) != 0) printf("Windowing\tOS/2 PM Compatible\n");
    if ((flags & ne_app_pmapiuse) != 0) printf("Windowing\tOS/2 PM API Used\n");
    if ((flags & ne_app_loadcode) != 0) printf("Structure\tLoad .CODE\n");
    if ((flags & ne_app_linkerrs) != 0) printf("Arch-flag\tLinking errors occured\n");
    if ((flags & ne_app_noncomfr) != 0) printf("Binary type\tNon-comforming\n");
    if ((flags & ne_app_library)  != 0) printf("Binary type\tDLL module\n");
}

void ne_GetProgramFlags(uint8_t flags) {
    if ((flags & ne_prog_datagrp) != 0)     printf("Structure\t.DATA group\n");
    if ((flags & ne_prog_datagrpnone) != 0) printf("Structure\tNo .DATA sections\n");
    if ((flags & ne_prog_datasshared) != 0) printf("Structure\tSingle .DATA shared section\n");
    if ((flags & ne_prog_datamshared) != 0) printf("Structure\tMultiple .DATA shared sections\n");
    if ((flags & ne_prog_globalinitl) != 0) printf("Structure\tGlobal initialization\n");
    if ((flags & ne_prog_protectmode) != 0) printf("Arch-flag\tOnly in Protected mode\n");
}

void ne_GetOtherFlags(uint8_t flags){
    if ((flags & ne_os2_longnames) != 0) printf("File System\tLong names support (HPFS)\n");
    if ((flags & ne_os2_longnames) == 0) printf("File System\tRule 8.3 naming (FAT16)\n");
    if ((flags & ne_os2_protectmd) != 0) printf("OS/2 Loader\tOnly in Protected mode\n");
    if ((flags & ne_os2_propfonts) != 0) printf("OS/2 Loader\tVector fonts\n");
    if ((flags & ne_os2_egangload) != 0) printf("Structure\tBinary has gangload area\n");
}

void ne_Get(struct NE_HEADER* ne){
    printf("NE HEADER TABLE\n");
    printf("----------------\n");

    printf("SIGNATURE\t\t");
    getu2r(&ne->signature);
    printf("NE VERSION\t\t");
    getu1r(&ne->version);
    printf("NE REVISION\t\t");
    getu1r(&ne->revision);
    printf("ENTRY TABLE \t\t");
    getu2r(&ne->entry_table_offset);
    printf("SIZE OF ENTRY TABLE\t");
    getu2r(&ne->sizeof_entry_table);
    printf("CHECKSUM\t\t");
    getu4r(&ne->checksum);
    printf("PROG FLAGS\t\t");
    getu1r(&ne->progflags);
    printf("APP FLAGS\t\t");
    getu1r(&ne->appflags);
    printf("AUTODATA SEGMENT\t");
    getu2r(&ne->auto_data_segment);
    printf("HEAP\tINIT VALUE\t");
    getu2r(&ne->heap);
    printf("STACK\tINIT VALUE\t");
    getu2r(&ne->stack);
    printf("CS:IP\tINIT VALUE\t");
    getu4r(&ne->csip);
    printf("SS:SP\tINIT VALUE\t");
    getu4r(&ne->sssp);
    printf("SEGMENTS\tCOUNT\t");
    getu2r(&ne->segments_count);
    printf("MODULE-REFERENCE TABLE\t");
    getu2r(&ne->module_reft_entries);
    printf("SIZE OF NR-NAME  TABLE\t");
    getu2r(&ne->sizeof_name_table);
    printf("SEGMENT-TABLE   OFFSET\t");
    getu2r(&ne->segment_table_offset);
    printf("RESOURCE TABLE  OFFSET\t");
    getu2r(&ne->resources_table_offset);
    printf("R-NAMES  TABLE  OFFSET\t");
    getu2r(&ne->resident_name_table_offset);
    printf("MOD-REF TABLE OFFSET\t");
    getu2r(&ne->module_reference_table_offset);
    printf("IMPORT TABLE OFFSET\t");
    getu2r(&ne->imports_table_offset);
    printf("NONR-NAMES TABLE OFFSET\t");
    getu4r(&ne->nonresident_name_table_offset);
    printf("COUNT OF MOVING ENTRIES\t");
    getu2r(&ne->mov_entries_count);
    printf("ALIGNMENT\t\t");
    getu2r(&ne->align);
    printf("RESOURCE SEGMENT COUNT\t");
    getu2r(&ne->resource_segments_count);
    printf("TARGETED OS\t\t");
    getu1r(&ne->os);
    printf("OTHER LOADER FLAGS\t");
    getu1r(&ne->oflag);
    printf("RETURN ENTRIES COUNT\t");
    getu2r(&ne->ret_entries_count);
    printf("REF-BYTES SEG. OFFSET\t");
    getu2r(&ne->refbytes_segment_offset);
    printf("SIZE OF CODE AREA\t");
    getu2r(&ne->sizeof_codearea);
    printf("MAJOR OS VERSION\t");
    getu1r(&ne->major);
    printf("MINOR OS VERSION\t");
    getu1r(&ne->minor);
    printf("-----------------\n");
}

void ne_GetVersion(uint8_t major, uint8_t minor) {
    // Сегменты заполняются от младшего к старшему
    printf("OS Version\t%d.%d\n", minor, major);
}
