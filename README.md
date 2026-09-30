# Countries & Cities API 🌍🌆

A production-ready ASP.NET Core Web API project for managing Countries and Cities, built following **Clean Architecture** principles.

![.NET Core](https://img.shields.io/badge/.NET-10.0-512BD4?style=flat&logo=dotnet)
![Entity Framework Core](https://img.shields.io/badge/EF_Core-10.0-3ba3f2?style=flat&logo=nuget)
![Swagger](https://img.shields.io/badge/Swagger-Supported-85EA2D?style=flat&logo=swagger)

## 📌 Project Overview
This project provides a robust RESTful API to manage CRUD operations for Countries and Cities. It implements advanced concepts like server-side pagination, search filtering, global error handling, and robust input validation, all structured within a scalable template without over-engineering.

## 🏗️ Architecture
The solution uses **Clean Architecture** to maintain a clear separation of concerns. The dependency direction is strictly pointed towards the center (Domain layer).

* **API:** Depends on Application and Infrastructure. It is the presentation layer and entry point.
* **Application:** Depends on Domain. Contains business logic, DTOs, interfaces, and validation rules.
* **Infrastructure:** Depends on Application and Domain. Provides the implementation for data access (EF Core).
* **Domain:** Completely independent. Holds the core business entities.

## 🚀 Technologies Used
- **.NET 10**
- **ASP.NET Core Web API**
- **C#**
- **Entity Framework Core (SQL Server)**
- **FluentValidation**
- **Swagger / OpenAPI**

## 📂 Project Structure
```text
CountriesCities.sln
src/
  ├── CountriesCities.API/            # Controllers, Middleware, Configuration
  ├── CountriesCities.Application/    # Services, DTOs, Validation, Interfaces
  ├── CountriesCities.Domain/         # Core Entities
  └── CountriesCities.Infrastructure/ # EF Core, DbContext, Configurations
```

## 🗄️ Database Schema & Decisions
- **Country**: `Id` (PK), `Name` (Required, Max 100), `Code` (Required, Max 10).
- **City**: `Id` (PK), `Name` (Required, Max 100), `CountryId` (FK).
- **Relationship**: `1-to-Many` between Country and City. 
  - **Delete Behavior**: Configured as `Restrict` to prevent accidental deletion of a Country if it still contains Cities.
- **Indexes**: 
  - `Country.Name` and `Country.Code` for fast searching.
  - `City.Name` and `City.CountryId` to optimize foreign key lookups.

## ⚙️ Setup & Installation

1. Clone this repository:
   ```bash
   git clone https://github.com/yourusername/CountriesCities.git
   ```
2. Open a terminal in the root directory.
3. Update the `DefaultConnection` string in `src/CountriesCities.API/appsettings.json` if necessary:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=.;Database=CountriesCitiesDb;Trusted_Connection=True;TrustServerCertificate=True;"
   }
   ```
4. Apply the EF Core Migrations to create the database:
   ```bash
   dotnet ef database update -s src/CountriesCities.API -p src/CountriesCities.Infrastructure
   ```
5. Run the application:
   ```bash
   dotnet run --project src/CountriesCities.API
   ```

## 📖 API Endpoints

Once the application is running, navigate to `https://localhost:<port>/swagger` to view the interactive API documentation.

### Countries
- `GET /api/countries` - Get paginated & filtered countries
- `GET /api/countries/{id}` - Get a specific country
- `POST /api/countries` - Create a country
- `PUT /api/countries/{id}` - Update a country
- `DELETE /api/countries/{id}` - Delete a country

### Cities
- `GET /api/cities` - Get paginated & filtered cities
- `GET /api/cities/{id}` - Get a specific city
- `POST /api/cities` - Create a city
- `PUT /api/cities/{id}` - Update a city
- `DELETE /api/cities/{id}` - Delete a city
- `GET /api/countries/{countryId}/cities` - Get paginated cities for a specific country

## 🛠️ Key Features Implemented

- **Unified Error Responses**: All errors (400, 404, 500) return a standardized `ErrorResponse` JSON.
- **Global Error Handling**: Centralized using the modern `IExceptionHandler` middleware.
- **Pagination**: Handled at the database level using `Skip()` and `Take()` for optimal performance.
- **Filtering**: Dynamic `IQueryable` filtering allowing case-insensitive search by Name/Code.
- **Validation**: `FluentValidation` validates request DTOs automatically using a custom ActionFilter.
