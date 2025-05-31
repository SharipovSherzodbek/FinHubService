using System;
using System.Threading.Tasks;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using Xunit;
namespace Tests
{
  public class StockServiceTests
  {
    private readonly IStockService? _stocksService;
    public StockServiceTests()
    {
      _stocksService = new StockService();
    }

    #region CreateBuyOrder
    [Fact]
    public async Task CreateBuyOrder_NullBuyOrder_ArgumentNullException()
    {
      // Arrange
      BuyOrderRequest? buyOrderRequest = null;

      // Act & Assert
      await Assert.ThrowsAsync<ArgumentNullException>(async () =>
      {
        await _stocksService.CreateBuyOrder(buyOrderRequest);
      });
    }

    [Theory] //we can pass parameters to the test method
    [InlineData(0)] //passing parameters to the method
    public async Task CreateBuyOrder_QuantityIsLessThanMinimum_ArgumentException(
      uint buyOrderQuantity)
    {
      //Arrange
      BuyOrderRequest buyOrderRequest = new BuyOrderRequest()
      {
        StockName = "Apple Ins",
        DateAndTimeOfOrder = DateTime.Parse("2012-02-04"),
        StockSymbol = "APPL",
        Price = 120,
        Quantity = buyOrderQuantity
      };
      
      //Assert
      await Assert.ThrowsAsync<ArgumentException>(async () => 
        await _stocksService.CreateBuyOrder(buyOrderRequest));
    }

    [Theory] 
    [InlineData(100001)] //passing parameters to the tet method
    public async Task CreateBuyOrder_QuantityIsGreaterThanMaximum_ArgumentException
      (uint buyOrderQuantity)
    {
      //Arrange
      BuyOrderRequest buyOrderRequest = new BuyOrderRequest()
      {
        StockName = "Apple Ins",
        DateAndTimeOfOrder = DateTime.Parse("2012-02-04"),
        StockSymbol = "APPL",
        Price = 120,
        Quantity = buyOrderQuantity
      };

      //Assert
      await Assert.ThrowsAsync<ArgumentException>(async () =>
        await _stocksService.CreateBuyOrder(buyOrderRequest));
    }

    [Theory]
    [InlineData(0)]
    public async Task CreateBuyOrder_PriceIsLessThanMinimum_ArgumentException(
      uint buyOrderPrice)
    {
      //Arrange
      BuyOrderRequest buyOrderRequest = new BuyOrderRequest
      {
        StockName = "Apple",
        StockSymbol = "APPL",
        Price = buyOrderPrice,
        Quantity = 20,
        DateAndTimeOfOrder = DateTime.Now
      };

      //Act & Assert
      await Assert.ThrowsAsync<ArgumentException>(async () =>
        await _stocksService.CreateBuyOrder(buyOrderRequest));     
    }

    [Theory]
    [InlineData(10001)]
    public async Task CreateBuyOrder_PriceIsGreaterThanMaximum_ArgumentException(
      uint buyOrderPrice)
    {
      //Arrange
      BuyOrderRequest buyOrderRequest = new BuyOrderRequest
      {
        StockName = "Apple",
        StockSymbol = "APPL",
        Price = buyOrderPrice,
        Quantity = 20,
        DateAndTimeOfOrder = DateTime.Now
      };

      //Act & Assert
      await Assert.ThrowsAsync<ArgumentException>(async () =>
        await _stocksService.CreateBuyOrder(buyOrderRequest));
    }

    [Fact]
    public async Task CreateBuyOrder_StockSymbolIsNull_TArgumentException()
    {
      //Arrange
      BuyOrderRequest buyOrderRequest = new BuyOrderRequest()
      {
        StockName = "Apple",
        StockSymbol = null,
        Price = 200,
        Quantity = 20
      };

      //Act & Assert
      await Assert.ThrowsAsync<ArgumentException>(async () =>
        await _stocksService.CreateBuyOrder(buyOrderRequest));
    }

    [Fact]
    public async Task CreateBuyOrder_DateOfOrderIsLessThanYear2000_ArgumetnException()
    {
      //Arrange
      BuyOrderRequest buyOrderRequest = new BuyOrderRequest
      {
        StockName = "Apple",
        StockSymbol = "APPL",
        Price = 10,
        Quantity = 10,
        DateAndTimeOfOrder = DateTime.Parse("1997-09-09")
      };

      //Act & Assert
      await Assert.ThrowsAsync<ArgumentException>(async () =>
          await _stocksService.CreateBuyOrder(buyOrderRequest));
    }

