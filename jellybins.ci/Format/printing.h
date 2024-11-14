#pragma once
#include "Table.h"

#include "../PortableExecutable/PEHeader.h"
#include "../NewExecutable/NEHeader.h"
#include "../LinearExecutable/LEHeader.h"
#include "../MarkExecutable/MZHeader.h"

/*		(С) Толстопятов Алексей
 * Содержание "Printing.h" -- функции, выводящие информацию о элементе.
 * Используются для формирования таблицы заголовка
 * 
 */

void printsig(uint16_t header);
void printmz(struct MZ_HEADER *exec);
void printpe(struct PE_HEADER *exec);
void printope(struct OPTIONAL_PE_HEADER *ope);
void printlx(struct LX_HEADER *orig);

#include "../LinearExecutable/ModuleFlags.h"
#include "../NewExecutable/ProgramFlags.h"
#include "../PortableExecutable/Characteristics.h"
#include "../LinearExecutable/ModuleFlags.h"

void osnefh(uint8_t *os);
void oslxfh(uint16_t *os);
void cpunefh(uint16_t *cpu);
void cpulxfh(uint16_t *cpu);
void osmode(uint8_t *mode);
void ospefh(uint8_t *os);
void osverpefh(uint8_t *ver);
void cpupefx(uint8_t *cpu);
