namespace JellyBins.LinearExecutable.Headers;

public struct LeVerifyDirective
{
    public UInt16 NumEntries;       // Количество записей
    public LeVerifyEntry[] Entries;
}

public struct LeVerifyEntry
{
    public UInt16 ModuleOrdinal;    // Индекс модуля в Import Module Table
    public UInt32 Version;            // Версия модуля
    public UInt16 NumObjects;       // Количество объектов для проверки
    public LeVerifyObject[] Objects;
}

public struct LeVerifyObject
{
    public UInt16 ObjectId;           // Индекс объекта
    public UInt32 BaseAddress;        // Базовый адрес
    public UInt32 VirtualSize;        // Виртуальный размер
}