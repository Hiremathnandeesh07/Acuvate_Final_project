USE DelhiveryDB;
GO

truncate table shipments

INSERT INTO Shipments
(AWBNumber, SenderName, ReceiverName, Origin, Destination, WeightKg, Status, BookedAt, DeliveredAt)
VALUES
('HYD2026001', 'Raj', 'Amit', 'Hyderabad', 'Bangalore', 2.5, 'Booked', GETDATE(), NULL),
('CHE2026002', 'Sita', 'Ravi', 'Chennai', 'Mumbai', 5.2, 'In Transit', GETDATE(), NULL),
('DEL2026003', 'John', 'Mike', 'Delhi', 'Pune', 3.0, 'Out for Delivery', GETDATE(), NULL),
('KOL2026004', 'Anu', 'Priya', 'Kolkata', 'Hyderabad', 4.7, 'Delivered', GETDATE(), GETDATE()),
('PUN2026005', 'Kiran', 'Deepak', 'Pune', 'Delhi', 6.1, 'RTO', GETDATE(), NULL),

('MUM2026006', 'Arjun', 'Rahul', 'Mumbai', 'Chennai', 1.5, 'Booked', GETDATE(), NULL),
('BAN2026007', 'Sneha', 'Pooja', 'Bangalore', 'Kolkata', 2.2, 'In Transit', GETDATE(), NULL),
('DEL2026008', 'David', 'Chris', 'Delhi', 'Hyderabad', 7.8, 'Out for Delivery', GETDATE(), NULL),
('CHE2026009', 'Neha', 'Ramesh', 'Chennai', 'Pune', 3.6, 'Delivered', GETDATE(), GETDATE()),
('HYD2026010', 'Asha', 'Vikram', 'Hyderabad', 'Delhi', 2.9, 'RTO', GETDATE(), NULL),

('MUM2026011', 'Manoj', 'Sunil', 'Mumbai', 'Bangalore', 4.1, 'Booked', GETDATE(), NULL),
('KOL2026012', 'Kavya', 'Divya', 'Kolkata', 'Chennai', 5.0, 'In Transit', GETDATE(), NULL),
('DEL2026013', 'Rohit', 'Virat', 'Delhi', 'Mumbai', 6.3, 'Out for Delivery', GETDATE(), NULL),
('PUN2026014', 'Pooja', 'Nisha', 'Pune', 'Kolkata', 3.2, 'Delivered', GETDATE(), GETDATE()),
('CHE2026015', 'Amit', 'Raj', 'Chennai', 'Hyderabad', 2.0, 'RTO', GETDATE(), NULL),

('BAN2026016', 'Deepa', 'Suresh', 'Bangalore', 'Delhi', 1.8, 'Booked', GETDATE(), NULL),
('HYD2026017', 'Rahul', 'Kiran', 'Hyderabad', 'Mumbai', 4.5, 'In Transit', GETDATE(), NULL),
('DEL2026018', 'Meena', 'Lakshman', 'Delhi', 'Chennai', 3.9, 'Out for Delivery', GETDATE(), NULL),
('PUN2026019', 'Sanjay', 'Anil', 'Pune', 'Bangalore', 2.7, 'Delivered', GETDATE(), GETDATE()),
('KOL2026020', 'Geeta', 'Rekha', 'Kolkata', 'Pune', 5.6, 'RTO', GETDATE(), NULL);



select * from shipments

