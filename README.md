Authors: **Maciej Bajer**, **Karolina Kulas**

---

## QUICKFUN

### What is QUICKFUN?

**QUICKFUN** is a web gaming platform built using **Clean Architecture**. It is a **Blazor WebAssembly** application that allows players to enjoy various online games featuring a scoring and ranking system.

---

### Programming Languages Used

| Language | Application |
| --- | --- |
| **C#** | Backend, game engines, business logic (.NET 6+) |
| **HTML/Razor** | Blazor WebAssembly frontend, UI components |
| **CSS** | User interface styling |
| **Haskell** | Sudoku API (sudoku-api module) |

---

### Project Structure

```
QuickFun/
├── QuickFun.Domain/          # Domain entities and interfaces
├── QuickFun.Application/     # Business logic and services
├── QuickFun.Infrastructure/  # Data access, external services
├── QuickFun.Web/             # Blazor WebAssembly frontend
├── QuickFun.Games/           # Game implementations
│   ├── Memory/               # Memory game
│   ├── MasterMind/           # MasterMind game
│   ├── Sudoku/               # Sudoku game (with Haskell API)
│   └── gamename/             # Other games
└── QuickFun.Tests/           # Unit and integration tests

```

---

### Main Modules

#### **QuickFun.Domain**

* Domain entities (`PlayerSession`, `GameResult`, `LeaderboardDto`)
* Interfaces and abstractions
* Enums (game types, difficulty levels)

#### **QuickFun.Application**

* Business services
* ViewModels (`HomeViewModel`)
* Application state management

#### **QuickFun.Infrastructure**

* Database context (`ApplicationDbContext`)
* ORM: **Entity Framework Core**
* Database: **SQLite** (`QuickFun.db`)
* Services (`StatsService`)
* Authentication and Authorization (`Microsoft.AspNetCore.Identity`)

#### **QuickFun.Web**

* Razor components (e.g., `MemoryView.razor`)
* ViewModels
* User Interface

#### **QuickFun.Games**

* `BaseGameEngine` – Base class for all games
* Difficulty strategies (Easy, Medium, Hard)
* *GameName* Engines – Specific implementations of game logic

---

### Architecture

The project utilizes:

* **Clean Architecture** – Layer separation (Domain, Application, Infrastructure, Presentation)
* **SOLID Principles** – Clean code standards
* **Design Patterns**:
* Repository Pattern
* Factory Pattern
* Strategy Pattern
* Observer Pattern
* State Pattern
* Command Pattern



---

### Available Games

1. **Hangman**
2. **MasterMind**
3. **Memory**
4. **Minesweeper**
5. **Sudoku**
6. **Tic-Tac-Toe** – Two-player mode and vs. CPU mode

---

### Running the Project

**Web:**

```bash
cd QuickFun/QuickFun/QuickFun.Web
dotnet watch 

```

**Database:**

```bash
cd QuickFun/QuickFun.Infrastructure/Server
dotnet run

```

**Sudoku API:**

```bash
cd QuickFun/QuickFun.Games/Sudoku/sudoku-api
runhaskell sudoku-api

```

**Clean and Rebuild:**

```bash
dotnet clean
dotnet restore
dotnet build

```

**If port is occupied:**

```bash
dotnet run --project QuickFun.Web --urls "http://localhost:5005"

```

---

### Database

* **System**: SQLite
* **File**: `QuickFun.db`
* **ORM**: Entity Framework Core
* **Authentication**: Microsoft.AspNetCore.Identity

---

### Features

* Play various types of games
* User login system
* In-game score tracking
* Leaderboard/Ranking system
* Player game history
* Multiple difficulty levels

---

### License

MIT License

---

### Requirements

* **.NET SDK** – [Download here](https://dotnet.microsoft.com/download)
* **Visual Studio Code** or **Visual Studio**
* **SQLite** (Built-in with .NET)

---

### Troubleshooting

| Problem | Solution |
| --- | --- |
| Port already in use | `dotnet run --project QuickFun.Web --urls "http://localhost:5005"` |
| Build errors | `dotnet clean && dotnet restore && dotnet build` |
| .NET SDK not found | Install from [https://dotnet.microsoft.com/download](https://dotnet.microsoft.com/download) |

---

Authors: **Maciej Bajer**, **Karolina Kulas**
