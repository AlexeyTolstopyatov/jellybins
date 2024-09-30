#include "printing.h"



/*
 * Выводит таблицу содержания MZ заголовка.
 */
void printmz(struct MZ_HEADER *exec)
{
    printf("MZ HEADER TABLE\n");
    printf("----------------\n");

    printf("MZ SIGNATURE   \t");
    winfo(&exec->mz_signature);
    printf("LAST BLOCK SIZE\t");
    winfo(&exec->last_block_size);
    printf("BLOCKS COUNT\t");
    winfo(&exec->file_blocks);
    printf("RELOCATIONS\t");
    winfo(&exec->relocations_count);
    printf("PARAGRAPHS (p)\t");
    winfo(&exec->paragraphs);
    printf("min(extra_p) \t");
    winfo(&exec->min_extra_paragraphs);
    printf("max(extra_p) \t");
    winfo(&exec->max_extra_paragraphs);
    printf("init(SS)  \t");
    winfo(&exec->ss);
    printf("init(SP)  \t");
    winfo(&exec->sp);
    printf("Checksum  \t");
    winfo(&exec->checksum);
    printf("init(CS)  \t");
    winfo(&exec->cs);
    printf("init(IP)  \t");
    winfo(&exec->ip);
    printf("RELOC OFFSET  \t");
    winfo(&exec->relocations_count);
    printf("OVERLAY NUMBER\t");
    winfo(&exec->overlay_number);
    printf("SECURED 0x1C[0]\t");
    winfo(&exec->secured_0x1c[0]);
    printf("OEM ID\t\t");
    winfo(&exec->oem_id);
    printf("OEM INFORMATION\t");
    winfo(&exec->oem_info);
    printf("SECURED 0x28[0]\t");
    winfo(&exec->secured_0x28[0]);
    printf("NEXT HEAD AT\t");
    winfo(&exec->next_signature);
    printf("----------------\n");
}

/*
 * Выводит таблицу содержания PE заголовка.
*/
void printpe(struct PE_HEADER *exec)
{
    printf("PE HEADER TABLE\n");
    printf("----------------\n");

    printf("HEADER\t\t\tPE\n");
    printf("MACHINE\t\t\t");
    binfo(&exec->machine);
    printf("SECTIONS \t\t");
    binfo(&exec->sections_count);
    printf("TIMESTAMP\t\t");
    winfo(&exec->time_stamp);
    printf("TABLE POINTER\t\t");
    winfo(&exec->symtable_ptr);
    printf("CHARS COUNT\t\t");
    winfo(&exec->symbols_count);
    printf("OPT HEADER \t\t");
    binfo(&exec->sizeof_optional_header);
    printf("CHARACTERS \t\t");
    binfo(&exec->characteristics);
}

/*
 * Выводит таблицу содержания опционального PE заголовка
 */
void printope(struct OPTIONAL_PE_HEADER *ope)
{
    printf("OPTIONAL SIGNATURE\t");
    binfo(&ope->signature);
    printf("MAJOR LINKER VERSION\t");
    binfo(&ope->maj_linkv);
    printf("MINOR LINKER VERSION\t");
    binfo(&ope->min_linkv);
    printf("SIZE OF CODE SEGMENT\t");
    winfo(&ope->sizeof_code);
    printf("SIZE OF INIT SEGMENT\t");
    winfo(&ope->sizeof_init);
    printf("SIZE OF UNIT SEGMENT\t");
    winfo(&ope->sizeof_uninit);
    printf("ENTRY POINT LOCATION\t");
    winfo(&ope->entry_point_address);
    printf(".text  EXEC  SECTION\t");
    winfo(&ope->code);
    printf(".data  EXEC  SECTION\t");
    winfo(&ope->data);
    printf("IMAGE  HEADER  VALUE\t");
    winfo(&ope->image);
    printf("SECTION    ALIGNMENT\t");
    winfo(&ope->sect_alignment);
    printf("FILE       ALIGNMENT\t");
    winfo(&ope->file_alignment);
    printf("MAJOR   OS   VERSION\t");
    binfo(&ope->maj_os_version);
    printf("MINOR   OS   VERSION\t");
    binfo(&ope->min_os_version);
    printf("MAJOR  SUBSUSYEM VER\t");
    binfo(&ope->maj_subenv_version);
    printf("MINOR  SUBSYSTEM VER\t");
    binfo(&ope->min_subenv_version);
    printf("WIN-32 EXTENTION VER\t");
    winfo(&ope->win32_version);
    printf("SIZE OF HEADER IMAGE\t");
    winfo(&ope->sizeof_image);
    printf("SIZE OF HEADERS\t\t");
    winfo(&ope->sizeof_headers);
    printf("CHECKSUM\t\t");
    winfo(&ope->checksum);
    printf("SUBSYSTEM TARGET\t");
    binfo(&ope->sub_environment);
    printf("DLL  CHARACTERISTICS\t");
    binfo(&ope->dll_characteristics);
    printf("SIZE OF STACK COMMIT\t");
    winfo(&ope->sizeof_stack_commit);
    printf("SIZE OF STACK RESERV\t");
    winfo(&ope->sizeof_stack_reserve);
    printf("SIZE OF HEAP  COMMIT\t");
    winfo(&ope->sizeof_heap_commit);
    printf("SIZE OF HEAP RESERVE\t");
    winfo(&ope->sizeof_heap_reserve);
    printf("LOADER\tFLAGS\t\t");
    winfo(&ope->loader_flags);
    printf("----------------\n");
}

