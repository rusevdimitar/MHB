using MHB.BL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;

namespace MHB.UnitTests
{
    [TestClass]
    public class ProductUnitTest
    {
        private const string ConnectionString = @"Data Source = localhost; Initial Catalog = Test01Db.dev; User ID =sa; Password = Mitko_123; MultipleActiveResultSets=True;Connect Timeout=90;";
        private const string ConnectionStringActual = "Data Source = localhost; Initial Catalog = Test01Db; User ID =sa; Password = Mitko_123; MultipleActiveResultSets=True;Connect Timeout=90;";

        [TestMethod]
        public void ChangeProductCategory()
        {
            ExpenditureManager expenseManager = new ExpenditureManager(ConnectionString, 1, 3, 2015, Enums.Language.Bulgarian);

            IEnumerable<ExpenditureDetail> details = Enumerable.Empty<ExpenditureDetail>();

            Product product = null;

            const int testProductID = 689; // banana
            const int newCategoryID = 644; // fruits

            product = new Product(testProductID, expenseManager.UserID, expenseManager.ConnectionString);
            int oldCategoryID = product.CategoryID;

            details = expenseManager.GetExpenditureDetailsForProduct(testProductID);
            Assert.IsTrue(details.All(d => d.ProductID == testProductID)); // Load all details have the same product id - GetExpenditureDetailsForProduct() works!
            Assert.IsTrue(details.All(d => d.CategoryID == product.CategoryID)); // All details have same CategoryID as the product.CategoryID - we have consistent data!

            product.CategoryID = newCategoryID; // Change the product's category
            expenseManager.UpdateProduct(product); // Update product with new category

            details = expenseManager.GetExpenditureDetailsForProduct(testProductID);
            product = new Product(testProductID, expenseManager.UserID, expenseManager.ConnectionString);
            Assert.IsTrue(product.CategoryID == newCategoryID); // Check if the product is updated to the new CategoryID
            Assert.IsTrue(details.All(d => d.CategoryID == newCategoryID)); // Load all details and check if they have the product's new CategoryID

            // Revert back to old CategoryID and confirm
            product = new Product(testProductID, expenseManager.UserID, expenseManager.ConnectionString);
            product.CategoryID = oldCategoryID;
            expenseManager.UpdateProduct(product);
            details = expenseManager.GetExpenditureDetailsForProduct(testProductID);
            Assert.IsTrue(details.All(d => d.CategoryID == oldCategoryID));
            Assert.IsTrue(product.CategoryID == oldCategoryID);
        }

        [TestMethod]
        public void ChangeProductDefaultMeasureType()
        {
            const decimal initialAmount = 0.215M;
            const Enums.MeasureType initialMeasureType = Enums.MeasureType.Weight;

            ExpenditureManager expenseManager = new ExpenditureManager(ConnectionString, 1, 3, 2015, Enums.Language.Bulgarian);

            IEnumerable<ExpenditureDetail> details = Enumerable.Empty<ExpenditureDetail>();

            Product product = null;

            const int testProductID = 689; // banana

            details = expenseManager.GetExpenditureDetailsForProduct(testProductID);

            Assert.IsTrue(details.All(d => d.InitialAmount == initialAmount && d.MeasureType == initialMeasureType));
            Assert.IsTrue(details.All(d => d.Amount == details.First().Amount)); // Ensure all details have same amount

            ExpenditureDetail originalDetail = details.First();

            // ---- Test changing volume
            product = new Product(testProductID, expenseManager.UserID, expenseManager.ConnectionString);

            const decimal volume = 2.2M;

            product.Volume = volume;
            product.Weight = 0;

            expenseManager.UpdateProduct(product);

            details = expenseManager.GetExpenditureDetailsForProduct(testProductID);
            product = new Product(testProductID, expenseManager.UserID, expenseManager.ConnectionString);

            Assert.IsTrue(details.All(d => d.MeasureType == MHB.BL.Enums.MeasureType.Volume));
            Assert.IsTrue(details.All(d => d.Amount == volume));
            Assert.IsTrue(product.Weight == 0);

            // ---- Test changing weight
            product = new Product(testProductID, expenseManager.UserID, expenseManager.ConnectionString);

            const decimal weight = 3.3M;

            product.Weight = weight;
            product.Volume = 0;

            expenseManager.UpdateProduct(product);

            details = expenseManager.GetExpenditureDetailsForProduct(testProductID);
            product = new Product(testProductID, expenseManager.UserID, expenseManager.ConnectionString);

            Assert.IsTrue(details.All(d => d.MeasureType == MHB.BL.Enums.MeasureType.Weight));
            Assert.IsTrue(details.All(d => d.Amount == weight));
            Assert.IsTrue(product.Volume == 0);

            // ---- Test revert to original product amount
            product = new Product(testProductID, expenseManager.UserID, expenseManager.ConnectionString);

            product.Weight = 0;

            expenseManager.UpdateProduct(product);

            details = expenseManager.GetExpenditureDetailsForProduct(testProductID);

            var ids = details.Select(x => x.ID);

            Assert.IsTrue(details.All(d => d.MeasureType == originalDetail.InitialMeasureType));
            Assert.IsTrue(details.All(d => d.Amount == originalDetail.InitialAmount));
            Assert.IsTrue(product.Volume == 0);
            Assert.IsTrue(product.Weight == 0);

            Assert.IsTrue(originalDetail.InitialAmount != volume && originalDetail.InitialAmount != weight);
        }

