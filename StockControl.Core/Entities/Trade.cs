using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockControl.Core.Entities
{
    public class Trade
    {
        public Guid Id { get; set; }
        public decimal? Tax { get; set; }
        public DateTime Date { get; set; }

        // Relationships
        public ICollection<Transaction> Transactions { get; set; } = [];
    }
}
