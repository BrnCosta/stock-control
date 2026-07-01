namespace StockControl.Core.Responses.Position
{
  public class PositionAssetOverviewResponse
  {
    public required string Ticker { get; set; }
    public required decimal AveragePrice { get; set; }
    public required decimal Quantity { get; set; }
    public decimal TotalInvested { get; set; }
    public required decimal CurrentPrice { get; set; }
    public decimal CurrentInvested { get; set; }
    public decimal CurrentGain { get; set; }
    public decimal GainPercentage { get; set; }
    public required string AssetType { get; set; }
    public required string Currency { get; set; }
  }
}
