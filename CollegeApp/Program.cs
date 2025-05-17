using CollegeApp.Configuration;
using CollegeApp.Data;
using CollegeApp.Data.Repository;
using CollegeApp.MyLogging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;

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
builder.Services.AddDbContext<CollegeDBContext>(options =>
                 options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Add services to the container.

builder.Services.AddControllers()
    .AddNewtonsoftJson()
    .AddXmlDataContractSerializerFormatters();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//78. Swagger Configuration for Authorization
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter Bearer [space] add your token in the text input. Example: Bearer ...",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = "Bearer",
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

builder.Services.AddAutoMapper(typeof(AutoMapperConfig));

builder.Services.AddTransient<IMyLogger, LogToServerMemory>();

//2. Loosely coupled
//builder.Services.AddScoped<IMyLogger, LogToFile>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped(typeof(ICollegeRepository<>), typeof(CollegeRepository<>));

//builder.Services.AddCors(options => options.AddPolicy("MyTestCORS", policy =>
//{
//    //Allow all origins
//    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();

//    //Allow specific origins
//    //policy.WithOrigins("http://localhost:4200");
//}));

//CORS Configuration
builder.Services.AddCors(options => 
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
    options.AddPolicy("AllowOnlyLocalHost", policy =>
    {
        policy.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod();
    });
    options.AddPolicy("AllowOnlyGoogle", policy =>
    {
        policy.WithOrigins("http://google.com","http://gmail.com").AllowAnyHeader().AllowAnyMethod();
    });
});

string GoogleAudience = builder.Configuration.GetValue<string>("GoogleAudience");
string MicrosoftAudience = builder.Configuration.GetValue<string>("MicrosoftAudience");
string LocalAudience = builder.Configuration.GetValue<string>("LocalAudience");
string GoogleIssuer = builder.Configuration.GetValue<string>("GoogleIssuer");
string MicrosoftIssuer = builder.Configuration.GetValue<string>("MicrosoftIssuer");
string LocalIssuer = builder.Configuration.GetValue<string>("LocalIssuer");

//JWT Authentication Configuration
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer("LoginforGoogleuser", options =>
{
    //options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII
            .GetBytes(builder.Configuration.GetValue<string>("JWTSecretforGoogle"))),
        ValidateIssuer = true,
        ValidIssuer = GoogleIssuer,
        ValidateAudience = true,
        ValidAudience = GoogleAudience,
    };
}).AddJwtBearer("LoginforLocaluser", options =>
{
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII
            .GetBytes(builder.Configuration.GetValue<string>("JWTSecretforLocaluser"))),
        ValidateIssuer = true,
        ValidIssuer = LocalIssuer,
        ValidateAudience = true,
        ValidAudience = LocalAudience,
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("api/testingendpoint",
        context => context.Response.WriteAsync("Test Response 1"))
        .RequireCors("AllowOnlyLocalHost");

    endpoints.MapControllers()
             .RequireCors("AllowOnlyGoogle");

    endpoints.MapGet("api/testingendpoint2",
        context => context.Response.WriteAsync(builder.Configuration.GetValue<string>("JWTSecret")));
});

//app.MapControllers();

app.Run();
