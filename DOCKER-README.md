Docker Pokretanje BeSkSalon Aplikacije


Korak 1: Kloniraj repository (ako već nije)
```bash
git clone https://github.com/[username]/BeSkSalon.git
cd BeSkSalon
```

Korak 2: Pokreni Docker containere
```bash
docker-compose up --build -d
```

Šta se dešava:
1. Build-uje se ASP.NET Core 8 aplikacija
2. Pokreće se SQL Server 2022 Express container
3. Čeka se da SQL Server postane healthy (healthcheck)
4. Pokreće se web aplikacija
5. Izvršavaju se migracije baze
6. Dodaju se seed podaci (Admin korisnik, Frizeri, Usluge)

Vrijeme pokretanja: ~30-60 sekundi

Pristup

Web aplikacija:
```
http://localhost:5000
```

Admin podaci:
```
Email: admin@besk.ba
Lozinka: Admin123!
```

SQL Server pristup

Connection String:
```
Server=localhost,1433;
Database=BeSkSalonDb;
User Id=sa;
Password=Admin123!@#;
TrustServerCertificate=True;
```

SQL Server Management Studio:
```
Server: localhost,1433
Authentication: SQL Server Authentication
Login: sa
Password: Admin123!@#
```

