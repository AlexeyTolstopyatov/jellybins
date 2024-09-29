# JellyBins (Console Interface)
Выводит таблицу заголовков исполняемых файлов OS/2 DOS Windows
Поддерживает три ключа для вывода информации
 - ```-p``` (Properties) Свойства исполняемого файла
 - ```-t``` (Table) Таблица сегментов и смещений в заголовках исполняемого файла
 - ```-a``` (All) Выводит таблицу и свойства исполняемого файла.

_Работает_ только с "доисторическими" 16-разрядными и 32-разрядными программами для MS-DOS, Windows 3x, Windows 9x, Windows NT, OS/2 типа
```NE```, ```LE```, ```LX```, ```PE```
_Не работает_ с исполняемыми файлами типа ```ELF```, ```MachO```, ```a.out```, ```FDPIC ELF```, ```EM86```.

### Синтаксис
```
./execmap [file] [parameter]
./execmap \\?\C:\Windows\winhlp32.exe a
./execmap \\?\C:\Windows\winhlp32.exe p
./execmap \\?\C:\Windows\winhlp32.exe t
```

### Свойства
При указании ключа ```p``` вывод информации о исполняемом файле будет содержать его требования к работе.
Пример: вывод информации о DOOM The Ultimate.
```
./execmap ...inst\DOOM.EXE

Runs at OS/2
Built for Intel 80386 CPU architecture
Executable file signature LE (hex=0x454c)
Microsoft Windows or IBM OS/2 "Linear Executable".

```

