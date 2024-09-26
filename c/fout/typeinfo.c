#include "typeinfo.h"

/*
 * binfo = BYTE info (__uint8_t)
 */
void binfo(__uint8_t *cell)
{
    t_offset += sizeof(*cell);

    printf("0x%x\t", *cell);
    printf("(offset=%zx,sizeof=%zx)\t__uint8_t\tBYTE\tunsigned char\n", t_offset, sizeof(*cell));
}

/*
 * winfo = WORD info (__uint16_t)
 */
void winfo(__uint16_t *cell)
{
    t_offset += sizeof(*cell);
    printf("0x%lx\t(offset=%x,sizeof=%zx)\t__uint16_t\tWORD\tunsigned short\n", *cell, t_offset, sizeof(*cell));
}

/*
 * dwinfo = DWORD (double WORD) info (__uint64_t)
 */
void dwinfo(__uint64_t* cell)
{
    t_offset += sizeof(*cell);
    printf("0x%lx\t(offset=%x,sizeof=%zx)\t__uint64_t\tDWORD\tunsigned long\n", *cell, t_offset, sizeof(*cell));
}

/*
 * Сброс значения всех смещений сегментов
 */
void nulloffset()
{
    t_offset = 0;
}
