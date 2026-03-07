# TSManager
TSManager is a .NET MVC application to manage domains, software subscriptions and Emails.

#TechStack 
- ASP.NET Core MVC (.NET 9)
- Entity Framework Core
- MySQL
- Bootstrap
- Gmail SMTP
  
## Setup
1. Clone repository
2. Copy appsettings.Example.json → appsettings.Development.json
3. Update database + email credentials
4. Run migrations:
   dotnet ef database update
5. Start app:
   dotnet run
