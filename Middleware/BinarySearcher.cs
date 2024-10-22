using System.IO;
using System.Runtime.InteropServices;
/*
 * Jelly Bins (C) Толстопятов Алексей 2024
 *         Binary Searcher
 * Класс, предоставляющий логику заполнения структур и поиска значений по
 * указанному файлу
 * Члены класса: Fill<T>(string, ref T): Заполняет структуру данными из файла сначала
 *               Fill<T>(string, ref T, int): Заполняет структуру данными из файла, начиная с... (int offset)
 *               GetUInt16<T>(string, int): byte[] -> UInt16
 *               GetUInt32<T>(string, int): byte[] -> UInt32 
 */
namespace jellybins.Middleware
{
    internal static class BinarySearcher
    {
        /// <summary>
        /// Читает файл и записывает в структуру
        /// </summary>
        /// <param name="path">Откуда читать</param>
        /// <param name="head">Куда писать</param>
        /// <typeparam name="T">Структура с неуправляемыми типами данных</typeparam>
        public static void Fill<T>(ref string path, ref T head) where T : struct
        {
            Fill(ref path, ref head, 0);
        }

        /// <summary>
        /// Возвращает СЛОВО сравниваемого типа из файла
        /// </summary>
        /// <param name="path">Гдк читать</param>
        /// <param name="offset">Откуда начать читать</param>
        public static ushort GetUInt16(string path, uint offset)
        {
            byte[] destination = new byte[2]; // WORD
            using (var file = File.Open(path, FileMode.Open, FileAccess.Read))
            {
                file.Seek(offset, SeekOrigin.Begin);
                file.Read(destination, 0, destination.Length);
            }
            // 0x3212 byte[] = 32, 12 => 0x32 * 0x100 => 0x3200 + 0x12 => 0x3212
            return (ushort)(destination[0] * 0x100 + destination[1]);
        }

        /// <summary>
        /// Возвращает 32 разрядное слово
        /// </summary>
        /// <param name="path">Где читать</param>
        /// <param name="offset">Откуда начать читать</param>
        /// <returns></returns>
        public static uint GetUInt32(string path, int offset)
        {
            byte[] destination = new byte[4]; // DWORD
            using (var file = File.Open(path, FileMode.Open, FileAccess.Read))
            {
                file.Seek(offset, SeekOrigin.Begin); // mov ptr, offset
                file.Read(destination, 0, destination.Length); // read 4 bytes
            }
            // 0xFFAAEE11 => [FF AA EE 11] => 0xFF * 0x10000 => 0xFF0000 + 0xAA00 => 0xFFAA00 => 0xFFAA00 + 0x11 => 0xFFAAEE11 
            return (uint)(destination[0] * 0x10000 + destination[1] + (destination[2] * 0x100) + destination[3]);
        }

        /// <summary>
        /// Заполняет структуру данными из файла по указанным параметрам
        /// </summary>
        /// <param name="path">Откуда читать</param>
        /// <param name="head">Куда писать</param>
        /// <param name="offset">С какого байта читать</param>
        /// <typeparam name="T">Тип нетипобезопасной структуры</typeparam>
        public static void Fill<T>(ref string path, ref T head, int offset)
        {
            using FileStream stream = new(path, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[Marshal.SizeOf<T>()];
            GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);

            stream.Seek(offset, SeekOrigin.Begin);
            stream.Read(buffer, 0, buffer.Length);

            head = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T))!;
            handle.Free();
        }
    }

}
