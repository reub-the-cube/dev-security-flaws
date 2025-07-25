Install Dotnet Entity Framework Tool

`dotnet tool update --global dotnet-ef `

Set up the database.

`dotnet ef database update --context ToDoItemContext`

`dotnet ef database update --context ApplicationContext`

Run with

`dotnet run --project ./Security101/Security101.csproj`

Visit: http://localhost:5196/swagger/index.html

Test with

`dotnet test`
