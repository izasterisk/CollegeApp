using CollegeApp.Configuration;
using CollegeApp.Data;
using CollegeApp.Data.Repository;
using CollegeApp.MyLogging;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
//builder.Logging.ClearProviders();
//builder.Logging.AddConsole();
//builder.Logging.AddDebug();

#region Serilog settings
//Log.Logger = new LoggerConfiguration().MinimumLevel.Information()
//    .WriteTo.File("Log/log.txt", rollingInterval : RollingInterval.Minute)
//    .CreateLogger();

//Write log to txt file
//builder.Host.UseSerilog();
//builder.Logging.AddSerilog();
#endregion

builder.Logging.ClearProviders();
builder.Logging.AddLog4Net();

// Cấu hình context database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<CollegeDBContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)
    //,b => b.MigrationsAssembly("DAL")
    ));

// Add services to the container.

builder.Services.AddControllers()
    .AddNewtonsoftJson()
    .AddXmlDataContractSerializerFormatters();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(AutoMapperConfig));

builder.Services.AddTransient<IMyLogger, LogToServerMemory>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped(typeof(ICollegeRepository<>), typeof(CollegeRepository<>));

//2. Loosely coupled
//builder.Services.AddScoped<IMyLogger, LogToFile>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
