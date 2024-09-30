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



