# 🎨 Product Web API - Visual Architecture & Flow

## Project Architecture Diagram

```
┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓
┃                   CLIENT LAYER                        ┃
┃        (Browser, Postman, Mobile App, etc.)           ┃
┣━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┫
                    HTTPS Request ↓
┌─────────────────────────────────────────────────────────┐
│              PRESENTATION LAYER                         │
│  ┌────────────────────────────────────────────────────┐ │
│  │      ProductController.cs                          │ │
│  │  ├─ [HttpGet] GetAllProducts()                     │ │
│  │  ├─ [HttpGet("{id}")] GetProductById(id)           │ │
│  │  ├─ [HttpPost] AddProduct(product)                 │ │
│  │  ├─ [HttpPut("{id}")] UpdateProduct(id, product)   │ │
│  │  └─ [HttpDelete("{id}")] DeleteProduct(id)         │ │
│  └────────────────────────────────────────────────────┘ │
│              Routes: /api/product/*                     │
└─────────────────────────────────────────────────────────┘
                    Method Call ↓
┌─────────────────────────────────────────────────────────┐
│           BUSINESS LOGIC LAYER (Services)               │
│  ┌────────────────────────────────────────────────────┐ │
│  │  IProduct.cs (Interface)                           │ │
│  │  ├─ GetAllProductsAsync()                          │ │
│  │  ├─ GetProductByIdAsync(id)                        │ │
│  │  ├─ AddProductAsync(product)                       │ │
│  │  ├─ UpdateProductAsync(id, product)                │ │
│  │  └─ DeleteProductAsync(id)                         │ │
│  └────────────────────────────────────────────────────┘ │
│                         ↓                               │
│  ┌────────────────────────────────────────────────────┐ │
│  │  ProductService.cs (Implementation)                │ │
│  │  - Implements IProduct interface                   │ │
│  │  - Contains CRUD logic                             │ │
│  │  - Uses ProductContext for DB access               │ │
│  └────────────────────────────────────────────────────┘ │
└─────────────────────────────────────────────────────────┘
                  DbContext Call ↓
┌─────────────────────────────────────────────────────────┐
│         DATA ACCESS LAYER (Entity Framework)            │
│  ┌────────────────────────────────────────────────────┐ │
│  │  ProductContext.cs                                 │ │
│  │  ├─ DbSet<Product> Products { get; set; }         │ │
│  │  ├─ OnModelCreating(ModelBuilder)                 │ │
│  │  │  ├─ HasPrecision(18, 2) for Price              │ │
│  │  │  └─ Seed initial data                          │ │
│  │  └─ Inherits from DbContext                       │ │
│  └────────────────────────────────────────────────────┘ │
│                                                         │
│  Generates SQL Queries:                                │
│  ├─ SELECT * FROM Products                             │
│  ├─ SELECT * FROM Products WHERE Id = @id              │
│  ├─ INSERT INTO Products (Name, Price, Category)       │
│  ├─ UPDATE Products SET ... WHERE Id = @id             │
│  └─ DELETE FROM Products WHERE Id = @id                │
└─────────────────────────────────────────────────────────┘
                     SQL Query ↓
┌─────────────────────────────────────────────────────────┐
│            DATABASE LAYER (SQL Server)                  │
│  ┌────────────────────────────────────────────────────┐ │
│  │  Database: ProductDb                               │ │
│  │  ┌──────────────────────────────────────────────┐  │ │
│  │  │  Table: Products                             │  │ │
│  │  ├──────────────────────────────────────────────┤  │ │
│  │  │ Id (INT PK)│Name (NVARCHAR) │Price │Category │  │ │
│  │  ├──────────────────────────────────────────────┤  │ │
│  │  │ 1         │ Laptop          │1200  │Electron │  │ │
│  │  │ 2         │ Mouse           │25.5  │Access   │  │ │
│  │  │ 3         │ Keyboard        │75    │Access   │  │ │
│  │  │ 4         │ Monitor         │350   │Electron │  │ │
│  │  │ 5         │ Headphones      │120   │Audio    │  │ │
│  │  └──────────────────────────────────────────────┘  │ │
│  └────────────────────────────────────────────────────┘ │
└─────────────────────────────────────────────────────────┘
                  Data Response ↓
┌─────────────────────────────────────────────────────────┐
│           Response JSON with Status Code                │
│  Status: 200 OK, 201 Created, 404 Not Found, etc.       │
│  Body: JSON representation of Product(s)                │
└─────────────────────────────────────────────────────────┘
```

---

## API Request/Response Flow

