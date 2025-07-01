using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockControl.Core.Requests
{
  public class UpdateTaxRequest
  {
    public required int TransactionId { get; set; }
    public required double TaxValue { get; set; }
  }
}
