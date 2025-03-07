using MHB.BL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MHB.UnitTests
{
    [TestClass]
    public class ExpendituresCRUDOperationsTests
    {
        private const string ConnectionString = "Data Source = localhost; Initial Catalog = Test01Db.dev; User ID =sa; Password = Mitko_123; MultipleActiveResultSets=True;Connect Timeout=90;";

        private readonly ExpenditureManager _expenseManager = null;

        public ExpendituresCRUDOperationsTests()
        {
            this._expenseManager = new ExpenditureManager(ConnectionString, 1, 3, 2015, Enums.Language.Bulgarian);
        }

        [TestMethod]
        public void AddNewParentExpenditure()
        {
            const string expectedValue = "7";
            const string dueDate = "2015-03-17";
            const string name = "Test Parent Expenditure Name";
            const string description = "Test Parent Expenditure Description";

            bool februaryException = false;
            string qry = string.Empty;

            var queryArgs = this._expenseManager.GetExpressionQueryArgs();

            queryArgs.Add("FieldName", name);
            queryArgs.Add("FieldDescription", description);
            queryArgs.Add("ExpectedValue", expectedValue);
            queryArgs.Add("IsDeleted", false);

            Expression<Func<Expenditure, bool>> queryWhereClause = exp =>
                   exp.FieldName == ExpressionQueryArgs.Parameter<string>("FieldName")
                && exp.FieldDescription == ExpressionQueryArgs.Parameter<string>("FieldDescription")
                && exp.FieldExpectedValue == ExpressionQueryArgs.Parameter<decimal>("ExpectedValue")
                && exp.IsDeleted == ExpressionQueryArgs.Parameter<bool>("IsDeleted");

            var existingTestExpenditures = this._expenseManager.GetUserExpenditures(queryWhereClause, queryArgs);

            this._expenseManager.DeleteParentExpense(existingTestExpenditures.Select(x => x.ID).ToArray(), out qry);

            this._expenseManager.AddNewParentExpense(expectedValue, false, false, dueDate, name, description, Product.PRODUCT_DEFAULT_ID, out februaryException, out qry);

            var newTestExpenditures = this._expenseManager.GetUserExpenditures(queryWhereClause, queryArgs);

            Assert.IsTrue(newTestExpenditures.Count() == 1);
            Assert.AreEqual(newTestExpenditures.First().FieldName, name);
            Assert.AreEqual(newTestExpenditures.First().FieldDescription, description);
            Assert.AreEqual(newTestExpenditures.First().FieldExpectedValue, decimal.Parse(expectedValue));
            Assert.AreEqual(newTestExpenditures.First().DueDate, DateTime.Parse(dueDate));
        }

        [TestMethod]
        public void UpdateParentExpenditure()
        {
            const decimal updatedPreviousValue = 44.42M;
            const decimal updatedExpectedValue = 77.72M;
            const decimal updatedActualValue = 88.82M;
            const decimal updatedOldValue = 99.92M;
            const decimal updatedInitialValue = 11.12M;
            DateTime updatedDueDate = new DateTime(2015, 03, 7);
            const string updatedName = "Updated Test Parent Expenditure Name";
            const string updatedDescription = "Updated Test Parent Expenditure Description";
            const int updatedCategoryID = 7;

            const string expectedValue = "7";
            const string dueDate = "2015-03-17";
            const string name = "Test Parent Expenditure Name";
            const string description = "Test Parent Expenditure Description";

            bool februaryException = false;
            string qry = string.Empty;

            var queryArgs = this._expenseManager.GetExpressionQueryArgs();

            queryArgs.Add("FieldName", name);
            queryArgs.Add("FieldDescription", description);
            queryArgs.Add("ExpectedValue", expectedValue);
            queryArgs.Add("IsDeleted", false);

            Expression<Func<Expenditure, bool>> queryWhereClause = exp =>
                   exp.FieldName == ExpressionQueryArgs.Parameter<string>("FieldName")
                && exp.FieldDescription == ExpressionQueryArgs.Parameter<string>("FieldDescription")
                && exp.FieldExpectedValue == ExpressionQueryArgs.Parameter<decimal>("ExpectedValue")
                && exp.IsDeleted == ExpressionQueryArgs.Parameter<bool>("IsDeleted");

            var existingTestExpenditures = this._expenseManager.GetUserExpenditures(queryWhereClause, queryArgs);

            this._expenseManager.DeleteParentExpense(existingTestExpenditures.Select(x => x.ID).ToArray(), out qry);

            this._expenseManager.AddNewParentExpense(expectedValue, false, false, dueDate, name, description, Product.PRODUCT_DEFAULT_ID, out februaryException, out qry);

            var newTestExpenditures = this._expenseManager.GetUserExpenditures(queryWhereClause, queryArgs);

            Assert.IsTrue(newTestExpenditures.Count() == 1);

            Expenditure expenditureToTupdate = newTestExpenditures.First();

            expenditureToTupdate.FieldName = updatedName;
            expenditureToTupdate.FieldDescription = updatedDescription;
            expenditureToTupdate.DueDate = updatedDueDate;
            expenditureToTupdate.FieldValue = updatedActualValue;
            expenditureToTupdate.Flagged = true;
            expenditureToTupdate.IsPaid = true;
            expenditureToTupdate.IsShared = true;
            expenditureToTupdate.FieldOldValue = updatedOldValue;
            expenditureToTupdate.FieldExpectedValue = updatedExpectedValue;
            expenditureToTupdate.FieldInitialValue = updatedInitialValue;
            expenditureToTupdate.CategoryID = updatedCategoryID;

            this._expenseManager.UpdateParentExpenses(newTestExpenditures.ToList(), string.Empty);

            var updatedExpenditures = this._expenseManager.GetUserExpenditures(new int[] { expenditureToTupdate.ID });

            Assert.IsTrue(updatedExpenditures.Count() == 1);

            Expenditure updatedExpenditure = updatedExpenditures.First();

            Assert.AreEqual(expenditureToTupdate.FieldName, updatedExpenditure.FieldName);
            Assert.AreEqual(expenditureToTupdate.FieldDescription, updatedExpenditure.FieldDescription);
            Assert.AreEqual(expenditureToTupdate.DueDate, updatedExpenditure.DueDate);
            Assert.AreEqual(expenditureToTupdate.FieldValue, updatedExpenditure.FieldValue);
            Assert.AreEqual(expenditureToTupdate.FieldValue, updatedExpenditure.FieldExpectedValue);
            Assert.AreEqual(expenditureToTupdate.Flagged, updatedExpenditure.Flagged);
            Assert.AreEqual(expenditureToTupdate.IsPaid, updatedExpenditure.IsPaid);
            Assert.AreEqual(expenditureToTupdate.IsShared, updatedExpenditure.IsShared);
            Assert.AreEqual(expenditureToTupdate.FieldOldValue, updatedExpenditure.FieldOldValue);
            Assert.AreEqual(expenditureToTupdate.FieldValue, updatedExpenditure.FieldInitialValue);
            Assert.AreEqual(expenditureToTupdate.CategoryID, updatedExpenditure.CategoryID);

            this._expenseManager.DeleteParentExpense(new int[] { expenditureToTupdate.ID }, out qry);
        }

        [TestMethod]
        public void DuplicateMonthRecords()
        {
            ExpenditureManager expenseManager = new ExpenditureManager(ConnectionString, 1, 3, 2015, Enums.Language.Bulgarian);

            const int destinationMonth = 4;

            const int destinationYear = 2015;

            string qry = string.Empty;

            expenseManager.DuplicateExpenditures(destinationMonth, destinationYear, true, false, true, true, out qry);

            var queryArgs = new ExpressionQueryArgs();
            queryArgs.Add("Month", destinationMonth);
            queryArgs.Add("Year", destinationYear);
            queryArgs.Add("UserID", 1);
            queryArgs.Add("IsDeleted", 0);

            Expression<Func<Expenditure, bool>> queryWhereClause = exp =>
                   exp.Month == ExpressionQueryArgs.Parameter<int>("Month")
                && exp.Year == ExpressionQueryArgs.Parameter<int>("Year")
                && exp.UserID == ExpressionQueryArgs.Parameter<int>("UserID")
                && exp.IsDeleted == ExpressionQueryArgs.Parameter<bool>("IsDeleted");

            var oldExpenditures = expenseManager.GetUserExpenditures(queryWhereClause, queryArgs);

            expenseManager.DeleteParentExpenses(destinationYear, destinationMonth);

            // Check if existing records are deleted
            var confirmNoExpenditures = expenseManager.GetUserExpenditures(queryWhereClause, queryArgs);
            Assert.IsTrue(confirmNoExpenditures == null || confirmNoExpenditures.Count() == 0);

            expenseManager.DuplicateExpenditures(destinationMonth, destinationYear, true, false, true, true, out qry);

            IEnumerable<Expenditure> newExpenditures = expenseManager.GetUserExpenditures(queryWhereClause, queryArgs);

            Assert.IsTrue(oldExpenditures.Count() == newExpenditures.Count());

            // Copied categories are the same as original
            CollectionAssert.AreEqual(oldExpenditures.Select(exp => exp.CategoryID).ToArray(), newExpenditures.Select(exp => exp.CategoryID).ToArray());

            // Copied names are the same as original
            CollectionAssert.AreEqual(oldExpenditures.Select(exp => exp.FieldName).ToArray(), newExpenditures.Select(exp => exp.FieldName).ToArray());

            // Copied initial values are the same as original
            CollectionAssert.AreEqual(oldExpenditures.Select(exp => exp.FieldInitialValue).ToArray(), newExpenditures.Select(exp => exp.FieldInitialValue).ToArray());

            // Copied expected values are the same as original
            CollectionAssert.AreEqual(oldExpenditures.Select(exp => exp.FieldExpectedValue).ToArray(), newExpenditures.Select(exp => exp.FieldExpectedValue).ToArray());

            // Copied actual values are the same as original
            CollectionAssert.AreEqual(oldExpenditures.Select(exp => exp.FieldValue).ToArray(), newExpenditures.Select(exp => exp.FieldValue).ToArray());

            // Copied old undo values are the same as original
            CollectionAssert.AreEqual(oldExpenditures.Select(exp => exp.FieldOldValue).ToArray(), newExpenditures.Select(exp => exp.FieldOldValue).ToArray());

            // Copied previous undo values are the same as original
            CollectionAssert.AreEqual(oldExpenditures.Select(exp => exp.FieldPreviousValue).ToArray(), newExpenditures.Select(exp => exp.FieldPreviousValue).ToArray());
        }
    }
}