﻿namespace StockControl.Core.Entities
{
  public class StockHolder
  {
    public int? Id { get; set; }
    public required double AveragePrice { get; set; }
    public required int Quantity { get; set; }

    // Relationships
    public required string StockSymbol { get; set; }
    public required Stock Stock { get; set; }
  }
}