    [Fact]
    public async Task CreateBuyOrder_ValidData_Successfull()
    {
      //Arrange
      BuyOrderRequest? buyOrderRequest = new BuyOrderRequest()
      {
        StockSymbol = "MSFT",
        StockName = "Microsoft",
        DateAndTimeOfOrder = Convert.ToDateTime("2024-12-31"),
        Price = 1,
        Quantity = 1
      };

      //Act 
      BuyOrderResponse buyOrderResponseFromCreate =
       await _stocksService.CreateBuyOrder(buyOrderRequest);

      //Assert
       Assert.NotEqual(Guid.Empty , buyOrderResponseFromCreate.BuyOrderID);
      
    }
    #endregion


    #region CreateSellOrder
    [Fact]
    public async Task CreateSellOder_NullSellOrder_ArgumentNullException()
    {
      //Arrange
      SellOrderRequest? sellOrderRequest = null;

      //Act & Assert
      await Assert.ThrowsAsync<ArgumentNullException>(async () =>
        await _stocksService.CreateSellOrder(sellOrderRequest));
    }

    [Theory]
    [InlineData(0)]
    public async Task CreateSellOrder_QuantityIsLessThanMinimum_ArgumentException(
      uint sellOrderQuantity)
    {
      //Arrange
      SellOrderRequest sellOrderRequest = new SellOrderRequest()
      {
        StockName = "Microsoft",
        StockSymbol = "MSFT",
        DateAndTimeOfOrder = DateTime.Parse("2025-01-01"),
        Price = 100,
        Quantity = sellOrderQuantity
      };

      //Act & Assert
      await Assert.ThrowsAsync<ArgumentException>(async () =>
      await _stocksService.CreateSellOrder(sellOrderRequest));
    }

    [Theory]
    [InlineData(100001)]
    public async Task CreateSellOrder_QuantityIsGreaterThanMaximum_ArgumentException(
     uint sellOrderQuantity)
    {
      //Arrange
      SellOrderRequest sellOrderRequest = new SellOrderRequest()
      {
        StockName = "Microsoft",
        StockSymbol = "MSFT",
        DateAndTimeOfOrder = DateTime.Parse("2025-01-01"),
        Price = 100,
        Quantity = sellOrderQuantity
      };

      //Act & Assert
      await Assert.ThrowsAsync<ArgumentException>(async () =>
      await _stocksService.CreateSellOrder(sellOrderRequest));
    }

    [Theory]
    [InlineData(0)]
    public async Task CreateSellOrder_PriceIsLessThanMinimum_ArgumentException(
      uint sellOrderPrice)
    {
      //Arrange
      SellOrderRequest sellOrderRequest = new SellOrderRequest()
      {
        StockName = "Microsoft",
        StockSymbol = "MSFT",
        DateAndTimeOfOrder = DateTime.Parse("2025-01-01"),
        Price = sellOrderPrice,
        Quantity = 10
      };

      //Act & Assert
      await Assert.ThrowsAsync<ArgumentException>(async () =>
        await _stocksService.CreateSellOrder(sellOrderRequest));
    }

    [Theory]
    [InlineData(10001)]
    public async Task CreateSellOrder_PriceIsGreaterThanMaximum_ArgumentException(
     uint sellOrderPrice)
    {
      //Arrange
      SellOrderRequest sellOrderRequest = new SellOrderRequest()
      {
        StockName = "Microsoft",
        StockSymbol = "MSFT",
        DateAndTimeOfOrder = DateTime.Parse("2025-01-01"),
        Price = sellOrderPrice,
        Quantity = 10
      };

      //Act & Assert
      await Assert.ThrowsAsync<ArgumentException>(async () =>
        await _stocksService.CreateSellOrder(sellOrderRequest));
    }

    [Fact]
    public async Task CreateSellOrder_StockSymbolIsNull_ArgumentException()
    {
      //Arrange
      SellOrderRequest sellOrderRequest = new SellOrderRequest 
      {
        StockName = "Apple",
        StockSymbol = null,
        Price = 10,
        Quantity = 10
      };

      //Act & Assert
      await Assert.ThrowsAsync<ArgumentException>(async () =>
        await _stocksService.CreateSellOrder(sellOrderRequest));
    }
   
    [Fact]
    public async Task CreateSellOrder_DateTimeOfOrderGreat2000_ArgumetnException()
    {
      SellOrderRequest sellOrderRequest = new SellOrderRequest
      {
        StockName = "Apple",
        StockSymbol = "APPL",
        Price = 10,
        Quantity = 10,
        DateAndTimeOfOrder = DateTime.Parse("1998-02-02")
      };

      //Act & Assert
      await Assert.ThrowsAsync<ArgumentException>(async () =>
          await _stocksService.CreateSellOrder(sellOrderRequest));
    }

