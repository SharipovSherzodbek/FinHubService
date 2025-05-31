
using ServiceContracts;
using ServiceContracts.DTO;
using Enities;
using Services.Helpers;

namespace Services
{
  public class StockService : IStockService
  {
    private readonly List<BuyOrder> _buyOrders;
    private readonly List<SellOrder> _sellOrders;
    public StockService()
    {
      _buyOrders = new List<BuyOrder>();
      _sellOrders = new List<SellOrder>();
    }

    // Inserts a new buy order into the database table called 'BuyOrders'.
    public async Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? requestBuyOrder)
    {
      if(requestBuyOrder == null) 
        throw new ArgumentNullException(nameof(requestBuyOrder));

      ValidationHelper.ModelValidation(requestBuyOrder);

      BuyOrder buyOrder = requestBuyOrder.ToBuyOrder();
      buyOrder.BuyOrderID = Guid.NewGuid();

      _buyOrders.Add(buyOrder);

      return buyOrder.ToBuyOrderResponse();
    }

    //Inserts a new sell order into the database table called 'SellOrders'.
    public async Task<SellOrderResponse> CreateSellOrder(
        SellOrderRequest? sellOrderRequest)
    {
     if(sellOrderRequest == null)
        throw new ArgumentNullException(nameof(sellOrderRequest));

     ValidationHelper.ModelValidation(sellOrderRequest);

      SellOrder sellOrder = sellOrderRequest.ToSellOrder();
      sellOrder.SellOrderID = Guid.NewGuid();

      _sellOrders.Add(sellOrder);

      return sellOrder.ToSellOrderResponse();
    }

    // Returns the existing list of buy orders retrieved
                    // from database table called 'BuyOrders'.
    public async Task<List<BuyOrderResponse>> GetBuyOrders()
    {
      return _buyOrders
        .OrderByDescending(temp => temp.DateAndTimeOfOrder)
        .Select(temp => temp.ToBuyOrderResponse()).ToList();
    }

    //Returns the existing list of sell orders retrieved
                      //from database table called 'SellOrders'.
    public async Task<List<SellOrderResponse>> GetSellOrders()
    {
      return _sellOrders
        .OrderByDescending(temp => temp.DateAndTimeOfOrder)
        .Select(temp => temp.ToSellOrderResponse()).ToList();
    }
    //It was hard to fix nested .git
  }
}
