# Employee Management Microservice

## Overview
This repository contains a scalable microservice for managing employee data. It is designed to efficiently handle CRUD (Create, Read, Update, Delete) operations for a large volume of employee records. The service ensures security by leveraging Azure Key Vault for managing sensitive configurations like connection strings and database credentials. Additionally, it integrates with Azure Cosmos DB for reliable data storage and Redis Cache for high-performance read operations.


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
The application is designed with a clean architecture, ensuring modularity, testability, and scalability:

Controllers: Handle incoming HTTP requests and route them to the appropriate services.
Services: Contain business logic and coordinate operations between controllers and repositories.
Repositories: Interact directly with Azure Cosmos DB and Redis Cache to perform data operations.
A detailed architecture diagram can be found in the Documentation folder as a PDF.

## Endpoints
- `GET /Employees/{id}`: Retrieve an employee by ID.
- `GET /Employees`: Retrieve all employees.
- `POST /Employees`: Create a new employee.
- `PUT /Employees/{id}`: Update an existing employee.
- `DELETE /Employees/{id}`: Delete an employee.

