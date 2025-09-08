CREATE DATABASE clipNchic;
GO

USE clipNchic;
GO

-- Roles
CREATE TABLE Roles (
    RoleId INT PRIMARY KEY IDENTITY(1,1),
    RoleName NVARCHAR(50) NOT NULL
);

-- Users
CREATE TABLE Users (
    UserId INT PRIMARY KEY IDENTITY(1,1),
    FullName NVARCHAR(100),
    Email NVARCHAR(100) UNIQUE NOT NULL,
    PasswordHash NVARCHAR(255) NOT NULL,
    RoleId INT FOREIGN KEY REFERENCES Roles(RoleId),
    CreatedAt DATETIME DEFAULT GETDATE()
);

-- ProductModels (File 3D)
CREATE TABLE ProductModels (
    Model3DId INT PRIMARY KEY IDENTITY(1,1),
    FileName NVARCHAR(255),
    FilePath NVARCHAR(500),
    FileType NVARCHAR(50) -- glb, gltf
);

-- Products
CREATE TABLE Products (
    ProductId INT PRIMARY KEY IDENTITY(1,1),
    ProductName NVARCHAR(100) NOT NULL,
    Description NVARCHAR(MAX), -- mô tả sản phẩm
    Price DECIMAL(18,2) NOT NULL,
    PreviewImage NVARCHAR(500), -- ảnh preview sản phẩm
    Model3DId INT FOREIGN KEY REFERENCES ProductModels(Model3DId),
    CreatedAt DATETIME DEFAULT GETDATE()
);

-- Materials (Màu sắc / Chất liệu)
CREATE TABLE Materials (
    MaterialId INT PRIMARY KEY IDENTITY(1,1),
    MaterialName NVARCHAR(100),
    Description NVARCHAR(MAX), -- mô tả chất liệu
    ColorCode NVARCHAR(20) -- mã HEX (#FFFFFF)
);

-- Textures (Hoạ tiết)
CREATE TABLE Textures (
    TextureId INT PRIMARY KEY IDENTITY(1,1),
    TextureName NVARCHAR(100),
    Description NVARCHAR(MAX), -- mô tả họa tiết
    TexturePath NVARCHAR(500) -- đường dẫn ảnh texture
);

-- Accessories (Phụ kiện đính kèm)
CREATE TABLE Accessories (
    AccessoryId INT PRIMARY KEY IDENTITY(1,1),
    AccessoryName NVARCHAR(100),
    Description NVARCHAR(MAX), -- mô tả phụ kiện
    AccessoryImage NVARCHAR(500), -- ảnh preview phụ kiện
    Price DECIMAL(18,2) DEFAULT 0
);

-- CustomDesigns (Thiết kế của khách)
CREATE TABLE CustomDesigns (
    DesignId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT FOREIGN KEY REFERENCES Users(UserId),
    BaseProductId INT FOREIGN KEY REFERENCES Products(ProductId),
    MaterialId INT FOREIGN KEY REFERENCES Materials(MaterialId),
    TextureId INT FOREIGN KEY REFERENCES Textures(TextureId),
    Model3DId INT FOREIGN KEY REFERENCES ProductModels(Model3DId), -- model riêng cho custom
    PreviewImage NVARCHAR(500), -- ảnh preview custom
    Description NVARCHAR(MAX), -- mô tả thiết kế
    IsPublic BIT DEFAULT 0, -- 1 = public, 0 = private
    CreatedAt DATETIME DEFAULT GETDATE()
);

-- CustomDesignAccessories (nhiều Accessories cho 1 thiết kế, có số lượng)
CREATE TABLE CustomDesignAccessories (
    DesignId INT FOREIGN KEY REFERENCES CustomDesigns(DesignId),
    AccessoryId INT FOREIGN KEY REFERENCES Accessories(AccessoryId),
    Quantity INT NOT NULL DEFAULT 1,
    PRIMARY KEY (DesignId, AccessoryId)
);

-- Orders
CREATE TABLE Orders (
    OrderId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT FOREIGN KEY REFERENCES Users(UserId),
    OrderDate DATETIME DEFAULT GETDATE(),
    TotalAmount DECIMAL(18,2) NOT NULL,
    Status NVARCHAR(50) DEFAULT 'Pending'
);

-- OrderDetails
CREATE TABLE OrderDetails (
    OrderDetailId INT PRIMARY KEY IDENTITY(1,1),
    OrderId INT FOREIGN KEY REFERENCES Orders(OrderId),
    ProductId INT NULL FOREIGN KEY REFERENCES Products(ProductId),
    DesignId INT NULL FOREIGN KEY REFERENCES CustomDesigns(DesignId),
    Quantity INT NOT NULL,
    Price DECIMAL(18,2) NOT NULL
);
