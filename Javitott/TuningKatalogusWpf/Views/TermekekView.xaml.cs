using System.Windows;
using System.Windows.Controls;
using TuningKatalogusWpf.ViewModels;

namespace TuningKatalogusWpf.Views
{
    public partial class TermekekView : UserControl
    {
        public TermekekView()
        {
            InitializeComponent();
        }

        // amikor a ful megjelenik betolti az adatokat az api-bol
        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = (TermekekViewModel)DataContext;
            await viewModel.Betoltes();
        }
    }
}