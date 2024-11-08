namespace jellybins.Report.Common.LinearExecutable;

internal enum ModuleFlag : uint
{
    /// <summary>
    /// EXE
    /// </summary>
    ProgramModule  = 0,
    /// <summary>
    /// DLL SYS
    /// </summary>
    LibraryModule  = 0x00008000,
    /// <summary>
    /// Инициализация по-процессу
    /// </summary>
    PerProcessInit = 0x00000004,
    /// <summary>
    /// Есть Внутренние фискированные точки
    /// </summary>
    InternalFixes  = 0x00000010,
    /// <summary>
    /// Есть внешние фискированные точки
    /// </summary>
    ExternalFixes  = 0x00000020,
    /// <summary>
    /// НеСовместим с OS/2 Presentation Manager
    /// </summary>
    InCompatiblePm = 0x00000100,
    /// <summary>
    /// Совместим с OS/2 Presentation Manager
    /// </summary>
    CompatiblePm   = 0x00000200,
    /// <summary>
    /// Использует OS/2 Presentation Manager API
    /// </summary>
    UsesPm         = 0x00000300,
    /// <summary>
    /// Не загружается
    /// </summary>
    NotLoadable    = 0x00002000,
    /// <summary>
    /// Драйвер физического устройства
    /// </summary>
    PhysicalDriver = 0x00020000,
    /// <summary>
    /// Драйвер виртуального устройства
    /// </summary>
    VirtualDriver  = 0x00028000,
    /// <summary>
    /// По-процессное уничтожение экземпляра
    /// </summary>
    PerProcessTermination = 0x0004000,
    /// <summary>
    /// Только для одного ядра (Процессора)
    /// </summary>
    MultiCpuUnsafe = 0x00080000,
}