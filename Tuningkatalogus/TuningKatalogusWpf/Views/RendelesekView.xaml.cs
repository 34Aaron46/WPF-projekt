using System.Windows;
using System.Windows.Controls;
using TuningKatalogusWpf.ViewModels;

namespace TuningKatalogusWpf.Views
{
    public partial class RendelesekView : UserControl
    {
        public RendelesekView()
        {
            InitializeComponent();
        }

        // amikor a fül megjelenik, betöltjük az aktuális adatokat az API-ból
        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = (RendelesekViewModel)DataContext;
            await viewModel.Betoltes();
        }
    }
}