        [TestMethod]
        public void ChangeProductDefaultSupplier()
        {
            ExpenditureManager expenseManager = new ExpenditureManager(ConnectionString, 1, 3, 2015, Enums.Language.Bulgarian);

            const int testProductID = 689; // banana
            const int testNewSupplierID = 2;
            Product product = null;

            product = new Product(testProductID, expenseManager.UserID, expenseManager.ConnectionString);

            product.VendorID = testNewSupplierID;

            expenseManager.UpdateProduct(product);

            product = new Product(testProductID, expenseManager.UserID, expenseManager.ConnectionString);

            Assert.IsTrue(product.VendorID == testNewSupplierID);

            product.VendorID = Supplier.SUPPLIER_DEFAULT_ID;

            expenseManager.UpdateProduct(product);

            product = new Product(testProductID, expenseManager.UserID, expenseManager.ConnectionString);

            Assert.IsTrue(product.VendorID == Supplier.SUPPLIER_DEFAULT_ID);
        }

        [TestMethod]
        public void MergeProducts()
        {
            ExpenditureManager expenseManager = new ExpenditureManager(ConnectionString, 1, 3, 2015, Enums.Language.Bulgarian);

            Product oldProduct = new Product();

            oldProduct.Name = "Old Product 1 Name";
            oldProduct.Description = "Old Product 1 Description";
            oldProduct.KeyWords = new string[] { "p1k1", "p1k2", "p1k3", "p1k4" };
            oldProduct.ListPrice = 17.82M;
            oldProduct.StandardCost = 82.17M;
            oldProduct.Volume = 0;
            oldProduct.Weight = 0;
            oldProduct.UserID = 1;
            oldProduct.ConnectionString = expenseManager.ConnectionString;
            oldProduct.VendorID = Supplier.SUPPLIER_DEFAULT_ID;
            oldProduct.CategoryID = Category.CATEGORY_DEFAULT_ID;

            int oldProductID = expenseManager.AddProduct(oldProduct); // add a new product to be merged

            Product newProduct = new Product();

            newProduct.Name = "New Product 2 Name";
            newProduct.Description = "New Product 2 Description";
            newProduct.KeyWords = new string[] { "p2k1", "p2k2", "p2k3", "p2k4" };
            newProduct.ListPrice = 16.82M;
            newProduct.StandardCost = 82.16M;
            newProduct.Volume = 0;
            newProduct.Weight = 0;
            newProduct.UserID = 1;
            newProduct.ConnectionString = expenseManager.ConnectionString;
            newProduct.VendorID = Supplier.SUPPLIER_DEFAULT_ID;
            newProduct.CategoryID = Category.CATEGORY_DEFAULT_ID;

            int newProductID = expenseManager.AddProduct(newProduct); // add a product to merge to

            bool f;
            string qry = "";

            expenseManager.AddNewParentExpense("10", false, false, "2015-03-17", "Test merge product entry", "Test", oldProductID, out f, out qry); // add a new expenditure with the old product

            var queryArgs = expenseManager.GetExpressionQueryArgs();
            queryArgs.Add("ProductID", oldProductID);
            queryArgs.Add("IsDeleted", 0);

            Expression<Func<Expenditure, bool>> queryWhereClause = exp =>
                   exp.ProductID == ExpressionQueryArgs.Parameter<int>("ProductID")
                && exp.IsDeleted == ExpressionQueryArgs.Parameter<bool>("IsDeleted");

            var newExpenses = expenseManager.GetUserExpenditures(queryWhereClause, queryArgs); // check if expenditure is added

            Assert.IsTrue(newExpenses.Count() == 1);

            ExpenditureDetail newDetail = new ExpenditureDetail(expenseManager);
            newDetail.ExpenditureID = newExpenses.First().ID;
            newDetail.ProductID = oldProductID;
            newDetail.Amount = 1;
            newDetail.Add(); // add new detail with the old product

            Product.Merge(newProductID, oldProductID, 1, expenseManager.ConnectionString); // merge products

            queryArgs["ProductID"] = newProductID;

            newExpenses = expenseManager.GetUserExpenditures(queryWhereClause, queryArgs);
            Assert.IsTrue(newExpenses.Count() == 1);
            var newDetails = expenseManager.GetExpenditureDetails(newExpenses.First(), new System.Data.SqlClient.SqlConnection(expenseManager.ConnectionString));
            Assert.IsTrue(newDetails.Count() == 1);

            Assert.AreEqual(newExpenses.First().ProductID, newProductID);
            Assert.AreEqual(newDetails.First().ProductID, newProductID);

            var checkIfOldProductExists = new Product(oldProductID, expenseManager.UserID, expenseManager.ConnectionString);

            Assert.IsTrue(string.IsNullOrEmpty(checkIfOldProductExists.Name)); // confirm merged product no longer exists; (object is never null since we load it by using the ctor)

            Product.Delete(newProductID, expenseManager.ConnectionString, expenseManager.UserID);

            expenseManager.DeleteParentExpense(newExpenses.Select(x => x.ID).ToArray(), out qry);
        }