/*
 * Выводит таблицу содержания PE заголовка
 */
void printne(struct NE_HEADER *ne) 
{
    printf("NE HEADER TABLE\n");
    printf("----------------\n");

    printf("SIGNATURE\t\t");
    winfo(&ne->signature);
    printf("NE VERSION\t\t");
    binfo(&ne->version);
    printf("NE REVISION\t\t");
    binfo(&ne->revision);
    printf("ENTRY TABLE \t\t");
    winfo(&ne->entry_table_offset);
    printf("SIZE OF ENTRY TABLE\t");
    winfo(&ne->sizeof_entry_table);
    printf("CHECKSUM\t\t");
    dwinfo(&ne->checksum);
    printf("LOADER FLAGS\t\t");
    winfo(&ne->flag);
    printf("AUTODATA SEGMENT\t");
    winfo(&ne->auto_data_segment);
    printf("HEAP\tINIT VALUE\t");
    winfo(&ne->heap);
    printf("STACK\tINIT VALUE\t");
    winfo(&ne->stack);
    printf("CS:IP\tINIT VALUE\t");
    dwinfo(&ne->csip);
    printf("SS:SP\tINIT VALUE\t");
    dwinfo(&ne->sssp);
    printf("SEGMENTS\tCOUNT\t");
    winfo(&ne->segments_count);
    printf("MODULE-REFERENCE TABLE\t");
    winfo(&ne->module_reft_entries);
    printf("SIZE OF NR-NAME  TABLE\t");
    winfo(&ne->sizeof_name_table);
    printf("SEGMENT-TABLE   OFFSET\t");
    winfo(&ne->segment_table_offset);
    printf("RESOURCE TABLE  OFFSET\t");
    winfo(&ne->resources_table_offset);
    printf("R-NAMES  TABLE  OFFSET\t");
    winfo(&ne->resident_name_table_offset);
    printf("MOD-REF TABLE OFFSET\t");
    winfo(&ne->module_reference_table_offset);
    printf("IMPORT TABLE OFFSET\t");
    winfo(&ne->imports_table_offset);
    printf("NONR-NAMES TABLE OFFSET\t");
    dwinfo(&ne->nonresident_name_table_offset);
    printf("COUNT OF MOVING ENTRIES\t");
    winfo(&ne->mov_entries_count);
    printf("ALIGNMENT\t\t");
    winfo(&ne->align);
    printf("RESOURCE SEGMENT COUNT\t");
    winfo(&ne->resource_segments_count);
    printf("TARGETED OS\t\t");
    binfo(&ne->os);
    printf("OTHER LOADER FLAGS\t");
    binfo(&ne->oflag);
    printf("RETURN ENTRIES COUNT\t");
    winfo(&ne->ret_entries_count);
    printf("REF-BYTES SEG. OFFSET\t");
    winfo(&ne->refbytes_segment_offset);
    printf("SIZE OF CODE AREA\t");
    winfo(&ne->sizeof_codearea);
    printf("EXPECTED OS\t\t");
    winfo(&ne->expected_os);
    printf("-----------------\n");
}

