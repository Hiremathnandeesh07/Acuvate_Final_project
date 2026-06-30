# Delhivery Shipment Intelligence Platform (DSIP)

## Overview

The **Delhivery Shipment Intelligence Platform (DSIP)** is a shipment management system developed as part of the final project assessment. It demonstrates a complete end-to-end implementation using SQL Server, ADO.NET, ASP.NET Core Web API, HTML/CSS/JavaScript, and Python.

The application allows users to:

- Book new shipments
- Track shipments using AWB Number
- Update shipment status
- Cancel shipments
- View shipment statistics
- Generate an End-of-Day analytics report using Python

---

# Technology Stack

- C#
- .NET 8
- ASP.NET Core Web API
- ADO.NET
- SQL Server
- HTML
- CSS
- JavaScript
- jQuery AJAX
- Python
- Swagger / OpenAPI

---

# Project Structure

```
FinalProject
│
├── Database
│   ├── Database.sql
│   ├── Tables.sql
│   ├── StoredProcedures.sql
│   ├── Views.sql
│   ├── Index.sql
│   └── SeedData.sql
│
├── Delhivery.Console
│
├── Delhivery.Dataa
│
├── Delhivery.API
│
├── UI
│
├── Python
│   └── report.py
│
├── GenAI
│
├── chatgpt_log.md
├── PROMPT_LOG.md
├── REFLECTION.md
└── README.md
```

---

# Database Setup

## Database Name

```
DelhiveryDB
```

---

## SQL Setup Instructions

Open **SQL Server Management Studio (SSMS)** and execute the SQL scripts in the following order.

### Step 1

Execute

```
Database.sql
```

This creates the database.

---

### Step 2

Select the newly created database

```
DelhiveryDB
```

or ensure every script begins with

```sql
USE DelhiveryDB;
GO
```

---

### Step 3

Execute

```
Tables.sql
```

This creates the **Shipments** table.

---

### Step 4

Execute

```
StoredProcedures.sql
```

This creates

- usp_GetAllShipments
- usp_GetShipmentByAWB
- usp_UpdateShipmentStatus
- usp_AddShipment
- usp_CancelShipment

---

### Step 5

Execute

```
Views.sql
```

This creates

```
ShipmentDashboard
```

---

### Step 6

Execute

```
Index.sql
```

This creates the non-clustered index on the **Status** column.

---

### Step 7

Execute

```
SeedData.sql
```

This inserts sample shipment records.

---

# Connection String

Update the connection string inside the data layer.

Example:

```text
Server=<SERVER_NAME>;
Database=DelhiveryDB;
Trusted_Connection=True;
TrustServerCertificate=True;
```

If SQL Authentication is used:

```text
Server=<SERVER_NAME>;
Database=DelhiveryDB;
User Id=<USERNAME>;
Password=<PASSWORD>;
TrustServerCertificate=True;
```

Replace the placeholder values with your own SQL Server details.

---

# Running the Console Application

1. Open the solution in Visual Studio.
2. Restore NuGet packages if prompted.
3. Build the solution.
4. Set **Delhivery.Console** as the Startup Project.
5. Press **Ctrl + F5** or **F5**.

The console application provides options to:

- View all shipments
- Search shipment by AWB
- Book shipment
- Update shipment status
- Cancel shipment

---

# Running the Web API

1. Build the solution.
2. Set **Delhivery.API** as the Startup Project.
3. Run the project.

The API listens on

```
http://localhost:5234
```

Swagger is available at

```
http://localhost:5234/swagger
```

Available endpoints:

| Method | Endpoint                    |
| ------ | --------------------------- |
| GET    | /api/shipments              |
| GET    | /api/shipments/{awb}        |
| POST   | /api/shipments              |
| PUT    | /api/shipments/{awb}/status |
| DELETE | /api/shipments/{id}         |
| GET    | /api/shipments/stats        |

---

# Running the UI

1. Ensure the Web API is running.
2. Open the **UI** folder.
3. Open **index.html** in a browser.

The UI communicates with the Web API using jQuery AJAX.

Available features:

- Shipment Statistics Dashboard
- Book Shipment
- Search by AWB
- Update Shipment Status
- Delete Shipment
- Client-side Status Filter

---

# Running the Python Analytics Report

Open a terminal inside the **Python** folder.

Run:

```bash
python report.py
```

This prints the End-of-Day Shipment Report.

To export the report as CSV:

```bash
python report.py --export
```

A CSV file named

```
delhivery_report_YYYYMMDD.csv
```

will be created in the Python folder.

---

# Features Implemented

## Console Application

- Book Shipment
- View All Shipments
- Search by AWB
- Update Shipment Status
- Cancel Shipment

---

## SQL Server

- Database
- Shipments Table
- Stored Procedures
- ShipmentDashboard View
- Non-clustered Index
- Seed Data

---

## ADO.NET Data Layer

- Repository Pattern
- Parameterized Queries
- SQL Connection Management using `using`
- Custom Exception Handling

---

## Web API

- RESTful Endpoints
- Dependency Injection
- Swagger Support
- CORS Enabled
- Request Validation

---

## User Interface

- Single Page Dashboard
- AJAX Communication
- Shipment Statistics
- Book Shipment
- Track Shipment
- Update Status
- Delete Shipment
- Status Filter

---

## Python Analytics

- Retrieves shipment data using the REST API
- Calculates shipment statistics
- Calculates average shipment weight
- Finds the heaviest shipment
- Generates End-of-Day report
- Supports CSV export using the `--export` CLI flag

---

# Known Issues

- The UI requires the Web API to be running before it can load shipment data.
- Swagger displays placeholder values such as `"string"` for text inputs. These should be replaced with valid values before submitting requests.
- The application is intended for local development and uses `localhost` URLs by default.
- Authentication and user authorization are not implemented because they were outside the scope of this assessment.

---

# Author

**Nandeesh Hiremath**
Final Project – Delhivery Shipment Intelligence Platform
