using Application.Profiles;
using FluentValidation;
using FluentValidation.AspNetCore;
using System.Reflection;
using System.Text.Json.Serialization;

namespace API.Configurations
{
    public static class ApiConfigurations
    {
        public static WebApplicationBuilder ConfigureAPI(this WebApplicationBuilder applicationBuilder)
        {
            applicationBuilder.Services.AddCors();
            applicationBuilder.Services.AddControllers().AddJsonOptions(
                opt => opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
            applicationBuilder.Services.AddEndpointsApiExplorer();

            applicationBuilder.Services.AddFluentValidationAutoValidation();
            applicationBuilder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            applicationBuilder.Services.AddAutoMapper(typeof(MapProfile));

            return applicationBuilder;
        }
    }
}
