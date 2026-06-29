use DelhiveryDB

-- views

CREATE OR ALTER VIEW vw_ShipmentDashboard
AS
SELECT
    ShipmentId,
    AWBNumber,
    SenderName,
    ReceiverName,
    Origin,
    Destination,
    Status,
    BookedAt,
    DeliveredAt
FROM Shipments;