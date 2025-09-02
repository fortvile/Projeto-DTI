using DTI.Infrastructure.Data; // DbContext
using Microsoft.EntityFrameworkCore;
using DTI.Api.Services;       // IEmailService / FakeEmailService

var builder = WebApplication.CreateBuilder(args);

// DbContext (SQL Server)
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// CORS para o React em http://localhost:3000
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// DI do "serviço de e-mail" falso
builder.Services.AddScoped<IEmailService, FakeEmailService>();

// MVC + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middlewares
app.UseHttpsRedirection();
app.UseCors("AllowReact");
app.UseAuthorization();

// Controllers
app.MapControllers();

// Rota raiz (teste rápido)
app.MapGet("/", () => "API rodando! 🚀");

// Garante a pasta Notifications para o 'email' falso
var notificationsDir = Path.Combine(app.Environment.ContentRootPath, "Notifications");
if (!Directory.Exists(notificationsDir))
{
    Directory.CreateDirectory(notificationsDir);
}

app.Run();
