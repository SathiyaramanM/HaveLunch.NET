using HaveLunch.Persistence;
using HaveLunch.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();

builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("Lunch"));

builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IEmployeeAttendanceService, EmployeeAttendanceService>();
builder.Services.AddScoped<IAdminService, AdminService>();

builder.Services.AddControllers();

var app = builder.Build();

app.UseCors();
app.UseRouting();

app.MapGet("/health-check", () => "Hello World!");

app.MapControllers();

await app.RunAsync();