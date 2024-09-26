using jellybins.Models;
using jellybins.Views;
using System.Drawing;
using System.Security.Principal;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Wpf.Ui.Controls;

namespace jellybins.Processing
{
    class NetComponentProcedures
    {

        [Obsolete]
        public static bool Create(string assembly, ref BinaryProceduresPage netp) 
        {
            netp.libmethods.Items.Clear();
            Models.NetComponentHeader netl = new Models.NetComponentHeader(assembly);
            
            if (!netl.Loaded)
            {
                new MessageBox()
                {
                    Title = "Jelly Bins",
                    Content = "Отказано в доступе",
                }.ShowDialogAsync();
                
                return netl.Loaded; // пока что будет так.
            }

            netp.libadd.Text  = netl.Description;
            netp.libname.Text = netl.Name;
            netp.libtype.Text = netl.Title;
            
            foreach (var item in netl.Types)
            {
                netp.libmethods.Items.Add(new Wpf.Ui.Controls.TextBlock()
                {
                    TextWrapping = System.Windows.TextWrapping.Wrap,
                    Foreground = Brushes.Cyan,
                    FontFamily = new FontFamily("Consolas"),
                    Text = item
                });
            }

            foreach (var item in netl.Methods)
            {
                netp.libmethods.Items.Add(new Wpf.Ui.Controls.TextBlock()
                {
                    TextWrapping = System.Windows.TextWrapping.Wrap,
                    Foreground = Brushes.Violet,
                    FontFamily = new FontFamily("Consolas"),
                    Text = item
                });
            }

            Models.NetComponentHeader.DestroyaMembersCollection(ref netl);
            // NetAssemblyReport.Destroy(ref netp); // NetAssemblyPage гомункул.
            return true;
        }

        public static Task<BinaryProceduresPage> CreateAsync(string path) 
        {
            Models.NetComponentHeader netl = new Models.NetComponentHeader(path);
            BinaryProceduresPage netp = new() { };

            netp.libmethods.Items.Clear();

            if (!netl.Loaded) 
            {
                new MessageBox()
                {
                    Title = "Jelly Bins",
                    Content = "Отказано в доступе",
                }.ShowDialogAsync();
                return Task.FromResult(netp);
            }

            //Parallel.ForEach(netl.Types, item => 
            //{
            //    netp.libmethods.Items.Add(new Wpf.Ui.Controls.TextBlock()
            //    {
            //        TextWrapping = System.Windows.TextWrapping.Wrap,
            //        Foreground = Brushes.Cyan,
            //        FontFamily = new FontFamily("Consolas"),
            //        Text = item
            //    });
            //});

            //Parallel.ForEach(netl.Methods, item => 
            //{
            //    netp.libmethods.Items.Add(new Wpf.Ui.Controls.TextBlock()
            //    {
            //        TextWrapping = System.Windows.TextWrapping.Wrap,
            //        Foreground = Brushes.Violet,
            //        FontFamily = new FontFamily("Consolas"),
            //        Text = item
            //    });
            //});

            netp.libadd.Text = netl.Description;
            netp.libname.Text = netl.Name;
            netp.libtype.Text = netl.Title;

            foreach (var item in netl.Methods)
            {
                netp.libmethods.Items.Add(new Wpf.Ui.Controls.TextBlock()
                {
                    TextWrapping = System.Windows.TextWrapping.Wrap,
                    Foreground = Brushes.Cyan,
                    FontFamily = new FontFamily("Consolas"),
                    Text = item
                });
            }

            return Task.FromResult(netp);
        }

        public static bool Destroy (ref BinaryProceduresPage page)
        {
            if (page == null)
                return false;

            page.libmethods.Items.Clear();
            page.libname.Text = string.Empty;
            page.libadd.Text = string.Empty;
            page.libtype.Text = string.Empty;
            
            return true;
        }
    }
}
