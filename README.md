# BenzForum Project

This repository contains two main projects: `BenzForumBackend` and `BenzForumFront`. The backend is built with .NET 8, and the frontend is built with Angular 18.

## Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/) (which includes npm)
- [Angular CLI](https://angular.io/cli)

## Getting Started

### Backend

1. Navigate to the `BenzForumBackend` directory:

    ```sh
    cd BenzForumBackend
    ```

2. Restore the .NET dependencies:

    ```sh
    dotnet restore
    ```

3. change the connection string to your MSSQL DB (I used MSSQLLocalDB) in appsettings.Development.json and appsettings.json files:

4. Update the database (if applicable):

    ```sh
    dotnet ef database update
    ```

5. Run the backend project:

    ```sh
    dotnet run
    ```

### Frontend

1. Navigate to the `BenzForumFront` directory:

    ```sh
    cd BenzForumFront
    ```

2. Install the npm dependencies:

    ```sh
    npm install
    ```

3. Run the frontend project:

    ```sh
    ng serve
    ```

## Configuration

### Backend

- Configuration files for different environments can be found in the `BenzForumBackend` directory:
  - [appsettings.json](BenzForumBackend/appsettings.json)
  - [appsettings.Development.json](BenzForumBackend/appsettings.Development.json)

### Frontend

- Configuration files for the Angular project can be found in the `BenzForumFront` directory:
  - [angular.json](BenzForumFront/angular.json)
  - [tsconfig.json](BenzForumFront/tsconfig.json)
  - [tsconfig.app.json](BenzForumFront/tsconfig.app.json)
  - [tsconfig.spec.json](BenzForumFront/tsconfig.spec.json)
