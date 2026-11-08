# Tuning katalógus

Egy autótuning alkatrészeket tartalmazó katalógus adminisztrációs felülete.
A projekt két részből áll:

- **TuningKatalogusApi** – ASP.NET Core Web API (.NET 8), Entity Framework Core, MySQL adatbázis
- **TuningKatalogusWpf** – WPF asztali alkalmazás (MVVM), ami az API-n keresztül kezeli az adatokat

## Szükséges programok

- Visual Studio 2022 (.NET desktop development + ASP.NET workload)
- .NET 8 SDK
- Docker Desktop (a MySQL adatbázishoz)

## Adatbázis

A MySQL Dockerben fut, a `docker-compose.yml` hozza létre.

| Beállítás      | Érték           |
|----------------|-----------------|
| Szerver        | localhost       |
| Port           | 3306            |
| Adatbázis neve | tuningkatalogus |
| Felhasználó    | tuning          |
| Jelszó         | tuning123       |
| Root jelszó    | root123         |

A táblákat és a kezdő adatokat (3 kategória, 5 termék) Code-First megközelítéssel, Entity Framework migrációval hozza létre az API az első induláskor.

Táblák:

- `Kategoriak` (Id, Nev, Leiras)
- `Termekek` (Id, Nev, MuszakiAdat, Ar, Keszlet, KategoriaId → Kategoriak)
- `Rendelesek` (Id, VevoNev, TermekId → Termekek, Darab, Allapot)

## Indítás

1. Adatbázis indítása (a projekt mappájában):
   ```
   docker compose up -d
   ```
   Az első indításnál kb. fél percet kell várni, amíg a `docker compose ps` szerint `healthy` lesz.
2. Migráció létrehozása (csak egyszer kell, ha még nincs `TuningKatalogusApi/Migrations` mappa):
   ```
   dotnet tool install --global dotnet-ef
   cd TuningKatalogusApi
   dotnet ef migrations add InitialCreate
   cd ..
   ```
3. A `TuningKatalogus.sln` megnyitása Visual Studióban.
4. Solution → jobb klikk → *Configure Startup Projects* → *Multiple startup projects*, mindkét projekt legyen `Start`, az API legyen elöl.
5. Indítás (F5). Az API indításkor lefuttatja a migrációt (`Database.Migrate()`). Az API Swagger felülete: http://localhost:5080/swagger

Ha az API nem tud csatlakozni az adatbázishoz, a `TuningKatalogusApi/appsettings.json` fájlban lévő connection stringet kell ellenőrizni.

## API végpontok

Mindhárom végpontban megvan az alap CRUD (GET lista, GET id, POST, PUT, DELETE) és saját függvények is.

### /api/kategoriak
| Metódus | Útvonal | Leírás |
|---------|---------|--------|
| GET | `/api/kategoriak` | kategóriák listája |
| GET | `/api/kategoriak/{id}` | egy kategória |
| POST | `/api/kategoriak` | új kategória |
| PUT | `/api/kategoriak/{id}` | kategória módosítása |
| DELETE | `/api/kategoriak/{id}` | törlés (csak ha nincs benne termék) |
| GET | `/api/kategoriak/{id}/termekek` | **saját:** a kategória termékei |

### /api/termekek
| Metódus | Útvonal | Leírás |
|---------|---------|--------|
| GET | `/api/termekek` | termékek listája |
| GET | `/api/termekek/{id}` | egy termék |
| POST | `/api/termekek` | új termék |
| PUT | `/api/termekek/{id}` | termék módosítása |
| DELETE | `/api/termekek/{id}` | törlés (csak ha nincs rá rendelés) |
| GET | `/api/termekek/kereses/{szoveg}` | **saját:** keresés a névben |
| PUT | `/api/termekek/{id}/keszlet/{mennyiseg}` | **saját:** készlet növelése |

### /api/rendelesek
| Metódus | Útvonal | Leírás |
|---------|---------|--------|
| GET | `/api/rendelesek` | rendelések listája |
| GET | `/api/rendelesek/{id}` | egy rendelés |
| POST | `/api/rendelesek` | új rendelés (levonja a készletet) |
| PUT | `/api/rendelesek/{id}` | vevő nevének és állapotának módosítása |
| DELETE | `/api/rendelesek/{id}` | törlés |
| PUT | `/api/rendelesek/{id}/allapot/{allapot}` | **saját:** állapot átírása |

## WPF alkalmazás

Egy ablak, három nézet (TabControl füleken): **Termékek**, **Kategóriák**, **Rendelések**. A nézetek megjelenésekor (Loaded esemény) frissülnek az adatok az API-ból.
A projekt MVVM mintát használ: Model, View, ViewModel, `INotifyPropertyChanged`, `ICommand`, `ObservableCollection`.
