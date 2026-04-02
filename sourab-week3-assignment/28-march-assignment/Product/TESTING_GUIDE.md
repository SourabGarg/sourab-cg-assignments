# Testing Product Web API

## Option 1: Swagger UI (Easiest)

1. Run the application: `dotnet run`
2. Open browser: `https://localhost:7091/swagger`
3. Click on each endpoint and click "Try it out" to test

---

## Option 2: cURL Commands

### Get All Products
```bash
curl -X GET "https://localhost:7091/api/product" -H "accept: application/json"
```

### Get Product by ID (ID = 1)
```bash
curl -X GET "https://localhost:7091/api/product/1" -H "accept: application/json"
```

### Create New Product
```bash
curl -X POST "https://localhost:7091/api/product" ^
  -H "Content-Type: application/json" ^
  -d "{\"name\":\"USB Cable\",\"price\":15.99,\"category\":\"Accessories\"}"
```

### Update Product (ID = 1)
```bash
curl -X PUT "https://localhost:7091/api/product/1" ^
  -H "Content-Type: application/json" ^
  -d "{\"name\":\"Premium Laptop\",\"price\":1800.00,\"category\":\"Electronics\"}"
```

### Delete Product (ID = 5)
```bash
curl -X DELETE "https://localhost:7091/api/product/5" -H "accept: application/json"
```

---

## Option 3: PowerShell

### Get All Products
```powershell
Invoke-RestMethod -Uri "https://localhost:7091/api/product" `
    -Method Get `
    -Headers @{"Accept"="application/json"}
```

### Get Product by ID
```powershell
Invoke-RestMethod -Uri "https://localhost:7091/api/product/1" `
    -Method Get `
    -Headers @{"Accept"="application/json"}
```

### Create New Product
```powershell
$body = @{
    name = "Wireless Mouse"
    price = 45.99
    category = "Accessories"
} | ConvertTo-Json

Invoke-RestMethod -Uri "https://localhost:7091/api/product" `
    -Method Post `
    -ContentType "application/json" `
    -Headers @{"Accept"="application/json"} `
    -Body $body
```

### Update Product
```powershell
$body = @{
    name = "Gaming Mouse"
    price = 85.99
    category = "Accessories"
} | ConvertTo-Json

Invoke-RestMethod -Uri "https://localhost:7091/api/product/2" `
    -Method Put `
    -ContentType "application/json" `
    -Headers @{"Accept"="application/json"} `
    -Body $body
```

### Delete Product
```powershell
Invoke-RestMethod -Uri "https://localhost:7091/api/product/5" `
    -Method Delete `
    -Headers @{"Accept"="application/json"}
```

---

## Option 4: Postman Collection JSON

Import this into Postman:

