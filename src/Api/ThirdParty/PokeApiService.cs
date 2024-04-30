using System.Text.Json;

namespace Api;

public class PokeApiService
{
  private readonly HttpClient _httpClient;

  public PokeApiService(HttpClient httpClient)
  {
    _httpClient = httpClient;
  }

  public async Task<Location> GetLocation(string locationName, CancellationToken cancellationToken)
  {
    var response = await _httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, $"location/{locationName}"), cancellationToken);
    var content = await response.Content.ReadAsStringAsync();

    var options = new JsonSerializerOptions
    {
      PropertyNameCaseInsensitive = true
    };

    if (response.IsSuccessStatusCode && string.IsNullOrEmpty(content) == false){
      return  JsonSerializer.Deserialize<Location>(content, options);
    }    
    
    return null;
  }
}
