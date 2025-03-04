using System;
using System.IO;
using System.Reflection.PortableExecutable;

public class VBPeAnalyzer
{
    public static void FindVBSignature(string filePath)
    {
        using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        using (var reader = new PEReader(stream))
        {
            // Получаем точку входа
            var entryPoint = reader.PEHeaders.PEHeader.AddressOfEntryPoint;
            var entryPointSection = reader.GetSectionContainingOffset(entryPoint);

            // Читаем данные секции, содержащей точку входа
            var sectionData = reader.GetSectionData(entryPointSection.RelativeVirtualAddress);
            var data = sectionData.GetContent();

            // Ищем подпись "VB5!" в данных секции
            var vbSignature = new byte[] { (byte)'V', (byte)'B', (byte)'5', (byte)'!' };
            int index = FindPattern(data, vbSignature);

            if (index != -1)
            {
                Console.WriteLine($"Подпись VB5! найдена по смещению: 0x{index:X}");
            }
            else
            {
                Console.WriteLine("Подпись VB5! не найдена.");
            }
        }
    }

    private static int FindPattern(byte[] data, byte[] pattern)
    {
        for (int i = 0; i <= data.Length - pattern.Length; i++)
        {
            bool match = true;
            for (int j = 0; j < pattern.Length; j++)
            {
                if (data[i + j] != pattern[j])
                {
                    match = false;
                    break;
                }
            }
            if (match)
            {
                return i;
            }
        }
        return -1;
    }

    public static void Main(string[] args)
    {
        string filePath = "path_to_your_executable.exe";
        FindVBSignature(filePath);
    }
}