var builder = WebApplication.CreateBuilder(args);

// ���������� �������� API
builder.Services.AddControllers();  

var app = builder.Build();


app.MapControllers();  // ��� MVC-������������


app.Run();