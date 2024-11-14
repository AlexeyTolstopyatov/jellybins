#pragma once

/*
 *			ОПРЕДЕЛЕНИЯ ДЛЯ NEW EXECUTABLE HEADER
 *		Как у линейного исполняемого файла, у "Нового Исполняемого" типа файла
 * есть поля с требованиями к запускаемому приложению. 
 * Описаны и изменяться будут в этом файле.
 */

#ifndef PROGRAMFLAGS_H
#define PROGRAMFLAGS_H

#include <stdint.h>
#include <stdio.h>
#include "NEHeader.h"
#include "../Format/Table.h"

#define NEW_OS_NUL 0x0 /* Неизвестная ОС */
#define NEW_OS_OS2 0x1 /* IBM  OS/2 */
#define NEW_OS_WIN 0x2 /* MS-DOS 4x */
#define NEW_OS_DOS 0x3 /* MS-Windows */
#define NEW_OS_WIN386 0x4   /* MS-Windows 386 */
#define NEW_OS_BOSS 0x5     /* Borland Operating System Services */

#define NEW_CPU_i386 0x6 /* IA 80386 */
#define NEW_CPU_i286 0x5 /* IA 80286 */
#define NEW_CPU_8086 0x4 /* IA 8086  */
#define NEW_CPU_8087 0x7 /* IA 8087  */

/* Определения флагов для процессора. */
#define NE_FLAGS_PROTECTED_MODE 0x08    /* Чтение только в защищенном режиме */

enum NE_PROGRAM_FLAGS {
    ne_prog_datagrp = 0,
    ne_prog_datagrpnone = 0x01,
    ne_prog_datasshared = 0x02,
    ne_prog_datamshared = 0x03,
    ne_prog_globalinitl = 0x2,
    ne_prog_protectmode = 0x08,
};

enum NE_APPLICATION_FLAGS {
    ne_app_fullscrn = 0x01,
    ne_app_pmcompat = 0x02,
    ne_app_pmapiuse = 0x03,
    ne_app_loadcode = 0x08,
    ne_app_linkerrs = 0x20,
    ne_app_noncomfr = 0x40,
    ne_app_library = 0x80
};

enum NE_OS2LOADER_FLAGS {
    ne_os2_longnames = 0,
    ne_os2_protectmd = 0x1,
    ne_os2_propfonts = 0x2,
    ne_os2_egangload = 0x3
};

void ne_GetCpu(uint8_t cpu);
void ne_GetOperatingSystem(uint8_t os);
void ne_GetProgramFlags(uint8_t flags);
void ne_GetApplicationFlags(uint8_t flags);
void ne_GetOtherFlags(uint8_t flags);
void ne_GetVersion(uint8_t major, uint8_t minor);
void ne_Get(struct NE_HEADER *ne);

#endif