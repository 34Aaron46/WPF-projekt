using System.Collections.ObjectModel;
using System.Windows.Input;
using BoostBayWpf.Models;

namespace BoostBayWpf.ViewModels
{
    public class OrdersViewModel : ViewModelBase
    {
        public ObservableCollection<OrderDto> Orders { get; set; } = new ObservableCollection<OrderDto>();
        public ObservableCollection<ProductDto> Products { get; set; } = new ObservableCollection<ProductDto>();
        public List<string> StatusList { get; set; } = new List<string> { "Új", "Feldolgozás alatt", "Kiszállítva", "Törölve" };

        private OrderDto _selectedOrder = new OrderDto();
        public OrderDto SelectedOrder
        {
            get { return _selectedOrder; }
            set
            {
                if (value == null)
                {
                    value = new OrderDto();
                }
                _selectedOrder = value;
                OnPropertyChanged("SelectedOrder");
            }
        }

        public ICommand NewCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand StatusCommand { get; set; }

        public OrdersViewModel()
        {
            NewCommand = new RelayCommand(NewOrder);
            SaveCommand = new RelayCommand(SaveOrder);
            DeleteCommand = new RelayCommand(DeleteOrder);
            StatusCommand = new RelayCommand(ChangeStatus);
        }

        public override async void Load()
        {
            try
            {
                var products = await ApiService.Get<List<ProductDto>>("products");
                Products.Clear();
                foreach (var p in products!)
                {
                    Products.Add(p);
                }
                var orders = await ApiService.Get<List<OrderDto>>("orders");
                Orders.Clear();
                foreach (var o in orders!)
                {
                    Orders.Add(o);
                }
                Status = Orders.Count + " rendelés betöltve";
            }
            catch (Exception ex)
            {
                Status = "Hiba: " + ex.Message;
            }
        }

        private void NewOrder()
        {
            SelectedOrder = new OrderDto();
        }

        private async void SaveOrder()
        {
            try
            {
                if (SelectedOrder.Id == 0)
                {
                    await ApiService.Post("orders", SelectedOrder);
                }
                else
                {
                    await ApiService.Put("orders/" + SelectedOrder.Id, SelectedOrder);
                }
                Load();
                SelectedOrder = new OrderDto();
                Status = "Mentve!";
            }
            catch (Exception ex)
            {
                Status = "Hiba: " + ex.Message;
            }
        }

        private async void DeleteOrder()
        {
            try
            {
                await ApiService.Delete("orders/" + SelectedOrder.Id);
                Load();
                SelectedOrder = new OrderDto();
                Status = "Törölve!";
            }
            catch (Exception ex)
            {
                Status = "Hiba: " + ex.Message;
            }
        }

        private async void ChangeStatus()
        {
            try
            {
                await ApiService.Put("orders/" + SelectedOrder.Id + "/status/" + SelectedOrder.Status, null);
                Load();
                Status = "Állapot átírva";
            }
            catch (Exception ex)
            {
                Status = "Hiba: " + ex.Message;
            }
        }
    }
}
