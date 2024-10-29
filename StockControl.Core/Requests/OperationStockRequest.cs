namespace StockControl.Core.Requests
{
  public class OperationStockRequest
  {
    public required string StockSymbol { get; set; }
    public required double Price { get; set; }
    public required int Quantity { get; set; }
  }
}
