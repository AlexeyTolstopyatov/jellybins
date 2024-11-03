using System.IO;
using System.Xml;
using System.Xml.Linq;
using jellybins.Middleware;

namespace jellybins.Config;


public static class JbConfigReader
{
    public static void Read()
    {
        string appc = AppDomain.CurrentDomain.BaseDirectory + "JbConfig.xml";
        Console.WriteLine(appc);
        if (!System.IO.File.Exists(appc))
        {
            JbConfig.SetInstance();
            return;
        }

        try
        {
            var list = from element in XDocument.Load(appc).Descendants()
                select (element.HasElements) ? element : new XElement("Null");

            // System.Linq.Enumerable.First[TSource](IEnumerable`1 source)
            // at jellybins.Config.JbConfigReader.Read() in D:\Projects\cs\jellybins\jellybins\Config\JbConfigReader.cs:line 18
        
            int expandHeaders = int.Parse(list.Elements("ExpandHeaders").First().Value);
            int expandFlags = int.Parse(list.Elements("ExpandFlags").First().Value);
            int filter = int.Parse(list.Elements("FilterIndex").First().Value);
        
            JbConfig.SetInstance(expandHeaders, expandFlags, filter);
            Console.WriteLine("Configuration from file:");
            Console.WriteLine($"\tExpandHeaders\t{expandHeaders}");
            Console.WriteLine($"\tExpandFlags\t{expandFlags}");
            Console.WriteLine($"\tFilter Index\t{filter}");

        }
        catch (Exception e)
        {
            Console.WriteLine("Factory-reset...");
            JbAppReport.ShowException(e);
            JbConfig.SetInstance();
            Console.WriteLine("Configuration restored.");
        }
    }
}