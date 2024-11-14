#include "framework.h"

/*
 *          JellyBins Console Interface
 * Автор: Толстопятов Алексей А. (https://github.com/AlexeyTolstopyatov)
 * Стандарт: C99
 * Сборщик: CLang
 * Компановщик: GNU Linker
 * Система управления заголовками: CMakeList
 */

// TODO: Синтаксис хрень
// Переделать структуру проекта
// Переназвать функции

// TODO: Эксплуатация хрень
// Поменять [a/p/t] на нормисные ключи

int main(int argc, char* argv[])
{
    printf("\nJellyBins (C) Tolstopyatov Alexey 2024\n");

    char fname[] = "D:\\FUJI9.DRV";
    char key[1] = "a";

    argc = 3;
    argv[1] = fname;
    argv[2] = key;

    if (argc != 3)
    {
        printf("[a] key Prints table of segments and analyse result\n");
        printf("[p] key Prints only analyse result\n");
        printf("execmap [path\\file.exe] [key]\n\n");
        printf("Incorrect syntax.\n\n");
        return -1;
    }


    /* Режим подробной диагностики. */
    if (*argv[2] == 'a' || *argv[2] == 't')
        printmz(&exec);
    
    nulloffset();

    fseek(file, exec.e_lfanew, 0); /*может быть любая подпись*/
    
    uint16_t signature;
    fread(&signature, sizeof(signature), 1, file); /*signature подпись заголовка*/

    switch (signature)
    {
        case PE_SIGNATURE_HIGHEND_SEGMENT:
        case PE_SIGNATURE_LITTLEEND_SEGMENT:
        {
            struct WINNT_HEADER nt;

            fseek(file, exec.e_lfanew, 0);
            fread(&nt, sizeof(struct WINNT_HEADER), 1, file);

            if (*argv[2] == 'a' || *argv[2] == 't')
            {
                printpe(&nt.winnt_main);
                printope(&nt.winnt_optional);
            }
            break;
        }
        case NE_SIGNATURE_HIGHEND_SEGMENT:
        case NE_SIGNATURE_LITTLEEND_SEGMENT:
        {
            struct NE_HEADER ne;

            fseek(file, exec.e_lfanew, 0);
            fread(&ne, sizeof(struct NE_HEADER), 1, file);

            if (*argv[2] == 'a' || *argv[2] == 't')
                ne_Get(&ne);

            if (*argv[2] == 'p' || *argv[2] == 'a')
            {
                if (ne.os == 0 && ne.major != 0)
                    ne.os = NEW_OS_WIN;
                le_GetPropertiesMark();
                ne_GetOperatingSystem(ne.os);
                ne_GetVersion(ne.major, ne.minor);
                ne_GetCpu(ne.progflags);
                ne_GetProgramFlags(ne.progflags);
                ne_GetApplicationFlags(ne.appflags);
                ne_GetOtherFlags(ne.oflag);
            }
            break;
        }


        case LX_SIGNATURE_HIGHEND_SEGMENT:
        case LX_SIGNATURE_LITTLEEND_SEGMENT:
        {
            struct LX_HEADER lx;
            fseek(file, exec.e_lfanew, 0);
            fread(&lx, sizeof(struct LX_HEADER), 1, file);

            if (*argv[2] == 'a' ||
                *argv[2] == 't')
                le_Get(&lx);

            if (*argv[2] == 'p' ||
                *argv[2] == 'a')
            {
                le_GetPropertiesMark();
                le_GetCpu(&lx.cpu);
                le_GetOperatingSystem(&lx.os);
                le_GetModuleTypeFlags(&lx.module_flags);
            }

            break;
        }
        case LE_SIGNATURE_HIGHEND_SEGMENT:
        case LE_SIGNATURE_LITTLEEND_SEGMENT:
        {
            struct LX_HEADER lx;
            fseek(file, exec.e_lfanew, 0);
            fread(&lx, sizeof(struct LX_HEADER), 1, file);

            if (*argv[2] == 'a' ||
                *argv[2] == 't') {
                le_Get(&lx);
                if (lx.driver.ddk_version_minor != 0){
                    le_GetVDDHeader(&lx.driver);
                }

            }
            if (*argv[2] == 'a' ||
                *argv[2] == 'p')
            {
                le_GetPropertiesMark(); // head
                le_GetCpu(&lx.cpu);
                le_GetOperatingSystem(&lx.os);
                le_GetVersion(&lx.driver.ddk_version_major, &lx.driver.ddk_version_minor);
                le_GetModuleTypeFlags(&lx.module_flags);
            }
            break;
        }
    }
    
    nulloffset();
    printf("\n");

    fclose(file);

    return 0;
}
