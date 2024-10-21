using System.IO;
using Wpf.Ui.Controls;
/*
 * Jelly Bins (С) Толстопятов Алексей 2024
 *      ExceptionsController -- Класс, предоставляющий 
 * возможность работы с непридвидинными ошибками
 * Члены класса:    ShowException<T>(T) -- Вызывает диалоговое окно
 *                  SaveException<T>(T) -- Сохраняет дерево внутренних исключений в файл
*/
namespace jellybins.Binary
{
    internal static class ExceptionsController
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
    }
}
