using System.ComponentModel;
using System.IO;
using System.Windows;
using jellybins.File.Modeling;
using jellybins.Middleware;

using Microsoft.Win32;

using Wpf.Ui.Controls;
using MessageBox = Wpf.Ui.Controls.MessageBox;


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
            //JbConfigReader.Read();
        }
        
        /// <summary>
        /// При нажатии на кнопку "Открыть"
        /// Если указан файл, выводит диалог с выбором анализа
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new()
            {
                Title = "Открытие двоичного файла",
                FilterIndex = 4,
                Filter = "Динамическая библиотека|*.dll|Статическая библиотека|*.lib|Приложение|*.exe|Все файлы|*.*",
                Multiselect = false,
            };

            if (dlg.ShowDialog() != true || dlg.FileName == string.Empty) return;
            if (string.IsNullOrEmpty(dlg.FileName)) return;

            ProjectWindow requirements = new();
            requirements.ShowDialog();
            
            if (!requirements.OnlyHeaderContentRequired)
                CreateSectionsTable(/*dlg.FileName*/);
            
            // По любому создастся окно основных свойств
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
            
            ExecutableAnalyser analysing  = ExecutableAnalyser.Instance().Get(path);
            ExecutableAnalyser reference  = ExecutableAnalyser.Instance().Set(@"C:\Windows\explorer.exe");
            
            // Главная таблица
            hPage.bintype.Text = FileTypeInformation.GetTitle(analysing.Chars.Type);
            hPage.binprops.Text = FileTypeInformation.GetInformation(analysing.Chars.Type);
            hPage.IsCompat.Text = ExecutableAnalyser.EqualsToString(analysing.Chars, reference.Chars);
            hPage.OsRequiredLabel.Text = analysing.Chars.Os;
            hPage.ThisOsLabel.Text = reference.Chars.Os;
            hPage.ThisOsVersionLabel.Text = reference.Chars.MajorVersion + "." + reference.Chars.MinorVersion;
            hPage.OsVerLabel.Text = analysing.Chars.MajorVersion + "." + analysing.Chars.MinorVersion;
            hPage.ArchRequiredLabel.Text = analysing.Chars.Cpu;
            hPage.ThisArchLabel.Text = reference.Chars.Cpu;
            
            // Характеристики
            PageFiller.SetFlags(analysing.View!.Flags, ref hPage);

            frame.Content = hPage;
        }

        
        /// <summary>
        /// Создает страницу со списком функций
        /// </summary>
        private static void CreateSectionsTable(/*string path*/)
        {
            new MessageBox()
            {
                Title = "Jellybins",
                Content = "Страница с таблицей секций в разработке."
            }.ShowDialogAsync();
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
            _ =  new MessageBox()
            {
                Title = "Jelly Bins",
                Content = @$"Рассчет кучи: {
                    Math.Round((after - before) / Math.Pow(1024, 2), 3)
                } Мб",
            }.ShowDialogAsync();
            frame.Content = new AboutPage();
            #endif
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            // Временное решение неувязок с памятью
            // Обязательно будет исправлено
            Application.Current.Shutdown();
        }
    }
}