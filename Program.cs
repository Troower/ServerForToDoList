var builder = WebApplication.CreateBuilder(args);

// Добавление сервисов API
builder.Services.AddControllers();  

var app = builder.Build();


app.MapControllers();  // Для MVC-контроллеров


app.Run();