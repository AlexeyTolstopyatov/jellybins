//
// Created by MagicBook on 30.09.2024.
//

#ifndef JBC_NEWMODEL_H
#define JBC_NEWMODEL_H

#include "retyping.h"

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
    __uint16_t signature;
    __uint8_t  version;
    __uint8_t  revision;
    __uint16_t entry_table_offset;
    __uint16_t sizeof_entry_table;
    __uint64_t checksum;
    __uint16_t flag;
    __uint16_t auto_data_segment;
    __uint16_t heap;
    __uint16_t stack;
    __uint64_t csip; /*Code Segment:IP*/
    __uint64_t sssp; /*Source Segment?:Source Page*/
    __uint16_t segments_count;
    __uint16_t module_reft_entries;
    __uint16_t sizeof_name_table;
    __uint16_t segment_table_offset;
    __uint16_t resources_table_offset;
    __uint16_t resident_name_table_offset;
    __uint16_t module_reference_table_offset;
    __uint16_t imports_table_offset;
    __uint64_t nonresident_name_table_offset;
    __uint16_t mov_entries_count;
    __uint16_t align;
    __uint16_t resource_segments_count;
    __uint8_t  os; /*target*/
    __uint8_t  oflag; /*another flags*/
    __uint16_t ret_entries_count;
    __uint16_t refbytes_segment_offset; /*???*/
    __uint16_t sizeof_codearea;
    __uint16_t expected_os;
};

#endif //JBC_NEWMODEL_H
