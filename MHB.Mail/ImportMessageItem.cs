using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHB.Mail
{
    public class ImportMessageItem
    {
        public ImportMessageItem(string categoryName, string productName, decimal value)
        {
            this.CategoryName = categoryName;
            this.ProductName = productName;
            this.Value = value;
        }

        public ImportMessageItem(string categoryName, string productName, decimal value, decimal quantity)
        {
            this.CategoryName = categoryName;
            this.ProductName = productName;
            this.Value = value;
            this.Quantity = quantity;
        }

        public string CategoryName { get; set; }
        public string ProductName { get; set; }
        public decimal Value { get; set; }
        public decimal Quantity { get; set; }
    }
}