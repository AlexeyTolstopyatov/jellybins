# JellyBins

<img src="JellyBins.Assets/beans512.png" width="128" height="128" align="right"/>

JellyBins это небольшое программное обеспечение,
которое предоставляет возможность сделать
дампы разных структур одного двоичного файла.
Это не отладчик, а скорее универсальный `CFF Explorer` который работает для всех сегментаций
исполняемых файлов.

## Поддерживаемые форматы
| Формат                       | Где искать?                                                                                                                     | Состояние |
|-----------------------------------|--------------------------------------------------------------------------------------------------------------------------------|-------|
| `COM`                             | `CP/M` `BW-DOS` `PC-DOS` `MS-DOS` и другие дисковые ОС                                                                         | [x]   |
| `NE` "New Executable"             | Microsoft Windows 3x, OS/2 системные компоненты  (около-пользовательские драйверы `.DRV`) Шрифты `.fon`                        | [x]   |
| `LE` and `LX` "Linear Executable" | Microsoft Windows 9x Драйвера VxD, IBM OS/2 абсолютно все программы (`.EXE`, `.DLL`, `.SYS`).                                  | [x]   |
| `PE` "Portable Executable"        | Microsoft Windows 9x, NT полностью (`.DLL`, `.EXE`, `.SYS`, `.OCX`) и EFI прошивки                                             | [x]   |
| "Assembler-Output"  objects       | Ранние `UNIX`-like операционные системы Или у файлов нет расширения или `.o`,  `.so`, `.d` на конце                            | []    |
| `ELF` "Executable Linkable"       | Современные `UNIX`-like операционные системы. Или у файлов нет расширения или `.o`,  `.so`, `.d`                               | []    |
| `Mach-O` "Mach Object"            | Apple `macOS`/`OS X`/`MacOS X` Специальный формат исполняемых файлов Именно объект по адресу `MyApp.app/Contents/MacOS/MyApp`. | []    |

### Core insights

Библиотека `JellyBins.Core.dll` содержит
только общие инструменты или обобщающие
материалы, которые служат мостом между доступными обработчиками
и пользователем, чтобы автоматизировать
процесс распознания и сбора данных.

На момент обновления `README`доступны к десериализации и созданию дампа памяти
следующие структуры программ:

  - Программные заголовки
  - Секции данных
  - Статические импорты
  - Экспортируемые сущности
  - Заголовки Runtime'а
  - Предполагаемые в использовании 
  внешние модули (библиотеки)

### Специально для каждой структуры дампа

Каждая структура дампа части программы
обязательно содержит эти поля:

```csharp
class BaseDump<T> {
   /// <summary> Название дампа (ориг. название структуры) </summary>
   public String Name {get;set;}
   /// <summary>Указатель на начало структуры</summary>
   public UInt64 Address {get;set;}
   /// <summary>размер ТОЛЬКО структуры</summary>
   public UInt64 Size {get;set;}
   /// <summary>Сама структура</summary>
   public T Segmentation {get;set;}
}

```

Это специальная структура, которая обозначена только для JellyBins, чтобы видеть поведение обработчика требуемой структуры.

Для пользователя она не должна иметь сокральный смысл.

### Abstractions insights

Библиотека `JellyBins.Abstractins.dll` содержит
только шаблоны и публичные интерфейсы, которые
обязаны реализоваться. Ничего больше.

### Документы и источники

 - [Использование JellyBins Console](README_JBC.md)
 - [NE Формат (детали сегментации)](JellyBins.Documents/Microsoft%20NE%20Segmentation.pdf)
 - [LE Формат (OS/2 16-32-смешанная модель)](JellyBins.Documents/IBM%20LE%20Segments.html.pdf)
 - [LX Формат (OS/2 32-разрядные SOM)](JellyBins.Documents/OS2_OMF_and_LX_Object_Formats_Revision_8_199406.pdf)
 - [Visual Basic Virtual Machine 5/6 метаданные](JellyBins.Documents/Visual%20Basic%20Compiled%20PE%20Internals.pdf)
 - [Semi VB Decompiler PCode4.bas](JellyBins.Documents/modPCode4.bas)
 - [.NET Runtime метаданные](JellyBins.Documents/The%20.NET%20File%20Format.pdf)

### Icons
Взято из информационного ресурса:  [icon8](https://icons8.com/), под условием свободного использования.
