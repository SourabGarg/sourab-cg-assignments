# Product Web API - CRUD Operations

This is a complete ASP.NET Core Web API project that implements CRUD (Create, Read, Update, Delete) operations for a Product model.

## Project Structure

```
Product/
├── Models/
│   ├── Product.cs           # Product model class
│   └── ProductContext.cs    # Entity Framework DbContext
├── Interfaces/
│   └── IProduct.cs          # Service interface for CRUD operations
├── Services/
│   └── ProductService.cs    # Service implementation
├── Controllers/
│   └── ProductController.cs # API controller with endpoints
├── Migrations/              # Database migrations (auto-generated)
├── Program.cs              # Application configuration
├── appsettings.json        # Configuration including connection string
└── Product.csproj          # Project file with NuGet dependencies
```

## Prerequisites

- .NET 8.0 SDK
- SQL Server 2019 or later (running on localhost:1433)
- Visual Studio 2022 or higher

## Database Connection

The connection string in `appsettings.json` is configured as:
```
Server=localhost,1433;Database=ProductDb;User Id=sa;Password=Anand@123;TrustServerCertificate=True
```

**Note:** Update the password in `appsettings.json` if your SQL Server has a different password.

## Setup Instructions

1. **Restore Dependencies:**
   ```powershell
   dotnet restore
   ```

2. **Apply Database Migrations:**
   ```powershell
   dotnet ef database update
   ```
   The database will be created with 5 sample products.

3. **Run the Application:**
   ```powershell
   dotnet run
   ```
   The API will start at: `https://localhost:7091` (or another port if 7091 is in use)

## API Endpoints

### Get All Products
- **Method:** `GET`
- **URL:** `/api/product`
- **Response:** Returns a list of all products

**Example:**
```
GET https://localhost:7091/api/product
```

### Get Product by ID
- **Method:** `GET`
- **URL:** `/api/product/{id}`
- **Parameters:** `id` (integer)
- **Response:** Returns a specific product by ID

**Example:**
```
GET https://localhost:7091/api/product/1
```

### Create a New Product
- **Method:** `POST`
- **URL:** `/api/product`
- **Body (JSON):**
```json
{
  "name": "New Product",
  "price": 99.99,
  "category": "Electronics"
}
```
- **Response:** Returns the created product with its ID

**Example:**
```
POST https://localhost:7091/api/product
Content-Type: application/json

{
  "name": "USB Cable",
  "price": 15.99,
  "category": "Accessories"
}
```

### Update a Product
- **Method:** `PUT`
- **URL:** `/api/product/{id}`
- **Parameters:** `id` (integer)
- **Body (JSON):**
```json
{
  "name": "Updated Name",
  "price": 199.99,
  "category": "Updated Category"
}
```
- **Response:** Returns the updated product

**Example:**
```
PUT https://localhost:7091/api/product/1
Content-Type: application/json

{
  "name": "Gaming Laptop",
  "price": 1500.00,
  "category": "Electronics"
}
```

### Delete a Product
- **Method:** `DELETE`
- **URL:** `/api/product/{id}`
- **Parameters:** `id` (integer)
- **Response:** Returns a success/failure message

**Example:**
```
DELETE https://localhost:7091/api/product/5
```

## Testing the API

### Using Swagger UI (Recommended)
1. Run the application
2. Open your browser and navigate to: `https://localhost:7091/swagger`
3. You can test all endpoints directly from the Swagger UI

### Using Postman
1. Import the endpoints from Swagger or create them manually
2. Set the base URL to `https://localhost:7091`
3. Test each endpoint with the examples provided above

### Using PowerShell
```powershell
# Get all products
Invoke-RestMethod -Uri "https://localhost:7091/api/product" -Method Get

# Get product by ID
Invoke-RestMethod -Uri "https://localhost:7091/api/product/1" -Method Get

# Create a new product
$body = @{
    name = "Wireless Mouse"
    price = 45.99
    category = "Accessories"
} | ConvertTo-Json

Invoke-RestMethod -Uri "https://localhost:7091/api/product" `
    -Method Post `
    -ContentType "application/json" `
    -Body $body
```

## Dummy Data

The database is pre-seeded with the following products:

| ID | Name | Price | Category |
|----|------|-------|----------|
| 1 | Laptop | 1200.00 | Electronics |
| 2 | Mouse | 25.50 | Accessories |
| 3 | Keyboard | 75.00 | Accessories |
| 4 | Monitor | 350.00 | Electronics |
| 5 | Headphones | 120.00 | Audio |

## NuGet Dependencies

- **Swashbuckle.AspNetCore** (6.6.2): Swagger/OpenAPI support
- **Microsoft.EntityFrameworkCore** (8.0.0): ORM framework
- **Microsoft.EntityFrameworkCore.SqlServer** (8.0.0): SQL Server provider
- **Microsoft.EntityFrameworkCore.Tools** (8.0.0): Migration tools

## Project Architecture

```
Data Layer (ProductContext)
        ↓
Service Layer (IProduct Interface & ProductService)
        ↓
Controller Layer (ProductController)
        ↓
API Endpoints
```

### Components

**Model Layer:**
- `Product.cs`: Defines the product entity with validation attributes

**Data Layer:**
- `ProductContext.cs`: DbContext that manages database operations and seeding

**Business Logic Layer:**
- `IProduct.cs`: Interface defining CRUD operations
- `ProductService.cs`: Implementation of CRUD operations using Entity Framework

**Presentation Layer:**
- `ProductController.cs`: RESTful API controller with endpoints

**Configuration:**
- `Program.cs`: Registers services, configures DbContext, and sets up the middleware
- `appsettings.json`: Stores connection string and other configuration

## Future Enhancements

- Add pagination to GetAllProducts endpoint
- Add filtering/searching capabilities
- Add sorting options
- Implement authentication and authorization
- Add logging
- Add unit tests
- Add data validation enhancements
- Implement caching
- Add image upload support (ImageUrl field)

## Troubleshooting

### Database Connection Error
- Ensure SQL Server is running on localhost:1433
- Verify the connection string in appsettings.json
- Check username and password are correct

### Swagger Not Loading
- Ensure the application is running in Development mode
- Check that Swashbuckle.AspNetCore is properly installed
- Verify the Swagger middleware is configured in Program.cs

### Migration Errors
- Delete the Migrations folder and database if you made schema changes
- Run `dotnet ef migrations add InitialCreate` again
- Run `dotnet ef database update`
