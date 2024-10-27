using System.Windows;
using System.Windows.Controls;
using jellybins.Models;

namespace jellybins.Views
{
    /// <summary>
    /// Логика взаимодействия для BinaryHeaderPage.xaml
    /// </summary>
    public partial class BinaryHeaderPage : Page
    {
        public List<string> FlagNamesSource { get; set; } = new();
        public List<string> FlagsSource { get; set; } = new();
        public List<JbStringHead> ListViewsSources = new();
        
        public BinaryHeaderPage()
        {
            InitializeComponent();
        }

        private void BinaryHeaderPage_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            MainCard.MaxHeight = WindowHeight - 30;
        }
    }
}
