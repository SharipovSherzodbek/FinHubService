﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceContracts.DTO;

namespace ServiceContracts
{
  public interface IStockService
  {
    Task<BuyOrderResponse> CreateBuyOrder(
      BuyOrderRequest? buyOrderRequest);

    Task<SellOrderResponse> CreateSellOrder(
      SellOrderRequest? sellOrderRequest);

    Task<List<BuyOrderResponse>> GetBuyOrders();
    Task<List<SellOrderResponse>> GetSellOrders();
  }
}
