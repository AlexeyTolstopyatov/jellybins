using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jellybins.Models
{
    internal enum TypeofFile
    {
        NetObject,          // Managed .NET Assembly
        Lib,                // Unmanaged Library
        Dll,                // Unmanaged DLL
        PortableExecutable, // Windows NT PE32/+
        LinearExecutable,   // MS-DOS/Windows/OS-2 LE/LX
        NewExecutable,      // MS-DOS/Windows/OS-2 NE
        AOutExecutable,     // a-out Linux/Solaris executable
        ExecutableLinkable, // ELF32/64
        MachObject,         // .app/.kext/.framework Apple Format
        JavaExecutable,     // Jar files
        StructDefinedByHand,// Structure will be defined "by hand"
        Other               // Other binary file (unknown)
    }

    internal static class TypeofInformation
    {
        public static string GetTitle(TypeofFile type)
        {
            string title = type switch
            {
                TypeofFile.Dll => "DLL",
                TypeofFile.Lib => "LIB",
                TypeofFile.NetObject => ".NET COM",
                TypeofFile.MachObject => "MachO",
                TypeofFile.JavaExecutable => "JAR",
                TypeofFile.ExecutableLinkable => "ELF",
                TypeofFile.AOutExecutable => "a-out",
                TypeofFile.NewExecutable => "NE",
                TypeofFile.PortableExecutable => "PE",
                TypeofFile.LinearExecutable => "LE",
                TypeofFile.StructDefinedByHand => "Структура файла указана вами",
                TypeofFile.Other => "Другой двоичный файл",
                _ => "Неизвестный двоичный файл"
            };

            return title;
        }

        public static string GetType(TypeofFile type) => type.ToString();
        public static string GetInformation(TypeofFile type) 
        {
            string info = type switch
            {
                TypeofFile.Dll => @"Динамически подключаемая библиотека (DLL) — это динамическая библиотека в операционных системах Microsoft Windows и IBM OS/2, которая даёт возможность многократного использования различными программными приложениями.",
                TypeofFile.NetObject => @".NET Компонент — это динамическая библиотека или исполняемый файл, которые используют платофрму .NET и IL для работы.",
                _ => "Информация отсутствует."
            };
            return info;
        }
    }
}
