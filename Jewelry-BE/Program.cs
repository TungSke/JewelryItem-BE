using Jewelry_BE;
using Jewelry_BE.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Register services using a custom extension method
ServiceRegister.Register(builder.Services, builder.Configuration);

builder.Services.AddControllers();

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "JewelryItem", Version = "v1" });

    // Add JWT Bearer security definition
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });

    // Add JWT Bearer authentication to operations
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
            new string[] { }
        }
    });
});

// Add authorization policy for JWT Bearer
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Bearer", policy =>
    {
        policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
        policy.RequireAuthenticatedUser();
    });
});

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        policy => policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseCors("CorsPolicy");
app.UseHttpsRedirection();

app.UseAuthentication(); // Ensure this is before UseAuthorization
app.UseAuthorization();

app.MapControllers();

app.Run();
