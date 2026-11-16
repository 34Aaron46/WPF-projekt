using System.ComponentModel;

namespace TuningKatalogusWpf.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string nev)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nev));
        }

        // ez jelenik meg az also sorban
        private string _uzenet = "";
        public string Uzenet
        {
            get { return _uzenet; }
            set
            {
                _uzenet = value;
                OnPropertyChanged(nameof(Uzenet));
            }
        }
    }
}