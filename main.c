#include "framework.h"

/*
 *          JellyBins Console Interface
 * Автор: Толстопятов Алексей А. (https://github.com/AlexeyTolstopyatov)
 * Стандарт: C99
 * Сборщик: CLang
 * Компановщик: GNU Linker
 * Система управления заголовками: CMake
 */

int main(int argc, char* argv[])
{
    printf("JellyBins (C) Tolstopyatov Alexey 2024\n");
    struct MZ_HEADER exec;
    
    if (argc != 3)
    {
        printf("\n[a] key Prints table of segments and analyse result\n");
        printf("[p] key Prints only analyse result\n");
        printf("execmap [path\\file.exe] [key]\n\n");
        printf("-1. (Incorrect syntax).\n\n");
        return -1;
    }
    FILE* file = fopen(argv[1], "rb");
    printf("\n");
    if (file == NULL)
    {
        fclose(file);
        printf("-2. (Not found).\n");
        return -2;
    }

    fread(&exec, sizeof(struct MZ_HEADER), 1, file);

    if (exec.mz_signature != MZ_SIGNATURE_INTEL_SEGMENT)
    {
        printf("-3 (File doesn't have signature [MZ]).\n");
        return -3;
    }
    /* Режим подробной диагностики. */
    if (*argv[2] == 'a' || *argv[2] == 't')
        printmz(&exec);
    
    nulloffset();

    fseek(file, exec.next_signature, 0); /*может быть любая подпись*/
    
    __uint16_t signature;
    fread(&signature, sizeof(signature), 1, file); /*signature подпись заголовка*/

    switch (signature)
    {
    case PE_SIGNATURE_ARM_SEGMENT:
    case PE_SIGNATURE_INTEL_SEGMENT:
    {
        struct WINNT_HEADER nt;

        fseek(file, exec.next_signature, 0);
        fread(&nt, sizeof(struct WINNT_HEADER), 1, file);
        
        if (*argv[2] == 'a' || *argv[2] == 't')
        {
            printpe(&nt.winnt_main);
            printope(&nt.winnt_optional);
        }
        break;
    }
    case NE_SIGNATURE_ARM_SEGMENT:
    case NE_SIGNATURE_INTEL_SEGMENT:
    {
        struct NE_HEADER ne;

        fseek(file, exec.next_signature, 0);
        fread(&ne, sizeof(struct NE_HEADER), 1, file);
        
        if (*argv[2] == 'a' || *argv[2] == 't')
            printne(&ne);

        if (*argv[2] == 'p' || *argv[2] == 'a') 
        {
            osnefh(&ne.os);
            cpunefh(&ne.flag);
            osmode(&ne.oflag);
        }
        break;
    }
    
    
    case LX_SIGNATURE_ARM_SEGMENT:
    case LX_SIGNATURE_INTEL_SEGMENT:
    {
        struct LX_HEADER lx;
        fseek(file, exec.next_signature, 0);
        fread(&lx, sizeof(struct LX_HEADER), 1, file);

        if (*argv[2] == 'a' || 
            *argv[2] == 't')
            printlx(&lx);
        
        if (*argv[2] == 'p' || 
            *argv[2] == 'a') 
        {
            oslxfh(&lx.os);
            cpulxfh(&lx.cpu);
        }

        break;
    }
    case LE_SIGNATURE_ARM_SEGMENT:
    case LE_SIGNATURE_INTEL_SEGMENT:
    {
        struct LX_HEADER lx;
        fseek(file, exec.next_signature, 0);
        fread(&lx, sizeof(struct LX_HEADER), 1, file);

        if (*argv[2] == 'a' ||
            *argv[2] == 't')
            printlx(&lx);
        
        if (*argv[2] == 'a' ||
            *argv[2] == 'p')
        {
            oslxfh(&lx.os);
            cpulxfh(&lx.cpu);
        }
        break;
    }
    }
    
    nulloffset();
    printsig(signature);
    printf("\n");

    fclose(file);

    return 0;
}
