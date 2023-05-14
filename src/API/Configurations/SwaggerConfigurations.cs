namespace API.Configurations
{
    public static class SwaggerConfigurations
    {
        public static WebApplicationBuilder ConfigureSwagger(this WebApplicationBuilder applicationBuilder)
        {
            applicationBuilder.Services.AddSwaggerGen();

            return applicationBuilder;
        }
    }
}
