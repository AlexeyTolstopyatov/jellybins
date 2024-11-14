//
// Created by MagicBook on 11.11.2024.
//

#include "ModuleFlags.h"

/*
 * Вывод флага требуемого ЦП
 */
void le_GetCpu(const uint16_t* cpuf) {
    printf("Targeted CPU:\t");
    switch (*cpuf) {
        case LINEAR_CPU_286:
            printf("Intel i286\n");
            break;
        case LINEAR_CPU_386:
            printf("Intel i386\n");
            break;
        case LINEAR_CPU_486:
            printf("Intel i486\n");
            break;
        case LINEAR_CPU_586:
            printf("Intel i586\n");
            break;
        case LINEAR_CPU_R20:
            printf("MIPS R2000\n");
            break;
        case LINEAR_CPU_R40:
            printf("MIPS R4000\n");
            break;
        case LINEAR_CPU_R60:
            printf("MIPS R6000\n");
            break;
        default:
            printf("Unknown Target CPU\n");
            break;
    }
}

/*
 * Вывод флага требуемой ОС
 */
void le_GetOperatingSystem(const uint16_t* osf) {
    printf("Targeted OS:\t");
    switch (*osf) {
        case LINEAR_OS_DOS:
            printf("Microsoft DOS 4x\n");
            break;
        case LINEAR_OS_OS2:
            printf("IBM OS/2\n");
            break;
        case LINEAR_OS_WIN:
            printf("Microsoft Windows\n");
            break;
        case LINEAR_OS_WIN386:
            printf("Microsoft Windows/386\n");
            break;
        default:
            printf("Unknown OS");
            break;
    }
}

void le_GetPropertiesMark() {
    printf("-----------------\n");
    printf("Binary Properties\n");
    printf("-----------------\n");
}

/*
 * Вывод всех определений двоичного файла
 * из таблицы типов
 */
void le_GetModuleTypeFlags(const uint32_t *modf) {
    if ((*modf & le_executable) == 0)       printf("Binary type\tEXE\n");
    if ((*modf & le_library) != 0)          printf("Binary type\tDLL\n");
    if ((*modf & le_perpinit) != 0)         printf("Behaviour\tPer-process Initialization\n");
    if ((*modf & le_internalf) != 0)        printf("Structure\tInternal Fixes exists\n");
    if ((*modf & le_externalf) != 0)        printf("Structure\tExternal Fixes exists\n");
    if ((*modf & le_pmincompat) != 0)       printf("Windowing\tOS/2 PM Incompatible\n");
    if ((*modf & le_pmcompat) != 0)         printf("Windowing\tOS/2 PM Compatible\n");
    if ((*modf & le_pmapiused) != 0)        printf("Windowing\tOS/2 PM API Used\n");
    if ((*modf & le_notload) != 0)          printf("Behaviour\tNot loadable\n");
    if ((*modf & le_physdrv) != 0)          printf("Binary type\tPhysical Device Driver\n");
    if ((*modf & le_virtdrv) != 0)          printf("Binary type\tVirtual Device Driver\n");
    if ((*modf & le_perpterm) != 0)         printf("Behaviour\tPer-process Termination\n");
    if ((*modf & le_mcpuunsafe) != 0)       printf("Behaviour\tMulti-CPU Unsafe");
}

/*
 * Версия драйвера виртуального устройства
 * предположительно: зависит от версии Windows DDK
 * с помощью которого его собрали.
 *
 * Совместимость: драйвера собранные
 * с более старшей версией DDK
 * запускаются на ОС более младшей версии
 * Не совместимость: наоброт.
 */
void le_GetVersion(const uint8_t* major, const uint8_t* minor){
    printf("OS Version\t%d.%d\n", *minor, *major);
}

void le_GetVDDHeader(struct VXD_HEADER* vxd){
    printf("----------------------------\n");
    printf("Virtual Device Driver TABLE\n");
    printf("----------------------------\n");

    printf("WINDOWS RESOURCES OFFSET\t");
    getu8r(&vxd->win_resoff);
    printf("WINDOWS RESOURCES LENGTH\t");
    getu8r(&vxd->win_reslen);
    printf("DRIVER DEVELOPMENT KIT ID\t");
    getu2r(&vxd->dev_id);
    printf("DRIVER MAJOR VERSION\t\t");
    getu1r(&vxd->ddk_version_major);
    printf("DRIVER MINOR VERSION\t\t");
    getu1r(&vxd->ddk_version_minor);
}


