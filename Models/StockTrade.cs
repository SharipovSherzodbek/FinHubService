namespace FinhubService.Models
{
  public class StockTrade
  {
    public string? StockSymbol { get; set; }
    public string? StockName { get; set; }
    public double StockPrice { get; set; } = 0;
    public uint StockQuantity { get; set; } = 0;
  }
}
