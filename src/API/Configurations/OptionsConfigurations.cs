using Infrastructure.Configurations.Options;

namespace API.Configurations
{
    public static class OptionsConfigurations
    {
        public static WebApplicationBuilder ConfigureOptions(this WebApplicationBuilder applicationBuilder)
        {
            applicationBuilder.Services.Configure<ConnectionStrings>(
                applicationBuilder.Configuration.GetSection(nameof(ConnectionStrings)));

            return applicationBuilder;
        }
    }
}
