using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

public class PEImportsTable
{
    // Структура IMAGE_IMPORT_DESCRIPTOR
    [StructLayout(LayoutKind.Sequential)]
    public struct IMAGE_IMPORT_DESCRIPTOR
    {
        public uint OriginalFirstThunk; // RVA к Import Name Table
        public uint TimeDateStamp;
        public uint ForwarderChain;
        public uint Name;               // RVA к имени DLL
        public uint FirstThunk;         // RVA к Import Address Table
    }

    // Структура IMAGE_THUNK_DATA
    [StructLayout(LayoutKind.Sequential)]
    public struct IMAGE_THUNK_DATA
    {
        public uint AddressOfData; // RVA к имени функции или ординалу
    }

    // Функция для преобразования RVA в физический указатель
    public long GetPtrFromRVA(int iRVA)
    {
        // Реализация функции (см. предыдущий ответ)
        // ...
        return iRVA; // Заглушка
    }

    // Функция для чтения таблицы импортов
    public void ReadImportsTable(byte[] fileData, uint importsTableRVA)
    {
        // Преобразуем RVA таблицы импортов в физический указатель
        long importsTablePointer = GetPtrFromRVA((int)importsTableRVA);

        // Читаем записи IMAGE_IMPORT_DESCRIPTOR
        int descriptorSize = Marshal.SizeOf(typeof(IMAGE_IMPORT_DESCRIPTOR));
        int offset = (int)importsTablePointer;

        while (true)
        {
            // Читаем текущий дескриптор
            IMAGE_IMPORT_DESCRIPTOR descriptor = ReadStruct<IMAGE_IMPORT_DESCRIPTOR>(fileData, offset);

            // Если все поля нулевые, это конец таблицы
            if (descriptor.OriginalFirstThunk == 0 && descriptor.Name == 0 && descriptor.FirstThunk == 0)
                break;

            // Получаем имя DLL
            long dllNamePointer = GetPtrFromRVA((int)descriptor.Name);
            string dllName = ReadNullTerminatedString(fileData, (int)dllNamePointer);

            Console.WriteLine($"DLL: {dllName}");

            // Читаем функции из Import Name Table
            ReadImportedFunctions(fileData, descriptor.OriginalFirstThunk);

            // Переходим к следующему дескриптору
            offset += descriptorSize;
        }
    }

    // Функция для чтения импортированных функций
    private void ReadImportedFunctions(byte[] fileData, uint thunkRVA)
    {
        long thunkPointer = GetPtrFromRVA((int)thunkRVA);
        int offset = (int)thunkPointer;

        while (true)
        {
            // Читаем IMAGE_THUNK_DATA
            IMAGE_THUNK_DATA thunkData = ReadStruct<IMAGE_THUNK_DATA>(fileData, offset);

            // Если значение нулевое, это конец таблицы
            if (thunkData.AddressOfData == 0)
                break;

            // Проверяем, импортируется ли функция по имени или по ординалу
            if ((thunkData.AddressOfData & 0x80000000) == 0)
            {
                // Импорт по имени
                long functionNamePointer = GetPtrFromRVA((int)thunkData.AddressOfData + 2); // +2 для пропуска Hint
                string functionName = ReadNullTerminatedString(fileData, (int)functionNamePointer);
                Console.WriteLine($"  Function: {functionName}");
            }
            else
            {
                // Импорт по ординалу
                ushort ordinal = (ushort)(thunkData.AddressOfData & 0xFFFF);
                Console.WriteLine($"  Function: Ordinal {ordinal}");
            }

            // Переходим к следующему элементу
            offset += Marshal.SizeOf(typeof(IMAGE_THUNK_DATA));
        }
    }

    // Вспомогательная функция для чтения структуры из массива байт
    private T ReadStruct<T>(byte[] data, int offset)
    {
        int size = Marshal.SizeOf(typeof(T));
        IntPtr ptr = Marshal.AllocHGlobal(size);
        Marshal.Copy(data, offset, ptr, size);
        T result = (T)Marshal.PtrToStructure(ptr, typeof(T));
        Marshal.FreeHGlobal(ptr);
        return result;
    }

    // Вспомогательная функция для чтения строки с нулевым окончанием
    private string ReadNullTerminatedString(byte[] data, int offset)
    {
        int end = offset;
        while (end < data.Length && data[end] != 0)
            end++;
        return System.Text.Encoding.ASCII.GetString(data, offset, end - offset);
    }
}