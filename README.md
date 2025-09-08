# ClipNchic.Api

This is the backend API for the ClipNchic project, built with ASP.NET Core.

## Getting Started

### 1. Clone the Repository

```sh
git clone <your-repo-url>
cd BE_clipNchick
```

### 2. Configure the Database

- Update your connection string in `appsettings.json` (or `appsettings.Development.json`) in the `ClipNchic.Api` project.
- Example:
  ```json
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=ClipNchicDb;Trusted_Connection=True;"
  }
  ```

### 3. Apply Migrations

If using Entity Framework Core, run:

```sh
dotnet ef database update --project .\ClipNchic.Api\ClipNchic.Api.csproj
```

### 4. Run the API

```sh
dotnet run --project .\ClipNchic.Api\ClipNchic.Api.csproj --launch-profile https
```

The API will be available at [https://localhost:7169](https://localhost:7169).

### 5. Access Swagger UI

Open your browser and go to:

```
https://localhost:7169/swagger
```

You can test all API endpoints from the Swagger UI.

## Project Structure

- `ClipNchic.Api/Controllers/` - API controllers
- `ClipNchic.Api/Models/` - Entity models
- `ClipNchic.Api/Data/` - Database context

## Common Commands

- **Restore packages:**  
  `dotnet restore`
- **Build the project:**  
  `dotnet build`
- **Run the project:**  
  `dotnet run --project .\ClipNchic.Api\ClipNchic.Api.csproj --launch-profile https`
- **Apply migrations:**  
  `dotnet ef database update --project .\ClipNchic.Api\ClipNchic.Api.csproj`

## Notes

- Make sure your database server is running and accessible.
- Do not commit sensitive data (like passwords or secrets) to the repository.
- Use the `.gitignore` file to avoid committing build outputs and user-specific files.

---

For any issues, please open an issue in this repository.
