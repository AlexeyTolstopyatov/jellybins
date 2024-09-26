# Немного о проекте
JellyBins ("измененно jelly beans") - Утилита для анализа двоичных исполняемых файлов. Мне было безумно интересно узнать больше о содержимом приложений в разных Операционных системах.
Планируется сделать логику для анализа
 - Приложений и компонентов Microsoft Windows 
    - ```PE``` "Portable Executable"
    - ```LE``` "Linear Executable"
    - ```VxD``` "Virtual Device Driver"
    - ```NE``` "New Executable"
    - ```MZ``` "Mark Zbickowsky's модель заголовка"
    - ```COFF``` "Common Object File Format"
- Приложений/Общих Двоичных файлов Linux
    - ```a-out``` "Assembler Output"
    - ```ELF``` "Executable Linkable Format"
- Приложений/драйверов Apple macOS
    - ```MachO``` "Mach-microkernel Object model"

# Теория
Все размышления, рассуждения и исторические справки указаны в файлах проекта

# Ветки
 - ```jellybins-gui``` - Ветка оконного приложения (C# 9.0)
 - ```jellybins-ci``` - Ветка интерактивной службы коммандной строки (C-99)
 - ```main``` - Теория, история и рассуждения о логике процессов