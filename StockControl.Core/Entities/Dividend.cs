namespace StockControl.Core.Entities
{
  public class Dividend
  {
    public int? Id { get; set; }
    public required double Value { get; set; }
    public required DateOnly Date { get; set; }

    //Relationships
    public required string StockSymbol { get; set; }
  }
}
