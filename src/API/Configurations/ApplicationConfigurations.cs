using Application.Mediator.RestaurantManager;
using Application.Services;
using Hangfire;
using Infrastructure.Configurations.Options;
using Microsoft.Extensions.Options;

namespace API.Configurations
{
    public static class ApplicationConfigurations
    {
        public static WebApplicationBuilder AddApplication(this WebApplicationBuilder applicationBuilder)
        {
            applicationBuilder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<AddClientsGroupRequest>());

            applicationBuilder.Services.AddHangfire();

            applicationBuilder.Services.AddSingleton<RestManager>();

            return applicationBuilder;
        }

        private static IServiceCollection AddHangfire(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var connectionStringsOption = serviceProvider.GetRequiredService<IOptions<ConnectionStrings>>().Value;

            services.AddHangfire(cfg =>
            {
                cfg.UseSqlServerStorage(connectionStringsOption.Db);
            });

            services.AddHangfireServer();

            return services;
        }
    }
}
