//
// Created by MagicBook on 30.09.2024.
//

#ifndef JBC_NEHEADER_H
#define JBC_NEHEADER_H

#include <stdint.h>

/*
 * New Executable Заголовок
 * Новый тип структуры данных для исполняемых файлов
 * Использовался в .EXE файлах собранных для MS-DOS и MS-Windows 3x
 *		Подпись
 *		Версия
 *		РЕвизия
 *		Смещение до таблицы сущностей
 *		Контрольная сумма
 *		Автоматически заполняемый сегмент (че...)
 *		Изначальное распределение Кучи
 *		Изначальное распределение Стэка
 *		Изначальные значения в CS:IP
 *		Изначальные значения в SS:SP
 *		Количество сегментов
 *		"Module Reference Table" Сущности модульно-ссылочной таблицы
 *		Размер таблицы имен (не резидентов?)
 *		Смещение таблицы сегментов
 *		Смещение таблицы ресурсов
 *		Смещение таблицы имен резидентов
 *		Смещение модульно-ссылочной таблицы
 * ёмаё Смещение таблицы импортов
 *		Выравнивание
 *		Количество сегментов ресурсов
 *		Операционная система (вероятно, под которой или для которой собиралось приложение)
 *		Другие флаги для приложения
 *		Другие возвращаемые сущности
 *		Смещение до возвращаемых значений
 *		Минимальный размер сегмента кода
 *		Ожидаемая версия Операционной системы
 */
struct NE_HEADER
{
    uint16_t signature;
    uint8_t  version;
    uint8_t  revision;
    uint16_t entry_table_offset;
    uint16_t sizeof_entry_table;
    uint32_t checksum;
    uint8_t progflags;
    uint8_t appflags;
    uint16_t auto_data_segment;
    uint16_t heap;
    uint16_t stack;
    uint32_t csip; /*Code Segment:IP*/
    uint32_t sssp; /*Source Segment?:Source Page*/
    uint16_t segments_count;
    uint16_t module_reft_entries;
    uint16_t sizeof_name_table;
    uint16_t segment_table_offset;
    uint16_t resources_table_offset;
    uint16_t resident_name_table_offset;
    uint16_t module_reference_table_offset;
    uint16_t imports_table_offset;
    uint32_t nonresident_name_table_offset;
    uint16_t mov_entries_count;
    uint16_t align;
    uint16_t resource_segments_count;
    uint8_t  os; /*target*/
    uint8_t  oflag; /*another flags*/
    uint16_t ret_entries_count;
    uint16_t refbytes_segment_offset; /*???*/
    uint16_t sizeof_codearea;
    uint8_t  major;
    uint8_t  minor;
};

#endif //JBC_NEHEADER_H
