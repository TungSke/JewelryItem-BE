-- Tạo bảng Customers
USE master
CREATE DATABASE JewelryItem
GO
USE JewelryItem
CREATE TABLE Customers (
    CustomerID INT IDENTITY(1,1) PRIMARY KEY,
    FullName VARCHAR(100) NOT NULL,
    PhoneNumber VARCHAR(20) NOT NULL,
    Email VARCHAR(100) UNIQUE,
    LoyaltyPoints INT DEFAULT 0,
    CreatedAt DATETIME NOT NULL,
    UpdatedAt DATETIME NOT NULL
);

-- Tạo bảng Products
CREATE TABLE Products (
    ProductID INT IDENTITY(1,1) PRIMARY KEY,
    ProductCode VARCHAR(50) NOT NULL UNIQUE,
    ProductName VARCHAR(100) NOT NULL,
    Description TEXT,
    Category VARCHAR(50) NOT NULL,
    UnitPrice DECIMAL(18,2) NOT NULL,
    CostPrice DECIMAL(18,2) NOT NULL,
    Weight DECIMAL(10,3) NOT NULL,
    IsJewelry BIT NOT NULL DEFAULT 1,
    IsGold BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME NOT NULL,
    UpdatedAt DATETIME NOT NULL
);

-- Tạo bảng Orders
CREATE TABLE Orders (
    OrderID INT IDENTITY(1,1) PRIMARY KEY,
    OrderNumber VARCHAR(20) NOT NULL UNIQUE,
    CustomerID INT NOT NULL FOREIGN KEY REFERENCES Customers(CustomerID),
    TotalAmount DECIMAL(18,2) NOT NULL,
    DiscountAmount DECIMAL(18,2) DEFAULT 0,
    FinalAmount DECIMAL(18,2) NOT NULL,
    PaymentMethod VARCHAR(50) NOT NULL,
    CreatedAt DATETIME NOT NULL,
    UpdatedAt DATETIME NOT NULL,
	[Status] varchar(50) default 'Not Yet'
);

-- Tạo bảng OrderItems
CREATE TABLE OrderItems (
    OrderItemID INT IDENTITY(1,1) PRIMARY KEY,
    OrderID INT NOT NULL FOREIGN KEY REFERENCES Orders(OrderID),
    ProductID INT NOT NULL FOREIGN KEY REFERENCES Products(ProductID),
    Quantity INT NOT NULL,
    UnitPrice DECIMAL(18,2) NOT NULL,
    DiscountAmount DECIMAL(18,2) DEFAULT 0,
    FinalPrice DECIMAL(18,2) NOT NULL,
    CreatedAt DATETIME NOT NULL,
    UpdatedAt DATETIME NOT NULL
);

-- Tạo bảng Employees
CREATE TABLE Employees (
    EmployeeID INT IDENTITY(1,1) PRIMARY KEY,
    FullName VARCHAR(100) NOT NULL,
    PhoneNumber VARCHAR(20) NOT NULL,
    Email VARCHAR(100) UNIQUE,
	Password VARCHAR(100),
    Role VARCHAR(50) NOT NULL,
    Department VARCHAR(50) NOT NULL,
    CreatedAt DATETIME NOT NULL,
    UpdatedAt DATETIME NOT NULL
);

-- Tạo bảng Stores
CREATE TABLE Stores (
    StoreID INT IDENTITY(1,1) PRIMARY KEY,
    StoreName VARCHAR(100) NOT NULL,
    Address VARCHAR(200) NOT NULL,
    PhoneNumber VARCHAR(20) NOT NULL,
    CreatedAt DATETIME NOT NULL,
    UpdatedAt DATETIME NOT NULL
);

-- Tạo bảng StoreEmployees (bảng liên kết giữa Employees và Stores)
CREATE TABLE StoreEmployees (
    StoreID INT NOT NULL FOREIGN KEY REFERENCES Stores(StoreID),
    EmployeeID INT NOT NULL FOREIGN KEY REFERENCES Employees(EmployeeID),
    PRIMARY KEY (StoreID, EmployeeID)
);

