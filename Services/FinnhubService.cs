using ServiceContracts;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text.Json;

namespace Services
{
  public class FinnhubService : IFinnhubService
  {
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public FinnhubService(IHttpClientFactory httpClientFactory, 
      IConfiguration configuration)
    {
      _configuration = configuration;
      _httpClientFactory = httpClientFactory;
    }

    public async Task<Dictionary<string, object>?> GetStockPriceQuote(
     string stockSymbol)
    {
      var httpClient = _httpClientFactory.CreateClient();

      HttpRequestMessage httpRequestMessage =
        new HttpRequestMessage()
        {
          RequestUri = new Uri($"https://finnhub.io/api/v1/quote?symbol=" +
              $"{stockSymbol}&token={_configuration["FinnHubToken"]}"),
          Method = HttpMethod.Get,
        };

      HttpResponseMessage httpResponseMessage =
        await httpClient.SendAsync(httpRequestMessage);

      if(!httpResponseMessage.IsSuccessStatusCode)
        throw new HttpRequestException("Http request failed with " +
          $"status code {httpResponseMessage.StatusCode}");

      string content =
          await httpResponseMessage.Content.ReadAsStringAsync();

      Dictionary<string, object>? responseDictionary =
       JsonSerializer.Deserialize<Dictionary<string, object>>(content);

      if(responseDictionary == null)
        throw new InvalidOperationException("No response from finnhub.io");

      if(responseDictionary.ContainsKey("error"))
        throw new InvalidOperationException(
          Convert.ToString(responseDictionary["error"]));
      return responseDictionary;
    }


    public async Task<Dictionary<string, object>?> GetCompanyProfile(
      string stockSymbol)
    {
      var httpClient = _httpClientFactory.CreateClient();

      HttpRequestMessage httpRequestMessage =
        new HttpRequestMessage()
        {
          RequestUri = new Uri($"https://finnhub.io/api/v1/stock/profile2?symbol=" +
              $"{stockSymbol}&token={_configuration["FinnHubToken"]}"),
          Method = HttpMethod.Get,
        };

      HttpResponseMessage httpResponseMessage =
        await httpClient.SendAsync(httpRequestMessage);

      if(!httpResponseMessage.IsSuccessStatusCode)
        throw new HttpRequestException("Http request failed with " +
          $"status code {httpResponseMessage.StatusCode}");

      string content =
          await httpResponseMessage.Content.ReadAsStringAsync();

      Dictionary<string, object>? responseDictionary =
       JsonSerializer.Deserialize<Dictionary<string, object>>(content);

      if(responseDictionary == null)
        throw new InvalidOperationException("No response from finnhub.io");

      if(responseDictionary.ContainsKey("error"))
        throw new InvalidOperationException(
          Convert.ToString(responseDictionary["error"]));

      return responseDictionary;
    }

   
  }
}
