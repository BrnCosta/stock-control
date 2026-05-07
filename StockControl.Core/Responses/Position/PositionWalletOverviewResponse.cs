using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockControl.Core.Responses.Position
{
    public class PositionWalletOverviewResponse
    {
        public IEnumerable<AssetTypeOverviewResponse> AssetTypes { get; set; }
        public decimal TotalValue { get; set; }
    }

    public class AssetTypeOverviewResponse
    {
        public string AssetType { get; set; }
        public decimal Value { get; set; }
    }
}
