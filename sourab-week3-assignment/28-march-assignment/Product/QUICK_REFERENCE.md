# 📖 Product Web API - Quick Reference & Index

## 🎯 Start Here

### To Run the API
```powershell
cd 'C:\Users\AnandThakur\Desktop\anand-assignments\28-march-assignment\Product'
dotnet run
```

### To Test the API
Open your browser: `https://localhost:7091/swagger`

---

## 📚 Documentation Files

| File | Purpose | When to Use |
|------|---------|------------|
| **README.md** | Complete setup & API documentation | Starting out, need full guide |
| **TESTING_GUIDE.md** | Testing examples (Swagger, Postman, cURL, PowerShell) | Testing the API |
| **PROJECT_SUMMARY.md** | Project overview & quick summary | Understanding project status |
| **SETUP_COMPLETE.md** | Setup checklist & verification | Checking what's been done |
| **IMPLEMENTATION_DETAILS.md** | Detailed implementation summary | Understanding the code |
| **API_ARCHITECTURE.md** | Visual diagrams & architecture flow | Learning how it works |

---

## 🔌 API Endpoints Quick Reference

```
GET    /api/product              → Get all products
GET    /api/product/{id}         → Get single product
POST   /api/product              → Create new product
PUT    /api/product/{id}         → Update product
DELETE /api/product/{id}         → Delete product
```

---

## 📂 File Structure

```
Models/
  ├── Product.cs                  ← Entity model
  └── ProductContext.cs           ← Database context

Interfaces/
  └── IProduct.cs                 ← Service interface

Services/
  └── ProductService.cs           ← Business logic

Controllers/
  └── ProductController.cs        ← API endpoints

Migrations/
  ├── ...InitialCreate.cs
  ├── ...ConfigurePriceDecimal.cs
  └── ...SeedProductData.cs

Program.cs                         ← Configuration
appsettings.json                   ← Connection string
Product.csproj                     ← Dependencies
```

---

## 💻 Quick Testing Commands

### PowerShell - Get All Products
```powershell
Invoke-RestMethod -Uri "https://localhost:7091/api/product" -Method Get
```

### PowerShell - Create Product
```powershell
$body = @{
    name = "USB Cable"
    price = 15.99
    category = "Accessories"
} | ConvertTo-Json

Invoke-RestMethod -Uri "https://localhost:7091/api/product" `
    -Method Post `
    -ContentType "application/json" `
    -Body $body
```

### cURL - Get Product by ID
```bash
curl -X GET "https://localhost:7091/api/product/1" -H "accept: application/json"
```

---

## 🗄️ Database

**Server:** localhost:1433  
**Database:** ProductDb  
**Table:** Products  
**Connection String:** appsettings.json

**Sample Data:**
| Id | Name | Price | Category |
|----|------|-------|----------|
| 1 | Laptop | 1200.00 | Electronics |
| 2 | Mouse | 25.50 | Accessories |
| 3 | Keyboard | 75.00 | Accessories |
| 4 | Monitor | 350.00 | Electronics |
| 5 | Headphones | 120.00 | Audio |

---

## 🔧 Configuration

### appsettings.json - Connection String
```json
"ConnectionStrings": {
  "ProductConnection": "Server=localhost,1433;Database=ProductDb;User Id=sa;Password=Anand@123;TrustServerCertificate=True"
}
```

### Program.cs - Key Registrations
```csharp
// DbContext
builder.Services.AddDbContext<ProductContext>(options =>
    options.UseSqlServer(connectionString));

// Service
builder.Services.AddScoped<IProduct, ProductService>();

// Swagger
builder.Services.AddSwaggerGen();
```

---

## ✅ Features Implemented

- [x] GET all products
- [x] GET product by ID
- [x] POST create product
- [x] PUT update product
- [x] DELETE product
- [x] Database seeding with sample data
- [x] Swagger/OpenAPI documentation
- [x] Async operations throughout
- [x] Proper error handling
- [x] Data validation
- [x] Dependency injection
- [x] N-tier architecture

---

## 📊 HTTP Status Codes

| Code | Scenario |
|------|----------|
| 200 | GET, PUT, DELETE success |
| 201 | POST success (created) |
| 400 | Validation error |
| 404 | Resource not found |

---

## 🎓 Architecture Layers

```
Controllers     ← HTTP Request Handler
    ↓
Services        ← Business Logic (IProduct interface)
    ↓
DbContext       ← Data Access (EF Core)
    ↓
Database        ← Data Storage (SQL Server)
```

---

## 📦 Dependencies

```
Microsoft.EntityFrameworkCore (8.0.0)
Microsoft.EntityFrameworkCore.SqlServer (8.0.0)
Microsoft.EntityFrameworkCore.Tools (8.0.0)
Swashbuckle.AspNetCore (6.6.2)
```

---

## 🚀 Common Tasks

### Run Application
```powershell
dotnet run
```

### Build Application
```powershell
dotnet build
```

### Add New Migration
```powershell
dotnet ef migrations add YourMigrationName
```

### Apply Migrations
```powershell
dotnet ef database update
```

### Drop Database
```powershell
dotnet ef database drop
```

---

## 🔍 Troubleshooting

| Issue | Solution |
|-------|----------|
| Port 7091 in use | App auto-selects different port |
| DB connection error | Check appsettings.json connection string |
| Swagger not loading | Verify app is in Development mode |
| Migration issues | Delete Migrations folder and recreate |

---

## 📞 Code Organization

### Models (Data Layer)
- Define entity structure
- Add validation attributes
- Configure database mapping

### Interfaces (Abstraction)
- Define service contract
- Enable dependency injection
- Support unit testing

### Services (Business Logic)
- Implement interface
- Handle database operations
- Apply business rules

### Controllers (API Layer)
- Handle HTTP requests
- Call services
- Return responses

---

## 🎯 Next Steps

1. **Run:** `dotnet run`
2. **Test:** Visit https://localhost:7091/swagger
3. **Explore:** Try all endpoints
4. **Learn:** Read the documentation files
5. **Extend:** Add more features as needed

---

## 📋 Verification Checklist

- [x] Project builds successfully
- [x] Database created (ProductDb)
- [x] Migrations applied
- [x] Sample data seeded
- [x] All endpoints working
- [x] Swagger UI accessible
- [x] Service layer implemented
- [x] Dependency injection configured
- [x] Documentation complete

---

## 🎉 You're All Set!

Your **Product Web API** is:
- ✅ Fully implemented
- ✅ Properly configured
- ✅ Ready to run
- ✅ Documented
- ✅ Tested and verified

**Next Action:** Run `dotnet run` and visit https://localhost:7091/swagger

---

## Quick Links

**Local Endpoints:**
- Swagger UI: https://localhost:7091/swagger
- API Base: https://localhost:7091/api/product
- Get All: GET https://localhost:7091/api/product
- Get One: GET https://localhost:7091/api/product/1

**Main Files:**
- Controller: Controllers/ProductController.cs
- Service: Services/ProductService.cs
- Model: Models/Product.cs
- Context: Models/ProductContext.cs
- Config: Program.cs & appsettings.json

---

💡 **Pro Tip:** Keep Swagger UI open while testing - it shows all endpoints and allows direct testing!

