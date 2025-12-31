# Merge Arrays REST API (.NET 8 + SQLite)

A simple REST API service that:
- Accepts **two sorted (ascending)** integer arrays
- Merges them into a single sorted array (O(n+m))
- Stores **request + response** in a SQLite database
- Provides an endpoint to query stored operations by **result array length**

---

## Tech Stack
- **.NET 8** (ASP.NET Core Web API)
- **EF Core** + **SQLite** (file-based DB)
- **Swagger/OpenAPI** for interactive testing

---

## Prerequisites
Install **.NET 8 SDK** (SDK includes ASP.NET Core runtime).

Verify:
```bash
dotnet --version

## Run the API

From the project root:

```bash
dotnet restore
dotnet run

You can use swagger to test api
http://localhost:listening URL printed in terminal/swagger

