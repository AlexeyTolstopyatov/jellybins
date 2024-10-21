
/*
 * Jelly Bins (C) Толстопятов Алексей 2024
 *      Binary Informatioin Printer
 * Выводит краткие сведения и историческую справку о определяемом
 * двоичном файле
 */
namespace jellybins.Binary
{
    internal enum BinaryInformationPrinter
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
        public static string GetTitle(BinaryInformationPrinter informationPrinter)
        {
            string title = informationPrinter switch
            {
                BinaryInformationPrinter.MarkZbykowski => "MZ",
                BinaryInformationPrinter.Dll => "DLL",
                BinaryInformationPrinter.Lib => "LIB",
                BinaryInformationPrinter.NetObject => ".NET COM",
                BinaryInformationPrinter.MachObject => "MachO",
                BinaryInformationPrinter.Java => "JAR",
                BinaryInformationPrinter.ExecutableLinkable => "ELF",
                BinaryInformationPrinter.AOut => "a-out",
                BinaryInformationPrinter.New => "NE",
                BinaryInformationPrinter.Portable => "PE",
                BinaryInformationPrinter.Linear => "LE",
                BinaryInformationPrinter.StructDefinedByHand => "Структура файла указана вами",
                BinaryInformationPrinter.Other => "Другой двоичный файл",
                _ => "Неизвестный двоичный файл"
            };

            return title;
        }

        public static string GetType(BinaryInformationPrinter informationPrinter) => informationPrinter.ToString();
        public static string GetInformation(BinaryInformationPrinter informationPrinter) 
        {
            string info = informationPrinter switch
            {
                BinaryInformationPrinter.New => "New Executable (NE) — «Новый исполняемый» — формат EXE-файлов, используемый в 16-битных операционных системах, таких, как Windows (до версий 3.x включительно), OS/2 1.x и MS-DOS 4.0.",
                BinaryInformationPrinter.Linear => "Linear Executable (LE) — смешанный 16/32-битный формат исполняемого файла, появившийся в OS/2 2.0. Может быть идентифицирован по сигнатуре «LE» (ASCII). Не используется в настоящее время в OS/2, но использовался для драйверов VxD под Windows 3.x и Windows 9x.",
                BinaryInformationPrinter.Dll => @"Динамически подключаемая библиотека (DLL) — это динамическая библиотека в операционных системах Microsoft Windows и IBM OS/2, которая даёт возможность многократного использования различными программными приложениями.",
                BinaryInformationPrinter.NetObject => @".NET Компонент — это динамическая библиотека или исполняемый файл, которые используют платофрму .NET и IL для работы.",
                _ => "Информация отсутствует."
            };
            return info;
        }

        /// <summary>
        /// Возвращает тип двоичного файла, на основе начала структуры
        /// </summary>
        /// <param name="word">Cловo (8/16/32/64 битного)</param>
        /// <returns></returns>
        public static BinaryInformationPrinter DetectType(uint word)
        {
            switch (word)
            {
                case 0x4d5a:
                case 0x5a4d:
                    return BinaryInformationPrinter.MarkZbykowski;

                case 0x4c58:
                case 0x584c:
                case 0x4c45:
                case 0x454c:
                    return BinaryInformationPrinter.Linear;

                case 0x454e:
                case 0x4e45:
                    return BinaryInformationPrinter.New;
                
                case 0x4550:
                case 0x5045:
                    return BinaryInformationPrinter.Portable;

                default:
                    return BinaryInformationPrinter.Other;
            }
        }
    }
}
