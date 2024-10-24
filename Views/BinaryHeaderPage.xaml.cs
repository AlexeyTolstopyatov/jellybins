using System.Windows;
using System.Windows.Controls;

namespace jellybins.Views
{
    /// <summary>
    /// Логика взаимодействия для BinaryHeaderPage.xaml
    /// </summary>
    public partial class BinaryHeaderPage : Page
    {
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
