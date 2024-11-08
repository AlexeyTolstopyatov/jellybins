namespace jellybins.Report.Common.NewExecutable;

public enum ApplicationFlags : byte
{
    None = 0,                   // 
    FullScreen = 0x01,          // Флаг полноэкранного режима 
    WindowsPmCompatible = 0x02, // Совместимость с OS/2 Presentation Manager
    WindowsPmUsage = 0x03,      // Использование OS/2 Presentation Manager
    LoadCodeSegment = 0x08,     // Есть ли у двоичного файла .code, который будет сначала загружен
    ImageLinkErrors = 0x20,     // В образе возникли ошибки при линковкe
    NonConforming = 0x40,       // Несоответствующая программа
    LibraryModule = 0x80,       // Модуль библиотеки или приложения
}