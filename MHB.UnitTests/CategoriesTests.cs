using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHB.UnitTests
{
    [TestClass]
    public class CategoriesTests
    {
        private const string ConnectionString = "Data Source = localhost; Initial Catalog = Test01Db.dev; User ID =sa; Password = Mitko_123; MultipleActiveResultSets=True;Connect Timeout=90;";
        private const string ConnectionStringActual = "Data Source = localhost; Initial Catalog = Test01Db; User ID =sa; Password = Mitko_123; MultipleActiveResultSets=True;Connect Timeout=90;";

        [TestMethod]
        public void CheckForCategoriesDuplicates()
        {
            string qry =
                @"SELECT DISTINCT c.UserCategoryID AS UserID, c.CategoryName FROM tbUsers u
	                INNER JOIN tbCategories c ON u.UserID = c.UserCategoryID
	                WHERE c.CategoryName <> ''
	                GROUP BY c.CategoryName, c.UserCategoryID
	                HAVING COUNT(*) > 1";

            IDataReader reader = MHB.DAL.DataBaseConnector.GetDataReader(qry, CategoriesTests.ConnectionStringActual);

            Assert.IsFalse(reader.Read());
        }
    }
}