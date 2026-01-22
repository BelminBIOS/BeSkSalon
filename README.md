# 💈 Frizerski Salon BeŠk

Web aplikacija za online zakazivanje termina u frizerskom salonu razvijena u ASP.NET Core 8.0.

## 📋 Opis

Informacioni sistem za online zakazivanje termina kod frizera. Aplikacija omogućava klijentima da jednostavno zakažu termine u frizerskom salonu, biraju frizera i usluge, te upravljaju svojim terminima. Administratori imaju potpunu kontrolu nad frizerima, uslugama i svim zakazanim terminima.

## ✨ Tehnologije

- **Framework:** ASP.NET Core 8.0 (MVC pattern)
- **Autentifikacija:** ASP.NET Core Identity
- **ORM:** Entity Framework Core
- **Baza podataka:** SQLite / Microsoft SQL Server
- **Frontend:** Bootstrap 5, jQuery
- **Kontejnerizacija:** Docker support

## 🎯 Funkcionalnosti

### 👤 Klijent
- ✅ Registracija i prijava korisnika
- 📅 Zakazivanje termina sa odabirom frizera i usluge
- 🕐 Pregled dostupnih slobodnih termina
- 📋 Pregled mojih termina (aktivnih i prošlih)
- ❌ Otkazivanje termina
- 🔒 Upravljanje ličnim podacima i sigurnošću naloga

### 👨‍💼 Admin
- 👥 Upravljanje frizerima (kreiranje, uređivanje, brisanje)
- 💇 Upravljanje uslugama (kreiranje, uređivanje, brisanje, dodjeljivanje frizerima)
- 📊 Pregled svih termina sa filterima
- ⏰ Validacija radnog vremena (Pon-Sub: 08:00-20:00, Nedjelja: zatvoreno)
- 🔄 Ažuriranje statusa termina

## 📦 Instalacija i pokretanje

### Preduvjeti
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Git
- (Opciono) Docker Desktop

### Lokalno pokretanje

1. **Klonirajte repozitorij:**
```bash
git clone https://github.com/BelminBIOS/BeSkSalon.git
cd BeSkSalon
```

2. **Restorujte NuGet pakete:**
```bash
dotnet restore
```

3. **Kreirajte bazu podataka:**
```bash
dotnet ef database update
```

4. **Pokrenite aplikaciju:**
```bash
dotnet run
```

5. **Otvorite browser:**
   - HTTPS: https://localhost:7195
   - HTTP: http://localhost:5074

### Docker pokretanje

```bash
docker-compose up --build
```

Aplikacija će biti dostupna na: **http://localhost:5000**

## 🔐 Admin pristup

**Email:** admin@besk.ba  
**Lozinka:** Admin123!

## 🗄️ Struktura baze podataka

### Glavne tabele:

- **AspNetUsers** - Korisnici sistema
- **Frizeri** - Informacije o frizerima (Id, Ime, Tip)
- **Usluge** - Usluge koje salon nudi (Id, Naziv, Trajanje, Cijena, FrizerId)
- **Termini** - Zakazani termini (Id, Datum, VrijemeOd, VrijemeDo, Status, FrizerId, UslugaId, UserId)

## 🕐 Radno vrijeme

- **Ponedeljak - Subota:** 08:00 - 20:00
- **Nedjelja:** Zatvoreno
- **Trajanje termina:** Zavisi od usluge (30-120 minuta)

## 📱 Struktura aplikacije

```
BeSkSalon/
├── Controllers/           # MVC kontroleri
│   ├── HomeController.cs
│   ├── TerminController.cs
│   └── AdminController.cs
├── Models/               # Modeli podataka
│   ├── Frizer.cs
│   ├── Termin.cs
│   ├── Usluga.cs
│   └── ViewModels/
├── Views/                # Razor view-i
│   ├── Home/
│   ├── Termin/
│   └── Admin/
├── Areas/Identity/       # Identity stranice
├── Data/                 # DbContext
├── Migrations/           # EF migracije
└── wwwroot/             # Statički resursi
```
