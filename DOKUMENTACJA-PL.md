Autorzy *Maciej Bajer*, *Karolina Kulas*

## QUICKFUN

### Czym jest QUICKFUN?

**QUICKFUN** to platforma gier przeglądarkowych zbudowana w architekturze Clean Architecture. Jest to aplikacja **Blazor WebAssembly** umożliwiająca graczom zabawy w różne gry online z systemem punktacji i rankingu.

---

### Używane Języki Programowania

| Język | Zastosowanie |
|-------|--------------|
| **C#** | Backend, silnik gier, logika biznesowa (.NET 6+) |
| **HTML/Razor** | Frontend Blazor WebAssembly, komponenty UI |
| **CSS** | Stylowanie interfejsu użytkownika |
| **Haskell** | API Sudoku (moduł sudoku-api) |

---

###  Struktura Projektu

```
QuickFun/
├── QuickFun.Domain/          # Encje domenowe i interfejsy
├── QuickFun.Application/     # Logika biznesowa i serwisy
├── QuickFun.Infrastructure/  # Dostęp do danych, usługi zewnętrzne
├── QuickFun.Web/             # Frontend Blazor WebAssembly
├── QuickFun.Games/           # Implementacje gier
│   ├── Memory/               # Gra Memory
│   ├── MasterMind/           # Gra MasterMind
│   ├── Sudoku/               # Gra Sudoku (z API w Haskelu)
│   └── gamename/             # inne gry
└── QuickFun.Tests/           # Testy jednostkowe i integracyjne
```

---

### Główne Moduły

#### **QuickFun.Domain**
- Encje domenowe (PlayerSession, GameResult, LeaderboardDto)
- Interfejsy i abstrakkcje
- Enumy (enum dla typów gier, poziomów trudności)

#### **QuickFun.Application**
- Serwisy biznesowe
- ViewModels (HomeViewModel)
- Zarządzanie stanem aplikacji

#### **QuickFun.Infrastructure**
- Kontekst bazy danych (ApplicationDbContext)
- ORM: **Entity Framework Core**
- Baza danych: **SQLite** (`QuickFun.db`)
- Serwisy (StatsService)
- Autentykacja i autoryzacja (Microsoft.AspNetCore.Identity)

#### **QuickFun.Web**
- Komponenty Razor (np.; MemoryView.razor)
- ViewModels
- Interfejs użytkownika

#### **QuickFun.Games**
- BaseGameEngine - Klasa bazowa dla wszystkich gier
- Strategie trudności (Easy, Medium, Hard)
- *gamename* Engine - zawierają implementacje silników poszczególnych gier

---

### Architektura

Projekt wykorzystuje:
- **Clean Architecture** - Separacja warstw (Domain, Application, Infrastructure, Presentation)
- **SOLID Principles** - Zasady czytelnego kodu
- **Design Patterns**:
  - Repository Pattern
  - Factory Pattern
  - Strategy Pattern 
  - Observer Pattern
  - State Pattern
  - Command Pattern

---

### Dostępne Gry

1. Hangman
2. MasterMind
3. Memory
4. Minesweeper
5. Sudoku
6. Kółko krzyżyk - w wersji dla dwóch graczy a także przeciwko komputerowi
---

### Uruchamianie Projektu

**Web:**
```bash
cd QuickFun/QuickFun/QuickFun.Web
dotnet watch 
```

**Baza danych:**
```bash
cd QuickFun/QuickFun.Infrastructure/Server
dotnet run
```

**Sudoku api**
```bash
cd QuickFun/QuickFun.Games/Sudoku/sudoku-api
runhaskell sudoku-api
```

**Czyszczenie i rebuild:**
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

### Baza Danych

- **System**: SQLite
- **Plik**: `QuickFun.db`
- **ORM**: Entity Framework Core
- **Autentykacja**: Microsoft.AspNetCore.Identity

---

### Funkcjonalności

- Rozgrywka w różne gry
- System logowania użytkowników
- Liczenie punktów za gry
- Leaderboard/Ranking
- Historia gier gracza
- Różne poziomy trudności

---


### Licencja

MIT License

---

### Wymagania

- **.NET SDK** - [Pobierz tutaj](https://dotnet.microsoft.com/download)
- **Visual Studio Code** lub **Visual Studio**
- **SQLite** (wbudowany w .NET)

---

### Troubleshooting !!

| Problem | Rozwiązanie |
|---------|-----------|
| Port już zajęty | `dotnet run --project QuickFun.Web --urls "http://localhost:5005"` |
| Błędy buildu | `dotnet clean && dotnet restore && dotnet build` |
| .NET SDK nie znaleziony | Zainstaluj z https://dotnet.microsoft.com/download |

---

Autorzy *Maciej Bajer*, *Karolina Kulas*