void printlx(struct LX_HEADER* orig)
{
    printf("LE HEADER TABLE\n");
    printf("----------------\n");

    printf("SIGNATURE\t\t");
    winfo(&orig->signature);
    printf("[BYTE] ORDER\t\t");
    binfo(&orig->byte_order);
    printf("[WORD] ORDER\t\t");
    binfo(&orig->word_order);
    printf("FORMAT\t\t\t");
    dwinfo(&orig->format);
    printf("CPU TYPE\t\t");
    winfo(&orig->cpu);
    printf("OS TYPE\t\t\t");
    winfo(&orig->os);
    printf("MODULE VERSION\t\t");
    dwinfo(&orig->module_version);
    printf("MODULE FLAGS\t\t");
    dwinfo(&orig->module_flags);
    printf("MODULE PAGES\t\t");
    dwinfo(&orig->module_pages);
    printf("IP OBJECTS\t\t");
    dwinfo(&orig->eip_object_num);
    printf("IP INIT VALUE\t\t");
    dwinfo(&orig->eip);
    printf("SP OBJECTS\t\t");
    dwinfo(&orig->esp_object_num);
    printf("SP INIT VALUE\t\t");
    dwinfo(&orig->esp);
    printf("SIZE OF PAGE\t\t");
    dwinfo(&orig->sizeof_page);
    printf("SIZE OF LAST PAGE\t");
    dwinfo(&orig->sizeof_last_page);
    printf("SIZE OF FIX SECTION\t");
    dwinfo(&orig->sizeof_fix_section);
    printf("SIZE OF FIX CHECKSUM\t");
    dwinfo(&orig->sizeof_fix_section_check);
    printf("SIZE OF LOADER SECTION\t");
    dwinfo(&orig->sizeof_loader_section);
    printf("SIZE OF LOADER CHECK\t");
    dwinfo(&orig->sizeof_loader_section_check);
    printf("OBJECT TABLE OFFSET\t");
    dwinfo(&orig->object_table_offset);
    printf("OBJECT TABLE ENTRIES\t");
    dwinfo(&orig->object_table_entries);
    printf("OBJECT PAGE MAP OFFSET\t");
    dwinfo(&orig->object_page_map_offset);
    printf("OBJECT ITER-DATA OFFSET\t");
    dwinfo(&orig->object_iterdata_offset);
    printf("RESOURCES TABLE OFFSET\t");
    dwinfo(&orig->rsrc_table_offset);
    printf("RESOURCES TABLE ENTRIES\t");
    dwinfo(&orig->rsrc_table_entries);
    printf("RESIDENT NAMES TABLE AT\t");
    dwinfo(&orig->resident_names_table_offset);
    printf("ENTRY TABLE OFFSET\t");
    dwinfo(&orig->entry_table_offset);
    printf("MODULE DIRECTION OFFSET\t");
    dwinfo(&orig->module_direct_offset);
    printf("MODULE DIRECT ENTRIES\t");
    dwinfo(&orig->module_direct_entries);
    printf("FIX PAGE SECTION OFFSET\t");
    dwinfo(&orig->fix_page_offset);
    printf("FIXUP RECORDS OFFSET\t");
    dwinfo(&orig->fix_records_offset);
    printf("IMPORTS OFFSET\t\t");
    dwinfo(&orig->imported_names_table_offset);
    printf("IMPORTS COUNT VALUE\t");
    dwinfo(&orig->imports_count);
    printf("IMPORT FUNCTIONS OFFSET\t");
    dwinfo(&orig->import_procs_offset);
    printf("PER-PAGE CHECK OFFSET\t");
    dwinfo(&orig->perpage_check_offset);
    printf("DATA PAGES OFFSET\t");
    dwinfo(&orig->data_pages_offset);
    printf("PRELOAD PAGES COUNT\t");
    dwinfo(&orig->preload_pages_count);
    printf("NONRESIDENT NAMES TABLE\t");
    dwinfo(&orig->nonresident_names_table_offset);
    printf("NONRESIDENT NAMES CHECK\t");
    dwinfo(&orig->nonresident_names_check);
    printf("AUTOMATIC DATA-OBJ IS\t");
    dwinfo(&orig->automatic_data_object);
    printf("DEBUG INFO OFFSET\t");
    dwinfo(&orig->dbg_information_offset);
    printf("DEBUG INFO LENGTH\t");
    dwinfo(&orig->dbg_information_len);
    printf("PRELOAD INSTANCES COUNT\t");
    dwinfo(&orig->preload_instance_pages_count);
    printf("DEMAND INST PAGES COUNT\t");
    dwinfo(&orig->demand_instance_pages_count);
    printf("HEAP INITAL VALUE\t");
    dwinfo(&orig->heap);
    printf("STACK INITAL VALUE\t");
    dwinfo(&orig->stack);
    printf("RESERVED[0]\t\t");
    binfo(&orig->reserved[0]);
}

