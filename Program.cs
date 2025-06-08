using Microsoft.EntityFrameworkCore;
using ServerForToDoList.DBContext;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ToDoContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 21)) // ”кажите вашу версию MySQL
    ));
var app = builder.Build();


app.MapControllers();  // ƒл€ MVC-контроллеров


app.Run();