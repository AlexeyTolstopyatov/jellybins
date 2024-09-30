//
// Created by MagicBook on 30.09.2024.
//

#ifndef JBC_VDDMODEL_H
#define JBC_VDDMODEL_H

#include "../../retyping.h"

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

#endif //JBC_VDDMODEL_H
