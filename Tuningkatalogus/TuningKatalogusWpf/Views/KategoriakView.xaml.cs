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

        // amikor a fül megjelenik, betöltjük az aktuális adatokat az API-ból
        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = (KategoriakViewModel)DataContext;
            await viewModel.Betoltes();
        }
    }
}
