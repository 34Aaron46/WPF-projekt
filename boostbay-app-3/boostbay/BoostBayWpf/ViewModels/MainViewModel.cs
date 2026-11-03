using System.Windows.Input;

namespace BoostBayWpf.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private ProductsViewModel products = new ProductsViewModel();
        private CategoriesViewModel categories = new CategoriesViewModel();
        private OrdersViewModel orders = new OrdersViewModel();

        private ViewModelBase _current;
        public ViewModelBase Current
        {
            get { return _current; }
            set { _current = value; OnPropertyChanged("Current"); }
        }

        public ICommand ProductsCommand { get; set; }
        public ICommand CategoriesCommand { get; set; }
        public ICommand OrdersCommand { get; set; }

        public MainViewModel()
        {
            ProductsCommand = new RelayCommand(() => Show(products));
            CategoriesCommand = new RelayCommand(() => Show(categories));
            OrdersCommand = new RelayCommand(() => Show(orders));
            _current = products;
            Show(products);
        }

        private void Show(ViewModelBase vm)
        {
            Current = vm;
            vm.Load();
        }
    }
}
