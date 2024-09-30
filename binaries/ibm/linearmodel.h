//
// Created by MagicBook on 30.09.2024.
//

#ifndef JBC_LINEARMODEL_H
#define JBC_LINEARMODEL_H

#include "../../retyping.h"

/*
 *		Немного о Microsoft Windows 9x
 *	По скольку Виртуальные драйвера для Windows 3x/9x
 * используют LE заголовок, пусть информация о драйвере тоже
 * будет в LE заголовке.
 *
 * LX заголовок был придуман раньше чем Microsoft Windows 95
 * И использовался только для IBM OS/2.
 *
 *		Если я помню понял, заголовки приложений OS/2 и VXD (или .386 драйвера)
 * отличаются. но чем????
 *		По идее, если приложения с LE/LX заголовком заводятся на Windows 98,
 * (или в DOS), то Windows 9x поддерживают выполнение и OS/2 приложений, только вот...
 * видимо API для OS/2 приложений нужно от OS/2.
 * Поэтому есть __uint8_t сегмент для определения ОС.
 */
struct LX_HEADER
{
    __uint16_t signature;
    __uint8_t  byte_order;
    __uint8_t  word_order;
    __uint64_t format;
    __uint16_t cpu;
    __uint16_t os;
    __uint64_t module_version;
    __uint64_t module_flags;
    __uint64_t module_pages;
    __uint64_t eip_object_num;
    __uint64_t eip;
    __uint64_t esp_object_num;
    __uint64_t esp;
    __uint64_t sizeof_page;
    __uint64_t sizeof_last_page;
    __uint64_t sizeof_fix_section;
    __uint64_t sizeof_fix_section_check;
    __uint64_t sizeof_loader_section;
    __uint64_t sizeof_loader_section_check;
    __uint64_t object_table_offset;
    __uint64_t object_table_entries;
    __uint64_t object_page_map_offset;
    __uint64_t object_iterdata_offset;
    __uint64_t rsrc_table_offset;
    __uint64_t rsrc_table_entries;
    __uint64_t resident_names_table_offset;
    __uint64_t entry_table_offset;
    __uint64_t module_direct_offset;
    __uint64_t module_direct_entries;
    __uint64_t fix_page_offset;
    __uint64_t fix_records_offset;
    __uint64_t imported_names_table_offset;
    __uint64_t imports_count;
    __uint64_t import_procs_offset;
    __uint64_t perpage_check_offset;
    __uint64_t data_pages_offset;
    __uint64_t preload_pages_count;
    __uint64_t nonresident_names_table_offset;
    __uint64_t nonresident_names_check;
    __uint64_t automatic_data_object;
    __uint64_t dbg_information_offset;
    __uint64_t dbg_information_len;
    __uint64_t preload_instance_pages_count;
    __uint64_t demand_instance_pages_count;
    __uint64_t heap;
    __uint64_t stack;
    __uint8_t  reserved[12];
};

#endif //JBC_LINEARMODEL_H
