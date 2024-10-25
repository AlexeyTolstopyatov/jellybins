using System.IO;
using System.Windows;
using jellybins.Config;
using jellybins.Middleware;
using Microsoft.Win32;
using Wpf.Ui.Controls;

namespace jellybins.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : FluentWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            JbConfigReader.Read();
            frame.Content = new AboutPage();
        }

        /// <summary>
        /// При нажатии на кнопку "Открыть"
        /// Если указан файл, выводит диалог с выбором анализа
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new()
            {
                Title = "Открытие двоичного файла",
                FilterIndex = 3,
                Filter = "Динамическая библиотека|*.dll|Статическая библиотека|*.lib|Приложение|*.exe|Все файлы|*.*",
                Multiselect = false,
            };

            if (dlg.ShowDialog() != true || dlg.FileName == string.Empty) return;
            if (string.IsNullOrEmpty(dlg.FileName)) return;

            ProjectWindow requirements = new();
            requirements.ShowDialog();

            //Checking for: loading 2 pages/1 page
            if (!requirements.OnlyHeaderContentRequired)
                CreateProceduresList(dlg.FileName);
            else
                CreateHeaderContent(dlg.FileName);
        }
        #region Business Logic ware
        /// <summary>
        /// Создает страницу с общей информацией о файле
        /// </summary>
        /// <param name="path"></param>
        private void CreateHeaderContent(string path) 
        {
            BinaryHeaderPage hPage = new() 
            {
                binname =
                {
                    Text = new FileInfo(path).Name
                },
                binpath =
                {
                    Text = path
                }
            };
            frame.Content = hPage;

            JbAnalyser analysing = JbAnalyser.Get(path, ref hPage);
            JbAnalyser reference = JbAnalyser.Set(@"C:\Windows\explorer.exe", ref hPage);
            
            hPage.IsCompat.Text = JbAnalyser.EqualsToString(
                analysing.Characteristics,
                reference.Characteristics);
        }

        
        /// <summary>
        /// Создает страницу со списком функций
        /// </summary>
        /// <param name="path"></param>
        private void CreateProceduresList(string path)
        {
            BinaryProceduresPage page = new();
            if (JbCommonReport.TryParseNetComponentMethods(path, ref page))
                frame.Content = page;
        }
        #endregion
        private void FluentWindow_SizeChanged(object sender, SizeChangedEventArgs e) 
        {
        }
        
#nullable disable
        private void callGC_Click(object sender, RoutedEventArgs e)
        {
            long before = 
                GC.GetGCMemoryInfo().HeapSizeBytes;
            
            frame.Content = null;
            while (frame.NavigationService.RemoveBackEntry() != null)
            {
            }

            GC.Collect();
            long after = 
                GC.GetGCMemoryInfo().HeapSizeBytes;
            
            #if DEBUG
            _ =  new Wpf.Ui.Controls.MessageBox()
            {
                Title = "Jelly Bins",
                Content = @$"Рассчет кучи: {
                    Math.Round((after - before) / Math.Pow(1024, 2), 3)
                } Мб",
            }.ShowDialogAsync();
            frame.Content = new AboutPage();
            #endif
        }
    }
}