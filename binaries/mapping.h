#pragma once
#include "../retyping.h"

/*
 * Толстопятов Алексей А.
 *                      *** О Архитектуре ***
 * 
 * Распозноваемые значения в сегменте подписи Исполняемого файла
 * Если я правильно помню, Исполняемые файлы, 
 * собранные под разные архитектуры могут отличаться заполнением сегментов.
 * Архитектуры
 *		ARM
 *		MIPS
 *		SPARC (в зависимости от ЦП)
 *		ITanium (intel)
 * Заполнение сегмента происходит "слева направо"
 *		0x0000
 *		  1234
 * Например __uint16_t (или DWORD) сегмент содержит 4 байта (4 нуля или 32 бита)
 *		0x0000 -> 0x1000 -> 0x1100 -> 0x0010 -> 0x1010 -> 0x1110 -> 0x0001
 */
#define LX_SIGNATURE_ARM_SEGMENT 0x4c58 /*'L' 'X'*/
#define LE_SIGNATURE_ARM_SEGMENT 0x4c45 /*'L' 'E'*/
#define NE_SIGNATURE_ARM_SEGMENT 0x4e45 /*'N' 'E'*/
#define PE_SIGNATURE_ARM_SEGMENT 0x5045 /*'P' 'E'*/
#define MZ_SIGNATURE_ARM_SEGMENT 0x4d5a /*'M' 'Z'*/ 
/*
 * Немного об Intel
 * 
 * Если я помню правильно, запись значений в ячейку памяти (например __uint16_t типа)
 * Происходит справа налево. То есть: 
 *		0x0000 (DWORD или __uint16_t) содержит 4 байта. (или 32 бита)
 *		  4321
 * Запись в сегмент будет так же как на схеме.
 * 0x0001 -> 0x0010 -> 0x0011 -> 0x0100 -> 0x0101 -> 0x0111 -> 0x1000
 * 
 */
#define MZ_SIGNATURE_INTEL_SEGMENT 0x5a4d /*'Z' 'M'*/
#define NE_SIGNATURE_INTEL_SEGMENT 0x454e /*'E' 'N'*/ 
#define PE_SIGNATURE_INTEL_SEGMENT 0x4550 /*'E' 'P'*/
#define LE_SIGNATURE_INTEL_SEGMENT 0x454c /*'E' 'L'*/ 
#define LX_SIGNATURE_INTEL_SEGMENT 0x584c /*'X' 'L'*/

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


/*
 * DOS-Заголовок или MZ заголовок
 * MZ это Mark Zbikowski (инженер Microsoft), предложивший модель
 * заголовка.
 *		Подпись MZ (или ZM)
 *		Размер последнего блока
 *		Блоки файла
 *		Колличество перемещений
 *		Абзацы
 *		Минимальное количество абзацев
 *		Максимальное количество абзацев
 *		Адрес Перемещаемого сегмента для регистра SS
 *		Изначальное значение сегмента для SP
 *		Контрольная сумма (больше не заполняется, может быть 0)
 *		Изначальное значение для IP
 *		Указатель на сегмент для CS регистра
 *		Таблица перемещений
 *		Номер дополнений (иногда приложение использует дополнительные данные, это для них)
 *		Зарезервированно под 0x1C (8 байт или QWORD или __uint64_t)
 *		OEM Идентификатор (OEM -- Оригинальный производитель оборудования)
 *		Информация о Производителе
 *		Разерзервированно под 0x28
 *		Адрес следующего заголовка (или E_LFANEW)
 */
struct MZ_HEADER 
{
	__uint16_t mz_signature;
	__uint16_t last_block_size;
	__uint16_t file_blocks;
	__uint16_t relocations_count;
	__uint16_t paragraphs;
	__uint16_t min_extra_paragraphs;
	__uint16_t max_extra_paragraphs;
	__uint16_t ss;
	__uint16_t sp;
	__uint16_t checksum;
	__uint16_t ip;
	__uint16_t cs;
	__uint16_t reloc_table_offset;
	__uint16_t overlay_number;
	__uint16_t secured_0x1c[4];
	__uint16_t oem_id;
	__uint16_t oem_info;
	__uint16_t secured_0x28[10];
	__uint16_t next_signature; /* e_lfanew (предположительно "long file address of New Executable"... но почему long???) */
};


struct NT_HEADER /*че за...*/
{
	__uint16_t pe_signature;
	struct PE_HEADER header;
	struct OPTIONAL_PE_HEADER optional_header;
};

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

/*
 *		Немного о Microsoft Windows 9x
 *	По скольку Виртуальные драйвера для Windows 3x/9x
 * используют LE заголовок, пусть информация о драйвере тоже
 * будет в LE заголовке.
 * 
 */


/*
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
	struct VXD_HEADER *driver;
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
struct VXD_HEADER 
{
	__uint64_t win_resoff;
	__uint64_t win_reslen;
	__uint16_t dev_id;
	__uint16_t ddk_version;
};