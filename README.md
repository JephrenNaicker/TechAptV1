TechAptV1 - Blazor Application
This is a Blazor application that demonstrates threading, data persistence, and file download functionality. The application generates random numbers (odd, even, and prime), saves them to a SQLite database, and allows users to download the data in XML or binary format.

Features
Threading:

Generates random odd, even, and prime numbers using multiple threads.

Persists the generated numbers to a SQLite database.

Data Persistence:

Uses Entity Framework Core (EF Core) to interact with a SQLite database.

Supports saving and retrieving data.

File Downloads:

Allows users to download the generated data in XML or binary format.

Prerequisites
Before running the project, ensure you have the following installed:

.NET 6 SDK

Visual Studio 2022 (or any IDE that supports .NET development)

SQLite (included with EF Core)

Installed Packages
The following NuGet packages are required for this project:

Microsoft.EntityFrameworkCore
Entity Framework Core is a modern object-database mapper for .NET. It supports LINQ queries, change tracking, and works with SQLite, SQL Server, and other databases.

bash
Copy
dotnet add package Microsoft.EntityFrameworkCore
Microsoft.EntityFrameworkCore.Design
Shared design-time components for Entity Framework Core tools.

bash
Copy
dotnet add package Microsoft.EntityFrameworkCore.Design
Microsoft.EntityFrameworkCore.Sqlite
SQLite database provider for Entity Framework Core.

bash
Copy
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
Getting Started

Build the project to restore NuGet packages.

Run the application using the following command:

bash
Copy
dotnet run
Open your browser and navigate to https://localhost:5001 (or the URL provided in the console).

Project Structure
Client/: Contains the Blazor client application.

Components/: Blazor components (e.g., pages, layouts).

Services/: Services for threading, data access, and file downloads.

Models/: Data models (e.g., Number).

wwwroot/: Static files (e.g., JavaScript, CSS).

DatabaseContext/: Contains the EF Core DataContext for database interactions.

Usage
Generate Numbers:

Navigate to the Threading page.

Click the Start button to begin generating numbers.

Save Data:

Once the number generation is complete, click the Save button to persist the data to the database.

Download Data:

Navigate to the Results page.

Click Download XML to download the data in XML format.

Click Download Binary to download the data in binary format.