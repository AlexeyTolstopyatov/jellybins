using System.IO;
using System.Runtime.InteropServices;
using System.Windows;

using jellybins.Binary;
using jellybins.Middleware;
using jellybins.Models;
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

            if (dlg.ShowDialog() == true && dlg.FileName != string.Empty)
            {
                if (string.IsNullOrEmpty(dlg.FileName))
                    return;

                ProjectWindow requirements = new();
                requirements.ShowDialog();

                //Checking for: loading 2 pages/1 page
                if (!requirements.OnlyHeaderContentRequired)
                    CreateProceduresList(dlg.FileName);
                else
                    CreateHeaderContent(dlg.FileName);
            }
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

            ushort mzSign = BinarySearcher.GetUInt16(path, 0);
            if (BinaryInformation.DetectType(mzSign) == BinaryType.Other)
            {
                BinaryReportCreator.TryParseUnknown(ref hPage, mzSign);
                return;
            }
            
            MzHeader mz = new();
            BinarySearcher.Fill(ref path, ref mz);
            BinaryReportCreator.TryParseMzHeader(ref hPage, ref mz);
            
            ushort word = BinarySearcher.GetUInt16(path, mz.e_lfanew);
            
            switch (BinaryInformation.DetectType(word))
            {
                case BinaryType.Linear:
                    LeHeader le = new();
                    BinarySearcher.Fill(ref path, ref le, (int)mz.e_lfanew);
                    BinaryReportCreator.TryParseLeHeader(ref hPage, ref le);
                    break;
                case BinaryType.New:
                    NeHeader ne = new();
                    BinarySearcher.Fill(ref path, ref ne, (int)mz.e_lfanew);
                    BinaryReportCreator.TryParseNeHeader(ref hPage, ref ne);
                    break;
                case BinaryType.Portable:
                    NtHeader nt = new();
                    BinarySearcher.Fill(ref path, ref nt, (int)mz.e_lfanew);
                    BinaryReportCreator.TryParsePortableHeader(ref hPage, ref nt);
                    
                    // Если внутри PE файла есть сведения о CIL/CLR Надо узнать где они будут
                    // располагаться.
                    // Смещение CLR заголовка зависит от разрядности файла, как и положение
                    // всех сегментов/структур (стоящих после PE заголовка)
                    int offset = nt.WinNtOptional.Magic switch
                    {
                        0x10b => 80,
                        0x20b => 200,
                        _ => 0
                    };
                    if (BinarySearcher.GetUInt32(path, offset) != 0)
                    {
                        ClrHeader clr = new();
                        BinarySearcher.Fill(ref path, ref clr);
                        BinaryReportCreator.TryParseCommonLangRuntimeHeader(ref hPage, ref clr);
                    }
                    break;
            }
            
        }

        
        /// <summary>
        /// Создает страницу со списком функций
        /// </summary>
        /// <param name="path"></param>
        private void CreateProceduresList(string path)
        {
            BinaryProceduresPage page = new();
            if (BinaryReportCreator.TryParseNetComponentMethods(path, ref page))
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
                    Math.Round((before - after) / Math.Pow(1024, 2), 3)
                } Мб",
            }.ShowDialogAsync();
            #endif
        }
    }
}