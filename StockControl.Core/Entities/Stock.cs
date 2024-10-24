using StockControl.Core.Enums;
using System.Text.Json.Serialization;

namespace StockControl.Core.Entities
{
  public class Stock
  {
    public required string Symbol { get; set; }
    public required double Price { get; set; }
    public required StockType StockType { get; set; }

    // Relationships
    [JsonIgnore]
    public StockHolder? StockHolder { get; set; }
    public ICollection<StockOperation> StockOperations { get; } = [];
  }
}
