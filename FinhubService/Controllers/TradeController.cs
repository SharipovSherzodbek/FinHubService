using FinhubService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;

namespace FinhubService.Controllers
{
  [Route("[controller]")]
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
    [Route("[action]")]
    [Route("~/[controller]")]
    public async Task<IActionResult> Index()
    {
      //reset stock symbol if not exists
      if(string.IsNullOrEmpty(_stockTradeOptions.DefaultStockSymbol))
        _stockTradeOptions.DefaultStockSymbol = "MSFT";


      //get company profile from API server
      Dictionary<string, object>? companyProfileDictionary = 
        await _finnhubService.GetCompanyProfile(
               _stockTradeOptions.DefaultStockSymbol);

      //get stock price quotes from API server
      Dictionary<string, object>? stockQuoteDictionary =
        await _finnhubService.GetStockPriceQuote(
                _stockTradeOptions.DefaultStockSymbol);


      //create model object
      StockTrade stockTrade = new StockTrade() { 
        StockSymbol = _stockTradeOptions.DefaultStockSymbol };

      //load data from finnHubService into model object
      if(companyProfileDictionary != null && 
                  stockQuoteDictionary != null)
      {
        stockTrade = new StockTrade() 
        { StockSymbol = companyProfileDictionary["ticker"].ToString(),
          StockName = companyProfileDictionary["name"].ToString(),
          StockQuantity = _stockTradeOptions.DefaultOrderQuantity ?? 0,
          StockPrice = Convert.ToDouble(stockQuoteDictionary["c"]
          .ToString()) };
      }
      //Send Finnhub token to view
      ViewBag.FinnhubToken = _configuration["FinnHubToken"];
      return View(stockTrade);
    }

    [Route("[action]")]
    public async Task<IActionResult> BuyOrder(BuyOrderRequest 
              buyOrderRequest)
    {
      //update date of order
      buyOrderRequest.DateAndTimeOfOrder = DateTime.Now;

      //re-validate the model object after updating the date
      ModelState.Clear();
      TryValidateModel(buyOrderRequest);


      if(!ModelState.IsValid)
      {
        ViewBag.Errors = ModelState.Values.SelectMany(v => 
              v.Errors).Select(e => e.ErrorMessage).ToList();

        StockTrade stockTrade = new StockTrade() { 
          StockName = buyOrderRequest.StockName, 
          StockQuantity = buyOrderRequest.Quantity, 
          StockSymbol = buyOrderRequest.StockSymbol };

        return View("Index", stockTrade);
      }

      //invoke service method
      BuyOrderResponse buyOrderResponse =
        await _stockService.CreateBuyOrder(buyOrderRequest);

      return RedirectToAction(nameof(Orders));
    }

    [Route("[action]")]
    public async Task<IActionResult> SellOrder(SellOrderRequest 
              sellOrderRequest)
    {
      //update date of order
      sellOrderRequest.DateAndTimeOfOrder = DateTime.Now;

      //re-validate the model object after updating the date
      ModelState.Clear();
      TryValidateModel(sellOrderRequest);

      if(!ModelState.IsValid)
      {
        ViewBag.Errors = ModelState.Values.SelectMany(v => 
            v.Errors).Select(e => e.ErrorMessage).ToList();
        StockTrade stockTrade = new StockTrade() {
          StockName = sellOrderRequest.StockName, 
          StockQuantity = sellOrderRequest.Quantity, 
          StockSymbol = sellOrderRequest.StockSymbol };

        return View("Index", stockTrade);
      }
      //invoke service method
      SellOrderResponse sellOrderResponse = 
            await _stockService.CreateSellOrder(sellOrderRequest);

      return RedirectToAction(nameof(Orders));
    }

    [Route("[action]")]
    public async Task<IActionResult> Orders()
    {
      //invoke service methods
      List<BuyOrderResponse> buyOrderResponses = 
          await _stockService.GetBuyOrders();
      List<SellOrderResponse> sellOrderResponses = 
          await _stockService.GetSellOrders();

      //create model object
      Orders orders = new Orders() { BuyOrders = buyOrderResponses, SellOrders = sellOrderResponses };

      ViewBag.TradingOptions = _stockTradeOptions;

      return View(orders);
    }
  }
}
