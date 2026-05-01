namespace StockControl.Core.Entities
{
    public class Dividend
    {
        public Guid Id { get; set; }
        public required decimal Value { get; set; }
        public required DateTime Date { get; set; }

        public Guid AssetId { get; set; }
        public Asset Asset { get; set; }
    }
}
