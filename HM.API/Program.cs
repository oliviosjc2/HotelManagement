using HM.API.Helpers;
using HM.Domain.Entities.Identity;
using HM.Infra.Context;
using Newtonsoft.Json.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Hangfire;
using Hangfire.MemoryStorage;

var builder = WebApplication.CreateBuilder(args);

#region Cors

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowAllOrigins",
        builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
    );
});

#endregion

#region Controllers
builder.Services
    .AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.UseCamelCasing(true);
        options.SerializerSettings.Converters.Add(new StringEnumConverter());
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    });
#endregion Controllers

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379";
    options.InstanceName = "AppCache:";
});

builder.Services.RegisterDependencies();

#region Hangfire

string hangfireConnectionString =
    $"Host=postgres;Port=5432;Username=user;Password=password;Database=db;CommandTimeout=120;Timeout=120;Pooling=true;MaxPoolSize=30;MinPoolSize=10";

builder.Services.AddHangfire(config =>
{
    config.UseSerializerSettings(new Newtonsoft.Json.JsonSerializerSettings
    { ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore });
    config.UseMemoryStorage();
});


builder.Services.AddHangfireServer();

#endregion Hangfire

#region Databases
builder.Services
    .AddEntityFrameworkNpgsql()
    .AddDbContext<HMContext>((serviceProvider, options) =>
    {
        options.UseNpgsql(
                $"Host=postgres;Port=5432;Username=user;Password=password;Database=db;CommandTimeout=120;Timeout=120;Pooling=true;MaxPoolSize=30;MinPoolSize=10",
                sqlOptions => { sqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", $"hm"); })
            .LogTo(Console.WriteLine);
    })
    .AddIdentity<ApplicationUser, ApplicationRole>()
    .AddEntityFrameworkStores<HMContext>()
    .AddDefaultTokenProviders();
#endregion Databases

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Hotel Management", Version = "v1" });

    c.IncludeXmlComments(
        Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization Header utilizando esquema Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var key = Encoding.UTF8.GetBytes("2b922a7b1d3521efd88a700abdeadbfa90b17c1f43a340cc168bb8bea759e33f");

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "HM.API",
            ValidAudience = "HM.CLIENT",
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ClockSkew = TimeSpan.Zero // Remove tolerância de tempo extra
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddLogging(logging =>
{
    logging.AddConsole();
    logging.SetMinimumLevel(LogLevel.Debug);
});

var app = builder.Build();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

#region Miogrations and Seed
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var seeder = services.GetRequiredService<DatabaseSeeder>();
    await seeder.SeedAsync();
}
#endregion

#region Cors
app.UseCors("AllowAllOrigins");
#endregion

app.UseResponseCaching();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseHangfireDashboard();
app.MapControllers();

app.Run();
