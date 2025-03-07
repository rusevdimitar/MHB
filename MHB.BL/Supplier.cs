using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace MHB.BL
{
    public class Supplier : SupplierBase
    {
        public Supplier()
        {
        }

        public Supplier(int id, int userID, string connectionString)
            : base(id, userID, connectionString)
        {
        }

        public Supplier(int id, int userID, SqlConnection connection)
            : base(id, userID, connection)
        {
        }

        public decimal TotalPurchasesSum { get; set; }

        public int TotalPurchasesCount { get; set; }

        public decimal TotalPurchasesSharePercentage { get; set; }

        public decimal TotalPurchasesSumPercentage { get; set; }

        public decimal Opacity { get; set; }

        public static string GetSupplierID()
        {
            return Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 14).ToUpper();
        }
    }
}