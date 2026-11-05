using System.Collections.ObjectModel;
using System.Windows.Input;
using BoostBayWpf.Models;

namespace BoostBayWpf.ViewModels
{
    public class ProductsViewModel : ViewModelBase
    {
        public ObservableCollection<ProductDto> Products { get; set; } = new ObservableCollection<ProductDto>();
        public ObservableCollection<CategoryDto> Categories { get; set; } = new ObservableCollection<CategoryDto>();

        private ProductDto _selectedProduct = new ProductDto();
        public ProductDto SelectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                if (value == null)
                {
                    value = new ProductDto();
                }
                _selectedProduct = value;
                OnPropertyChanged("SelectedProduct");
            }
        }

        public string SearchText { get; set; } = "";

        public ICommand LoadCommand { get; set; }
        public ICommand NewCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand RestockCommand { get; set; }

        public ProductsViewModel()
        {
            LoadCommand = new RelayCommand(Load);
            NewCommand = new RelayCommand(NewProduct);
            SaveCommand = new RelayCommand(SaveProduct);
            DeleteCommand = new RelayCommand(DeleteProduct);
            SearchCommand = new RelayCommand(Search);
            RestockCommand = new RelayCommand(Restock);
        }

        public override async void Load()
        {
            try
            {
                var categories = await ApiService.Get<List<CategoryDto>>("categories");
                Categories.Clear();
                foreach (var c in categories!)
                {
                    Categories.Add(c);
                }
                var products = await ApiService.Get<List<ProductDto>>("products");
                Products.Clear();
                foreach (var p in products!)
                {
                    Products.Add(p);
                }
                Status = Products.Count + " termék betöltve";
            }
            catch (Exception ex)
            {
                Status = "Hiba: " + ex.Message;
            }
        }

        private void NewProduct()
        {
            SelectedProduct = new ProductDto();
        }

        private async void SaveProduct()
        {
            try
            {
                if (SelectedProduct.Id == 0)
                {
                    await ApiService.Post("products", SelectedProduct);
                }
                else
                {
                    await ApiService.Put("products/" + SelectedProduct.Id, SelectedProduct);
                }
                Load();
                SelectedProduct = new ProductDto();
                Status = "Mentve!";
            }
            catch (Exception ex)
            {
                Status = "Hiba: " + ex.Message;
            }
        }

        private async void DeleteProduct()
        {
            try
            {
                await ApiService.Delete("products/" + SelectedProduct.Id);
                Load();
                SelectedProduct = new ProductDto();
                Status = "Törölve!";
            }
            catch (Exception ex)
            {
                Status = "Hiba: " + ex.Message;
            }
        }

        private async void Search()
        {
            try
            {
                if (SearchText == "")
                {
                    Load();
                    return;
                }
                var found = await ApiService.Get<List<ProductDto>>("products/search/" + SearchText);
                Products.Clear();
                foreach (var p in found!)
                {
                    Products.Add(p);
                }
                Status = Products.Count + " találat";
            }
            catch (Exception ex)
            {
                Status = "Hiba: " + ex.Message;
            }
        }

        // 10 darabbal növeli a készletet
        private async void Restock()
        {
            try
            {
                await ApiService.Put("products/" + SelectedProduct.Id + "/restock/10", null);
                Load();
                Status = "Készlet +10";
            }
            catch (Exception ex)
            {
                Status = "Hiba: " + ex.Message;
            }
        }
    }
}
