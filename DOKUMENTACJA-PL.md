

## TYTUŁ PROJEKTU

**QUICKFUN** – Platforma gamingowa z systemem oceniania i rankingu

---

## Imiona i nazwiska autorów projektu

* **Maciej Bajer**
* **Karolina Kulas**

---

## Data wykonania projektu

**styczen - maj 2026**

---

## Krótki opis projektu

### Założenia
QUICKFUN to zaawansowana platforma internetowa do gier wieloosobowych zbudowana przy użyciu nowoczesnych technologii Microsoft .NET i Blazor. Projekt zakłada stworzenie uniwersalnego systemu do grania w różnorodne gry online z wbudowanym systemem punktacji, rankingu i historii rozgrywek.

### Cele projektowe
* Stworzenie funkcjonalnej platformy gamingowej dostępnej w przeglądarce
* Implementacja systemu autentykacji i zarządzania użytkownikami
* Zbudowanie systemu oceniania i rankingu graczy
* Zapewnienie skalowalnej architektur aplikacji
* Implementacja co najmniej 6 różnych gier z różnymi poziomami trudności

### Funkcje
* Rejestracja i logowanie użytkowników
* Gra w różne typy gier (Hangman, MasterMind, Memory, Minesweeper, Sudoku, Tic-Tac-Toe)
* Śledzenie wyników i punktów
* Publiczny system rankingowy/leaderboard
* Historia rozgrywek dla każdego gracza
* Wsparcie dla różnych poziomów trudności (Easy, Medium, Hard)
* Gra dwuosobowa i gra przeciwko AI (dla wybranych gier)

### Przewidywane przeznaczenie
Platforma przeznaczona jest dla użytkowników chcących spędzać czas na grach logicznych i strategicznych online, z możliwością współzawodnictwa z innymi graczami poprzez system rankingowy.

---

## Wybór technologii informatycznych

| Technologia | Zastosowanie | Uzasadnienie |
| --- | --- | --- |
| **C#** | Backend, silniki gier, logika biznesowa | Nowoczesny, bezpieczny, zarządzany typ (.NET 6+) |
| **Blazor WebAssembly** | Frontend aplikacji | Umożliwia pisanie UI w C#, brak potrzeby JavaScript, wysoka wydajność |
| **.NET 6+** | Framework aplikacji | Zmultiplikowany, szybki, z bogatą biblioteką standardową |
| **Entity Framework Core** | Mapowanie obiektowo-relacyjne (ORM) | Automatyczne migracje, LINQ, obsługa wielu baz danych |
| **SQLite** | Baza danych | Lekka, wbudowana, idealna dla aplikacji desktopowych i PWA |
| **HTML/Razor** | Szablonowanie interfejsu | Natywna obsługa w Blazor, integracja z komponentami |
| **CSS** | Stylowanie interfejsu | Bootstrap i niestandardowe style dla responsywnego designu |
| **Haskell** | API do gry Sudoku | Specjalizowana logika generowania i sprawdzania sudoku |
| **Microsoft.AspNetCore.Identity** | Autentykacja i autoryzacja | Bezpieczne zarządzanie użytkownikami, role i uprawnienia |

---

## Projekt architektury aplikacji z uzasadnieniem wyboru technologii

### Architektura: Clean Architecture

```
┌─────────────────────────────────────────────────────────┐
│              Presentation Layer (UI)                     │
│            QuickFun.Web (Blazor WebAssembly)            │
│  - Razor Components                                     │
│  - ViewModels                                           │
│  - State Management                                     │
└──────────────────────┬──────────────────────────────────┘
                       │
┌──────────────────────▼──────────────────────────────────┐
│            Application Layer (Services)                  │
│        QuickFun.Application                             │
│  - Business Logic Services                              │
│  - Game Orchestration                                   │
│  - Use Cases                                            │
└──────────────────────┬──────────────────────────────────┘
                       │
┌──────────────────────▼──────────────────────────────────┐
│             Domain Layer (Business Rules)                │
│           QuickFun.Domain                               │
│  - Entities                                             │
│  - Interfaces                                           │
│  - Value Objects                                        │
└──────────────────────┬──────────────────────────────────┘
                       │
┌──────────────────────▼──────────────────────────────────┐
│         Infrastructure Layer (Data Access)               │
│        QuickFun.Infrastructure                          │
│  - Database Context (EF Core)                           │
│  - Repository Implementations                           │
│  - External Service Integrations                        │
└──────────────────────┬──────────────────────────────────┘
                       │
         ┌─────────────┴─────────────┐
         │                           │
    ┌────▼─────┐           ┌────────▼─────┐
    │  SQLite  │           │  Sudoku API  │
    │  (Local) │           │   (Haskell)  │
    └──────────┘           └──────────────┘
```

### Uzasadnienie wyboru architektury

**Clean Architecture** zapewnia:
* **Niezależność od frameworków** – łatwa zmiana technologii
* **Testowalność** – izolacja logiki biznesowej
* **Łatwość utrzymania** – jasne rozdzielenie odpowiedzialności
* **Skalowalność** – łatwe dodawanie nowych funkcji
* **SOLID Principles** – czysty kod