    [Fact]
    public async Task CreateSellOrder_ValidData_Successfull()
    {
      //Arrange
      SellOrderRequest sellOrderRequest = new SellOrderRequest()
      {
        StockSymbol = "MSFT",
        StockName = "Microsoft",
        DateAndTimeOfOrder = Convert.ToDateTime("2024-12-31"),
        Price = 1,
        Quantity = 1
      };

      //Act 
      SellOrderResponse sellOrderResponseFromCreate =
       await _stocksService.CreateSellOrder(sellOrderRequest);

      //Assert
      Assert.NotEqual(Guid.Empty, sellOrderResponseFromCreate.SellOrderID);

    }
    #endregion

    #region GetBuyOrders
    [Fact]
    public async Task GetAllBuyOrders_DefaultList_ToBeEmpty()
    {
      //Arrange  //Act
      List<BuyOrderResponse> buyOrdersFromGet = 
       await _stocksService.GetBuyOrders();

      //Assert
      Assert.Empty(buyOrdersFromGet);
    }

    [Fact]
    public async Task GetAllBuyOrders_ValidData_ToBeSuccessful()
    {
      //Arrange
      BuyOrderRequest buyOrderRequest1 = new BuyOrderRequest()
      {
        StockName = "Apple",
        StockSymbol = "APPL",
        DateAndTimeOfOrder = DateTime.Now,
        Price = 1,
        Quantity = 1
      };

      BuyOrderRequest buyOrderRequest2 = new BuyOrderRequest()
      {
        StockName = "Microsoft",
        StockSymbol = "MSFT",
        DateAndTimeOfOrder = DateTime.Now,
        Price = 2,
        Quantity = 4
      };

      List<BuyOrderRequest> buyOrderRequests = new List<BuyOrderRequest>
      {
        buyOrderRequest1, buyOrderRequest2
      };
      
      List<BuyOrderResponse> buyOrderResponseListFromAdd =
        await _stocksService.GetBuyOrders();

      foreach(var buyOrder in buyOrderRequests)
      {
        BuyOrderResponse buyOrderResponse = 
          await _stocksService.CreateBuyOrder(buyOrder);

        buyOrderResponseListFromAdd.Add(buyOrderResponse);
      }

      List<BuyOrderResponse> buyOrderResponseListFromGet = 
        await _stocksService.GetBuyOrders();

      foreach( var buyOrderResponse in buyOrderResponseListFromAdd)
      {
        Assert.Contains(buyOrderResponse, buyOrderResponseListFromGet);
      }
    }
    #endregion

    #region GetSellOrders
    [Fact]
    public async Task GetAllSellOrders_DefaultList_ToBeEmpty()
    {
      //Act
      List<SellOrderResponse> sellOrdersFromGet =
        await _stocksService.GetSellOrders();

      //Assert
      Assert.Empty(sellOrdersFromGet);
    }

    [Fact]
    public async Task GetAllSellOrder_ValidData_ToBeSuccessful()
    {
      SellOrderRequest sellOrderRequest1 = new SellOrderRequest()
      {
        StockName = "Apple",
        StockSymbol = "APPL",
        Price = 2,
        Quantity = 2,
        DateAndTimeOfOrder = DateTime.UtcNow
      };

      SellOrderRequest sellOrderRequest2 = new SellOrderRequest()
      {
        StockName = "Apple",
        StockSymbol = "APPL",
        Price = 2,
        Quantity = 2,
        DateAndTimeOfOrder = DateTime.UtcNow
      };

      List<SellOrderRequest> sellOrderRequests =
        new List<SellOrderRequest>(){sellOrderRequest1, sellOrderRequest2};

      List<SellOrderResponse> sellOrderResponsesListFromAdd =
        new List<SellOrderResponse>();

      foreach(var sellOrderRequest in sellOrderRequests)
      {
        SellOrderResponse sellOrderResponse =
          await _stocksService.CreateSellOrder(sellOrderRequest);

        sellOrderResponsesListFromAdd.Add(sellOrderResponse);
      }

      List<SellOrderResponse> sellOrderResponseListFromGet =
        await _stocksService.GetSellOrders();

      //Assert
      foreach(var sellOrderResponseFromAdd in sellOrderResponsesListFromAdd)
      {
        Assert.Contains(
          sellOrderResponseFromAdd, sellOrderResponseListFromGet);
      }
    }
    #endregion
  }
}
