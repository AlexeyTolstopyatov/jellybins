using jellybins.File.Modeling.Controls;

namespace jellybins.File.Modeling.Base;

/// <summary>
/// Точное определение функций для работы
/// с NE "Новыми исполняемыми" двоичными файлами.
/// </summary>
public interface IExecutableAnalyser
{
    //      Что же предстоит здесь делать....
    // Обрабатывать Экземпляр анализатора (FileChars + FileView)
    // 1) Распознать флаги процессора
    // 2) Загрузить секции заголовка
    // 3) Загрузить используемые (импортируемые) функции и названия библиотек
    //
    
    /// <summary>
    /// Должен дополнить DOS отчет и изменить
    /// характеристики, если это требуется
    /// </summary>
    /// <param name="chars"></param>
    /// <param name="view"></param>
    void ProcessChars(ref FileChars chars);

    /// <summary>
    /// Создает программные, прикладные флаги и флаги процессора
    /// для нового исполняемого файла.
    /// </summary>
    /// <param name="view"></param>
    void ProcessFlags(ref FileView view);

    /// <summary>
    /// Создает таблицу секций и значений
    /// заголовка для нового исполняемого файла 
    /// </summary>
    /// <param name="view"></param>
    void ProcessHeaderSections(ref FileView view);

    /// <summary>
    /// Создает таблицу секций и значений
    /// (использует оригинальные названия полей в структуре)
    /// </summary>
    /// <param name="view"></param>
    void ProcessHeaderSectionNames(ref FileView view);
}