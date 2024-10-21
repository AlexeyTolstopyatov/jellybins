using System.IO;
using System.Runtime.InteropServices;

/*
 * Jelly Bins (C) Толстопятов Алексей 2024
 *         Binary Searcher
 * Класс, предоставляющий логику заполнения структур и поиска значений по
 * указанному файлу
 * Члены класса: Fill<T>(string, ref T): Заполняет структуру данными из файла сначала
 *               Fill<T>(string, ref T, int): Заполняет структуру данными из файла, начиная с... (int offset)
 *                Get<T>(string, uint): Получает значение MZ структуры (e_lfanew)
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
        /// <param name="path"></param>
        /// <param name="lfanew">Long File address of New header</param>
        public static ushort Get(string path, uint lfanew)
        {
            byte[] destination = new byte[2]; // WORD
            using (var file = File.Open(path, FileMode.Open, FileAccess.Read))
            {
                file.Seek(lfanew, SeekOrigin.Begin);
                file.Read(destination, 0, destination.Length);
            }

            return (ushort)(destination[0] * 0x100 + destination[1]);
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