### Таблица
При указании ключа ```t``` вывод информации о программе будет содержать ее таблицу заголовков
Пример: вывод таблиц о DOOM The Ultimate
```
MZ HEADER TABLE
----------------
MZ SIGNATURE    0x5a4d  (offset=2,sizeof=2)     __uint16_t      WORD    unsigned short
LAST BLOCK SIZE 0x90    (offset=4,sizeof=2)     __uint16_t      WORD    unsigned short
BLOCKS COUNT    0x1b    (offset=6,sizeof=2)     __uint16_t      WORD    unsigned short
RELOCATIONS     0x0     (offset=8,sizeof=2)     __uint16_t      WORD    unsigned short
PARAGRAPHS (p)  0x4     (offset=a,sizeof=2)     __uint16_t      WORD    unsigned short
min(extra_p)    0x0     (offset=c,sizeof=2)     __uint16_t      WORD    unsigned short
max(extra_p)    0xffff  (offset=e,sizeof=2)     __uint16_t      WORD    unsigned short
init(SS)        0x0     (offset=10,sizeof=2)    __uint16_t      WORD    unsigned short
init(SP)        0x0     (offset=12,sizeof=2)    __uint16_t      WORD    unsigned short
Checksum        0x0     (offset=14,sizeof=2)    __uint16_t      WORD    unsigned short
init(CS)        0x0     (offset=16,sizeof=2)    __uint16_t      WORD    unsigned short
init(IP)        0x100   (offset=18,sizeof=2)    __uint16_t      WORD    unsigned short
RELOC OFFSET    0x0     (offset=1a,sizeof=2)    __uint16_t      WORD    unsigned short
OVERLAY NUMBER  0x0     (offset=1c,sizeof=2)    __uint16_t      WORD    unsigned short
SECURED 0x1C[0] 0x0     (offset=1e,sizeof=2)    __uint16_t      WORD    unsigned short
OEM ID          0x0     (offset=20,sizeof=2)    __uint16_t      WORD    unsigned short
OEM INFORMATION 0x0     (offset=22,sizeof=2)    __uint16_t      WORD    unsigned short
SECURED 0x28[0] 0x0     (offset=24,sizeof=2)    __uint16_t      WORD    unsigned short
NEXT HEAD AT    0x3490  (offset=26,sizeof=2)    __uint16_t      WORD    unsigned short
----------------
LE HEADER TABLE
----------------
SIGNATURE               0x454c  (offset=2,sizeof=2)     __uint16_t      WORD    unsigned short
[BYTE] ORDER            0x0     (offset=3,sizeof=1)     __uint8_t       BYTE    unsigned char
[WORD] ORDER            0x0     (offset=4,sizeof=1)     __uint8_t       BYTE    unsigned char
FORMAT                  0x0     (offset=8,sizeof=4)     __uint64_t      DWORD   unsigned long
CPU TYPE                0x2     (offset=a,sizeof=2)     __uint16_t      WORD    unsigned short
OS TYPE                 0x1     (offset=c,sizeof=2)     __uint16_t      WORD    unsigned short
MODULE VERSION          0x0     (offset=10,sizeof=4)    __uint64_t      DWORD   unsigned long
MODULE FLAGS            0x200   (offset=14,sizeof=4)    __uint64_t      DWORD   unsigned long
MODULE PAGES            0x21    (offset=18,sizeof=4)    __uint64_t      DWORD   unsigned long
IP OBJECTS              0x1     (offset=1c,sizeof=4)    __uint64_t      DWORD   unsigned long
IP INIT VALUE           0x15630 (offset=20,sizeof=4)    __uint64_t      DWORD   unsigned long
SP OBJECTS              0x2     (offset=24,sizeof=4)    __uint64_t      DWORD   unsigned long
SP INIT VALUE           0x3eec0 (offset=28,sizeof=4)    __uint64_t      DWORD   unsigned long
SIZE OF PAGE            0x1000  (offset=2c,sizeof=4)    __uint64_t      DWORD   unsigned long
SIZE OF LAST PAGE       0x3d3   (offset=30,sizeof=4)    __uint64_t      DWORD   unsigned long
SIZE OF FIX SECTION     0xb86f  (offset=34,sizeof=4)    __uint64_t      DWORD   unsigned long
SIZE OF FIX CHECKSUM    0x0     (offset=38,sizeof=4)    __uint64_t      DWORD   unsigned long
SIZE OF LOADER SECTION  0xb92c  (offset=3c,sizeof=4)    __uint64_t      DWORD   unsigned long
SIZE OF LOADER CHECK    0x0     (offset=40,sizeof=4)    __uint64_t      DWORD   unsigned long
OBJECT TABLE OFFSET     0xc4    (offset=44,sizeof=4)    __uint64_t      DWORD   unsigned long
OBJECT TABLE ENTRIES    0x2     (offset=48,sizeof=4)    __uint64_t      DWORD   unsigned long
OBJECT PAGE MAP OFFSET  0xf4    (offset=4c,sizeof=4)    __uint64_t      DWORD   unsigned long
OBJECT ITER-DATA OFFSET 0x0     (offset=50,sizeof=4)    __uint64_t      DWORD   unsigned long
RESOURCES TABLE OFFSET  0x178   (offset=54,sizeof=4)    __uint64_t      DWORD   unsigned long
RESOURCES TABLE ENTRIES 0x0     (offset=58,sizeof=4)    __uint64_t      DWORD   unsigned long
RESIDENT NAMES TABLE AT 0x178   (offset=5c,sizeof=4)    __uint64_t      DWORD   unsigned long
ENTRY TABLE OFFSET      0x180   (offset=60,sizeof=4)    __uint64_t      DWORD   unsigned long
MODULE DIRECTION OFFSET 0x0     (offset=64,sizeof=4)    __uint64_t      DWORD   unsigned long
MODULE DIRECT ENTRIES   0x0     (offset=68,sizeof=4)    __uint64_t      DWORD   unsigned long
FIX PAGE SECTION OFFSET 0x181   (offset=6c,sizeof=4)    __uint64_t      DWORD   unsigned long
FIXUP RECORDS OFFSET    0x209   (offset=70,sizeof=4)    __uint64_t      DWORD   unsigned long
IMPORTS OFFSET          0xb9ef  (offset=74,sizeof=4)    __uint64_t      DWORD   unsigned long
IMPORTS COUNT VALUE     0x0     (offset=78,sizeof=4)    __uint64_t      DWORD   unsigned long
IMPORT FUNCTIONS OFFSET 0xb9ef  (offset=7c,sizeof=4)    __uint64_t      DWORD   unsigned long
PER-PAGE CHECK OFFSET   0x0     (offset=80,sizeof=4)    __uint64_t      DWORD   unsigned long
DATA PAGES OFFSET       0xf000  (offset=84,sizeof=4)    __uint64_t      DWORD   unsigned long
PRELOAD PAGES COUNT     0x0     (offset=88,sizeof=4)    __uint64_t      DWORD   unsigned long
NONRESIDENT NAMES TABLE 0x0     (offset=8c,sizeof=4)    __uint64_t      DWORD   unsigned long
NONRESIDENT NAMES CHECK 0x0     (offset=90,sizeof=4)    __uint64_t      DWORD   unsigned long
AUTOMATIC DATA-OBJ IS   0x0     (offset=94,sizeof=4)    __uint64_t      DWORD   unsigned long
DEBUG INFO OFFSET       0x2     (offset=98,sizeof=4)    __uint64_t      DWORD   unsigned long
DEBUG INFO LENGTH       0x0     (offset=9c,sizeof=4)    __uint64_t      DWORD   unsigned long
PRELOAD INSTANCES COUNT 0x0     (offset=a0,sizeof=4)    __uint64_t      DWORD   unsigned long
DEMAND INST PAGES COUNT 0x0     (offset=a4,sizeof=4)    __uint64_t      DWORD   unsigned long
HEAP INITAL VALUE       0x0     (offset=a8,sizeof=4)    __uint64_t      DWORD   unsigned long
STACK INITAL VALUE      0x0     (offset=ac,sizeof=4)    __uint64_t      DWORD   unsigned long
RESERVED[0]             0x0     (offset=ad,sizeof=1)    __uint8_t       BYTE    unsigned char
```
В таблице содержатся название сегмента, его значение, смещение относительно начала заголовка, размер, (мой) тип данных, тип данных Windows, тип данных C