### 1. GET All Products
```
┌──────────────────────────────────────────────────────────┐
│ CLIENT (Browser/Postman)                                 │
│ GET /api/product                                         │
└───────────────────┬──────────────────────────────────────┘
                    │
                    ▼
┌──────────────────────────────────────────────────────────┐
│ CONTROLLER                                               │
│ GetAllProducts() method called                            │
│ → Calls _productService.GetAllProductsAsync()            │
└───────────────────┬──────────────────────────────────────┘
                    │
                    ▼
┌──────────────────────────────────────────────────────────┐
│ SERVICE                                                  │
│ GetAllProductsAsync()                                    │
│ → Calls _context.Products.ToListAsync()                 │
└───────────────────┬──────────────────────────────────────┘
                    │
                    ▼
┌──────────────────────────────────────────────────────────┐
│ DATABASE CONTEXT                                         │
│ Generates SQL: SELECT * FROM Products                    │
│ Executes query on SQL Server                             │
└───────────────────┬──────────────────────────────────────┘
                    │
                    ▼
┌──────────────────────────────────────────────────────────┐
│ SQL SERVER                                               │
│ Returns 5 product rows                                   │
└───────────────────┬──────────────────────────────────────┘
                    │ Data flows back up
                    ▼
┌──────────────────────────────────────────────────────────┐
│ SERVICE converts to List<Product>                        │
└───────────────────┬──────────────────────────────────────┘
                    │
                    ▼
┌──────────────────────────────────────────────────────────┐
│ CONTROLLER serializes to JSON                            │
│ return Ok(products)  → 200 OK Status                     │
└───────────────────┬──────────────────────────────────────┘
                    │
                    ▼
┌──────────────────────────────────────────────────────────┐
│ HTTP RESPONSE                                            │
│ Status: 200 OK                                           │
│ Body:                                                    │
│ [                                                        │
│   { "id": 1, "name": "Laptop", "price": 1200, ... },    │
│   { "id": 2, "name": "Mouse", "price": 25.5, ... },     │
│   ...                                                    │
│ ]                                                        │
└──────────────────────────────────────────────────────────┘
```

### 2. POST Create Product
```
┌──────────────────────────────────────────────────────────┐
│ CLIENT sends                                             │
│ POST /api/product                                        │
│ Body: { "name": "USB Cable", "price": 15.99, ... }      │
└───────────────────┬──────────────────────────────────────┘
                    │
                    ▼
┌──────────────────────────────────────────────────────────┐
│ CONTROLLER                                               │
│ AddProduct(product) method called                        │
│ Validates ModelState                                     │
│ → If valid: Calls _productService.AddProductAsync()     │
│ → If invalid: Returns 400 Bad Request                    │
└───────────────────┬──────────────────────────────────────┘
                    │
                    ▼
┌──────────────────────────────────────────────────────────┐
│ SERVICE                                                  │
│ AddProductAsync(product)                                 │
│ 1. _context.Products.Add(product)                        │
│ 2. await _context.SaveChangesAsync()                     │
│ 3. return product (with auto-generated Id)               │
└───────────────────┬──────────────────────────────────────┘
                    │
                    ▼
┌──────────────────────────────────────────────────────────┐
│ DATABASE CONTEXT                                         │
│ SQL: INSERT INTO Products (Name, Price, Category)        │
│      VALUES (@name, @price, @category)                   │
│ Returns: Id = 6 (auto-incremented)                       │
└───────────────────┬──────────────────────────────────────┘
                    │
                    ▼
┌──────────────────────────────────────────────────────────┐
│ HTTP RESPONSE                                            │
│ Status: 201 Created                                      │
│ Body:                                                    │
│ { "id": 6, "name": "USB Cable", "price": 15.99, ... }   │
│ Location: /api/product/6                                │
└──────────────────────────────────────────────────────────┘
```

### 3. PUT Update Product
```
PUT /api/product/1
Body: { "name": "Gaming Laptop", "price": 1800, ... }
        │
        ▼
    CONTROLLER validates & calls service
        │
        ▼
    SERVICE FindOrDefault + Update + SaveChanges
        │
        ▼
    SQL: UPDATE Products SET Name=@name, Price=@price
         WHERE Id = 1
        │
        ▼
    Response: 200 OK with updated product
```

### 4. DELETE Product
```
DELETE /api/product/5
        │
        ▼
    CONTROLLER calls service
        │
        ▼
    SERVICE FindOrDefault + Remove + SaveChanges
        │
        ▼
    SQL: DELETE FROM Products WHERE Id = 5
        │
        ▼
    Response: 200 OK with success message
```

---

## Dependency Injection Flow

