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

        // amikor a ful megjelenik betolti az adatokat az api-bol
        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = (RendelesekViewModel)DataContext;
            await viewModel.Betoltes();
        }
    }
}