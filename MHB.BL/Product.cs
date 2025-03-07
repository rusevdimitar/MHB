using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace MHB.BL
{
    public class Product : ProductBase
    {
        public Product()
        {
        }

        public Product(int id, int userID, string connectionString)
            : base(id, userID, connectionString)
        {
        }

        public Product(int id, int userID, SqlConnection connection)
            : base(id, userID, connection)
        {
        }

        public Product(int id, int userID, int parentID, object parent, SqlConnection connection, IEnumerable<Category> categoriesCache = null, IEnumerable<Supplier> suppliersCache = null)
            : base(id, userID, parentID, parent, connection, categoriesCache, suppliersCache)
        {
        }

        public Product(Product product)
            : base(product)
        {
        }

        public int Priority { get; internal set; }

        public IEnumerable<ProductParameter> GetParameters()
        {
            return SQLHelper.GetProductParameters(this.ParentID, this.ID, this.UserID, this.Connection, this.ConnectionString);
        }

        public IEnumerable<ProductParameter> GetParameters(int productID, int parentID, int userID, SqlConnection connection, string connectionString)
        {
            return SQLHelper.GetProductParameters(parentID, productID, userID, connection, connectionString);
        }

        public void AddParameter(ProductParameter parameter, SqlConnection connection, string connectionString = "")
        {
            SQLHelper.AddProductParameter(parameter.Key, parameter.Value, parameter.ParentID, parameter.ProductID, parameter.ProductParameterTypeID, connection, ExpenditureManager.GetUserMainTableNames(this.UserID).Item2, connectionString);
        }

        public void AddParameter(string key, string value, int parentID, int productID, int productParameterTypeID, SqlConnection connection, string connectionString = "")
        {
            SQLHelper.AddProductParameter(key, value, parentID, productID, productParameterTypeID, connection, ExpenditureManager.GetUserMainTableNames(this.UserID).Item2, connectionString);
        }
    }
}