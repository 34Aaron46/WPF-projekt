using System.Windows;
using System.Windows.Controls;
using TuningKatalogusWpf.ViewModels;

namespace TuningKatalogusWpf.Views
{
    public partial class KategoriakView : UserControl
    {
        public KategoriakView()
        {
            InitializeComponent();
        }

        // amikor a ful megjelenik betolti az adatokat az api-bol
        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = (KategoriakViewModel)DataContext;
            await viewModel.Betoltes();
        }
    }
}