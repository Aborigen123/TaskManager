Task Manager
=====================

Tech Stack
-----------------------------------
* .Net 7
* Asp .Net Core Web API
* Entity Framework Core
* In-Memory database

Libraries
-----------------------------------
* LINQ
* JWT
* Hangfire
* BCrypt.Net-Next
* Swagger

Getting Started
=====================

Prerequisites
-----------------------------------
.NET version 7

Installation
-----------------------------------
1. Clone the repo
```bash
git clone https://github.com/Aborigen123/TaskManager
```
2. Compilation of the project
```bash
dotnet build
```

3. Run application
```bash
dotnet run
```

Usage
-----------------------------------
After successfully launching the application, you will be able to utilize its features. The application runs locally on `http://localhost:5067` and consists of two main pages:

## Pages

### Swagger Documentation
- **URL:** `http://localhost:5067/index.html`
- This page provides detailed documentation of the available endpoints in the application using Swagger. It is an essential resource for understanding the API functionalities and how to interact with them.

### Hangfire Dashboard
- **URL:** `http://localhost:5067/hangfire`
- This page displays information about the background job that reassigns users every two minutes. You can monitor the execution of this process in real-time using the Realtime Graph feature. This dashboard is particularly useful for observing the backend processes and ensuring they are functioning correctly.

## InMemory Database
- The application utilizes an InMemory Database to avoid dependencies on external databases, enhancing the convenience and efficiency of the code review process. This setup is ideal for reviewers who prefer a streamlined and self-contained application environment.

## Important Note
- **Data Persistence:** Please note that the data does not persist after each restart of the application. All information is temporarily stored and will be lost upon application shutdown.

## Initial Setup
- **Default Data:** Upon launching the application for the first time, the database is automatically populated with 3 users, 2 tasks, and 1 assigned history. This initial setup helps in demonstrating the application's functionalities without the need for manual data entry.


