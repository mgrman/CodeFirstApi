using CodeFirstApi.Core;

namespace BlazorApp.Shared;

// CodeFirstApi
[GenerateServices]
public interface IClass1
{
    [PersistForPrerendering]
    ValueTask<IReadOnlyList<WeatherForecast>> GetForecastsAsync();
    ValueTask<int> IncrementCountAsync(int currentCount);
}