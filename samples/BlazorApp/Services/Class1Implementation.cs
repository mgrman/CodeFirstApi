using BlazorApp.Shared;

namespace BlazorApp.Services;


// CodeFirstApi
public class Class1Implementation : IClass1
{
    public async ValueTask<IReadOnlyList<WeatherForecast>> GetForecastsAsync()
    {
        var startDate = DateOnly.FromDateTime(DateTime.Now);
        var summaries = new[] { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = startDate.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = summaries[Random.Shared.Next(summaries.Length)]
        }).ToList();
    }

    public async ValueTask<int> IncrementCountAsync(int currentCount)
    {
        return currentCount + 1;
    }

    public async ValueTask<int> IncrementAuthorizedCountAsync(int currentCount)
    {
        return currentCount + 1;
    }
}