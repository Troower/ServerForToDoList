using Microsoft.EntityFrameworkCore;
using ServerForToDoList.DBContext;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ToDoContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 30)) 
    ));
builder.Services.AddControllers();
var app = builder.Build();
app.MapControllers();
app.MapGet("/", () => "Hello World!");
app.Run();