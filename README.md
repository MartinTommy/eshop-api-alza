
# Eshop API

A REST API for managing products in an e-shop. This API provides functionality to list products, retrieve a product by ID, update a product's description, and versioned API support with pagination.

## Project Structure

- **API Versioning**: Version 1 (v1) provides basic CRUD functionality for products, and Version 2 (v2) adds pagination.
- **Swagger**: Auto-generated API documentation available at `/swagger`.
- **Database**: The API uses Microsoft SQL Server with Entity Framework Core as the ORM.
- **Unit Testing**: Unit tests cover key API functionality using xUnit and EF Core InMemory for mock data.

## Features

- `GET /api/v1/products`: List all products.
- `GET /api/v1/products/{id}`: Retrieve product by ID.
- `PATCH /api/v1/products/{id}/description`: Update product description.
- `GET /api/v2/products?page={page}&size={size}`: Paginated product list.

## Prerequisites

Before running the project, make sure you have the following installed on your system:

- **.NET 8.0 SDK**
- **Microsoft SQL Server**
- **SQL Server Management Studio (SSMS)** (optional for database management)
- **Visual Studio 2022**

## Installation Instructions

1. **Clone the Repository**  
   Use the following command to clone the repository from GitHub:

   ```bash
   git clone https://github.com/MartinTommy/eshop-api-alza.git
   ```

2. **Set Up the Database**  
   Open SQL Server Management Studio (SSMS) or any SQL client.  
   Create a new database called `EshopDbAlza` using the following query:

   ```sql
   CREATE DATABASE EshopDbAlza;
   ```

3. **Update `appsettings.json`**  
   In the `appsettings.json` file, update the `ConnectionStrings` to match your SQL Server instance:

   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=EshopDbAlza;Trusted_Connection=True;TrustServerCertificate=true;"
     },
     "Logging": {
       "LogLevel": {
         "Default": "Information",
         "Microsoft.AspNetCore": "Warning"
       }
     },
     "AllowedHosts": "*"
   }
   ```

   If you're using SQL Server with Docker, use the appropriate connection string (e.g., using localhost with the Docker port or using SQL Server authentication).

## Running the Application

### Using Visual Studio

1. Open the solution file (`EshopApiAlzaSolution.sln`) in Visual Studio.
2. Select `EshopApiAlza` as the startup project.
3. Press `F5` to run the application.

Once the app is running, you can access the Swagger documentation at:

```
http://localhost:<port>/swagger
```

First run of the `EshopApiAlza` profile will automatically start the initial database migration. This will add Products table into the EshopDbAlza database and seed initial data.

## API Documentation

The API documentation is automatically generated using Swagger. 

## Versioning

The API supports two versions:

- **v1**: Basic product listing and retrieval.
- **v2**: Adds pagination to product listings.

## Running Unit Tests

Unit tests are implemented using xUnit and EF Core InMemory to mock the database.

### Run Unit Tests in Visual Studio

1. Open the solution in Visual Studio.
2. Go to the Test Explorer window.
3. Click `Run All` to execute the tests.

### Unit Test configuration

In the `appsettings.json` file, update the `ConnectionStrings` to match your SQL Server instance. Use the `UseMockData` configuration value to switch between mock data and DB data.

```json
{
  "ConnectionString": "Server=localhost;Database=EshopDbAlza;Trusted_Connection=True;TrustServerCertificate=true;",
  "UseMockData": true
}
```

## Project Structure

```plaintext
EshopApiAlza/
├── Controllers/
│   ├── ProductsController.cs   # Version 1 API
│   ├── ProductsV2Controller.cs   # Version 2 API with pagination
├── Data/
│   └── AppDbContext.cs           # EF Core DbContext for SQL Server
├── Models/
│   ├── Product.cs                # Product model
│   └── PaginatedResponse.cs      # Pagination model for v2
├── appsettings.json              # Configuration file (Connection Strings, etc.)
└── Program.cs                    # Main entry point for configuring services (Swagger, DbContext, etc.)
```

## Troubleshooting

### Common Issues

- **Database Connection**: Ensure your SQL Server instance is running and the connection string in `appsettings.json` is correct.
- **EF Core Migrations**: If migrations fail, ensure that Entity Framework Core tools are installed and the connection to the database is valid.
- **Port Conflicts**: If you receive a port conflict error, change the port number in `launchSettings.json`.

