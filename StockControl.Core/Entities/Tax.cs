using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockControl.Core.Entities
{
  public class Tax
  {
    public required DateTime Date { get; set; }
    public required double Value { get; set; }
  }
}
