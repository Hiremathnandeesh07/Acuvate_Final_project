use DelhiveryDB

-- creating indexes

IF NOT EXISTS
(
    SELECT *
    FROM sys.indexes
    WHERE name = 'IX_Shipments_Status'
)
BEGIN
    CREATE NONCLUSTERED INDEX IX_Shipments_Status
    ON Shipments(Status);
END
GO