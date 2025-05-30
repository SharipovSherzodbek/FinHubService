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

    public TradeController(IOptions<TradingOptions> stockTradeOptions, 
      IFinnhubService finnhubService, IConfiguration configuration)
    {
      _configuration = configuration;
      _stockTradeOptions = stockTradeOptions.Value;
      _finnhubService = finnhubService;
    }

    [Route("/")]
    [Route("~/[controller]")]
    [Route("[action]")]
    public IActionResult Index()
    {
      StockTrade stockTrade = new StockTrade();


      return View();
    }
  }
}
