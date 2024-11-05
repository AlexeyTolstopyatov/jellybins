using System.Collections.Specialized;
using System.IO;
using System.Windows;
using System.Windows.Media;
using jellybins.Config;
using jellybins.File.Modeling;
using jellybins.File.Modeling.Controls;
using jellybins.Middleware;

using Microsoft.Win32;

using Wpf.Ui.Controls;
using Wpf.Ui.Extensions;


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
                CreateSectionsTable(dlg.FileName);
            
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
            
            Analyser analysing  = Analyser.Get(path);
            Analyser reference  = Analyser.Set(@"C:\Windows\explorer.exe");
            
            // Главная таблица
            hPage.bintype.Text = FileTypeInformation.GetTitle(analysing.Result.Type);
            hPage.binprops.Text = FileTypeInformation.GetInformation(analysing.Result.Type);
            hPage.IsCompat.Text = Analyser.EqualsToString(analysing.Result, reference.Result);
            hPage.OsRequiredLabel.Text = analysing.Result.Os;
            hPage.ThisOsLabel.Text = reference.Result.Os;
            hPage.ThisOsVersionLabel.Text = reference.Result.MajorVersion + "." + reference.Result.MinorVersion;
            hPage.OsVerLabel.Text = analysing.Result.MajorVersion + "." + analysing.Result.MinorVersion;
            hPage.ArchRequiredLabel.Text = analysing.Result.Cpu;
            hPage.ThisArchLabel.Text = reference.Result.Cpu;
            
            // Характеристики
            PageFiller.SetFlags(analysing.View!.Flags, ref hPage);
            
            frame.Content = hPage;
        }

        
        /// <summary>
        /// Создает страницу со списком функций
        /// </summary>
        /// <param name="path"></param>
        private void CreateSectionsTable(string path)
        {
            
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