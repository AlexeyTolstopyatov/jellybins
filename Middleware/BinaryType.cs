
namespace jellybins.Binary
{
    public enum BinaryType
    {
        MarkZbykowski,
        NetObject,          // Managed .NET Assembly
        Lib,                // Unmanaged Library
        Dll,                // Unmanaged DLL
        Portable,           // Windows NT PE32/+
        Linear,             // MS-DOS/Windows/OS-2 LE/LX
        New,                // MS-DOS/Windows/OS-2 NE
        AOut,               // a-out Linux/Solaris executable
        ExecutableLinkable, // ELF32/64
        MachObject,         // Binary inside .app/.kext/.framework
        Java,               // Jar files
        StructDefinedByHand,// Structure will be defined "by hand"
        Other               // Other binary file (unknown)
    }

    internal static class BinaryInformation
    {
        public static string GetTitle(BinaryType type)
        {
            string title = type switch
            {
                BinaryType.MarkZbykowski => "MZ",
                BinaryType.Dll => "DLL",
                BinaryType.Lib => "LIB",
                BinaryType.NetObject => ".NET COM",
                BinaryType.MachObject => "MachO",
                BinaryType.Java => "JAR",
                BinaryType.ExecutableLinkable => "ELF",
                BinaryType.AOut => "a-out",
                BinaryType.New => "NE",
                BinaryType.Portable => "PE",
                BinaryType.Linear => "LE",
                BinaryType.StructDefinedByHand => "Структура файла указана вами",
                BinaryType.Other => "Другой двоичный файл",
                _ => "Неизвестный двоичный файл"
            };

            return title;
        }

        public static string GetType(BinaryType type) => type.ToString();
        public static string GetInformation(BinaryType type) 
        {
            string info = type switch
            {
                BinaryType.New => "New Executable (NE) — «Новый исполняемый» — формат EXE-файлов, используемый в 16-битных операционных системах, таких, как Windows (до версий 3.x включительно), OS/2 1.x и MS-DOS 4.0.\n\nПри запуске из Windows NT или OS/2 2.x NE-программы запускаются под Virtual DOS Machine (NTVDM.EXE и VDM, соответственно), которая обеспечивает их выполнение и почти полную совместимость с операционной системой DOS. Начиная с Windows NT 6.0 (Vista) оболочка Windows не поддерживает извлечение ресурсов из New Executable",
                BinaryType.Linear => "Linear Executable (LE) — смешанный 16/32-битный формат исполняемого файла, появившийся в OS/2 2.0. Может быть идентифицирован по сигнатуре «LE» (ASCII). Не используется в настоящее время в OS/2, ранее использовался для драйверов VxD под Windows 3.x и Windows 9x.\n\nДля открытия файла LE необходимо использовать подходящее программное обеспечение, например DOS/4GW PM 32X DOS Extender. Если открыть файл не удаётся, можно нажать на него правой кнопкой мыши, выбрать «Открыть с помощью» и выбрать приложение",
                BinaryType.NetObject => @".NET Компонент — это динамическая библиотека или исполняемый файл, которые используют платофрму .NET и IL для работы.",
                BinaryType.Portable => "Portable Executable (PE, «переносимый исполняемый») — формат исполняемых файлов, объектного кода и динамических библиотек (DLL), используемый в 32- и 64-разрядных версиях операционной системы Microsoft Windows",
                BinaryType.MarkZbykowski => "MZ — стандартный формат 16-битных исполняемых файлов с расширением .EXE для DOS.\n\nНазван так по сигнатуре — ASCII-символам MZ (4D 5A) в первых двух байтах. Эта сигнатура — инициалы Марка Збиковски, одного из создателей MS-DOS.\n\nФормат был разработан как замена устаревшему формату .COM.",
                _ => "Информация отсутствует."
            };
            return info;
        }

        /// <summary>
        /// Возвращает тип двоичного файла, на основе начала структуры
        /// </summary>
        /// <param name="word">Cловo (8/16/32/64 битного)</param>
        /// <returns></returns>
        public static BinaryType DetectType(uint word)
        {
            return word switch
            {
                0x4d5a or 0x5a4d => BinaryType.MarkZbykowski,
                0x4c58 or 0x584c or 0x4c45 or 0x454c => BinaryType.Linear,
                0x454e or 0x4e45 => BinaryType.New,
                0x4550 or 0x5045 => BinaryType.Portable,
                _ => BinaryType.Other
            };
        }
    }
}
