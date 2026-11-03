# BoostBay - tuning alkatrész webshop

Beadandó: REST API (ASP.NET Core + EF Core + MySQL) és hozzá egy WPF kliens (MVVM).

## Adatbázis (MySQL Dockerben)
Egyszer kell lefuttatni a parancssorban (a docker hub-os mysql image alapján):

    docker run --name boostbay-mysql -e MYSQL_ROOT_PASSWORD=root123 -e MYSQL_DATABASE=boostbay -e MYSQL_USER=boost -e MYSQL_PASSWORD=boost123 -p 3306:3306 -d mysql:8.4

Később elindítani: `docker start boostbay-mysql`

A paraméterek (jelszó, felhasználó) megegyeznek a `BoostBayApi/appsettings.json` connection stringjével.

## Indítás
1. MySQL elindítása (lásd fent)
2. `cd BoostBayApi` majd `dotnet run` (a táblákat magától létrehozza, Swagger: http://localhost:5080/swagger)
3. `cd BoostBayWpf` majd `dotnet run`

## Endpointok
- `api/products` - CRUD + `GET search/{text}` + `PUT {id}/restock/{amount}`
- `api/categories` - CRUD + `GET {id}/products`
- `api/orders` - CRUD + `PUT {id}/status/{status}`
