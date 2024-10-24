﻿using StockControl.Core.Enums;
using System.Text.Json.Serialization;

namespace StockControl.Core.Entities
{
    public class StockOperation
    {
        public int? Id { get; set; }
        public required int Quantity { get; set; }
        public required double Price { get; set; }
        public required DateTime Date { get; set; }
        public required OperationType OperatingType { get; set; }

        // Relationships
        public required string StockSymbol { get; set; }
    }
}