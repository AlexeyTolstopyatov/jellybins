using System.Windows;
using System.Windows.Controls;

namespace jellybins.Views
{
    /// <summary>
    /// Логика взаимодействия для NetAssemblyPage.xaml
    /// </summary>
    public partial class BinarySectionTablesPage : Page
    {
        public BinarySectionTablesPage()
        {
            InitializeComponent();
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // libmethods.MaxHeight = this.WindowHeight -230;
            // libmethods.Height = this.Height;
        }
    }
}
