using StockControl.Core.Entities;

namespace StockControl.Core.Requests
{
  public class TransactionRequest
  {
    public required double Value { get; set; }
    public required DateOnly Date { get; set; }
    public required ICollection<StockOperationRequest> StockOperations { get; set; } = [];
    public double? Tax { get; set; }
  }
}
