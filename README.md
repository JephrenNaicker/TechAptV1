# TechAptV1 - Blazor Application
This is a Blazor application that demonstrates threading, data persistence, and file download functionality. The application generates random numbers (odd, even, and prime), saves them to a SQLite database, and allows users to download the data in XML or binary format.

## Features

* Threading:
  Generates random odd, even, and prime numbers using multiple threads.
  Persists the generated numbers to a SQLite database.
* Data Persistence:
  Uses Entity Framework Core (EF Core) to interact with a SQLite database.
  Supports saving and retrieving data.
* File Downloads:
  Allows users to download the generated data in XML or binary format.

## Prerequisites

Before running the project, ensure you have the following installed:

* .NET 6 SDK
* Visual Studio 2022 (or any IDE that supports .NET development)
* SQLite (included with EF Core)

## Installed Packages

The following NuGet packages are required for this project:

* Microsoft.EntityFrameworkCore
* Microsoft.EntityFrameworkCore.Design
* Microsoft.EntityFrameworkCore.Sqlite

To install these packages, use the following commands:
```
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
```

## Getting Started

1. Build the project to restore NuGet packages.
2. Run the application using the following command:
```bash
dotnet run
```
3. Open your browser and navigate to https://localhost:5001 (or the URL provided in the console).

## Project Structure

The project consists of the following folders:

* Client/: Contains the Blazor client application.
* Components/: Blazor components (e.g., pages, layouts).
* Services/: Services for threading, data access, and file downloads.
* Models/: Data models (e.g., Number).
* wwwroot/: Static files (e.g., JavaScript, CSS).
* DatabaseContext/: Contains the EF Core DataContext for database interactions.

# Approach

## Dependency Injection
*	Used interface-based design `(IThreadingService, IDataService)`
*	Makes components testable and replaceable i.e mocking,unit tests `(SOLID)`

## Threading
*	Used `SemaphoreSlim` for thread synchronization
*	Which allows locking around shared resources
*   Cooperative cancellation using `CancellationTokenSource`
 
## EF Core Implementation
*	Added primary key for proper entity tracking, for the Number (Data Structure)
*	Code-First approach

## Notification System
*	Toast notifications for user feedback
## UI
* UI refresh showing updated counters
* Success/error notifications
* Use of JavaScript for Download files as this was the better and simpler option for the XML
* Binary file makes use of JavaScrip and C# as to create the binary hexadecimal file. 
* Hexadecimal file approach, as the binary(1001) would have taken more space , making the file larger to download.

