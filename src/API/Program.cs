using API.Configurations;
using Hangfire;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureOptions()
       .ConfigureAPI()
       .AddApplication()
       .AddPersistence()
       .AddInfrastructure()
       .ConfigureSwagger();

var app = builder.Build();

app.UseDbMigrations();
app.UseHangfireDashboard();//hangfire tasks can be monitored by api/hangfire route

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(builder =>
{
    builder.AllowAnyOrigin();
    builder.AllowAnyHeader();
    builder.AllowAnyMethod();
});

app.UseHttpsRedirection();

app.MapControllers();

app.Run();