#include "Table.h"

/*
 * getu1r = BYTE info (uint8_t)
 */
void getu1r(uint8_t* cell)
{
    t_offset += sizeof(*cell);

    printf("0x%.4x\t", *cell);
    printf("(offset=%zx,sizeof=%zx)\t__uint8_t\tBYTE\tunsigned char\n", t_offset, sizeof(*cell));
}

void getu4r(uint32_t* cell)
{
    t_offset += sizeof (*cell);
    printf("0x%.4x\t(offset=%x,sizeof=%zx)\t__uint32_t\tDWORD\tunsigned int\n", *cell, t_offset, sizeof(*cell));
}

/*
 * getu2r = WORD info (uint16_t)
 */
void getu2r(uint16_t* cell)
{
    t_offset += sizeof(*cell);
    printf("0x%.4lx\t(offset=%x,sizeof=%zx)\t__uint16_t\tWORD\tunsigned short\n", *cell, t_offset, sizeof(*cell));
}

/*
 * getu8r = DWORD (double WORD) info (uint64_t)
 */
void getu8r(uint64_t* cell)
{
    t_offset += sizeof(*cell);
    printf("0x%.4lx\t(offset=%x,sizeof=%zx)\t__uint64_t\tDWORD\tunsigned long\n", *cell, t_offset, sizeof(*cell));
}

/*
 * Сброс значения всех смещений сегментов
 */
void nulloffset()
{
    t_offset = 0;
}
