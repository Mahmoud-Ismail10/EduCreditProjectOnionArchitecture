# EduCredit

## Description

EduCredit is a modern, modular academic management system designed to streamline communication, scheduling, enrollment, and notifications for educational institutions. Built with a clean architecture, it provides robust APIs for managing users, courses, notifications, chat, and more, supporting both teachers and students with real-time features.

## How to Run the Project

1. *Prerequisites:*
   - [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
   - [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
   - [Visual Studio 2022](https://visualstudio.microsoft.com/vs/)

2. *Clone the Repository:*
   bash
   git clone https://github.com/GraduationDevs/EduCredit.Solution.git
   cd EduCredit
   

3. *Configure the Application:*
   - Update appsettings.json with your environment-specific settings (e.g., connection strings, JWT secrets).

4. *Restore Dependencies:*
   bash
   dotnet restore
   

5. *Build the Solution:*
   bash
   dotnet build
   

6. *Run the API:*
   bash
   dotnet run
   
   The API will be available at https://localhost:5001 or as configured.

## Technologies Used

- *.NET 8*
- *C# 12*
- *ASP.NET Core Web API*
- *SignalR* (for real-time notifications and chat)
- *Entity Framework Core* (for data access)
- *In-Memory Caching*
- *JWT Authentication & Authorization*
- *Swagger/OpenAPI* (for API documentation)

## Author

Toqa, Mahmoud

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

## Screenshot

![App Screenshot](https://github.com/user-attachments/assets/4571e531-727c-4877-b265-59366ddb1fb7)

## Live Demo

[Click here to try the app](https://educredit.runasp.net/swagger/index.html)
