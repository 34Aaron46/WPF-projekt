using System.Collections.ObjectModel;
using System.Windows.Input;
using TuningKatalogusWpf.Models;

namespace TuningKatalogusWpf.ViewModels
{
    public class KategoriakViewModel : ViewModelBase
    {
        public ObservableCollection<Kategoria> Kategoriak { get; } = new ObservableCollection<Kategoria>();
        public ObservableCollection<Termek> KategoriaTermekek { get; } = new ObservableCollection<Termek>();

        private Kategoria _kivalasztottKategoria = new Kategoria();
        public Kategoria KivalasztottKategoria
        {
            get { return _kivalasztottKategoria; }
            set
            {
                // ha nincs kijeloles akkor ures kategoriat hasznalunk
                if (value == null)
                {
                    value = new Kategoria();
                }
                _kivalasztottKategoria = value;
                OnPropertyChanged(nameof(KivalasztottKategoria));
                TermekekBetoltese();
            }
        }

        public ICommand UjCommand { get; }
        public ICommand MentesCommand { get; }
        public ICommand TorlesCommand { get; }

        public KategoriakViewModel()
        {
            UjCommand = new RelayCommand(Uj);
            MentesCommand = new RelayCommand(Mentes);
            TorlesCommand = new RelayCommand(Torles);
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
                Uzenet = Kategoriak.Count + " kategória betöltve";
            }
            catch (Exception ex)
            {
                Uzenet = "Hiba: " + ex.Message;
            }
        }

        // a kijelolt kategoria termekeit kerjuk le (sajat api vegpont)
        private async void TermekekBetoltese()
        {
            KategoriaTermekek.Clear();
            if (KivalasztottKategoria.Id == 0)
            {
                return;
            }
            try
            {
                var termekek = await ApiKliens.Get<List<Termek>>("kategoriak/" + KivalasztottKategoria.Id + "/termekek");
                foreach (var termek in termekek!)
                {
                    KategoriaTermekek.Add(termek);
                }
            }
            catch (Exception ex)
            {
                Uzenet = "Hiba: " + ex.Message;
            }
        }

        private void Uj()
        {
            KivalasztottKategoria = new Kategoria();
        }

        private async void Mentes()
        {
            if (string.IsNullOrWhiteSpace(KivalasztottKategoria.Nev))
            {
                Uzenet = "Add meg a kategória nevét!";
                return;
            }

            try
            {
                if (KivalasztottKategoria.Id == 0)
                {
                    await ApiKliens.Post("kategoriak", KivalasztottKategoria);
                }
                else
                {
                    await ApiKliens.Put("kategoriak/" + KivalasztottKategoria.Id, KivalasztottKategoria);
                }
                await Betoltes();
                KivalasztottKategoria = new Kategoria();
                Uzenet = "Mentve!";
            }
            catch (Exception ex)
            {
                Uzenet = "Hiba: " + ex.Message;
            }
        }

        private async void Torles()
        {
            if (KivalasztottKategoria.Id == 0)
            {
                Uzenet = "Előbb válassz ki egy kategóriát!";
                return;
            }
            try
            {
                await ApiKliens.Delete("kategoriak/" + KivalasztottKategoria.Id);
                await Betoltes();
                KivalasztottKategoria = new Kategoria();
                Uzenet = "Törölve!";
            }
            catch (Exception ex)
            {
                Uzenet = "Hiba: " + ex.Message;
            }
        }
    }
}