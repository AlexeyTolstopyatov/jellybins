#pragma once
#include "typeinfo.h"

#include "../binaries/mapping.h"
#include "../binaries/microsoft/portablemodel.h"
#include "../binaries/microsoft/newmodel.h"
#include "../binaries/ibm/linearmodel.h"
#include "../binaries/microsoft/mzmodel.h"

/*		(С) Толстопятов Алексей
 * Содержание "Printing.h" -- функции, выводящие информацию о элементе.
 * Используются для формирования таблицы заголовка
 * 
 */

void printsig(__uint16_t header);
void printmz(struct MZ_HEADER *exec);
void printpe(struct PE_HEADER *exec);
void printope(struct OPTIONAL_PE_HEADER *ope);
void printne(struct NE_HEADER *exec);
void printvxd(struct VXD_HEADER *exec);
void printlx(struct LX_HEADER *orig);

#include "../binaries/ibm/linearflags.h"
#include "../binaries/microsoft/newflags.h"
#include "../binaries/microsoft/portableflags.h"
#include "../binaries/ibm/linearflags.h"

void osnefh(__uint8_t *os);
void oslxfh(__uint16_t *os);
void cpunefh(__uint16_t *cpu);
void cpulxfh(__uint16_t *cpu);
void osmode(__uint8_t *mode);
void ospefh(__uint8_t *os);
void osverpefh(__uint8_t *ver);
void cpupefx(__uint8_t *cpu);
