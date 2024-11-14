#pragma once

#ifndef LINEAREXEC_DEFINITIONS_H

#define LINEAREXEC_DEFINITIONS_H

#include <stdint.h>
#include <stdio.h>

#include "LEHeader.h"
#include "../Format/Table.h"

/*			ОПРЕДЕЛЕНИЯ LINEAR EXECUTABLE ИНФОРМАЦИОННОГО ЗАГОЛОВКА
 *		Внутри LE/LX Заголовка находится информация о требованиях к программе,
 * запускаемой на машине.
 * 
 * Операционная система, требуемая архитектура, ...?
 */

#define LENEAR_OS_NULL 0x00 /* Неизвестная (или новая) ОС */
#define LINEAR_OS_WIN 0x02 /* MS-Windows */
#define LINEAR_OS_OS2 0x01 /* IBM OS/2 */
#define LINEAR_OS_DOS 0x03 /* MS-DOS 4x */
#define LINEAR_OS_WIN386 0x04 /* MS-Windows 386 */

/*			ОПРЕДЕЛЕНИЯ АРХИТЕКТУРЫ ЦП
 * Не могу никак вспомнить и найти где же я читал, но есть значения других архитектур
 * Определения процессора которые я помню:
 */

#define LINEAR_CPU_286 0x01 /* IA 80286 (i286) */
#define LINEAR_CPU_386 0x02 /* IA 80386 (i386) */
#define LINEAR_CPU_486 0x03 /* IA 80486 (i486) */
#define LINEAR_CPU_586 0x04 /* IA 80586 (i586) */
#define LINEAR_CPU_R20 0x40 /* MIPS R2000 */
#define LINEAR_CPU_R60 0x41 /* MIPS R6000 */
#define LINEAR_CPU_R40 0x42 /* MIPS R4000 */

enum LE_MODULETYPE_FLAGS {
    le_executable = 0,
    le_library = 0x0008,
    le_perpinit = 0x0004,
    le_internalf = 0x0010,
    le_externalf = 0x0020,
    le_pmincompat = 0x0100,
    le_pmcompat = 0x0200,
    le_pmapiused = 0x0300,
    le_notload = 0x2000,
    le_physdrv = 0x20000,
    le_virtdrv = 0x28000,
    le_perpterm = 0x4000,
    le_mcpuunsafe = 0x80000
};

void le_GetCpu(const uint16_t *cpuf);
void le_GetOperatingSystem(const uint16_t *osf);
void le_GetModuleTypeFlags(const uint32_t *modf);
void le_Get(struct LX_HEADER *orig);
void le_GetVersion(const uint8_t *major, const uint8_t *minor);
void le_GetVDDHeader(struct VXD_HEADER *vxd);
void le_GetPropertiesMark(void);

#endif