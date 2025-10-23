-- ClipNchic Database Creation Script
-- SQL Server / Azure SQL Database
-- Created for deployment

-- Create Database
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'ClipNChic')
BEGIN
    CREATE DATABASE ClipNChic;
END
GO

USE ClipNChic;
GO

-- Drop existing tables if they exist (for fresh deployment)
IF OBJECT_ID('dbo.OrderDetail', 'U') IS NOT NULL DROP TABLE dbo.OrderDetail;
IF OBJECT_ID('dbo.CharmProduct', 'U') IS NOT NULL DROP TABLE dbo.CharmProduct;
IF OBJECT_ID('dbo.[Order]', 'U') IS NOT NULL DROP TABLE dbo.[Order];
IF OBJECT_ID('dbo.Image', 'U') IS NOT NULL DROP TABLE dbo.Image;
IF OBJECT_ID('dbo.Product', 'U') IS NOT NULL DROP TABLE dbo.Product;
IF OBJECT_ID('dbo.Charm', 'U') IS NOT NULL DROP TABLE dbo.Charm;
IF OBJECT_ID('dbo.Base', 'U') IS NOT NULL DROP TABLE dbo.Base;
IF OBJECT_ID('dbo.Model', 'U') IS NOT NULL DROP TABLE dbo.Model;
IF OBJECT_ID('dbo.BlindBox', 'U') IS NOT NULL DROP TABLE dbo.BlindBox;
IF OBJECT_ID('dbo.Voucher', 'U') IS NOT NULL DROP TABLE dbo.Voucher;
IF OBJECT_ID('dbo.Ship', 'U') IS NOT NULL DROP TABLE dbo.Ship;
IF OBJECT_ID('dbo.Collection', 'U') IS NOT NULL DROP TABLE dbo.Collection;
IF OBJECT_ID('dbo.EmailVerificationToken', 'U') IS NOT NULL DROP TABLE dbo.EmailVerificationToken;
IF OBJECT_ID('dbo.[User]', 'U') IS NOT NULL DROP TABLE dbo.[User];
GO

-- Create [User] table
CREATE TABLE dbo.[User] (
    id INT PRIMARY KEY IDENTITY(1,1),
    email NVARCHAR(255) NULL,
    password NVARCHAR(255) NULL,
    phone NVARCHAR(50) NULL,
    birthday DATETIME2 NULL,
    name NVARCHAR(255) NULL,
    address NVARCHAR(500) NULL,
    image NVARCHAR(255) NULL,
    createDate DATETIME2 NULL,
    status NVARCHAR(50) NULL,
    isEmailVerified BIT NOT NULL DEFAULT 0
);

-- Create EmailVerificationToken table
CREATE TABLE dbo.EmailVerificationToken (
    id INT PRIMARY KEY IDENTITY(1,1),
    userId INT NOT NULL,
    token NVARCHAR(255) NOT NULL,
    expiryDate DATETIME2 NOT NULL,
    isUsed BIT NOT NULL DEFAULT 0,
    createdDate DATETIME2 NOT NULL,
    CONSTRAINT FK_EmailVerificationToken_User FOREIGN KEY (userId) REFERENCES dbo.[User](id)
);

-- Create Collection table
CREATE TABLE dbo.Collection (
    id INT PRIMARY KEY IDENTITY(1,1),
    name NVARCHAR(255) NULL,
    descript NVARCHAR(500) NULL
);

-- Create Model table
CREATE TABLE dbo.Model (
    id INT PRIMARY KEY IDENTITY(1,1),
    name NVARCHAR(255) NULL,
    address NVARCHAR(255) NULL
);

-- Create Base table
CREATE TABLE dbo.Base (
    id INT PRIMARY KEY IDENTITY(1,1),
    name NVARCHAR(255) NULL,
    color NVARCHAR(100) NULL,
    price DECIMAL(10,2) NULL,
    modelId INT NULL,
    CONSTRAINT FK_Base_Model FOREIGN KEY (modelId) REFERENCES dbo.Model(id)
);

-- Create Charm table
CREATE TABLE dbo.Charm (
    id INT PRIMARY KEY IDENTITY(1,1),
    name NVARCHAR(255) NULL,
    price DECIMAL(10,2) NULL,
    modelId INT NULL,
    CONSTRAINT FK_Charm_Model FOREIGN KEY (modelId) REFERENCES dbo.Model(id)
);

-- Create BlindBox table
CREATE TABLE dbo.BlindBox (
    id INT PRIMARY KEY IDENTITY(1,1),
    collectId INT NULL,
    name NVARCHAR(255) NULL,
    descript NVARCHAR(500) NULL,
    price DECIMAL(10,2) NULL,
    stock INT NULL,
    status NVARCHAR(50) NULL,
    CONSTRAINT FK_BlindBox_Collection FOREIGN KEY (collectId) REFERENCES dbo.Collection(id)
);

-- Create Product table
CREATE TABLE dbo.Product (
    id INT PRIMARY KEY IDENTITY(1,1),
    collectId INT NULL,
    title NVARCHAR(255) NULL,
    descript NVARCHAR(500) NULL,
    baseId INT NULL,
    price DECIMAL(10,2) NULL,
    userId INT NULL,
    stock INT NULL,
    modelId INT NULL,
    createDate DATETIME2 NULL,
    status NVARCHAR(50) NULL,
    CONSTRAINT FK_Product_Collection FOREIGN KEY (collectId) REFERENCES dbo.Collection(id),
    CONSTRAINT FK_Product_Base FOREIGN KEY (baseId) REFERENCES dbo.Base(id),
    CONSTRAINT FK_Product_User FOREIGN KEY (userId) REFERENCES dbo.[User](id),
    CONSTRAINT FK_Product_Model FOREIGN KEY (modelId) REFERENCES dbo.Model(id)
);

