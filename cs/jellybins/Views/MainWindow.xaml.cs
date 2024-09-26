using jellybins.Views;
using jellybins.Processing;

using Microsoft.Win32;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Wpf.Ui.Controls;
using System.IO;
using Wpf.Ui.Extensions;
using System.Diagnostics;
using Accessibility;
using System;
using jellybins.Models;

namespace jellybins
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
        
        private void open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog()
            {
                Title = "Открытие двоичного файла",
                Filter = "Динамическая библиотека|*.dll|Статическая библиотека|*.lib|Приложение|*.exe|Все файлы|*.*",
                Multiselect = false,
            };

            if (dlg.ShowDialog() == true && dlg.FileName != string.Empty)
            {
                if (string.IsNullOrEmpty(dlg.FileName))
                    return;

                var requirements = new ProjectWindow();
                requirements.ShowDialog();

                // Checking for: loading 2 pages/1 page
                if (!requirements.OnlyHeaderContentRequired)
                    CreateProceduresList(dlg.FileName);

                CreateHeaderContent();
            }
        }

        private void CreateHeaderContent() 
        {

        }

#pragma warning disable
        private void CreateProceduresList(string path)
        {
            var page = new BinaryProceduresPage();
            if (NetComponentProcedures.Create(path, ref page))
                frame.Content = page;

            // Если никто из всех возможных моделей не вернет True,
            // Вернуть справку и поддержку
        }

        private void FluentWindow_SizeChanged(object sender, SizeChangedEventArgs e) 
        {
        }

        private void callGC_Click(object sender, RoutedEventArgs e)
        {
            long before = GC.GetGCMemoryInfo().HeapSizeBytes;
            
            frame.Content = null!;
            while (frame.NavigationService.RemoveBackEntry() != null);

            GC.Collect();
            GC.WaitForPendingFinalizers();

            long after = GC.GetGCMemoryInfo().HeapSizeBytes;
            
#if DEBUG
            var msg = new Wpf.Ui.Controls.MessageBox()
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