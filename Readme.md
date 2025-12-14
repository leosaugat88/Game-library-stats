🎮 Game Library & Player Stats Manager
A .NET 8 console application that demonstrates core software engineering principles including design patterns, clean architecture, file persistence, search/sort algorithms, and internal unit testing — built for the CET2007 assignment.

✨ Features
Player Management: Add, update, and view player records
JSON Persistence: Saves player data to players.json
Activity Logging: All actions logged to actions.txt
Search Algorithms:
Linear search (by ID or username, case-insensitive)
Binary search (by ID on sorted data)
Sorting Strategies (Strategy Pattern):
Manual Insertion Sort
Built-in LINQ Sort (for comparison)
CSV Export: Generate player reports in .csv format
Internal Unit Test Harness: 5 built-in tests runnable from the menu
Robust Error Handling: Custom exceptions and validation
Thread-Safe Logging: Safe concurrent writes to log file

🔧 Design Patterns Used
Singleton (via Lazy<T>): PlayerRepository
Factory: PlayerFactory
Strategy: ISortStrategy with InsertionSortStrategy / BuiltInSortStrategy
Repository: PlayerRepository abstracts data access
📐 SOLID Principles
Single Responsibility: Each class has one clear purpose
Open/Closed: Extendable via interfaces (e.g., new sort strategies)
Liskov Substitution: Strategies are interchangeable
Interface Segregation: Small, focused interfaces (ILogger, IPersistence)
Dependency Inversion: High-level modules depend on abstractions
🛠️ Setup & Running
Prerequisites
.NET 8 SDK
Visual Studio Code (or any C# IDE)
Steps
Clone or download this project

Open terminal in project root (GameLibraryManager/)
dotnet build

Run the application:
dotnet run