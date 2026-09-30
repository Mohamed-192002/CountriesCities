# CountriesCities API

A production-quality ASP.NET Core Web API project for managing Countries and Cities, built using Clean Architecture principles.

## 1. Project Overview
This project provides a robust RESTful API to manage CRUD operations for Countries and Cities. It implements advanced concepts like server-side pagination, search filtering, global error handling, and robust input validation, all structured within a scalable Clean Architecture template without over-engineering.

## 2. Architecture Explanation
The solution uses **Clean Architecture** to maintain a clear separation of concerns. The dependency direction is strictly pointed towards the center (Domain layer).

- **API** depends on Application and Infrastructure. It is the presentation layer and entry point.
- **Application** depends on Domain. It contains business logic, DTOs, interfaces, and validation rules.
- **Infrastructure** depends on Application and Domain. It provides the implementation for data access (EF Core) and external services.
- **Domain** is completely independent. It holds the core business entities (Country, City).

By avoiding unnecessary abstractions like Generic Repository and Unit of Work, the project remains highly maintainable, pragmatic, and leverages EF Core's built-in Repository (DbSet) and Unit of Work (DbContext) patterns.

## 3. Technologies
- .NET 10
- ASP.NET Core Web API
- C#
- Entity Framework Core (SQL Server)
- FluentValidation
- Swagger / OpenAPI
- Dependency Injection
- Async/Await

## 4. Project Structure
```text
CountriesCities.sln
src/
  ├── CountriesCities.API/            # Controllers, Middleware, Configuration
  ├── CountriesCities.Application/    # Services, DTOs, Validation, Interfaces
  ├── CountriesCities.Domain/         # Core Entities
  └── CountriesCities.Infrastructure/ # EF Core, DbContext, Configurations
```

## 5. Database Schema Explanation
- **Country Table**: Has `Id` (Primary Key), `Name` (Required, Max 100), `Code` (Required, Max 10).
- **City Table**: Has `Id` (Primary Key), `Name` (Required, Max 100), `CountryId` (Foreign Key).

### Design Decisions: Relationship and Cascade Delete
The relationship is configured as a `1-to-Many` between Country and City. 
- **Delete Behavior**: Configured as `Restrict` (`OnDelete(DeleteBehavior.Restrict)`). 
- **Reasoning**: This prevents accidental deletion of a Country if it still contains Cities. It forces the client to handle orphan cities or explicitly delete them first, protecting the integrity of the data. 

### Indexes
Indexes were created on:
- `Country.Name` and `Country.Code` for fast searching and filtering.
- `City.Name` and `City.CountryId` to optimize foreign key lookups and search filtering.

## 6. Setup Instructions
1. Ensure you have the **.NET 10 SDK** installed.
2. Clone this repository.
3. Open a terminal in the solution root directory.
4. Run `dotnet restore` to restore dependencies.

## 7. Connection String Configuration
The connection string is defined in `src/CountriesCities.API/appsettings.json`.
Update the `DefaultConnection` if you are using a different SQL Server instance (like SQLEXPRESS):
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=CountriesCitiesDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

## 8. EF Core Migration Commands
To apply the initial migrations and create the database, run:
```bash
dotnet ef database update -s src/CountriesCities.API -p src/CountriesCities.Infrastructure
```

## 9. How to run the API
Run the following command from the root folder:
```bash
dotnet run --project src/CountriesCities.API
```

## 10. Swagger URL
When running in Development mode, navigate to:
`https://localhost:<port>/swagger` (or `http://localhost:<port>/swagger`) to view the interactive API documentation.

## 11. API Endpoints

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

### Country Cities
- `GET /api/countries/{countryId}/cities` - Get paginated cities for a specific country

## 12. Example Requests
**Create a Country:**
```bash
curl -X POST "https://localhost:5001/api/countries" \
     -H "Content-Type: application/json" \
     -d "{\"name\":\"Egypt\",\"code\":\"EG\"}"
```

**Get Cities with Pagination and Search:**
```bash
curl -X GET "https://localhost:5001/api/cities?pageNumber=1&pageSize=10&search=cairo"
```

## 13. Example Responses
**Successful Response (`200 OK`) for Paged Countries:**
```json
{
  "items": [
    {
      "id": 1,
      "name": "Egypt",
      "code": "EG"
    }
  ],
  "pageNumber": 1,
  "pageSize": 10,
  "totalCount": 1,
  "totalPages": 1,
  "hasPreviousPage": false,
  "hasNextPage": false
}
```

## 14. Validation and Error Handling Explanation
- **Validation**: Achieved using `FluentValidation`. Validations are executed globally via a custom ASP.NET Core `ActionFilter` (`ValidationFilterAttribute`). If a request model is invalid, it short-circuits the pipeline and returns a standard `400 Bad Request` with `ValidationProblemDetails`.
- **Global Error Handling**: Centralized using the modern `IExceptionHandler` interface middleware. Unhandled exceptions are logged, and a standardized `500 Internal Server Error` conforming to `ProblemDetails` is returned, ensuring sensitive stack traces aren't leaked in production.

## 15. Pagination Explanation
Pagination is handled completely at the **database level** to optimize memory and performance. 
- Using `Skip((pageNumber - 1) * pageSize).Take(pageSize)` inside EF Core generates optimized SQL offset/fetch queries.
- Results are wrapped in a generic `PagedResult<T>` structure that includes essential metadata like `TotalCount` and `TotalPages`.

## 16. Filtering Explanation
Filtering is achieved dynamically via `IQueryable`. 
- By utilizing `Where(c => c.Name.Contains(search))`, EF Core generates standard SQL `LIKE '%search%'` queries.
- SQL Server performs case-insensitive comparisons by default, matching our requirement without any overhead in the application logic. 
- Projection (`.Select(...)`) ensures we avoid loading full entities when returning lightweight DTOs.
