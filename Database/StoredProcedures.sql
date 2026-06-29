-- stored Procedures

USE DelhiveryDB;
GO


-- Get All Shipments
CREATE OR ALTER PROCEDURE usp_GetAllShipments
AS
BEGIN
    SELECT *
    FROM vw_ShipmentDashboard
    
    ORDER BY BookedAt DESC;
END;
GO

-- Get Shipment By AWB

CREATE OR ALTER PROCEDURE usp_GetShipmentByAWB
(
    @AWBNumber NVARCHAR(20)
)
AS
BEGIN
    SELECT *
    FROM Shipments
    WHERE AWBNumber = @AWBNumber
END;
GO


-- Update Shipment Status

CREATE OR ALTER PROCEDURE usp_UpdateShipmentStatus
(
    @AWBNumber NVARCHAR(20),
    @NewStatus NVARCHAR(30)
)
AS
BEGIN
    BEGIN TRY

        UPDATE Shipments
        SET
            Status = @NewStatus,
            DeliveredAt =
                CASE
                    WHEN @NewStatus = 'Delivered'
                    THEN GETDATE()
                    ELSE DeliveredAt
                END
        WHERE AWBNumber = @AWBNumber

    END TRY

    BEGIN CATCH

        THROW;

    END CATCH
END;
GO

-- adding shipment

CREATE OR ALTER PROCEDURE usp_AddShipment
(
    @AWBNumber NVARCHAR(20),
    @SenderName NVARCHAR(100),
    @ReceiverName NVARCHAR(100),
    @Origin NVARCHAR(100),
    @Destination NVARCHAR(100),
    @WeightKg DECIMAL(10,2),
    @Status NVARCHAR(30)
)
AS
BEGIN
    BEGIN TRY

        INSERT INTO Shipments
        (
            AWBNumber,
            SenderName,
            ReceiverName,
            Origin,
            Destination,
            WeightKg,
            Status
        )
        VALUES
        (
            @AWBNumber,
            @SenderName,
            @ReceiverName,
            @Origin,
            @Destination,
            @WeightKg,
            @Status
        );

    END TRY

    BEGIN CATCH

        THROW;

    END CATCH
END;
GO


-- cancel shipment

CREATE OR ALTER PROCEDURE usp_CancelShipment
(
    @ShipmentId INT
)
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        
        UPDATE Shipments
        SET IsDeleted = 1
        WHERE ShipmentId = @ShipmentId


        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;

        THROW;
    END CATCH
END;
GO