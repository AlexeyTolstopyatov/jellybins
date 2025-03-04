using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

public static class VbDetector
{
    public static bool IsVbApp(string filePath)
    {
       try
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader reader = new BinaryReader(fs))
                {
                    // Читаем DOS заголовок (MZ)
                    ushort mzSignature = reader.ReadUInt16();
                   if (mzSignature != 0x5A4D)
                    {
                        return false; // Not a valid EXE
                    }

                   // Переходим к PE заголовку
                    reader.BaseStream.Seek(0x3C, SeekOrigin.Begin);
                    uint peOffset = reader.ReadUInt32();
                   reader.BaseStream.Seek(peOffset, SeekOrigin.Begin);


                    // Читаем PE сигнатуру (PE00)
                    uint peSignature = reader.ReadUInt32();
                    if (peSignature != 0x00004550)
                    {
                        return false; // Not a valid PE File
                    }


                    reader.BaseStream.Seek(peOffset+4, SeekOrigin.Begin);
                      ushort machineType = reader.ReadUInt16();
                      // Проверяем на 64-разрядность
                        if(machineType == 0x8664 || machineType == 0xAA64) {
                          return false;
                        }


                      reader.BaseStream.Seek(peOffset + 20, SeekOrigin.Begin);
                    ushort optionalHeaderSize = reader.ReadUInt16();


                    // Ищем RVA таблицы импорта
                    reader.BaseStream.Seek(peOffset + 24 + optionalHeaderSize, SeekOrigin.Begin);
                     ushort numberOfSections = reader.ReadUInt16();

                     reader.BaseStream.Seek(peOffset + 20 + 96, SeekOrigin.Begin);
                    uint importDirectoryRVA = reader.ReadUInt32();



                    if (importDirectoryRVA == 0) return false;
                   
                    uint importDirectorySize = reader.ReadUInt32();
                    var importAddress = (peOffset + importDirectoryRVA);

                     reader.BaseStream.Seek(importAddress, SeekOrigin.Begin);

                    // Перебираем все зависимости
                    while(true)
                    {

                    var nameRVA =  reader.ReadUInt32();
                    if(nameRVA == 0) break;
                    reader.ReadUInt32();
                    reader.ReadUInt32();
                    reader.ReadUInt32();
                     var importNameAddress = (peOffset + nameRVA);
                       reader.BaseStream.Seek(importNameAddress, SeekOrigin.Begin);

                    var dllName = string.Empty;
                    while(true) {
                       var nameChar = reader.ReadChar();
                        if(nameChar == 0) break;
                         dllName += nameChar;
                    }
                        if (dllName.Equals("msvbvm60.dll", StringComparison.OrdinalIgnoreCase) ||
                           dllName.Equals("msvbvm50.dll", StringComparison.OrdinalIgnoreCase))
                         {
                            return true; // Found msvbvm60.dll or msvbvm50.dll in Import table
                        }
                        reader.BaseStream.Seek(importAddress + 20, SeekOrigin.Begin);
                    importAddress = (int)reader.BaseStream.Position;
                  }
                }
            }
       
            return false; // If not found it not VB
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
            return false; // Handle error
        }
    }
}