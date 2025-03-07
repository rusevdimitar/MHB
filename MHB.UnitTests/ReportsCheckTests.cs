using MHB.BL;
using MHB.DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace MHB.UnitTests
{
    [TestClass]
    public class ReportsCheckTests
    {
        private const string ConnectionString = "Data Source = localhost; Initial Catalog = Test01Db.dev; User ID =sa; Password = Mitko_123; MultipleActiveResultSets=True;Connect Timeout=90;";
        private const string ConnectionStringActual = "Data Source = localhost; Initial Catalog = Test01Db; User ID =sa; Password = Mitko_123; MultipleActiveResultSets=True;Connect Timeout=90;";

        //private const string ConnectionStringActual = @"Data Source = localhost\sqlexpress; Initial Catalog = Test01Db; User ID =sa; Password = Password.123; MultipleActiveResultSets=True;Connect Timeout=90;";

        [TestMethod]
        public void CheckSumSpendPerDayTest()
        {
            string qry = "sp_UnitTests_CheckSumSpendPerDayTest @date, @userID";

            //DateTime date = DateTime.Now.Date;

            DateTime date = new DateTime(2016, 3, 24);

            decimal result = DataBaseConnector.GetSingleValue<decimal>(qry, ConnectionStringActual, new SqlParameter("date", date), new SqlParameter("userID", 1));

            ExpenditureManager expenseManager = new ExpenditureManager(ConnectionStringActual, 1, DateTime.Now.Month, DateTime.Now.Year, Enums.Language.Bulgarian);

            IEnumerable<Expenditure> mainDataSource = expenseManager.GetUserExpenditures(new DateTime[] { date });

            IEnumerable<KeyValuePair<DateTime, decimal>> sumsForDate = expenseManager.GetSumSpentPerDay(mainDataSource);

            if (sumsForDate.Any() && result != 0)
            {
                decimal sumForDate = sumsForDate.First(s => s.Key.Date == date.Date).Value;

                Assert.IsTrue(sumForDate == result);
            }
        }
    }
}