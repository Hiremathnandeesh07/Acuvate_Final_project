IF DB_ID('DelhiveryDB') IS NULL
BEGIN
    CREATE DATABASE DelhiveryDB;
END
GO

USE DelhiveryDB;
GO

-- creating table

USE DelhiveryDB;
GO

IF OBJECT_ID('Shipments', 'U') IS NULL
BEGIN
CREATE TABLE Shipments
(
    ShipmentId INT IDENTITY(1,1) PRIMARY KEY,

    AWBNumber VARCHAR(20) NOT NULL UNIQUE,

    SenderName VARCHAR(100) NOT NULL,

    ReceiverName VARCHAR(100) NOT NULL,

    Origin VARCHAR(100) NOT NULL,

    Destination VARCHAR(100) NOT NULL,

    WeightKg DECIMAL(10,2) NOT NULL
        CHECK (WeightKg > 0),

    Status VARCHAR(30) NOT NULL
        CHECK (Status IN ('Booked',
                          'In Transit',
                          'Out for Delivery',
                          'Delivered',
                          'RTO')),

    BookedAt DATETIME NOT NULL
        DEFAULT GETDATE(),
    DeliveredAt DATETIME NULL
);
END




ALTER TABLE Shipments
ADD IsDeleted BIT NOT NULL DEFAULT 0;








