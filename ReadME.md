# Employee Management Microservice

## Overview
This repository contains a microservice for managing employee data, designed to handle CRUD operations for a large number of employee entities. The service utilizes Azure Cosmos DB for data storage and Redis Cache for high-performance read operations.

## Features
- RESTful API for CRUD operations on employee entities.
- Integration with Azure Cosmos DB and Redis Cache.
- Caching mechanism to optimize read operations.

## Getting Started
1. Clone the repository.
2. Ensure you have [.NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) installed.
3. Set up Azure Cosmos DB and Redis Cache in Azure. 
4. Set up the key vault in Azure and enter your secrets there (you need connection strings, database name and container name)
4. Update `appsettings.json` with your Azure Key Vault name. 
5. Ensure that sensitive information such as connection strings, database names, and container names are stored in Azure Key Vault.
5. Build and run the application.

## Architecture
The microservice follows a clean architecture with separation of concerns among Controllers, Services, and Repositories.

Architecture diagram can be found in the "Documentation" folder as a pdf. Feel free to download it to get a clear view. 

## Endpoints
- `GET /Employees/{id}`: Retrieve an employee by ID.
- `GET /Employees`: Retrieve all employees.
- `POST /Employees`: Create a new employee.
- `PUT /Employees/{id}`: Update an existing employee.
- `DELETE /Employees/{id}`: Delete an employee.

