using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using System.Text;
using TaskManagementSystem.Database;
using TaskManagementSystem.DependencyInjections;
using TaskManagementSystem.DTOs;

public class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
                 .WriteTo.File($"Log\\Error_{DateTime.Now:MM-dd-yyyy}_{DateTime.Now:hh-mm-ss}.txt", LogEventLevel.Error)
                 .CreateLogger();

        
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);

        if (builder.Environment.IsDevelopment())
        {
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Description = "Bearer Authentication with JWT Token",
                    Type = SecuritySchemeType.Http
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        new List<string>()
                    }
                });
            });
        }

        var _jwtSettings = builder.Configuration.GetSection("JWTSettings");
        builder.Services.Configure<JWTSettings>(_jwtSettings);
        var authKey = _jwtSettings.GetValue<string>("JWT_Key");
        builder.Services.AddAuthentication(item => {
            item.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            item.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(item => {
            item.RequireHttpsMetadata = true;
            item.SaveToken = true;
            item.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authKey!)),
                ValidateIssuer = false,
                ValidateAudience = false

            };
        });

        var uploadPath = builder.Configuration.GetSection("UploadPath");
        builder.Services.Configure<UploadPath>(uploadPath);

        
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddDbContext<TaskManagementDbContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("API_CONNECTION_STRING")));

        builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy
                .WithOrigins(builder.Configuration.GetValue<string>("CorsPolicy:AllowOrigin")!)
                .AllowAnyHeader()
                .AllowAnyMethod()));

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddDatabaseAccessors()
            .AddServices();

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
        app.UseAuthentication();

        app.MapControllers();

        app.Run();
    }
}

