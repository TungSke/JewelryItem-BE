using BOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repository.Interface;
using Repository;
using System.Text.Json.Serialization;
using DAOs;
using Razor.Templating.Core;

public static class ServiceRegister
{
    public static void Register(IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        //add template engine
        services.AddRazorTemplating();
        services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        // Get the connection string from the configuration
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        // Register the DbContext with the connection string
        services.AddDbContext<JewelryItemContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<EmployeDAO>();
        services.AddScoped<CustomerDAO>();
        services.AddScoped<ProductDAO>();
        services.AddScoped<OrderDAO>();
        services.AddScoped<PromotionDAO>();

        services.AddScoped<IPromotionRepo, PromotionRepo>();
        services.AddScoped<IOrderRepo, OrderRepo>();
        services.AddScoped<IEmployeeRepo, EmployeeRepo>();
        services.AddScoped<IProductRepo, ProductRepo>();
        services.AddScoped<ICustomerRepo, CustomerRepo>();
    }
}
