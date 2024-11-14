//
// Created by MagicBook on 30.09.2024.
//

#ifndef JBC_LEHEADER_H
#define JBC_LEHEADER_H
#include <stdint.h>

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
 * Поэтому есть uint8_t сегмент для определения ОС.
 */
struct LX_HEADER
{
    uint16_t signature;
    uint8_t  byte_order;
    uint8_t  word_order;
    uint32_t format;
    uint16_t cpu;
    uint16_t os;
    uint32_t module_version;
    uint32_t module_flags;
    uint32_t module_pages;
    uint32_t eip_object_num;
    uint32_t eip;
    uint32_t esp_object_num;
    uint32_t esp;
    uint32_t sizeof_page;
    uint32_t sizeof_last_page;
    uint32_t sizeof_fix_section;
    uint32_t sizeof_fix_section_check;
    uint32_t sizeof_loader_section;
    uint32_t sizeof_loader_section_check;
    uint32_t object_table_offset;
    uint32_t object_table_entries;
    uint32_t object_page_map_offset;
    uint32_t object_iterdata_offset;
    uint32_t rsrc_table_offset;
    uint32_t rsrc_table_entries;
    uint32_t resident_names_table_offset;
    uint32_t entry_table_offset;
    uint32_t module_direct_offset;
    uint32_t module_direct_entries;
    uint32_t fix_page_offset;
    uint32_t fix_records_offset;
    uint32_t imported_names_table_offset;
    uint32_t imports_count;
    uint32_t import_procs_offset;
    uint32_t perpage_check_offset;
    uint32_t data_pages_offset;
    uint32_t preload_pages_count;
    uint32_t nonresident_names_table_offset;
    uint32_t nonresident_names_check;
    uint32_t automatic_data_object;
    uint32_t dbg_information_offset;
    uint32_t dbg_information_len;
    uint32_t preload_instance_pages_count;
    uint32_t demand_instance_pages_count;
    uint32_t heap;
    uint32_t stack;
    struct VXD_HEADER
    {
        uint64_t win_resoff;
        uint64_t win_reslen;
        uint16_t dev_id;
        uint8_t ddk_version_major;
        uint8_t ddk_version_minor;
    } driver;
};


/*		Немного о "Virtual Device Driver Model"
 * 	Если я помню правильно, в линейке Microsoft Windows 9x
 * было определение драйвера виртуального устройства.
 *		Для совместного использования физических ресурсов виртуальными машинами,
 * Microsoft были введены драйверы виртуальных устройств (virtual device drivers).
 * Эти драйверы решали вопросы, связанные с конфликтами, возникающими при использовании физических ресурсов,
 * путем перехвата обращений к аппаратному обеспечению
 *
 * Вроде бы даже в Windows 3x были драйвера VxD.
 * vJOYd.386 (могли быть и vKEYBOARDd.386 vMOUSEd.386 и тд.)
 *
 * Структура заголовка для определения Драйвера Виртуального устройства
 */

#endif //JBC_LEHEADER_H
