using System.Collections.ObjectModel;
using System.Windows.Input;
using TuningKatalogusWpf.Models;

namespace TuningKatalogusWpf.ViewModels
{
    public class TermekekViewModel : ViewModelBase
    {
        public ObservableCollection<Termek> Termekek { get; } = new ObservableCollection<Termek>();
        public ObservableCollection<Kategoria> Kategoriak { get; } = new ObservableCollection<Kategoria>();

        private Termek _kivalasztottTermek = new Termek();
        public Termek KivalasztottTermek
        {
            get { return _kivalasztottTermek; }
            set
            {
                // ha nincs kijeloles akkor ures termeket hasznalunk
                if (value == null)
                {
                    value = new Termek();
                }
                _kivalasztottTermek = value;
                OnPropertyChanged(nameof(KivalasztottTermek));
            }
        }

        public string KeresoSzoveg { get; set; } = "";

        public ICommand OsszesCommand { get; }
        public ICommand UjCommand { get; }
        public ICommand MentesCommand { get; }
        public ICommand TorlesCommand { get; }
        public ICommand KeresesCommand { get; }
        public ICommand KeszletCommand { get; }

        public TermekekViewModel()
        {
            OsszesCommand = new RelayCommand(Osszes);
            UjCommand = new RelayCommand(Uj);
            MentesCommand = new RelayCommand(Mentes);
            TorlesCommand = new RelayCommand(Torles);
            KeresesCommand = new RelayCommand(Kereses);
            KeszletCommand = new RelayCommand(KeszletNoveles);
        }

        public async Task Betoltes()
        {
            try
            {
                var kategoriak = await ApiKliens.Get<List<Kategoria>>("kategoriak");
                Kategoriak.Clear();
                foreach (var kategoria in kategoriak!)
                {
                    Kategoriak.Add(kategoria);
                }

                var termekek = await ApiKliens.Get<List<Termek>>("termekek");
                Termekek.Clear();
                foreach (var termek in termekek!)
                {
                    Termekek.Add(termek);
                }
                Uzenet = Termekek.Count + " termék betöltve";
            }
            catch (Exception ex)
            {
                Uzenet = "Hiba: " + ex.Message;
            }
        }

        private async void Osszes()
        {
            KeresoSzoveg = "";
            OnPropertyChanged(nameof(KeresoSzoveg));
            await Betoltes();
        }

        private void Uj()
        {
            KivalasztottTermek = new Termek();
        }

        private async void Mentes()
        {
            if (string.IsNullOrWhiteSpace(KivalasztottTermek.Nev))
            {
                Uzenet = "Add meg a termék nevét!";
                return;
            }
            if (KivalasztottTermek.KategoriaId == 0)
            {
                Uzenet = "Válassz ki egy kategóriát!";
                return;
            }
            if (KivalasztottTermek.Ar < 0 || KivalasztottTermek.Keszlet < 0)
            {
                Uzenet = "Az ár és a készlet nem lehet negatív!";
                return;
            }

            try
            {
                if (KivalasztottTermek.Id == 0)
                {
                    await ApiKliens.Post("termekek", KivalasztottTermek);
                }
                else
                {
                    await ApiKliens.Put("termekek/" + KivalasztottTermek.Id, KivalasztottTermek);
                }
                await Betoltes();
                KivalasztottTermek = new Termek();
                Uzenet = "Mentve!";
            }
            catch (Exception ex)
            {
                Uzenet = "Hiba: " + ex.Message;
            }
        }

        private async void Torles()
        {
            if (KivalasztottTermek.Id == 0)
            {
                Uzenet = "Előbb válassz ki egy terméket!";
                return;
            }
            try
            {
                await ApiKliens.Delete("termekek/" + KivalasztottTermek.Id);
                await Betoltes();
                KivalasztottTermek = new Termek();
                Uzenet = "Törölve!";
            }
            catch (Exception ex)
            {
                Uzenet = "Hiba: " + ex.Message;
            }
        }

        // kereses a termek neveben (sajat api vegpont)
        private async void Kereses()
        {
            try
            {
                if (KeresoSzoveg == "")
                {
                    await Betoltes();
                    return;
                }
                var talalatok = await ApiKliens.Get<List<Termek>>("termekek/kereses/" + Uri.EscapeDataString(KeresoSzoveg));
                Termekek.Clear();
                foreach (var termek in talalatok!)
                {
                    Termekek.Add(termek);
                }
                Uzenet = Termekek.Count + " találat";
            }
            catch (Exception ex)
            {
                Uzenet = "Hiba: " + ex.Message;
            }
        }

        // 10-zel noveli a kijelolt termek keszletet (sajat api vegpont)
        private async void KeszletNoveles()
        {
            if (KivalasztottTermek.Id == 0)
            {
                Uzenet = "Előbb válassz ki egy terméket!";
                return;
            }
            try
            {
                await ApiKliens.Put("termekek/" + KivalasztottTermek.Id + "/keszlet/10", null);
                await Betoltes();
                Uzenet = "Készlet +10";
            }
            catch (Exception ex)
            {
                Uzenet = "Hiba: " + ex.Message;
            }
        }
    }
}