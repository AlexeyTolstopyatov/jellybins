using System.Windows.Controls;
using System.Xml.Linq;
using jellybins.Views;

namespace jellybins.Middleware;

public static class JbReport
{
    public static void CommonCollect()
    {
        
    }
    
    /// <summary>
    /// Сохранить общий тип отчета как XML
    /// </summary>
    public static void CommonSaveAsXml(ref BinaryHeaderPage bin, string path)
    {
        // Приблизительно структура отчета
        // название файла
        //  - Путь
        //  - Тип двоичного файла
        //  - Типы флагов
        //  - Флаги
        //
        XDocument document = new(
            XName.Get("Binary"),
            new XElement(
                XName.Get("Name"),
                bin.binname.Text),
            new XElement(
                "Path", 
                bin.binpath.Text),
            new XElement(
                XName.Get("Properties"),
                bin.binprops.Text),
            new XElement(
                XName.Get("Runs"),
                bin.IsCompat),
            new XElement(
                "flags" /*from item in FlagNames select item*/),
            new XElement(
                XName.Get("Flags")));
        
        
    }
}