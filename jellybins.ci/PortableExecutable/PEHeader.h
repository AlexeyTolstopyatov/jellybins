//
// Created by MagicBook on 30.09.2024.
//

#ifndef JBC_PEHEADER_H
#define JBC_PEHEADER_H

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
    uint8_t  machine;
    uint8_t  sections_count;
    uint16_t time_stamp;
    uint16_t symtable_ptr;
    uint16_t symbols_count;
    uint8_t  sizeof_optional_header;
    uint8_t  characteristics;
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
    uint8_t  signature;
    uint8_t  maj_linkv;
    uint8_t  min_linkv;
    uint16_t sizeof_code;
    uint16_t sizeof_init;
    uint16_t sizeof_uninit;
    uint16_t entry_point_address;
    uint16_t code;
    uint16_t data;
    uint16_t image;
    uint16_t sect_alignment;
    uint16_t file_alignment;
    uint8_t  maj_os_version;
    uint8_t  min_os_version;
    uint8_t  maj_subenv_version;
    uint8_t  min_subenv_version;
    uint16_t win32_version;
    uint16_t sizeof_image;
    uint16_t sizeof_headers;
    uint16_t checksum;
    uint8_t  sub_environment;
    uint8_t  dll_characteristics;
    uint16_t sizeof_stack_reserve;
    uint16_t sizeof_stack_commit;
    uint16_t sizeof_heap_reserve;
    uint16_t sizeof_heap_commit;
    uint16_t loader_flags;
    /* .data catalog struct */
};

// Для заполнения свойств используется это.
struct WINNT_HEADER{
    uint32_t signature;
    struct PE_HEADER winnt_main;
    struct OPTIONAL_PE_HEADER winnt_optional;
};

#endif //JBC_PEHEADER_H
