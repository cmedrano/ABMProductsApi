# Backend-Product

Este proyecto es un **API REST** para manejar productos y categorías con relación muchos a muchos, usando **.NET 8** y **Entity Framework Core**.

---

## Tablas de la base de datos

### Categories
- **Id**: int, clave primaria, autoincremental.
- **Name**: string, obligatorio.

### Products
- **Id**: int, clave primaria, autoincremental.
- **Name**: string, obligatorio.

### ProductCategories (relación muchos a muchos)
- **ProductId**: int, FK a Products(Id).
- **CategoryId**: int, FK a Categories(Id).
- Clave primaria compuesta: (ProductId, CategoryId).

---

## Scripts SQL

```sql
CREATE TABLE Categories (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(255) NOT NULL
);

CREATE TABLE Products (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(255) NOT NULL
);

CREATE TABLE ProductCategories (
    ProductId INT NOT NULL,
    CategoryId INT NOT NULL,
    PRIMARY KEY (ProductId, CategoryId),
    CONSTRAINT FK_ProductCategories_Products FOREIGN KEY (ProductId)
        REFERENCES Products(Id) ON DELETE CASCADE,
    CONSTRAINT FK_ProductCategories_Categories FOREIGN KEY (CategoryId)
        REFERENCES Categories(Id) ON DELETE CASCADE
);
