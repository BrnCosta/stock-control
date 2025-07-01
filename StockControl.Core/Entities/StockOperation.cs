using StockControl.Core.Enums;
using System.Text.Json.Serialization;

namespace StockControl.Core.Entities
{
  public class StockOperation
  {
    public int? Id { get; set; }
    public required int Quantity { get; set; }
    public required double Price { get; set; }
    public required OperationType OperatingType { get; set; }

    // Relationships
    [JsonIgnore]
    public int? TransactionId { get; set; }
    [JsonIgnore]
    public Transaction? Transaction { get; set; }

    public required string StockSymbol { get; set; }
  }
}