### Warstwy projektowe

#### 1. **Warstwa Domenowa (QuickFun.Domain)**
Zawiera:
* Encje biznesowe (`PlayerSession`, `GameResult`, `LeaderboardDto`)
* Interfejsy i abstrakcje
* Enumeracje (typy gier, poziomy trudności)
* Reguły biznesowe

#### 2. **Warstwa Aplikacji (QuickFun.Application)**
Zawiera:
* Serwisy biznesowe
* ViewModels (`HomeViewModel`)
* Use Cases
* Orkiestracja procesów

#### 3. **Warstwa Infrastruktury (QuickFun.Infrastructure)**
Zawiera:
* Kontekst bazy danych (`ApplicationDbContext`)
* Implementacje wzorca Repository
* Entity Framework Core
* Serwisy integracji z bazą danych
* Autentykacja i autoryzacja

#### 4. **Warstwa Prezentacji (QuickFun.Web)**
Zawiera:
* Komponenty Razor (`.razor`)
* ViewModels
* Interfejs użytkownika
* Blazor WebAssembly

#### 5. **Warstwa Gier (QuickFun.Games)**
Zawiera:
* `BaseGameEngine` – Klasa bazowa dla wszystkich gier
* Strategie trudności (Easy, Medium, Hard)
* Implementacje konkretnych gier

### Zastosowane wzorce projektowe

| Wzorzec | Zastosowanie |
| --- | --- |
| **Repository Pattern** | Abstrakcja dostępu do danych |
| **Factory Pattern** | Tworzenie instancji gier |
| **Strategy Pattern** | Różne strategie trudności |
| **Observer Pattern** | Obserwacja zmian stanu gry |
| **State Pattern** | Zarządzanie stanami gry |
| **Command Pattern** | Wykonywanie akcji w grze |
| **Dependency Injection** | Zarządzanie zależnościami |

---

## Projekt bazy danych

### System: SQLite + Entity Framework Core

### Tabele relacyjne

```sql
-- Tabela użytkowników
CREATE TABLE AspNetUsers (
    Id TEXT PRIMARY KEY,
    UserName TEXT NOT NULL UNIQUE,
    Email TEXT NOT NULL UNIQUE,
    PasswordHash TEXT NOT NULL,
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- Tabela sesji gry
CREATE TABLE GameSessions (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    UserId TEXT NOT NULL,
    GameType TEXT NOT NULL,
    Difficulty TEXT NOT NULL,
    StartTime DATETIME DEFAULT CURRENT_TIMESTAMP,
    EndTime DATETIME,
    Status TEXT DEFAULT 'In Progress',
    FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id)
);

-- Tabela wyników
CREATE TABLE GameResults (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    GameSessionId INTEGER NOT NULL,
    UserId TEXT NOT NULL,
    Score INTEGER NOT NULL,
    Moves INTEGER,
    TimeSpent INTEGER,
    IsWon BOOLEAN DEFAULT FALSE,
    GameType TEXT NOT NULL,
    ResultDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (GameSessionId) REFERENCES GameSessions(Id),
    FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id)
);

-- Tabela rankingu/leaderboard
CREATE TABLE LeaderboardEntries (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    UserId TEXT NOT NULL,
    GameType TEXT NOT NULL,
    TotalScore INTEGER NOT NULL,
    TotalGamesPlayed INTEGER NOT NULL,
    AverageScore DECIMAL(10,2),
    WinRate DECIMAL(5,2),
    LastUpdated DATETIME DEFAULT CURRENT_TIMESTAMP,
    Rank INTEGER,
    FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id)
);

-- Tabela historii rozgrywek
CREATE TABLE GameHistory (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    UserId TEXT NOT NULL,
    GameType TEXT NOT NULL,
    Score INTEGER NOT NULL,
    PlayDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    Difficulty TEXT,
    Duration INTEGER,
    IsWon BOOLEAN,
    FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id)
);
```

### Diagram Entity-Relationship (ER)

```
┌──────────────┐
│  AspNetUsers │
├──────────────┤
│ Id (PK)      │
│ UserName     │
│ Email        │
│ PasswordHash │
│ CreatedAt    │
└────┬─────────┘
     │ 1:N
     │
     ├─────────────────────────────┐
     │                             │
     │ 1:N                     1:N │
┌────▼─────────────┐    ┌────────▼──────────┐
│ GameSessions     │    │ GameResults       │
├──────────────────┤    ├───────────────────┤
│ Id (PK)          │    │ Id (PK)           │
│ UserId (FK)      │    │ GameSessionId (FK)│
│ GameType         │    │ UserId (FK)       │
│ Difficulty       │    │ Score             │
│ StartTime        │    │ Moves             │
│ EndTime          │    │ TimeSpent         │
│ Status           │    │ IsWon             │
└──────────────────┘    │ GameType          │
                        │ ResultDate        │
                        └───────────────────┘

     1:N                     1:N
     │                       │
     ├─────────────────────┬─┘
     │                     │
┌────▼──────────────┐  ┌──▼──────────────────┐
│ LeaderboardEntr.  │  │ GameHistory         │
├───────────────────┤  ├─────────────────────┤
│ Id (PK)           │  │ Id (PK)             │
│ UserId (FK)       │  │ UserId (FK)         │
│ GameType          │  │ GameType            │
│ TotalScore        │  │ Score               │
│ TotalGamesPlayed  │  │ PlayDate            │
│ AverageScore      │  │ Difficulty          │
│ WinRate           │  │ Duration            │
│ LastUpdated       │  │ IsWon               │
│ Rank              │  └─────────────────────┘
└───────────────────┘
```

