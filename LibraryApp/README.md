# University Library – ASP.NET Core MVC

A small MVC application for managing books, authors, and borrowings in a university library.  
Built with ASP.NET Core 8, Entity Framework Core 8 (Code First), and SQLite.

---

## How to Run

```bash
# 1. Restore packages
dotnet restore

# 2. Apply the migration (creates library.db in the project root)
dotnet ef database update

# 3. Run
dotnet run
```

Open your browser at `https://localhost:5001` (or `http://localhost:5000`).

---

## How to Create or Restore the Database

The application calls `db.Database.Migrate()` on startup, so the database is created and  
seeded automatically the first time the app runs.

To reset from scratch:

```bash
# Delete the SQLite file
rm library.db

# Re-apply the migration (recreates schema + seed data)
dotnet ef database update
```

---

## Migration Command Used

```bash
dotnet ef migrations add InitialCreate
```

The resulting files live in the `Migrations/` folder:

| File | Purpose |
|---|---|
| `20240101000000_InitialCreate.cs` | Schema + seed `Up`/`Down` |
| `LibraryDbContextModelSnapshot.cs` | EF's snapshot of the current model |

---

## Where Things Are

| What | Where |
|---|---|
| **DbContext** | `Data/LibraryDbContext.cs` |
| **Relationship configuration (Fluent API)** | `Data/LibraryDbContext.cs` → `OnModelCreating` |
| **Seed data (`HasData`)** | `Data/LibraryDbContext.cs` → `OnModelCreating` |
| **Service interfaces** | `Services/IBookService.cs`, `IBorrowingService.cs`, `IAuthorService.cs` |
| **Service implementations** | `Services/BookService.cs`, `BorrowingService.cs`, `AuthorService.cs` |
| **Controllers** | `Controllers/` |
| **Views** | `Views/Books/`, `Views/Authors/`, `Views/Borrowings/` |

---

## Answers to README Questions

### 1. What does ORM mean and what problem does EF Core solve?

**ORM** stands for *Object-Relational Mapper*. It is a library that translates between the  
object-oriented world of C# (classes, properties, references) and the relational world of SQL  
(tables, columns, foreign keys). EF Core lets you write plain C# instead of raw SQL, handles  
connection management and parameterisation, and keeps your schema in sync with your classes  
via migrations.

---

### 2. What is the role of `DbContext`?

`DbContext` is the **unit of work and gateway to the database**. It:

- holds the connection (or connection pool reference),
- tracks which entities have been loaded, added, modified, or deleted,
- translates LINQ queries into SQL and executes them,
- persists changes with `SaveChangesAsync()`.

One `DbContext` instance represents one logical transaction/request cycle.

---

### 3. How is `DbSet` different from a normal C# list?

A `DbSet<T>` is a **query root** that is backed by a database table.  
Calling `.ToListAsync()` on it sends a SQL `SELECT` to the database.  
Calling `.Add()` marks an entity for insertion — nothing is written until `SaveChangesAsync()`.  
A plain `List<T>` is just an in-memory collection with no database connection at all.

---

### 4. Why should `DbContext` be Scoped in a web application?

HTTP requests are isolated, short-lived operations. A **Scoped** lifetime creates one  
`DbContext` per request, which means:

- the change tracker is always in a clean state at the start of each request,
- no entities leak between concurrent requests (which would happen with Singleton),
- the context is not created for every service call (which would happen with Transient and  
  would break multi-step operations sharing the same transaction).

---

### 5. What does an EF Core migration do?

A migration is a **versioned, reversible diff** of the database schema. When you run  
`dotnet ef migrations add <Name>`, EF Core compares the current model to the snapshot of the  
previous state and generates C# code with `Up()` (apply) and `Down()` (roll back) methods.  
Running `dotnet ef database update` executes the pending `Up()` methods as SQL against the  
real database and records which migrations have been applied in the `__EFMigrationsHistory` table.

---

### 6. Why should seeding be idempotent?

Because the seeder runs every time the application starts. If it simply `INSERT`ed rows on  
every startup, each restart would duplicate the data. Idempotent seeding means "insert only  
if the row does not already exist", so the result is the same whether you run it once or a  
hundred times.  
`HasData` achieves this automatically: EF Core tracks seeded rows by their fixed `Id` and  
only inserts them once (via the migration). A manual seeder should check first:  
`if (!db.Authors.Any()) { db.Authors.AddRange(...); }`.

---

### 7. When is Code First a good choice, and when should Database First be considered?

**Code First** is ideal when:
- you are starting a **new project** and the database does not exist yet,
- the team is more comfortable with C# than SQL,
- you want source-controlled schema evolution through migrations.

**Database First** is better when:
- a **legacy database already exists** and cannot be changed,
- a DBA owns the schema and the application must follow it,
- the database uses advanced features (stored procedures, views, custom types) that EF Core  
  cannot easily represent in a model.