-- Create Image table
CREATE TABLE dbo.Image (
    id INT PRIMARY KEY IDENTITY(1,1),
    name NVARCHAR(255) NULL,
    address NVARCHAR(255) NULL,
    baseId INT NULL,
    charmId INT NULL,
    productId INT NULL,
    blindBoxId INT NULL,
    CONSTRAINT FK_Image_Base FOREIGN KEY (baseId) REFERENCES dbo.Base(id),
    CONSTRAINT FK_Image_Charm FOREIGN KEY (charmId) REFERENCES dbo.Charm(id),
    CONSTRAINT FK_Image_Product FOREIGN KEY (productId) REFERENCES dbo.Product(id),
    CONSTRAINT FK_Image_BlindBox FOREIGN KEY (blindBoxId) REFERENCES dbo.BlindBox(id)
);

-- Create CharmProduct table
CREATE TABLE dbo.CharmProduct (
    id INT PRIMARY KEY IDENTITY(1,1),
    productId INT NULL,
    charmId INT NULL,
    CONSTRAINT FK_CharmProduct_Product FOREIGN KEY (productId) REFERENCES dbo.Product(id),
    CONSTRAINT FK_CharmProduct_Charm FOREIGN KEY (charmId) REFERENCES dbo.Charm(id)
);

-- Create [Order] table
CREATE TABLE dbo.[Order] (
    id INT PRIMARY KEY IDENTITY(1,1),
    userId INT NULL,
    phone NVARCHAR(50) NULL,
    address NVARCHAR(500) NULL,
    name NVARCHAR(255) NULL,
    createDate DATETIME2 NULL,
    totalPrice DECIMAL(10,2) NULL,
    shipPrice DECIMAL(10,2) NULL,
    payPrice DECIMAL(10,2) NULL,
    status NVARCHAR(50) NULL,
    payMethod NVARCHAR(100) NULL,
    CONSTRAINT FK_Order_User FOREIGN KEY (userId) REFERENCES dbo.[User](id)
);

-- Create OrderDetail table
CREATE TABLE dbo.OrderDetail (
    id INT PRIMARY KEY IDENTITY(1,1),
    orderId INT NULL,
    productId INT NULL,
    blindBoxId INT NULL,
    quantity INT NULL,
    price DECIMAL(10,2) NULL,
    CONSTRAINT FK_OrderDetail_Order FOREIGN KEY (orderId) REFERENCES dbo.[Order](id),
    CONSTRAINT FK_OrderDetail_Product FOREIGN KEY (productId) REFERENCES dbo.Product(id),
    CONSTRAINT FK_OrderDetail_BlindBox FOREIGN KEY (blindBoxId) REFERENCES dbo.BlindBox(id)
);

-- Create Ship table
CREATE TABLE dbo.Ship (
    id INT PRIMARY KEY IDENTITY(1,1),
    name NVARCHAR(255) NULL,
    price DECIMAL(10,2) NULL
);

-- Create Voucher table
CREATE TABLE dbo.Voucher (
    id INT PRIMARY KEY IDENTITY(1,1),
    name NVARCHAR(255) NULL,
    discount DECIMAL(10,2) NULL,
    stock INT NULL,
    [start] DATETIME2 NULL,
    [end] DATETIME2 NULL
);

-- Create Indexes for better query performance
CREATE INDEX IX_User_Email ON dbo.[User](email);
CREATE INDEX IX_User_Phone ON dbo.[User](phone);
CREATE INDEX IX_Product_UserId ON dbo.Product(userId);
CREATE INDEX IX_Product_BaseId ON dbo.Product(baseId);
CREATE INDEX IX_Product_ModelId ON dbo.Product(modelId);
CREATE INDEX IX_Product_CollectId ON dbo.Product(collectId);
CREATE INDEX IX_Order_UserId ON dbo.[Order](userId);
CREATE INDEX IX_Order_CreateDate ON dbo.[Order](createDate);
CREATE INDEX IX_Order_Status ON dbo.[Order]([status]);
CREATE INDEX IX_OrderDetail_OrderId ON dbo.OrderDetail(orderId);
CREATE INDEX IX_OrderDetail_ProductId ON dbo.OrderDetail(productId);
CREATE INDEX IX_OrderDetail_BlindBoxId ON dbo.OrderDetail(blindBoxId);
CREATE INDEX IX_Image_ProductId ON dbo.Image(productId);
CREATE INDEX IX_Image_BlindBoxId ON dbo.Image(blindBoxId);
CREATE INDEX IX_Image_BaseId ON dbo.Image(baseId);
CREATE INDEX IX_Image_CharmId ON dbo.Image(charmId);
CREATE INDEX IX_CharmProduct_ProductId ON dbo.CharmProduct(productId);
CREATE INDEX IX_CharmProduct_CharmId ON dbo.CharmProduct(charmId);
CREATE INDEX IX_EmailVerificationToken_UserId ON dbo.EmailVerificationToken(userId);

PRINT 'Database ClipNChic created successfully!';
GO