### Atrybuty tabel

| Tabela | Kolumna | Typ | Opis |
| --- | --- | --- | --- |
| AspNetUsers | Id | TEXT (PK) | Unikalny identyfikator użytkownika |
| | UserName | TEXT | Nazwa użytkownika |
| | Email | TEXT | Adres email |
| | PasswordHash | TEXT | Zaszyfrowane hasło |
| | CreatedAt | DATETIME | Data utworzenia konta |
| GameSessions | Id | INT (PK) | Unikalny identyfikator sesji |
| | UserId | TEXT (FK) | Referencja do użytkownika |
| | GameType | TEXT | Typ gry (Hangman, Sudoku, itd.) |
| | Difficulty | TEXT | Poziom trudności |
| | StartTime | DATETIME | Czas rozpoczęcia |
| | EndTime | DATETIME | Czas zakończenia |
| | Status | TEXT | Status sesji |
| GameResults | Id | INT (PK) | Unikalny identyfikator wyniku |
| | GameSessionId | INT (FK) | Referencja do sesji |
| | UserId | TEXT (FK) | Referencja do użytkownika |
| | Score | INT | Uzyskane punkty |
| | Moves | INT | Liczba ruchów |
| | TimeSpent | INT | Czas spędzony (w sekundach) |
| | IsWon | BOOLEAN | Czy gra wygrana |
| | GameType | TEXT | Typ gry |
| | ResultDate | DATETIME | Data wyniku |
| LeaderboardEntries | Id | INT (PK) | Unikalny identyfikator |
| | UserId | TEXT (FK) | Referencja do użytkownika |
| | GameType | TEXT | Typ gry |
| | TotalScore | INT | Całkowita liczba punktów |
| | TotalGamesPlayed | INT | Łączna liczba gier |
| | AverageScore | DECIMAL | Średni wynik |
| | WinRate | DECIMAL | Procent zwycięstw |
| | LastUpdated | DATETIME | Ostatnia aktualizacja |
| | Rank | INT | Pozycja w rankingu |
| GameHistory | Id | INT (PK) | Unikalny identyfikator |
| | UserId | TEXT (FK) | Referencja do użytkownika |
| | GameType | TEXT | Typ gry |
| | Score | INT | Wynik |
| | PlayDate | DATETIME | Data rozgrywki |
| | Difficulty | TEXT | Poziom trudności |
| | Duration | INT | Czas trwania gry |
| | IsWon | BOOLEAN | Czy gra wygrana |

---

## Dostępne gry

1. **Hangman** – Zgadywanie słów
2. **MasterMind** – Łamanie kodu
3. **Memory** – Gra na pamięć
4. **Minesweeper** – Saper
5. **Sudoku** – Puzzle logiczne (z Haskell API)
6. **Tic-Tac-Toe** – Kolko i krzyżyk (tryb 2-osobowy i vs CPU)

---

## Jak uruchamiać projekt

**Frontend (Web):**
```bash
cd QuickFun/QuickFun.Web
dotnet watch
```

**Baza danych:**
```bash
cd QuickFun/QuickFun.Infrastructure/Server
dotnet run
```

**Sudoku API (Haskell):**
```bash
cd QuickFun/QuickFun.Games/Sudoku/sudoku-api
runhaskell sudoku-api
```

**Czyszczenie i przebudowa:**
```bash
dotnet clean
dotnet restore
dotnet build
```

**Jeśli port jest zajęty:**
```bash
dotnet run --project QuickFun.Web --urls "http://localhost:5005"
```

---

## Wymagania systemowe

* **.NET SDK 6.0+** – [Pobierz tutaj](https://dotnet.microsoft.com/download)
* **Visual Studio Code** lub **Visual Studio**
* **SQLite** (wbudowany w .NET)
* **Haskell** (dla Sudoku API)

---

## Rozwiązywanie problemów

| Problem | Rozwiązanie |
| --- | --- |
| Port już w użyciu | `dotnet run --project QuickFun.Web --urls "http://localhost:5005"` |
| Błędy budowania | `dotnet clean && dotnet restore && dotnet build` |
| .NET SDK nie znaleziony | Pobierz z https://dotnet.microsoft.com/download |

---

## Licencja

MIT License

---

**Ostatnia aktualizacja: 2026-05-11**
