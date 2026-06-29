# Copilot Usage Log

## Entry 1

### What I was working on

Building the ADO.NET `ShipmentRepository` methods for database operations.

### What Copilot suggested

Suggested using `SqlConnection` inside a `using` block along with parameterized `SqlCommand` objects for all CRUD operations.

### Decision

**ACCEPTED**

### Reason

The suggestion followed ADO.NET best practices, prevented SQL injection, and ensured database connections were disposed properly.

---

## Entry 2

### What I was working on

Creating the ASP.NET Core Web API endpoints for shipment management.

### What Copilot suggested

Suggested returning the newly created shipment from the `Book()` repository method so that the controller could directly return `201 Created` without making another database call.

### Decision

**MODIFIED**

### Reason

Initially, Copilot suggested calling `GetByAWB()` after inserting the shipment. I modified the implementation by changing the repository itself to return the inserted shipment. This removed the need for an additional database query and kept the controller cleaner.

---

## Entry 3

### What I was working on

Creating the SQL Server database objects.

### What Copilot suggested

Suggested creating a view named `vw_ShipmentDashboard`.

### Decision

**REJECTED**

### Reason

The project requirements specifically required the view to be named `ShipmentDashboard`. I rejected the suggested name and created the view using the required name so that it matched the assessment specification exactly.

---

## Entry 4

### What I was working on

Building the Shipment Tracking Dashboard using HTML, CSS and jQuery.

### What Copilot suggested

Suggested using jQuery `$.ajax()` to call the REST API and dynamically populate the shipments table and statistics section.

### Decision

**ACCEPTED**

### Reason

The assessment explicitly required using jQuery AJAX instead of the Fetch API. The suggested implementation satisfied the requirement and integrated well with the existing Web API.

---

## Entry 5

### What I was working on

Developing the Python end-of-day analytics report.

### What Copilot suggested

Suggested placing the `export_csv()` function inside the `main()` function.

### Decision

**REJECTED**

### Reason

The implementation produced an `UnboundLocalError` because the function was called before it was defined. After debugging, I moved `export_csv()` outside `main()` as a separate function, which resolved the issue and followed better Python coding practices.
