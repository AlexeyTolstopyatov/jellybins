#include "printing.h"



/*
 * Выводит таблицу содержания MZ заголовка.
 */
void printmz(struct MZ_HEADER *exec)
{
    printf("MZ HEADER TABLE\n");
    printf("----------------\n");

    printf("MZ SIGNATURE   \t");
    getu2r(&exec->e_sign);
    printf("LAST BLOCK SIZE\t");
    getu2r(&exec->e_lb);
    printf("BLOCKS COUNT\t");
    getu2r(&exec->e_flb);
    printf("RELOCATIONS\t");
    getu2r(&exec->e_relc);
    printf("PARAGRAPHS (p)\t");
    getu2r(&exec->e_pars);
    printf("min(extra_p) \t");
    getu2r(&exec->e_minep);
    printf("max(extra_p) \t");
    getu2r(&exec->e_maxep);
    printf("init(SS)  \t");
    getu2r(&exec->e_ss);
    printf("init(SP)  \t");
    getu2r(&exec->e_sp);
    printf("Checksum  \t");
    getu2r(&exec->e_ch);
    printf("init(CS)  \t");
    getu2r(&exec->e_cs);
    printf("init(IP)  \t");
    getu2r(&exec->e_ip);
    printf("RELOC OFFSET  \t");
    getu2r(&exec->e_relc);
    printf("OVERLAY NUMBER\t");
    getu2r(&exec->e_on);
    printf("SECURED 0x1C[0]\t");
    getu2r(&exec->e_res0x1c[0]);
    printf("OEM ID\t\t");
    getu2r(&exec->e_oemid);
    printf("OEM INFORMATION\t");
    getu2r(&exec->e_oeminfo);
    printf("SECURED 0x28[0]\t");
    getu2r(&exec->e_res0x28[0]);
    printf("NEXT HEAD AT\t");
    getu2r(&exec->e_lfanew);
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
    getu1r(&exec->machine);
    printf("SECTIONS \t\t");
    getu1r(&exec->sections_count);
    printf("TIMESTAMP\t\t");
    getu2r(&exec->time_stamp);
    printf("TABLE POINTER\t\t");
    getu2r(&exec->symtable_ptr);
    printf("CHARS COUNT\t\t");
    getu2r(&exec->symbols_count);
    printf("OPT HEADER \t\t");
    getu1r(&exec->sizeof_optional_header);
    printf("CHARACTERS \t\t");
    getu1r(&exec->characteristics);
}

/*
 * Выводит таблицу содержания опционального PE заголовка
 */
void printope(struct OPTIONAL_PE_HEADER *ope)
{
    printf("OPTIONAL SIGNATURE\t");
    getu1r(&ope->signature);
    printf("MAJOR LINKER VERSION\t");
    getu1r(&ope->maj_linkv);
    printf("MINOR LINKER VERSION\t");
    getu1r(&ope->min_linkv);
    printf("SIZE OF CODE SEGMENT\t");
    getu2r(&ope->sizeof_code);
    printf("SIZE OF INIT SEGMENT\t");
    getu2r(&ope->sizeof_init);
    printf("SIZE OF UNIT SEGMENT\t");
    getu2r(&ope->sizeof_uninit);
    printf("ENTRY POINT LOCATION\t");
    getu2r(&ope->entry_point_address);
    printf(".text  EXEC  SECTION\t");
    getu2r(&ope->code);
    printf(".data  EXEC  SECTION\t");
    getu2r(&ope->data);
    printf("IMAGE  HEADER  VALUE\t");
    getu2r(&ope->image);
    printf("SECTION    ALIGNMENT\t");
    getu2r(&ope->sect_alignment);
    printf("FILE       ALIGNMENT\t");
    getu2r(&ope->file_alignment);
    printf("MAJOR   OS   VERSION\t");
    getu1r(&ope->maj_os_version);
    printf("MINOR   OS   VERSION\t");
    getu1r(&ope->min_os_version);
    printf("MAJOR  SUBSUSYEM VER\t");
    getu1r(&ope->maj_subenv_version);
    printf("MINOR  SUBSYSTEM VER\t");
    getu1r(&ope->min_subenv_version);
    printf("WIN-32 EXTENTION VER\t");
    getu2r(&ope->win32_version);
    printf("SIZE OF HEADER IMAGE\t");
    getu2r(&ope->sizeof_image);
    printf("SIZE OF HEADERS\t\t");
    getu2r(&ope->sizeof_headers);
    printf("CHECKSUM\t\t");
    getu2r(&ope->checksum);
    printf("SUBSYSTEM TARGET\t");
    getu1r(&ope->sub_environment);
    printf("DLL  CHARACTERISTICS\t");
    getu1r(&ope->dll_characteristics);
    printf("SIZE OF STACK COMMIT\t");
    getu2r(&ope->sizeof_stack_commit);
    printf("SIZE OF STACK RESERV\t");
    getu2r(&ope->sizeof_stack_reserve);
    printf("SIZE OF HEAP  COMMIT\t");
    getu2r(&ope->sizeof_heap_commit);
    printf("SIZE OF HEAP RESERVE\t");
    getu2r(&ope->sizeof_heap_reserve);
    printf("LOADER\tFLAGS\t\t");
    getu2r(&ope->loader_flags);
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