void le_Get(struct LX_HEADER *orig){
    printf("LE HEADER TABLE\n");
    printf("----------------\n");

    printf("SIGNATURE\t\t");
    getu2r(&orig->signature);
    printf("[BYTE] ORDER\t\t");
    getu1r(&orig->byte_order);
    printf("[WORD] ORDER\t\t");
    getu1r(&orig->word_order);
    printf("FORMAT\t\t\t");
    getu4r(&orig->format);
    printf("CPU TYPE\t\t");
    getu2r(&orig->cpu);
    printf("OS TYPE\t\t\t");
    getu2r(&orig->os);
    printf("MODULE VERSION\t\t");
    getu4r(&orig->module_version);
    printf("MODULE FLAGS\t\t");
    getu4r(&orig->module_flags);
    printf("MODULE PAGES\t\t");
    getu4r(&orig->module_pages);
    printf("IP OBJECTS\t\t");
    getu4r(&orig->eip_object_num);
    printf("IP INIT VALUE\t\t");
    getu4r(&orig->eip);
    printf("SP OBJECTS\t\t");
    getu4r(&orig->esp_object_num);
    printf("SP INIT VALUE\t\t");
    getu4r(&orig->esp);
    printf("SIZE OF PAGE\t\t");
    getu4r(&orig->sizeof_page);
    printf("SIZE OF LAST PAGE\t");
    getu4r(&orig->sizeof_last_page);
    printf("SIZE OF FIX SECTION\t");
    getu4r(&orig->sizeof_fix_section);
    printf("SIZE OF FIX CHECKSUM\t");
    getu4r(&orig->sizeof_fix_section_check);
    printf("SIZE OF LOADER SECTION\t");
    getu4r(&orig->sizeof_loader_section);
    printf("SIZE OF LOADER CHECK\t");
    getu4r(&orig->sizeof_loader_section_check);
    printf("OBJECT TABLE OFFSET\t");
    getu4r(&orig->object_table_offset);
    printf("OBJECT TABLE ENTRIES\t");
    getu4r(&orig->object_table_entries);
    printf("OBJECT PAGE MAP OFFSET\t");
    getu4r(&orig->object_page_map_offset);
    printf("OBJECT ITER-DATA OFFSET\t");
    getu4r(&orig->object_iterdata_offset);
    printf("RESOURCES TABLE OFFSET\t");
    getu4r(&orig->rsrc_table_offset);
    printf("RESOURCES TABLE ENTRIES\t");
    getu4r(&orig->rsrc_table_entries);
    printf("RESIDENT NAMES TABLE AT\t");
    getu4r(&orig->resident_names_table_offset);
    printf("ENTRY TABLE OFFSET\t");
    getu4r(&orig->entry_table_offset);
    printf("MODULE DIRECTION OFFSET\t");
    getu4r(&orig->module_direct_offset);
    printf("MODULE DIRECT ENTRIES\t");
    getu4r(&orig->module_direct_entries);
    printf("FIX PAGE SECTION OFFSET\t");
    getu4r(&orig->fix_page_offset);
    printf("FIXUP RECORDS OFFSET\t");
    getu4r(&orig->fix_records_offset);
    printf("IMPORTS OFFSET\t\t");
    getu4r(&orig->imported_names_table_offset);
    printf("IMPORTS COUNT VALUE\t");
    getu4r(&orig->imports_count);
    printf("IMPORT FUNCTIONS OFFSET\t");
    getu4r(&orig->import_procs_offset);
    printf("PER-PAGE CHECK OFFSET\t");
    getu4r(&orig->perpage_check_offset);
    printf("DATA PAGES OFFSET\t");
    getu4r(&orig->data_pages_offset);
    printf("PRELOAD PAGES COUNT\t");
    getu4r(&orig->preload_pages_count);
    printf("NONRESIDENT NAMES TABLE\t");
    getu4r(&orig->nonresident_names_table_offset);
    printf("NONRESIDENT NAMES CHECK\t");
    getu4r(&orig->nonresident_names_check);
    printf("AUTOMATIC DATA-OBJ IS\t");
    getu4r(&orig->automatic_data_object);
    printf("DEBUG INFO OFFSET\t");
    getu4r(&orig->dbg_information_offset);
    printf("DEBUG INFO LENGTH\t");
    getu4r(&orig->dbg_information_len);
    printf("PRELOAD INSTANCES COUNT\t");
    getu4r(&orig->preload_instance_pages_count);
    printf("DEMAND INST PAGES COUNT\t");
    getu4r(&orig->demand_instance_pages_count);
    printf("HEAP INITAL VALUE\t");
    getu4r(&orig->heap);
    printf("STACK INITAL VALUE\t");
    getu4r(&orig->stack);
}
