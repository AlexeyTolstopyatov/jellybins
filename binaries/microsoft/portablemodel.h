//
// Created by MagicBook on 30.09.2024.
//

#ifndef JBC_PORTABLEMODEL_H
#define JBC_PORTABLEMODEL_H

#include "../../retyping.h"

/*
 * Заголовок "Portable Executable"
 *		Тип архитектуры (как я понимаю)
 *		Колличество секций (.data .rdata .text .src .rcrs)
 *
 *		Указатель на таблицу символов
 *		Колличество символов
 *		Размер (не обязательного?) опционального заголовка
 *		Характеристики
 */
struct PE_HEADER
{
    __uint8_t  machine;
    __uint8_t  sections_count;
    __uint16_t time_stamp;
    __uint16_t symtable_ptr;
    __uint16_t symbols_count;
    __uint8_t  sizeof_optional_header;
    __uint8_t  characteristics;
};

/*
 * НЕобязательный PE заголовок
 *		Подпись (PE32 или PE32+)
 *		Мажорная (или основная) Версия сборщика (линкера)
 *		Минорная (дополнительная) версия сборщика
 *		Размер кода
 *		Размер инициализированной части данных
 *		Размер не_инициализированной части данных
 *		Указатель на точку входа (main(...) _tmain(...) или др)
 *		код		(.code)
 *		данные	(.data)
 *		образ заголовка
 *		Выравнивание секций
 *		Выравнивание файлов
 *		Основная версия ОС
 *		Дополнительная версия ОС
 *		Основная версия подсистемы
 *		Дополнительная версия подсистемы
 *		Версия дополнения (расширения) Win32
 *		Размер образа (заголовка)
 *		Размер заголовка
 *		Контрольная сумма
 *		Подсистема
 *		Характеристики DLL (Динамически собранной библиотеки)
 *		Размер резерва стэка
 *		Размер запроса в стэк
 *		Размер резерва кучи
 *		Размер запроса в кучу
 *		Флаги загрузчика
 *		(дальше Бога нет... Дальше только следующая таблица)
 */
struct OPTIONAL_PE_HEADER
{
    __uint8_t  signature;
    __uint8_t  maj_linkv;
    __uint8_t  min_linkv;
    __uint16_t sizeof_code;
    __uint16_t sizeof_init;
    __uint16_t sizeof_uninit;
    __uint16_t entry_point_address;
    __uint16_t code;
    __uint16_t data;
    __uint16_t image;
    __uint16_t sect_alignment;
    __uint16_t file_alignment;
    __uint8_t  maj_os_version;
    __uint8_t  min_os_version;
    __uint8_t  maj_subenv_version;
    __uint8_t  min_subenv_version;
    __uint16_t win32_version;
    __uint16_t sizeof_image;
    __uint16_t sizeof_headers;
    __uint16_t checksum;
    __uint8_t  sub_environment;
    __uint8_t  dll_characteristics;
    __uint16_t sizeof_stack_reserve;
    __uint16_t sizeof_stack_commit;
    __uint16_t sizeof_heap_reserve;
    __uint16_t sizeof_heap_commit;
    __uint16_t loader_flags;
    /* .data catalog struct */
};

// Для заполнения свойств используется это.
struct WINNT_HEADER{
    struct PE_HEADER *winnt_main;
    struct OPTIONAL_PE_HEADER *winnt_optional;
};

#endif //JBC_PORTABLEMODEL_H
