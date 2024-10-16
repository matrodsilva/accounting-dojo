using ArquiveStatus.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
       .AddHostedService<GetArchivesStatusService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicyForDashboard", builder =>
            builder
                   .WithOrigins("http://localhost:4200")
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowCredentials());
});

builder.Services.AddSignalR();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast",
            () =>
            {
                var forecast = Enumerable.Range(1, 5)
                                         .Select(index =>
                                                  new WeatherForecast(
                                                          DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                                                          Random.Shared.Next(-20, 55),
                                                          summaries[Random.Shared.Next(summaries.Length)]
                                                  ))
                                         .ToArray();

                return forecast;
            })
   .WithName("GetWeatherForecast")
   .WithOpenApi();

app.MapHub<ArchiveHub>("/archivehub");

app.UseCors("CorsPolicyForDashboard");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
