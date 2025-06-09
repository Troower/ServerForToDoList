using Microsoft.EntityFrameworkCore;
using ServerForToDoList.DBContext;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ToDoContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 30)) 
    ));
var app = builder.Build();


app.MapControllers();  // Для MVC-контроллеров


app.Run();