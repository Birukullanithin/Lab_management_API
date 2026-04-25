# LabManagementAPI

`LabManagementAPI` is an ASP.NET Core Web API for a laboratory management system. The current codebase focuses on patient management and related lab order details, and it is structured so more modules can be added over time.

This README is written for the project in its current state and can continue to grow as new features are added in the future.

## Current Scope

At the moment, the API includes:

- Patient CRUD operations
- Patient order lookup
- Test lookup linked to patient orders
- Swagger/OpenAPI support for local testing
- CORS configuration for an Angular frontend running on `http://localhost:4200`
- PostgreSQL connectivity using `Npgsql`
- JWT authentication for patient API access

## Tech Stack

- .NET 8
- ASP.NET Core Web API
- PostgreSQL
- Npgsql
- Swagger / Swashbuckle
- ADO.NET-style data access with a custom database context helper

## Project Structure

```text
LabManagementAPI/
|-- LabManagement.slnx
|-- README.md
`-- LabManagement/
    |-- Controllers/
    |-- Data/
    |-- Dtos/
    |-- Interfaces/
    |-- Models/
    |-- Repositories/
    |-- Services/
    |-- Program.cs
    |-- appsettings.json
    |-- appsettings.Development.json
    `-- LabManagement.csproj
```

## Architecture Overview

The API currently follows a layered structure:

- `Controllers` handle HTTP requests and responses
- `Services` contain application logic
- `Repositories` contain SQL queries and database operations
- `Data` contains database connection and query helper classes
- `Models` represent database/domain entities
- `Dtos` represent request and response payloads

## Current API Endpoints

Base route:

```text
/api/patients
```

Available endpoints:

- `GET /api/patients` - Get all patients with related order and ordered test names
- `GET /api/patients/{patientId}` - Get one patient by ID
- `POST /api/patients` - Create a patient, create a related order, and attach tests by test name
- `PUT /api/patients/{patientId}` - Update a patient
- `DELETE /api/patients/{patientId}` - Delete a patient
- `POST /api/patients/auth/login` - Validate username/password and return a JWT token
- `POST /api/patients/tree` - Save nested tree JSON and return it with generated node IDs
- `GET /api/patients/tree/{treeId}` - Return a saved tree in the same nested JSON structure

## Example Request Payload

Example `POST /api/patients` body:

```json
{
  "organizationId": 1,
  "legalEntityId": 1,
  "patientName": "John Doe",
  "age": 30,
  "gender": "Male",
  "dateOfBirth": "1994-05-10T00:00:00",
  "phone": "9876543210",
  "email": "john@example.com",
  "address": "Chennai",
  "createdAt": "2026-04-17T10:00:00",
  "updatedAt": null,
  "testNames": ["Blood Test", "Thyroid Test"]
}
```

## Database Notes

The current repository code uses PostgreSQL tables under the `lab` schema:

- `lab.patient`
- `lab.orders`
- `lab.order_tests`
- `lab.test_master`
- `lab.patient_users`
- `lab.patient_trees`
- `lab.patient_tree_nodes`

The API expects test names passed in `testNames` to already exist in `lab.test_master`.

### Authentication Table Structure

Use this table for the login endpoint:

```sql
CREATE SCHEMA IF NOT EXISTS lab;

CREATE TABLE IF NOT EXISTS lab.patient_users (
    user_id SERIAL PRIMARY KEY,
    username VARCHAR(100) NOT NULL UNIQUE,
    password VARCHAR(255) NOT NULL,
    full_name VARCHAR(150),
    role_name VARCHAR(50) NOT NULL DEFAULT 'PatientUser',
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);
```

Sample user for testing:

```sql
INSERT INTO lab.patient_users (username, password, full_name, role_name)
VALUES ('patientadmin', 'Admin@123', 'Patient Admin', 'PatientAdmin');
```

For quick local testing, the sample stores the password as plain text. In production, store only a password hash.

### Tree Storage Structure

