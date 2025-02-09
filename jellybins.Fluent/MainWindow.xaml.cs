using MicaWPF.Controls;
using Wpf.Ui;
using Wpf.Ui.Appearance;

namespace jellybins.Fluent
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MicaWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            ApplicationThemeManager.ApplySystemTheme();
            
        }
    }
}