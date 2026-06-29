# Prompt Engineering Log

## Task

Write a SQL query to find all shipments that have been **In Transit** for more than **3 days**.

---

## Iteration 1

### Prompt

Write an SQL query to find all shipments that have been In Transit for more than 3 days.

### AI Output

```sql
SELECT *
FROM Shipments
WHERE Status = 'In Transit'
AND DATEDIFF(DAY, BookedAt, GETDATE()) > 3;
```

---

## Iteration 2

### Prompt

Write a Microsoft SQL Server query for the Delhivery Shipments table to find all shipments whose Status is 'In Transit' for more than 3 days. Return ShipmentId, AWBNumber, SenderName, ReceiverName, Origin, Destination, WeightKg, Status, and BookedAt. Use GETDATE() and DATEDIFF().

### AI Output

```sql
SELECT
    ShipmentId,
    AWBNumber,
    SenderName,
    ReceiverName,
    Origin,
    Destination,
    WeightKg,
    Status,
    BookedAt
FROM Shipments
WHERE Status = 'In Transit'
AND DATEDIFF(DAY, BookedAt, GETDATE()) > 3
ORDER BY BookedAt;
```

---

## Reflection

The first prompt was short and only asked for the basic query, so the response returned all columns using `SELECT *`. The second prompt provided additional context by specifying SQL Server, the table name, the required columns, and the functions to use. As a result, the generated query was more precise, easier to read, and better suited for production because it avoids using `SELECT *` and returns only the required information.
