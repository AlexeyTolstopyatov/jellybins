namespace jellybins.Report.Common.PortableExecutable;

public enum Environment
{
    /// <summary>
    /// Немного отсебя(тины)
    /// Это используется для определения анализатором
    /// Определение Win16 среды (Windows 3x...)
    /// </summary>
    Win16GraphicalInterface = 101,
    /// <summary>
    /// Немного отсебя(тины)
    /// Я буду это использовать для определения
    /// необходимости NTVDM. В официальной документации Microsoft
    /// этого флага нет.
    /// </summary>
    DosConsoleInterface = 102,
    
    
    /// <summary>
    /// Нет подсистемы.
    /// Обычно указывается для драйверов устройств
    /// (чтобы работать в реальном режиме?)
    /// </summary>
    Native = 1,
    /// <summary>
    /// Консольное приложение
    /// Для запуска используется ConDrv и Service Host
    /// </summary>
    ConsoleInterface = 3,
    /// <summary>
    /// Оконное приложение
    /// Используется Windows API для рисования и реагирования
    /// и Win32 подсистема для поддержки жизни
    /// </summary>
    GraphicalInterface = 2,
    /// <summary>
    /// Консольное приложение OS/2
    /// Используется netapi.dll doscalls.dll для запуска
    /// (Было убрано после Windows NT 5.0)
    /// </summary>
    Os2ConsoleInterface = 5,
    /// <summary>
    /// Консольное приложение POSIX
    /// Использует функции из POSIX API такие как:
    /// kill() alarm() exit() calloc() и другие. 
    /// </summary>
    PosixConsoleInterface = 7,
    /// <summary>
    /// Оконное приложение
    /// Использует WinRT (Windows Runtime)
    /// Вместо Win32 подсистемы
    /// </summary>
    WindowsNative = 8,
    /// <summary>
    /// Оконное приложение Windows CE
    /// Использует подсистему Win32 для Windows CE
    /// </summary>
    WindowsCeGraphicalInterface = 9
}