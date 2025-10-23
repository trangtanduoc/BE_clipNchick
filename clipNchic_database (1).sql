-- Create Database
CREATE DATABASE ClipNChic;
GO

USE ClipNChic;
GO

-- ========================
-- Table: User
-- ========================
CREATE TABLE [User] (
    id INT IDENTITY(1,1) PRIMARY KEY,
    email NVARCHAR(255),
    password NVARCHAR(255),
    phone NVARCHAR(50),
    birthday DATE,
    name NVARCHAR(255),
    address NVARCHAR(500),
    image NVARCHAR(255),
    createDate DATETIME2,
    status NVARCHAR(50)
);

-- ========================
-- Table: Collection
-- ========================
CREATE TABLE Collection (
    id INT IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(255),
    descript NVARCHAR(500)
);

-- ========================
-- Table: Model
-- ========================
CREATE TABLE Model (
    id INT IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(255),
    address NVARCHAR(255)
);

-- ========================
-- Table: Image
-- ========================
CREATE TABLE Image (
    id INT IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(255),
    address NVARCHAR(255)
);

-- ========================
-- Table: Base
-- ========================
CREATE TABLE Base (
    id INT IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(255),
    color NVARCHAR(100),
    price DECIMAL(10,2),
    imageId INT NULL FOREIGN KEY REFERENCES Image(id),
    modelId INT NULL FOREIGN KEY REFERENCES Model(id)
);

-- ========================
-- Table: Charm
-- ========================
CREATE TABLE Charm (
    id INT IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(255),
    price DECIMAL(10,2),
    imageId INT NULL FOREIGN KEY REFERENCES Image(id),
    modelId INT NULL FOREIGN KEY REFERENCES Model(id)
);

-- ========================
-- Table: Product
-- ========================
CREATE TABLE Product (
    id INT IDENTITY(1,1) PRIMARY KEY,
    collectId INT NULL FOREIGN KEY REFERENCES Collection(id),
    title NVARCHAR(255),
    descript NVARCHAR(500),
    baseId INT NULL FOREIGN KEY REFERENCES Base(id),
    price DECIMAL(10,2),
    userId INT NULL FOREIGN KEY REFERENCES [User](id),
    stock INT,
    modelId INT NULL FOREIGN KEY REFERENCES Model(id),
    createDate DATETIME2,
    status NVARCHAR(50)
);

-- ========================
-- Table: CharmProduct (Many-to-Many for Product & Charm)
-- ========================
CREATE TABLE CharmProduct (
    id INT IDENTITY(1,1) PRIMARY KEY,
    productId INT NULL FOREIGN KEY REFERENCES Product(id),
    charmId INT NULL FOREIGN KEY REFERENCES Charm(id)
);

-- ========================
-- Table: ProductPic
-- ========================
CREATE TABLE ProductPic (
    id INT IDENTITY(1,1) PRIMARY KEY,
    productId INT NULL FOREIGN KEY REFERENCES Product(id),
    imageId INT NULL FOREIGN KEY REFERENCES Image(id)
);

-- ========================
-- Table: BlindBox
-- ========================
CREATE TABLE BlindBox (
    id INT IDENTITY(1,1) PRIMARY KEY,
    collectId INT NULL FOREIGN KEY REFERENCES Collection(id),
    name NVARCHAR(255),
    descript NVARCHAR(500),
    price DECIMAL(10,2),
    stock INT,
    status NVARCHAR(50)
);

-- ========================
-- Table: BlindPic
-- ========================
CREATE TABLE BlindPic (
    id INT IDENTITY(1,1) PRIMARY KEY,
    blindId INT NULL FOREIGN KEY REFERENCES BlindBox(id),
    imageId INT NULL FOREIGN KEY REFERENCES Image(id)
);

-- ========================
-- Table: Ship
-- ========================
CREATE TABLE Ship (
    id INT IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(255),
    price DECIMAL(10,2)
);

-- ========================
-- Table: Voucher
-- ========================
CREATE TABLE Voucher (
    id INT IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(255),
    discount DECIMAL(10,2),
    stock INT,
    start DATETIME2,
    [end] DATETIME2
);

-- ========================
-- Table: Order
-- ======================id = table.Column<int>(type: "int", nullable: false)
    .Annotation("SqlServer:Identity", "1, 1"),  // ✅ Auto-increment enabled
CREATE TABLE [Order] (
    id INT IDENTITY(1,1) PRIMARY KEY,
    userId INT NULL FOREIGN KEY REFERENCES [User](id),
    phone NVARCHAR(50),
    address NVARCHAR(500),
    name NVARCHAR(255),
    createDate DATETIME2,
    totalPrice DECIMAL(10,2),
    shipPrice DECIMAL(10,2),
    payPrice DECIMAL(10,2),
    status NVARCHAR(50),
    payMethod NVARCHAR(100)
);

-- ========================
-- Table: OrderDetail
-- ========================
CREATE TABLE OrderDetail (
    id INT IDENTITY(1,1) PRIMARY KEY,
    orderId INT NULL FOREIGN KEY REFERENCES [Order](id),
    productId INT NULL FOREIGN KEY REFERENCES Product(id),
    quantity INT,
    price DECIMAL(10,2)
);
