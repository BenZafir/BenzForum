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

3. Set the JWT signing key. It is not in source control — provide it through
   user secrets:

```sh
   cd BenzForum
   dotnet user-secrets set "Jwt:Key" "$(openssl rand -base64 32)"
```

   Any value of 32 characters or more works; HMAC-SHA256 needs a 256-bit key.
   Outside Development, set the `Jwt__Key` environment variable instead.
   See `appsettings.Example.json` for the full shape.

4. The default connection string points at MSSQLLocalDB. Change it in
   `appsettings.json` if you use a different server.

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
  - [appsettings.json](BenzForumBackend/BenzForum/appsettings.json)
  - [appsettings.Development.json](BenzForumBackend/appsettings.Development.json)

### Frontend

- Configuration files for the Angular project can be found in the `BenzForumFront` directory:
  - [angular.json](BenzForumFront/angular.json)
  - [tsconfig.json](BenzForumFront/tsconfig.json)
  - [tsconfig.app.json](BenzForumFront/tsconfig.app.json)
  - [tsconfig.spec.json](BenzForumFront/tsconfig.spec.json)