void printvxd(struct VXD_HEADER *le)
{
    printf("----------------------------\n");
    printf("Virtual Device Driver TABLE\n");
    printf("----------------------------\n");

    printf("WINDOWS  RESOURCES  OFFSET\t");
    dwinfo(&le->win_resoff);
    printf("WINDOWS  RESOURCES  LENGTH\t");
    dwinfo(&le->win_reslen);
    printf("DRIVER DEVELOPMENT  KIT ID\t");
    winfo(&le->dev_id);
    printf("DRIVER DEVELOPMENT VERSION\t");
    winfo(&le->ddk_version);
}

void printsig(__uint16_t header)
{
    
    printf("Executable file signature %c%c (hex=0x%x) \n", (char)header, (char)(header >> 8), header);

    switch (header)
    {
    case LE_SIGNATURE_INTEL_SEGMENT:
    case LE_SIGNATURE_ARM_SEGMENT:
        printf("Microsoft Windows or IBM OS/2 \"Linear Executable\".\n");
        break;
    case LX_SIGNATURE_INTEL_SEGMENT:
    case LX_SIGNATURE_ARM_SEGMENT:
        printf("IBM OS/2 or eComStation \"Linear Executable\"\n");
        break;
    case PE_SIGNATURE_INTEL_SEGMENT:
    case PE_SIGNATURE_ARM_SEGMENT:
        printf("Microsoft Windows NT \"Portable Executable\".\n");
        break;
    case NE_SIGNATURE_INTEL_SEGMENT:
    case NE_SIGNATURE_ARM_SEGMENT:
        printf("Microsoft Windows or Microsoft DOS \"New Executable\".\n");
        break;
    default:
        printf("Unknown Header of Executable.\n");
        break;
    }
    
}

/* 
 * OS from NE FILE HEADER
 */
void osnefh(__uint8_t *os)
{
    printf("Runs at ");
    switch (*os)
    {
    case __NE_DOS:
        printf("DOS\n");
        break;
    case __NE_OS2:
        printf("OS/2\n");
        break;
    case __NE_WIN:
        printf("Windows\n");
        break;
    default:
        printf("unknown Operating System\n");
        break;
    }
}

/*
 * OS FROM LX FILE HEADER
 */
void oslxfh(__uint16_t *os) 
{
    printf("Runs at ");
    switch (*os)
    {
    case __LE_DOS:
        printf("DOS\n");
        break;
    case __LE_OS2:
        printf("OS/2\n");
        break;
    case __LE_WIN:
        printf("Windows\n");
        break;
    case __LE_386:
        printf("Windows 386\n");
        break;
    default:
        printf("unknown Operating System\n");
        break;
    }
}

/*
 * CPU NE file header
 */
void cpunefh(__uint16_t *cpu) 
{
    printf("Built for ");
    switch (*cpu)
    {
    case __NE_FLAGS_286:
        printf("Intel 80286");
        break;
    case __NE_FLAGS_386:
        printf("Intel 80386");
        break;
    case __NE_FLAGS_8086:
        printf("Intel 8086");
        break;
    default:
        printf("unknown");
        break;
    }
    printf(" CPU architecture\n");

}

/*
 * CPU LX/LE file header
 */
void cpulxfh(__uint16_t* cpu)
{
    printf("Built for ");
    switch (*cpu)
    {
    case __LE_CPU_286:
        printf("Intel 80286");
        break;
    case __LE_CPU_386:
        printf("Intel 80386");
        break;
    case __LE_CPU_486:
        printf("Intel 80486");
        break;
    case __LE_CPU_586:
        printf("Intel 80586");
        break;
    case __LE_CPU_R20:
        printf("MIPS R2000");
        break;
    case __LE_CPU_R40:
        printf("MIPS R4000");
        break;
    case __LE_CPU_R60:
        printf("MIPS R6000");
        break;
    default:
        printf("unknown");
        break;
    }
    printf(" CPU architecture\n");
}

/* Ищет и выводит флаги для процессора */
void osmode(__uint8_t *flag)
{
    if (*flag == NE_FLAGS_PROTECTED_MODE)
        printf("Application runs only in PROTECTED MODE.\n");
}
