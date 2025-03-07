
using System;

namespace MHB.BL
{
    public class ProductInfo
    {
        public int ID { get; set; }

        public Guid UniqueID { get; set; }

        public string Name { get; set; }

        public decimal Amount { get; set; }

        public decimal Total { get; set; }

        public decimal Average { get; set; }

        public decimal Max { get; set; }

        public decimal Min { get; set; }

        public decimal Consumption { get; set; }

        public decimal ExpectedRange { get; set; }

        public decimal RemainingRange { get; set; }

        public decimal ExpectedQuantity { get; set; }

        public decimal PricePerKm { get; set; }

        public decimal PricePerHundredKms { get; set; }

        public int SupplierID { get; set; }

        public string SupplierName { get; set; }

        public MHB.BL.Enums.MeasureType MeasureType { get; set; }

        public bool HasLowestPrice { get; set; }

        public bool HasHighestPrice { get; set; }

    }
}
