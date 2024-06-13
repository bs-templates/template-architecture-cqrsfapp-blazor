# template-architecture-cqrsfapp-blazor
Architecture CQRS FullAPP + Blazor (.NET 8.0)

#### Migrations

Go to "BAYSOFT.Infrastructures.Data" project folder and open cmd

> dotnet ef --startup-project ../BAYSOFT.Presentations.WebAPP.Blazor migrations add InitialMigrationDefaultDbContext -c DefaultDbContext -o Default/Migrations