The correct way to store tree data is a normalized parent-child structure, not a single JSON text column. Use one table for the tree and one table for the nodes.

```sql
CREATE TABLE IF NOT EXISTS lab.patient_trees (
    tree_id SERIAL PRIMARY KEY,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE IF NOT EXISTS lab.patient_tree_nodes (
    node_id SERIAL PRIMARY KEY,
    tree_id INT NOT NULL REFERENCES lab.patient_trees(tree_id) ON DELETE CASCADE,
    parent_node_id INT NULL REFERENCES lab.patient_tree_nodes(node_id) ON DELETE CASCADE,
    node_value INT NOT NULL,
    display_order INT NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE INDEX IF NOT EXISTS ix_patient_tree_nodes_tree_id
    ON lab.patient_tree_nodes(tree_id);

CREATE INDEX IF NOT EXISTS ix_patient_tree_nodes_parent_node_id
    ON lab.patient_tree_nodes(parent_node_id);
```

Input JSON:

```json
{
  "value": 2,
  "children": [
    {
      "value": 7,
      "children": [
        { "value": 2, "children": [] },
        { "value": 10, "children": [] },
        {
          "value": 6,
          "children": [
            { "value": 5, "children": [] },
            { "value": 11, "children": [] }
          ]
        }
      ]
    },
    {
      "value": 5,
      "children": [
        {
          "value": 9,
          "children": [
            { "value": 4, "children": [] }
          ]
        }
      ]
    }
  ]
}
```

Returned JSON:

```json
{
  "id": 1,
  "value": 2,
  "children": [
    {
      "id": 2,
      "value": 7,
      "children": [
        { "id": 4, "value": 2, "children": [] },
        { "id": 5, "value": 10, "children": [] },
        {
          "id": 6,
          "value": 6,
          "children": [
            { "id": 7, "value": 5, "children": [] },
            { "id": 8, "value": 11, "children": [] }
          ]
        }
      ]
    },
    {
      "id": 3,
      "value": 5,
      "children": [
        {
          "id": 9,
          "value": 9,
          "children": [
            { "id": 10, "value": 4, "children": [] }
          ]
        }
      ]
    }
  ]
}
```

## Configuration

The development connection string is currently stored in:

`LabManagement/appsettings.Development.json`

Example:

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=lab_management;Username=postgres;Password=1234"
},
"Jwt": {
  "Key": "ThisIsADevelopmentOnlyJwtKeyChangeIt123!",
  "Issuer": "LabManagementAPI",
  "Audience": "LabManagementClient",
  "ExpiryMinutes": 60
}
```

Before pushing to GitHub, it is a good idea to replace real credentials with environment-specific or secret-based configuration.

## Running the Project Locally

### Prerequisites

- .NET 8 SDK
- PostgreSQL
- A database named `lab_management`
- Required tables under the `lab` schema

### Run Steps

1. Open a terminal in `LabManagementAPI/LabManagement`
2. Restore packages:

```bash
dotnet restore
```

3. Run the API:

```bash
dotnet run
```

4. Open Swagger in the browser:

```text
http://localhost:5112/swagger
```

or

```text
https://localhost:7071/swagger
```

## Frontend Integration

The API currently allows CORS requests from:

```text
http://localhost:4200
```

This is intended for local Angular frontend development.

## Important Current Limitations

- Only patient-related endpoints are implemented right now
- Database migrations or schema scripts are not included yet
- Automated tests are not included yet
- The sample development connection string should be secured before production use
- The sample JWT key and sample login password should be replaced before production use

## Future Enhancements

This repository is ready to expand with new modules such as:

- Test catalog management
- Billing and payments
- Sample collection and tracking
- Result entry and approval
- Report generation
- Authentication and role-based access
- Validation and error handling improvements
- Unit and integration tests

## GitHub Notes

This README is meant to be a living project document. As new code is added in the future, update the following sections first:

- `Current Scope`
- `Current API Endpoints`
- `Database Notes`
- `Future Enhancements`

## License

Add your preferred license here before publishing publicly.
