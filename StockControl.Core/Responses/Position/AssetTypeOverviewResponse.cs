using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockControl.Core.Responses.Position
{
    public class AssetTypeOverviewResponse
    {
        public string AssetType { get; set; }
        public decimal Value { get; set; }
        public string Currency { get; set; }
    }
}