```
┌──────────────────────────────────────────────────────┐
│  Program.cs                                          │
│                                                      │
│  // Register DbContext                              │
│  builder.Services.AddDbContext<ProductContext>(     │
│      options => options.UseSqlServer(connectionStr) │
│  );                                                  │
│                                                      │
│  // Register Service                                │
│  builder.Services.AddScoped<IProduct,                │
│      ProductService>();                            │
└────────────────┬─────────────────────────────────────┘
                 │
                 │ ASP.NET Core DI Container
                 │ ├─ ProductContext → SQL Server
                 │ ├─ IProduct → ProductService
                 │ └─ ProductService uses ProductContext
                 │
                 ▼
┌──────────────────────────────────────────────────────┐
│  ProductController                                   │
│                                                      │
│  public ProductController(IProduct productService)  │
│  {                                                   │
│      _productService = productService;              │
│      // DI Container injects ProductService here    │
│  }                                                   │
└──────────────────────────────────────────────────────┘
```

---

## Folder Structure

```
Product/
│
├── Models/                          ← Data Models & DbContext
│   ├── Product.cs                   • Entity class with validation
│   └── ProductContext.cs            • EF Core DbContext
│
├── Interfaces/                      ← Service Abstractions
│   └── IProduct.cs                  • CRUD operation contract
│
├── Services/                        ← Business Logic
│   └── ProductService.cs            • Implements CRUD operations
│
├── Controllers/                     ← API Endpoints
│   └── ProductController.cs         • HTTP methods & routing
│
├── Migrations/                      ← Database Migrations
│   ├── 20250328045219_InitialCreate.cs
│   ├── 20250328045257_ConfigurePriceDecimal.cs
│   ├── 20250328045322_SeedProductData.cs
│   └── ProductContextModelSnapshot.cs
│
├── Configuration Files
│   ├── Program.cs                   • App setup & DI registration
│   ├── appsettings.json            • Connection string
│   └── Product.csproj              • NuGet dependencies
│
└── Documentation/
    ├── README.md                    • Full documentation
    ├── TESTING_GUIDE.md            • Testing instructions
    ├── PROJECT_SUMMARY.md          • Overview
    ├── SETUP_COMPLETE.md           • Setup checklist
    └── IMPLEMENTATION_DETAILS.md   • This architecture guide
```

---

## HTTP Status Codes Used

| Code | Meaning | Usage |
|------|---------|-------|
| 200 | OK | GET, PUT, DELETE success |
| 201 | Created | POST success - resource created |
| 400 | Bad Request | Validation error |
| 404 | Not Found | Resource doesn't exist |
| 500 | Server Error | Database/server error |

---

## Data Flow in Code

```csharp
// 1. HTTP Request arrives at Controller
[HttpPost]
public async Task<ActionResult<Product>> AddProduct([FromBody] Product product)
{
    // 2. Controller validates via ModelState
    if (!ModelState.IsValid)
        return BadRequest(ModelState);

    // 3. Controller calls Service method (Dependency Injection)
    var createdProduct = await _productService.AddProductAsync(product);
    
    // 4. Returns HTTP response with status 201
    return CreatedAtAction(nameof(GetProductById), 
        new { id = createdProduct.Id }, createdProduct);
}

// Service Layer
public async Task<Product> AddProductAsync(Product product)
{
    // 5. Service uses DbContext to add entity
    _context.Products.Add(product);
    
    // 6. Service saves changes (generates INSERT SQL)
    await _context.SaveChangesAsync();
    
    // 7. Returns created product with auto-generated Id
    return product;
}

// Entity Framework translates to SQL:
// INSERT INTO Products (Name, Price, Category) 
// VALUES (@p0, @p1, @p2)
// SELECT SCOPE_IDENTITY()
```

---

## Validation Flow

```
Client sends Invalid Data
        ↓
Controller receives request
        ↓
ModelState validation (attributes check)
        ├─ [Required] - Is field provided?
        ├─ [Range(0.01, 10000)] - Is price valid?
        └─ Message - "Price must be between 0.01 and 10000"
        ↓
If Invalid: Return 400 Bad Request with errors
If Valid: Continue to Service layer
        ↓
Service processes and saves to database
```

---

## Complete Request Lifecycle Example

```
1. Browser sends:  GET /api/product

2. ASP.NET Core routes to ProductController.GetAllProducts()

3. Controller calls: await _productService.GetAllProductsAsync()

4. Service executes: await _context.Products.ToListAsync()

5. EF Core translates to SQL: SELECT * FROM Products

6. SQL Server executes query and returns rows

7. EF Core maps rows to Product objects

8. Service returns List<Product>

9. Controller calls: return Ok(products)

10. ASP.NET Core serializes to JSON

11. HTTP Response:
    Status: 200 OK
    Content-Type: application/json
    Body: [{"id":1,"name":"Laptop",...}, ...]

12. Browser receives and displays response
```

---

✅ Your complete Web API is structured following professional N-tier architecture!

