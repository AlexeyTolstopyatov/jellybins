//
// Created by MagicBook on 30.09.2024.
//

#ifndef JBC_MZHEADER_H
#define JBC_MZHEADER_H

//      (С) Толстопятов Алексей А. 2024
// Я хотел бы назвать этот файл базовым для Microsoft
// Поскольку именно с этого момента, что здесь указан
// начинается история еще больших предположений и
// проектных решений.
//
// Во времена DOS изначально были COM файлы (сокращенно Command)
// которые могли занимать только один сегмент Оперативной памяти
// (64 Кб)
// Исполняемые файлы (ориг. "Executables") могут весить неограниченное
// колличество байт. Но внутри такого файла обязательно хранится
// его оглавление. *.EXE файл можно понимать как книгу, в которой есть
// содержание, примечание, главы/разделы.
//
// DOS исполняемые файлы все создаются с их оглавлением
// называется - MZ-header
//

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
 *		Зарезервированно под 0x1C (8 байт или QWORD или uint64_t)
 *		OEM Идентификатор (OEM -- Оригинальный производитель оборудования)
 *		Информация о Производителе
 *		Разерзервированно под 0x28
 *		Адрес следующего заголовка (или E_LFANEW)
 */
struct MZ_HEADER
{
    uint16_t e_sign;
    uint16_t e_lb;
    uint16_t e_flb;
    uint16_t e_relc;
    uint16_t e_pars;
    uint16_t e_minep;
    uint16_t e_maxep;
    uint16_t e_ss;
    uint16_t e_sp;
    uint16_t e_ch;
    uint16_t e_ip;
    uint16_t e_cs;
    uint16_t e_relto;
    uint16_t e_on;
    uint16_t e_res0x1c[4];
    uint16_t e_oemid;
    uint16_t e_oeminfo;
    uint16_t e_res0x28[10];
    // e_lfanew в оригинале назывался
    // "e_lfanew"
    // что предположительно
    // executable_long address ....
    uint16_t e_lfanew;
};



#endif //JBC_MZHEADER_H