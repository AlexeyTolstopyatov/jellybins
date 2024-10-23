using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using Wpf.Ui.Controls;

namespace jellybins.Views
{
    /// <summary>
    /// Логика взаимодействия для ProjectWindow.xaml
    /// </summary>
    public partial class ProjectWindow : FluentWindow
    {
        public bool OnlyHeaderContentRequired
        {
            get;
            private set;
        } = true;

        public ProjectWindow()
        {
            InitializeComponent();
        }

        private void header_Click(object sender, RoutedEventArgs e)
        { 
            OnlyHeaderContentRequired = true;
            Close();
        }

        private void full_Click(object sender, RoutedEventArgs e)
        {
            OnlyHeaderContentRequired = false;
            Close();
        }
        
    }
}
