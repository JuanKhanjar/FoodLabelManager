# Food Label Manager Application

This project consists of two main applications:
1.  **FoodLabelManager.API**: An ASP.NET Core Web API for managing food labels, users, and authentication using JWT.
2.  **FoodLabelManager.Blazor**: A Blazor Server web application for the user interface, consuming the API.

## **Project Structure**

```
FoodLabelManager/
├── FoodLabelManager.sln
├── FoodLabelManager.API/
│   ├── Controllers/
│   ├── Data/
│   │   └── ApplicationDbContext.cs
│   ├── DTOs/
│   ├── Models/
│   ├── Services/
│   ├── appsettings.json
│   └── Program.cs
└── FoodLabelManager.Blazor/
    ├── Auth/
    │   └── CustomAuthenticationStateProvider.cs
    ├── Components/
    │   ├── App.razor
    │   └── Layout/
    │       ├── MainLayout.razor
    │       ├── NavMenu.razor
    │       └── LoginDisplay.razor
    ├── DTOs/
    ├── Pages/
    │   ├── Auth/
    │   │   ├── Login.razor
    │   │   └── Register.razor
    │   └── FoodLabels/
    │       ├── FoodLabelList.razor
    │       ├── FoodLabelDetails.razor
    │       └── FoodLabelForm.razor
    ├── Services/
    ├── appsettings.json
    └── Program.cs
```

## **Prerequisites**

Before you begin, ensure you have the following installed on your machine:

*   **.NET 9 SDK (or higher)**: Download from [dotnet.microsoft.com](https://dotnet.microsoft.com/download/dotnet/9.0)
*   **Visual Studio 2022 (or Visual Studio Code with C# extension)**
*   **SQL Server LocalDB** (usually included with Visual Studio)

## **Setup and Running the Application**

Follow these steps to get the application up and running on your local machine:

### **1. Clone the Repository (if applicable)**

If you received this project as a `.zip` file, extract it to your desired location. If it's a Git repository, clone it:

```bash
git clone <repository-url>
cd FoodLabelManager
```

### **2. Restore NuGet Packages**

Open your terminal or command prompt, navigate to the `FoodLabelManager` directory (where `FoodLabelManager.sln` is located), and run:

```bash
dotnet restore
```

### **3. Update Database and Apply Migrations (for API)**

The API project uses Entity Framework Core with SQL Server LocalDB. You need to create the database and apply the initial migration.

Navigate to the `FoodLabelManager.API` directory:

```bash
cd FoodLabelManager.API
```

Now, apply the migrations. If you encounter issues with `dotnet ef`, ensure you have the `dotnet-ef` tool installed globally:

```bash
dotnet tool install --global dotnet-ef
```

Then, run the migration command:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

**Note:** The `ApplicationDbContext.cs` file includes a `HasData` method to seed an initial Admin user (`username: admin`, `password: Admin@123`).

### **4. Configure Blazor API Base URL**

Open `FoodLabelManager.Blazor/appsettings.json` and ensure the `ApiSettings:BaseUrl` matches the URL where your API will be running. By default, the API runs on `https://localhost:7290`.

```json
{
  "ApiSettings": {
    "BaseUrl": "https://localhost:7290"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

### **5. Run the API Project**

Open a new terminal or command prompt, navigate to the `FoodLabelManager.API` directory, and run the API:

```bash
cd FoodLabelManager/FoodLabelManager.API
dotnet run
```

The API will typically run on `https://localhost:7290` (check the console output for the exact URL). You can test the API endpoints using Swagger UI by navigating to `https://localhost:7290/swagger` in your browser.

### **6. Run the Blazor Server Project**

Open another new terminal or command prompt, navigate to the `FoodLabelManager.Blazor` directory, and run the Blazor application:

```bash
cd FoodLabelManager/FoodLabelManager.Blazor
dotnet run
```

The Blazor application will typically run on `https://localhost:7001` (check the console output for the exact URL).

### **7. Access the Application**

Open your web browser and navigate to the Blazor application's URL (e.g., `https://localhost:7001`).

## **Using the Application**

*   **Login/Register**: Use the navigation links to log in or register a new user.
    *   **Admin User**: `username: admin`, `password: Admin@123`
*   **Food Labels**: Navigate to the "Food Labels" section to view, add, edit, or delete food labels.
    *   **Admin Role**: Only users with the "Admin" role can add, edit, or delete food labels.

## **Key Features**

*   **Authentication & Authorization**: JWT-based authentication with role-based authorization (Admin/User).
*   **CRUD Operations**: Full Create, Read, Update, Delete functionality for food labels.
*   **Entity Framework Core**: Database interaction.
*   **Blazor Server**: Interactive UI.
*   **MudBlazor**: Modern UI components.
*   **Clean Architecture Principles**: Separation of concerns for maintainability and scalability.

## **Troubleshooting**

*   **`dotnet` command not found**: Ensure .NET SDK is installed and added to your system's PATH.
*   **`dotnet ef` command not found**: Ensure `dotnet-ef` global tool is installed.
*   **Database connection issues**: Check your connection string in `appsettings.json` of the API project.
*   **API not reachable from Blazor**: Verify the `BaseUrl` in `appsettings.json` of the Blazor project matches the API's running URL.
*   **Certificate issues (HTTPS)**: Run `dotnet dev-certs https --trust` to trust the development certificate.

If you encounter any further issues, please refer to the official .NET documentation or seek assistance from the community.

