using FinhubService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServiceContracts;

namespace FinhubService.Controllers
{
  public class TradeController : Controller
  {
    private readonly IFinnhubService _finnhubService;
    private readonly IConfiguration _configuration;
    private readonly TradingOptions _stockTradeOptions;
    private readonly IStockService _stockService;

    public TradeController(IOptions<TradingOptions> stockTradeOptions, 
      IFinnhubService finnhubService, IConfiguration configuration,
        IStockService stockService)
    {
      _configuration = configuration;
      _stockTradeOptions = stockTradeOptions.Value;
      _finnhubService = finnhubService;
      _stockService = stockService;
    }

    [Route("/")]
    [Route("~/[controller]")]
    [Route("[action]")]
    public async Task<IActionResult>  Index()
    {
      if(string.IsNullOrEmpty(_stockTradeOptions.DefaultStockSymbol))
        _stockTradeOptions.DefaultStockSymbol = "MSFT";

      Dictionary<string, object>? responseDictionary =
       await _finnhubService.GetStockPriceQuote(
         _stockTradeOptions.DefaultStockSymbol);

      Dictionary<string, object>? responseCompanyProfiel = 
        await _finnhubService.GetCompanyProfile(
          _stockTradeOptions.DefaultStockSymbol);

      StockTrade stockTrade = new StockTrade()
      {
        StockName = _stockTradeOptions.DefaultStockSymbol   
      };

      if(responseCompanyProfiel != null && responseDictionary != null)
      {
        stockTrade = new StockTrade()
        {
          StockSymbol = Convert.ToString(
            responseCompanyProfiel["ticker"]),
          StockName = Convert.ToString(
            responseCompanyProfiel["name"]),
          StockPrice = Convert.ToDouble(
            responseDictionary["c"].ToString())
        };
      }

      ViewBag.FinnhubToken = _configuration["FinnhubToken"];
      return View(stockTrade);
    }
  }
}
