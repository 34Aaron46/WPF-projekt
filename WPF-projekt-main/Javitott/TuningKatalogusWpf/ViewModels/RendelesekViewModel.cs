using System.Collections.ObjectModel;
using System.Windows.Input;
using TuningKatalogusWpf.Models;

namespace TuningKatalogusWpf.ViewModels
{
    public class RendelesekViewModel : ViewModelBase
    {
        public ObservableCollection<Rendeles> Rendelesek { get; } = new ObservableCollection<Rendeles>();
        public ObservableCollection<Termek> Termekek { get; } = new ObservableCollection<Termek>();
        public List<string> AllapotLista { get; } = new List<string> { "Új", "Feldolgozás alatt", "Kiszállítva", "Törölve" };

        private Rendeles _kivalasztottRendeles = new Rendeles();
        public Rendeles KivalasztottRendeles
        {
            get { return _kivalasztottRendeles; }
            set
            {
                // ha nincs kijeloles akkor ures rendelest hasznalunk
                if (value == null)
                {
                    value = new Rendeles();
                }
                _kivalasztottRendeles = value;
                OnPropertyChanged(nameof(KivalasztottRendeles));
            }
        }

        public ICommand UjCommand { get; }
        public ICommand MentesCommand { get; }
        public ICommand TorlesCommand { get; }
        public ICommand AllapotCommand { get; }

        public RendelesekViewModel()
        {
            UjCommand = new RelayCommand(Uj);
            MentesCommand = new RelayCommand(Mentes);
            TorlesCommand = new RelayCommand(Torles);
            AllapotCommand = new RelayCommand(AllapotValtas);
        }

        public async Task Betoltes()
        {
            try
            {
                var termekek = await ApiKliens.Get<List<Termek>>("termekek");
                Termekek.Clear();
                foreach (var termek in termekek!)
                {
                    Termekek.Add(termek);
                }

                var rendelesek = await ApiKliens.Get<List<Rendeles>>("rendelesek");
                Rendelesek.Clear();
                foreach (var rendeles in rendelesek!)
                {
                    Rendelesek.Add(rendeles);
                }
                Uzenet = Rendelesek.Count + " rendelés betöltve";
            }
            catch (Exception ex)
            {
                Uzenet = "Hiba: " + ex.Message;
            }
        }

        private void Uj()
        {
            KivalasztottRendeles = new Rendeles();
        }

        private async void Mentes()
        {
            if (string.IsNullOrWhiteSpace(KivalasztottRendeles.VevoNev))
            {
                Uzenet = "Add meg a vevő nevét!";
                return;
            }
            if (KivalasztottRendeles.TermekId == 0)
            {
                Uzenet = "Válassz ki egy terméket!";
                return;
            }
            if (KivalasztottRendeles.Darab <= 0)
            {
                Uzenet = "A darabszám legyen legalább 1!";
                return;
            }

            try
            {
                if (KivalasztottRendeles.Id == 0)
                {
                    await ApiKliens.Post("rendelesek", KivalasztottRendeles);
                }
                else
                {
                    await ApiKliens.Put("rendelesek/" + KivalasztottRendeles.Id, KivalasztottRendeles);
                }
                await Betoltes();
                KivalasztottRendeles = new Rendeles();
                Uzenet = "Mentve!";
            }
            catch (Exception ex)
            {
                Uzenet = "Hiba: " + ex.Message;
            }
        }

        private async void Torles()
        {
            if (KivalasztottRendeles.Id == 0)
            {
                Uzenet = "Előbb válassz ki egy rendelést!";
                return;
            }
            try
            {
                await ApiKliens.Delete("rendelesek/" + KivalasztottRendeles.Id);
                await Betoltes();
                KivalasztottRendeles = new Rendeles();
                Uzenet = "Törölve!";
            }
            catch (Exception ex)
            {
                Uzenet = "Hiba: " + ex.Message;
            }
        }

        // csak az allapotot irja at (sajat api vegpont)
        private async void AllapotValtas()
        {
            if (KivalasztottRendeles.Id == 0)
            {
                Uzenet = "Előbb válassz ki egy rendelést!";
                return;
            }
            try
            {
                string allapot = Uri.EscapeDataString(KivalasztottRendeles.Allapot);
                await ApiKliens.Put("rendelesek/" + KivalasztottRendeles.Id + "/allapot/" + allapot, null);
                await Betoltes();
                Uzenet = "Állapot átírva";
            }
            catch (Exception ex)
            {
                Uzenet = "Hiba: " + ex.Message;
            }
        }
    }
}