```json
{
  "info": {
    "name": "Product Web API",
    "schema": "https://schema.getpostman.com/json/collection/v2.1.0/collection.json"
  },
  "item": [
    {
      "name": "Get All Products",
      "request": {
        "method": "GET",
        "header": [
          {
            "key": "Accept",
            "value": "application/json"
          }
        ],
        "url": {
          "raw": "https://localhost:7091/api/product",
          "protocol": "https",
          "host": ["localhost"],
          "port": "7091",
          "path": ["api", "product"]
        }
      }
    },
    {
      "name": "Get Product by ID",
      "request": {
        "method": "GET",
        "header": [
          {
            "key": "Accept",
            "value": "application/json"
          }
        ],
        "url": {
          "raw": "https://localhost:7091/api/product/1",
          "protocol": "https",
          "host": ["localhost"],
          "port": "7091",
          "path": ["api", "product", "1"]
        }
      }
    },
    {
      "name": "Create Product",
      "request": {
        "method": "POST",
        "header": [
          {
            "key": "Content-Type",
            "value": "application/json"
          }
        ],
        "body": {
          "mode": "raw",
          "raw": "{\n  \"name\": \"USB Cable\",\n  \"price\": 15.99,\n  \"category\": \"Accessories\"\n}"
        },
        "url": {
          "raw": "https://localhost:7091/api/product",
          "protocol": "https",
          "host": ["localhost"],
          "port": "7091",
          "path": ["api", "product"]
        }
      }
    },
    {
      "name": "Update Product",
      "request": {
        "method": "PUT",
        "header": [
          {
            "key": "Content-Type",
            "value": "application/json"
          }
        ],
        "body": {
          "mode": "raw",
          "raw": "{\n  \"name\": \"Premium Laptop\",\n  \"price\": 1800.00,\n  \"category\": \"Electronics\"\n}"
        },
        "url": {
          "raw": "https://localhost:7091/api/product/1",
          "protocol": "https",
          "host": ["localhost"],
          "port": "7091",
          "path": ["api", "product", "1"]
        }
      }
    },
    {
      "name": "Delete Product",
      "request": {
        "method": "DELETE",
        "header": [
          {
            "key": "Accept",
            "value": "application/json"
          }
        ],
        "url": {
          "raw": "https://localhost:7091/api/product/5",
          "protocol": "https",
          "host": ["localhost"],
          "port": "7091",
          "path": ["api", "product", "5"]
        }
      }
    }
  ]
}
```

### How to Import into Postman:
1. Open Postman
2. Click "Import" (top left)
3. Paste the JSON above
4. Click "Import"
5. Use the requests in the "Product Web API" collection

---

## Expected Responses

### Get All Products (Success)
```json
[
  {
    "id": 1,
    "name": "Laptop",
    "price": 1200.00,
    "category": "Electronics"
  },
  {
    "id": 2,
    "name": "Mouse",
    "price": 25.50,
    "category": "Accessories"
  }
]
```
**Status Code:** 200 OK

### Get Product by ID (Success)
```json
{
  "id": 1,
  "name": "Laptop",
  "price": 1200.00,
  "category": "Electronics"
}
```
**Status Code:** 200 OK

### Get Product by ID (Not Found)
```json
{
  "message": "Product not found"
}
```
**Status Code:** 404 Not Found

### Create Product (Success)
```json
{
  "id": 6,
  "name": "USB Cable",
  "price": 15.99,
  "category": "Accessories"
}
```
**Status Code:** 201 Created

### Create Product (Validation Error)
```json
{
  "errors": {
    "Name": ["Please enter product name"],
    "Price": ["Price must be between 0.01 and 10000"]
  }
}
```
**Status Code:** 400 Bad Request

### Update Product (Success)
```json
{
  "id": 1,
  "name": "Premium Laptop",
  "price": 1800.00,
  "category": "Electronics"
}
```
**Status Code:** 200 OK

### Delete Product (Success)
```json
{
  "message": "Product deleted successfully"
}
```
**Status Code:** 200 OK

### Delete Product (Not Found)
```json
{
  "message": "Product not found"
}
```
**Status Code:** 404 Not Found

---

## Validation Rules

- **Name:** Required, string
- **Price:** Required, decimal between 0.01 and 10000
- **Category:** Required, string

### Invalid Request Example:
```json
{
  "name": "",
  "price": 50000,
  "category": "Electronics"
}
```

**Response (400 Bad Request):**
```json
{
  "errors": {
    "Name": ["Please enter product name"],
    "Price": ["Price must be between 0.01 and 10000"]
  }
}
```

---

## Tips for Testing

1. **Always use HTTPS:** The API uses HTTPS, so use `https://` not `http://`
2. **Check Status Codes:** Different operations return different codes:
   - 200: Success (GET, PUT, DELETE)
   - 201: Created (POST)
   - 400: Bad Request (validation error)
   - 404: Not Found
3. **Use Swagger First:** It's the easiest way to test and understand the API
4. **Test All Validations:** Try creating/updating with invalid data
5. **Keep Port Number:** If port 7091 is in use, the app will use a different port (check console output)