        [TestMethod]
        public void CheckCategoryReportResultsAndCompareWithPlainSQLQuery()
        {
            int categoryID = 644;

            ExpenditureManager expenseManager = new ExpenditureManager(ConnectionStringActual, 1, DateTime.Now.Month, DateTime.Now.Year, Enums.Language.Bulgarian);

            Expression<Func<ExpenditureDetail, bool>> queryWhereClauseFunc = null;

            ExpressionQueryArgs args = expenseManager.GetExpressionQueryArgs();

            args.Add("IsDeleted", false);

            args.Add("CategoryID", categoryID);

            queryWhereClauseFunc = d => d.CategoryID == ExpressionQueryArgs.Parameter<int>("CategoryID") && d.IsDeleted == ExpressionQueryArgs.Parameter<bool>("IsDeleted");

            IEnumerable<ExpenditureDetail> productEntries = expenseManager.GetExpenditureDetails(queryWhereClauseFunc, args);

            // get the most frequently occuring measure type
            MHB.BL.Enums.MeasureType measureType = productEntries.GroupBy(d => d.MeasureType).OrderByDescending(d => d.Count()).Select(d => d.Key).First();

            ExpenditureDetail[] expenditureDetailsToRemove = productEntries.Where(d => d.MeasureType != measureType).ToArray();

            productEntries = productEntries.Except(expenditureDetailsToRemove);

            productEntries = productEntries.Where(d => d.MeasureType == measureType);

            IEnumerable<ExpenditureDetail> productEntriesThisMonth = productEntries.Where(d => d.DetailDate.Year == DateTime.Now.Year && d.DetailDate.Month == DateTime.Now.Month);
            IEnumerable<ExpenditureDetail> productEntriesThisYear = productEntries.Where(d => d.DetailDate.Year == DateTime.Now.Year);

            string qry = "sp_UnitTests_GetDetailsPerCategory @Year, @Month, @CategoryID";

            // Per month
            DataTable table = MHB.DAL.DataBaseConnector.GetDataTable(qry, ConnectionStringActual, new SqlParameter("Year", DateTime.Now.Year), new SqlParameter("Month", DateTime.Now.Month), new SqlParameter("CategoryID", categoryID));

            object sumObject = table.Compute("Sum(Amount)", "");

            if (productEntriesThisMonth.Any()) // 2015-07-01 - there are no records still as it is the begging of the month so ignore that check
                Assert.IsTrue(productEntriesThisMonth.Sum(d => d.Amount) == (decimal)sumObject);

            Assert.IsTrue(productEntriesThisMonth.Count() == table.Rows.Count);

            // Per year

            qry = "sp_UnitTests_GetDetailsPerCategory @Year, 0, @CategoryID";

            DataTable tablePerYear = MHB.DAL.DataBaseConnector.GetDataTable(qry, ConnectionStringActual, new SqlParameter("Year", DateTime.Now.Year), new SqlParameter("CategoryID", categoryID));

            object sumObjectPerYear = tablePerYear.Compute("Sum(Amount)", "");

            Assert.IsTrue(productEntriesThisYear.Sum(d => d.Amount) == (sumObjectPerYear == DBNull.Value ? 0 : (decimal)sumObjectPerYear));
            Assert.IsTrue(productEntriesThisYear.Count() == tablePerYear.Rows.Count);
        }
    }
}