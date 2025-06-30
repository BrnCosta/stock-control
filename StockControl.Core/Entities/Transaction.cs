using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockControl.Core.Entities
{
  public class Transaction
  {
    public int? Id { get; set; }
    public required double Value { get; set; }
    public double? Tax { get; set; }
    public required DateOnly Date { get; set; }

    // Relationships
    public required ICollection<StockOperation> StockOperations { get; set; } = [];
  }
}
