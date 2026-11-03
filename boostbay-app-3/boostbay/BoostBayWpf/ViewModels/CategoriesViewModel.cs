using System.Collections.ObjectModel;
using System.Windows.Input;
using BoostBayWpf.Models;

namespace BoostBayWpf.ViewModels
{
    public class CategoriesViewModel : ViewModelBase
    {
        public ObservableCollection<CategoryDto> Categories { get; set; } = new ObservableCollection<CategoryDto>();
        public ObservableCollection<ProductDto> CategoryProducts { get; set; } = new ObservableCollection<ProductDto>();

        private CategoryDto _selectedCategory = new CategoryDto();
        public CategoryDto SelectedCategory
        {
            get { return _selectedCategory; }
            set
            {
                if (value == null)
                {
                    value = new CategoryDto();
                }
                _selectedCategory = value;
                OnPropertyChanged("SelectedCategory");
                LoadCategoryProducts();
            }
        }

        public ICommand NewCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public CategoriesViewModel()
        {
            NewCommand = new RelayCommand(NewCategory);
            SaveCommand = new RelayCommand(SaveCategory);
            DeleteCommand = new RelayCommand(DeleteCategory);
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
                Status = Categories.Count + " kategória betöltve";
            }
            catch (Exception ex)
            {
                Status = "Hiba: " + ex.Message;
            }
        }

        private async void LoadCategoryProducts()
        {
            CategoryProducts.Clear();
            if (SelectedCategory.Id == 0)
            {
                return;
            }
            try
            {
                var products = await ApiService.Get<List<ProductDto>>("categories/" + SelectedCategory.Id + "/products");
                foreach (var p in products!)
                {
                    CategoryProducts.Add(p);
                }
            }
            catch (Exception ex)
            {
                Status = "Hiba: " + ex.Message;
            }
        }

        private void NewCategory()
        {
            SelectedCategory = new CategoryDto();
        }

        private async void SaveCategory()
        {
            try
            {
                if (SelectedCategory.Id == 0)
                {
                    await ApiService.Post("categories", SelectedCategory);
                }
                else
                {
                    await ApiService.Put("categories/" + SelectedCategory.Id, SelectedCategory);
                }
                Load();
                SelectedCategory = new CategoryDto();
                Status = "Mentve!";
            }
            catch (Exception ex)
            {
                Status = "Hiba: " + ex.Message;
            }
        }

        private async void DeleteCategory()
        {
            try
            {
                await ApiService.Delete("categories/" + SelectedCategory.Id);
                Load();
                SelectedCategory = new CategoryDto();
                Status = "Törölve!";
            }
            catch (Exception ex)
            {
                Status = "Hiba: " + ex.Message;
            }
        }
    }
}
