﻿using System.IO;
using Wpf.Ui.Controls;
/*
 * Jelly Bins (С) Толстопятов Алексей 2024
 *      JbAppReport -- Класс, предоставляющий
 * возможность работы с непридвидинными ошибками
 * Члены класса:    ShowException<T>(T) -- Вызывает диалоговое окно
 *                  SaveException<T>(T) -- Сохраняет дерево внутренних исключений в файл
 */
namespace jellybins.Binary
{
    internal static class JbAppReport
    {
        public static void ShowException<T>(T exception) where T : Exception
        {
            new MessageBox() 
            {
#if DEBUG
                Title   = exception.GetType().Name,
                Content = exception
#else
                Title   = "Прервано!",
                Content = exception.Message
#endif
            }.ShowDialogAsync();
        }

        public static void SaveException<T>(T exception) where T : Exception
        {
            string reportName = $"Report{DateTime.UtcNow.Millisecond}.exception";
            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "\\" + reportName, 
                exception.ToString());
        }

        public static void Information(string message)
            => WriteLine(message, ConsoleColor.Cyan);

        public static void Warning(string message)
            => WriteLine(message, ConsoleColor.Yellow);

        public static void Exception(string message)
            => WriteLine(message, ConsoleColor.Red);
        
        
        // ReSharper disable once MemberCanBePrivate.Global
        public static void WriteLine(string message, ConsoleColor cc)
        {
            Console.ForegroundColor = cc;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}