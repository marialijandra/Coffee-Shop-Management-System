DROP DATABASE IF EXISTS coffee_shop_db;
CREATE DATABASE coffee_shop_db
    CHARACTER SET utf8mb4
    COLLATE utf8mb4_general_ci;
USE coffee_shop_db;

CREATE TABLE Drinks (
    DrinkID INT AUTO_INCREMENT PRIMARY KEY,
    DrinkName VARCHAR(100) NOT NULL,
    Category ENUM('Coffee','Tea') NOT NULL,          
    Tag VARCHAR(40),                                 
    Description TEXT,
    BasePrice DECIMAL(10,2) NOT NULL,
    ImageUrl VARCHAR(160),                           
    Badge VARCHAR(40),                               
    IsSoldOut BOOLEAN DEFAULT FALSE,
    DisplayOrder INT DEFAULT 0                       
);

CREATE TABLE Orders (
    OrderID INT AUTO_INCREMENT UNIQUE PRIMARY KEY,
    OrderNumber VARCHAR(30) NOT NULL UNIQUE,
    RefCode VARCHAR(8),                              
    CustomerName VARCHAR(100) NOT NULL,
    PaymentMethod ENUM('Cash','Card') NULL,
    OrderType ENUM('Dine-in','Take-out') NOT NULL,
    Subtotal DECIMAL(10,2) NOT NULL DEFAULT 0,       
    DiscountApplied DECIMAL(10,2) DEFAULT 0,
    DiscountQuantity INT DEFAULT 0,                  
    TotalPrice DECIMAL(10,2) NOT NULL,
    AmountPaid DECIMAL(10,2) NULL,
    IsPaid BOOLEAN DEFAULT FALSE,                    
    OrderDateTime DATETIME DEFAULT CURRENT_TIMESTAMP,
    OrderStatus ENUM(
        'New Order',
        'In Progress',
        'Served',
        'Cancelled'
    ) DEFAULT 'New Order',
    KEY idx_orders_status (OrderStatus),
    KEY idx_orders_date (OrderDateTime)
) AUTO_INCREMENT = 926;

CREATE TABLE OrderDetails (
    OrderDetailID INT AUTO_INCREMENT PRIMARY KEY,

    OrderID INT NOT NULL,
    DrinkID INT NOT NULL,

    DrinkName VARCHAR(100) NOT NULL,
    Description TEXT,

    Temperature ENUM('Hot','Iced') NOT NULL,
    Size ENUM('S','L') NOT NULL,

    Quantity INT NOT NULL,

    PricePerDrink DECIMAL(10,2) NOT NULL,
    ImageUrl VARCHAR(160),                           -- b2

    FOREIGN KEY (OrderID)
        REFERENCES Orders(OrderID)
        ON DELETE CASCADE,

    FOREIGN KEY (DrinkID)
        REFERENCES Drinks(DrinkID)
);

INSERT INTO Drinks
    (DrinkID, DrinkName, Category, Tag, Description, BasePrice, ImageUrl, Badge, DisplayOrder)
VALUES
    (1, 'Cinnamon Latte', 'Coffee', 'Espresso',
     'Espresso, steamed milk, and a warm dusting of cinnamon.',
     150.00, 'Images/CinnamonLatte.png', 'Best Seller', 1),

    (2, 'Cappuccino', 'Coffee', 'Espresso',
     'Bold espresso topped with thick, velvety steamed foam.',
     140.00, 'Images/Cappuccino.png', 'Classic', 2),

    (3, 'Salted Caramel Frappe', 'Coffee', 'Cold Brew',
     'Blended cold brew, caramel, and a touch of sea salt cream.',
     180.00, 'Images/SaltedCaramelFrappe.png', 'Seasonal', 3),

    (4, 'Mocha Cream Latte', 'Coffee', 'Tea & Cream',
     'Rich espresso and chocolate, finished with a cloud of cream.',
     165.00, 'Images/MochaCreamLatte.png', 'Popular', 4),

    (5, 'Matcha Cortado', 'Tea', 'Tea & Cream',
     'Ceremonial-grade matcha layered with a touch of milk.',
     160.00, 'Images/MatchaCortado.png', 'New', 5),

    (6, 'Peach Iced Tea', 'Tea', 'Seasonal',
     'Black tea steeped with real peach, served over ice.',
     140.00, 'Images/PeachIcedTea.png', 'Seasonal', 6);
