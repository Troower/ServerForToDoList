using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using ServerForToDoList.DBContext;
using System.Text;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All);
    });


builder.Services.AddDbContext<ToDoContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 30))
    ));

builder.Logging.AddConsole().SetMinimumLevel(LogLevel.Debug);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});
var firebaseApp = FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromFile("firebase-adminsdk.json")
});
builder.Services.AddScoped<DeviceTokenService>();
builder.Services.AddSingleton(firebaseApp);
builder.Services.AddTransient<FcmNotificationService>();
builder.Services.AddControllers();
var app = builder.Build();
app.UseStaticFiles(); 
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "Help", "images")),
    RequestPath = "/images"
});
app.MapControllers();
app.MapGet("/", () => "Hello World!");
app.Run();