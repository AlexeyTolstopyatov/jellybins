﻿namespace jellybins.Report.Common.PortableExecutable;

/// <summary>
/// Если внутри двоичного файла есть Точка входа DLLMain()
/// </summary>
public enum DllCharacteristics : ushort
{
    /// <summary>
    /// Флаг указывает на то, что разброс виртуальных адресов
    /// увиличивается с 8 байт до 17
    /// </summary>
    HighEntropyOfVirtualAddresses = 0x0020,
    /// <summary>
    /// Библиотека может быть перемещена во время загрузки
    /// </summary>
    DynamicBase = 0x0040,
    /// <summary>
    /// Этот флаг используется компиляторами и линкерами
    /// для указания того, что библиотека (DLL) должна быть
    /// проверена на целостность перед загрузкой в память.
    /// </summary>
    ForceIntegrity = 0x0080,
    /// <summary>
    ///  Флаг image_nx_compatible связан с
    /// поддержкой функции NX (No execute),
    /// которая является частью архитектуры процессоров x86.
    /// Эта функция позволяет помечать страницы памяти как
    /// недопустимые для исполнения,
    /// что предотвращает выполнение кода в областях памяти,
    /// предназначенных для данных, таких как стек и куча.
    /// Это помогает защитить систему от некоторых видов атак,
    /// включая те, которые пытаются выполнить вредоносный код
    /// через уязвимости в программах. 
    /// </summary>
    ImageNxCompat = 0x0100,
    /// <summary>
    /// Обычно каждая DLL работает в своем собственном
    /// изолированном окружении, что помогает избегать конфликтов
    /// между различными версиями одной и той же библиотеки,
    /// используемыми разными приложениями. Но иногда приложения
    /// требуют доступа к общим ресурсам или данным,
    /// и тогда использование флага NO_ISOLATION становится
    /// полезным.
    /// </summary>
    NoIsolation = 0x0200,
    /// <summary>
    /// Нет обработчика структурных исключений
    /// (No Structured Exception's handler)
    /// </summary>
    NoSeh = 0x0400,
    /// <summary>
    /// Не связывать образ
    /// </summary>
    NoBind = 0x0800,
    /// <summary>
    /// Выполняется только в AppContainer
    /// </summary>
    ApplicationContainer = 0x1000,
    /// <summary>
    /// Драйвер устройства на основе WDM
    /// (Замена VxD)
    /// </summary>
    WindowsDriverModel = 0x2000,
    /// <summary>
    /// Control Flow Guard — механизм защиты Windows,
    /// который усложняет процесс эксплуатации бинарных уязвимостей
    /// в пользовательских приложениях и
    /// приложениях режима ядра. Он валидирует неявные вызовы,
    /// предотвращая перехват потока исполнения
    /// злоумышленником (например, посредством
    /// перезаписи таблицы виртуальных функций).
    /// (Появился в Windows NT 10.0)
    /// </summary>
    ControlFlowGuard = 0x4000,
    /// <summary>
    /// Если приложение известно Terminal Server,
    /// оно не должно полагаться на файлы INI или записывать
    /// данные в реестр (дерево пользователя hkey_current_user)
    /// во время настройки.
    /// Опция включена по умолчанию для Windows и
    /// консольных приложений.
    /// Она не действует на драйверы и DLL
    /// </summary>
    TerminalServerAware = 0x8000
}