using BOs;
using Repository;
using Repository.Interface;
using System.Text.Json.Serialization;

namespace Jewelry_BE
{
    public static class ServiceRegister
    {
        public static IServiceCollection Register(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            services.AddScoped<IEmployeeRepo, EmployeeRepo>();
            services.AddScoped<JewelryItemContext>();
            return services;
        }
    }
}