-- Tạo bảng ProductWarranties
CREATE TABLE ProductWarranties (
    WarrantyID INT IDENTITY(1,1) PRIMARY KEY,
    OrderItemID INT NOT NULL FOREIGN KEY REFERENCES OrderItems(OrderItemID),
    WarrantyPeriod INT NOT NULL,
    WarrantyExpireDate DATETIME NOT NULL,
    CreatedAt DATETIME NOT NULL,
    UpdatedAt DATETIME NOT NULL
);

-- Tạo bảng GoldPriceHistory
CREATE TABLE GoldPriceHistory (
    GoldPriceHistoryID INT IDENTITY(1,1) PRIMARY KEY,
    GoldPrice DECIMAL(18,2) NOT NULL,
    UpdatedAt DATETIME NOT NULL
);

-- Tạo bảng Promotions
CREATE TABLE Promotions (
    PromotionID INT IDENTITY(1,1) PRIMARY KEY,
    PromotionName VARCHAR(100) NOT NULL,
    DiscountPercentage DECIMAL(5,2) NOT NULL,
    StartDate DATETIME NOT NULL,
    EndDate DATETIME NOT NULL,
    CreatedAt DATETIME NOT NULL,
    UpdatedAt DATETIME NOT NULL
);

-- Tạo bảng CustomerDiscounts (bảng liên kết giữa Customers và Promotions)
CREATE TABLE CustomerDiscounts (
    CustomerID INT NOT NULL FOREIGN KEY REFERENCES Customers(CustomerID),
    PromotionID INT NOT NULL FOREIGN KEY REFERENCES Promotions(PromotionID),
    DiscountPercentage DECIMAL(5,2) NOT NULL,
    PRIMARY KEY (CustomerID, PromotionID)
);

-- Thêm dữ liệu mẫu
INSERT INTO Stores (StoreName, Address, PhoneNumber, CreatedAt, UpdatedAt)
VALUES
    ('Store A', '123 Main St, Anytown USA', '555-1234', GETDATE(), GETDATE()),
    ('Store B', '456 Oak Rd, Anytown USA', '555-5678', GETDATE(), GETDATE()),
    ('Store C', '789 Elm St, Anytown USA', '555-9012', GETDATE(), GETDATE());

INSERT INTO Employees (FullName, PhoneNumber, Email, Password, Role, Department, CreatedAt, UpdatedAt)
VALUES
	('Admin', '555-1111', 'admin@gmail.com','12345', 'Admin', 'Sales', GETDATE(), GETDATE()),
    ('Staff', '555-1111', 'staff@gmail.com','12345', 'Staff', 'Sales', GETDATE(), GETDATE()),
    ('Manager', '555-2222', 'manager@gmail.com','12345', 'Manager', 'Sales', GETDATE(), GETDATE()),
    ('Bob Johnson', '555-3333', 'bob.johnson@example.com','12345', 'Inventory Clerk', 'Operations', GETDATE(), GETDATE());

INSERT INTO StoreEmployees (StoreID, EmployeeID)
VALUES
    (1, 1), -- John Doe works at Store A
    (1, 2), -- Jane Smith works at Store A
    (2, 3); -- Bob Johnson works at Store B

INSERT INTO Products (ProductCode, ProductName, Description, Category, UnitPrice, CostPrice, Weight, IsJewelry, IsGold, CreatedAt, UpdatedAt)
VALUES
    ('DIA001', 'Diamond Solitaire Ring', 'Elegant 1-carat diamond solitaire ring', 'Rings', 3999.99, 2500.00, 5.0, 1, 1, GETDATE(), GETDATE()),
    ('DIA002', 'Diamond Stud Earrings', 'Classic 0.5-carat diamond stud earrings', 'Earrings', 999.99, 600.00, 2.0, 1, 1, GETDATE(), GETDATE()),
    ('DIA003', 'Diamond Pendant Necklace', 'Sparkling 0.75-carat diamond pendant necklace', 'Necklaces', 1499.99, 900.00, 3.5, 1, 1, GETDATE(), GETDATE()),
    ('DIA004', 'Diamond Tennis Bracelet', 'Stunning 2-carat diamond tennis bracelet', 'Bracelets', 4999.99, 3000.00, 10.0, 1, 1, GETDATE(), GETDATE()),
    ('DIA005', 'Diamond Hoop Earrings', 'Elegant 0.4-carat diamond hoop earrings', 'Earrings', 799.99, 450.00, 1.8, 1, 1, GETDATE(), GETDATE());