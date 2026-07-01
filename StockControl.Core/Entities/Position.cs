namespace StockControl.Core.Entities
{
    public class Position
    {
        public Guid AssetId { get; set; }
        public Asset Asset { get; set; }

        public decimal Quantity { get; set; }
        public decimal AveragePrice { get; set; }
    }
}
