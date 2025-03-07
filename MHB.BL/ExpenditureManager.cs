using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MHB.BL
{
    public class ExpenditureManager : ExpenditureManagerBase
    {
        public ExpenditureManager(string connectionString, int userID, int month, int year, MHB.BL.Enums.Language language)
            : base(connectionString, userID, month, year, language)
        {
        }

        public ExpressionQueryArgs GetExpressionQueryArgs()
        {
            return new ExpressionQueryArgs()
            {
                {"MainTableName", base.MainTableName},
                {"DetailsTableName", base.DetailsTableName},
                {"UserID", base.UserID},
                {"Month", base.Month},
                {"Year", base.Year}
            };
        }

        public ProductInfo[] GetMostFrequentlyPurchasedItems(int month, int year, int recordsReturnedCount)
        {
            List<ProductInfo> productItems = new List<ProductInfo>();

            IDataReader reader = SQLHelper.GetFrequentlyPurchasedItems(recordsReturnedCount, month, year, this.UserID, this.ConnectionString);

            while (reader.Read())
            {
                ProductInfo item = new ProductInfo();

                if (!reader.IsDBNull(reader.GetOrdinal("TotalSumAmount")))
                    item.Amount = (decimal)reader["TotalSumAmount"];

                if (!reader.IsDBNull(reader.GetOrdinal("Count")))
                    item.Total = (int)reader["Count"];

                if (!reader.IsDBNull(reader.GetOrdinal("ProductID")))
                    item.ID = (int)reader["ProductID"];

                if (!reader.IsDBNull(reader.GetOrdinal("DetailName")))
                    item.Name = reader["DetailName"].ToString();

                if (!reader.IsDBNull(reader.GetOrdinal("SupplierName")))
                    item.SupplierName = reader["SupplierName"].ToString();

                productItems.Add(item);
            }

            return productItems.ToArray();
        }

        public IEnumerable<KeyValuePair<DateTime, decimal>> GetSumSpentPerDay(IEnumerable<Expenditure> mainTableDataSource)
        {
            IEnumerable<KeyValuePair<DateTime, decimal>> parentDates = mainTableDataSource.Where(exp => exp.HasDetails == false).OrderBy(exp => exp.DateRecordUpdated)
               .Select(exp => exp.DateRecordUpdated.Date).Distinct()
               .Select(d => this.GetParentExpendituresForDate(d, mainTableDataSource));

            IEnumerable<ExpenditureDetail> details = mainTableDataSource.SelectMany(exp => exp.Details);

            IEnumerable<KeyValuePair<DateTime, decimal>> detailDates = details.Select(det => det.DetailDate.Date).Distinct()
                .Select(d => new KeyValuePair<DateTime, decimal>(d, details.Where(det => det.DetailDate.Date == d).Sum(det => det.DetailValue)));

            IEnumerable<KeyValuePair<DateTime, decimal>> dataSource = parentDates.Union(detailDates).OrderBy(kv => kv.Key);

            var duplicates = dataSource.GroupBy(kv => kv.Key).Where(g => g.Skip(1).Any()).SelectMany(g => g).GroupBy(kv => kv.Key);

            IEnumerable<KeyValuePair<DateTime, decimal>> mergedDuplicates = duplicates.Select(group => new KeyValuePair<DateTime, decimal>(group.Key, group.Sum(g => g.Value)));

            DateTime[] mergedDuplicatesDates = mergedDuplicates.Select(mkv => mkv.Key).ToArray();

            var dataSourceWithoutDuplicates = dataSource.Where(kv => !mergedDuplicatesDates.Contains(kv.Key));

            var result = dataSourceWithoutDuplicates.Union(mergedDuplicates).OrderBy(kv => kv.Key);

            return result;
        }

        private KeyValuePair<DateTime, decimal> GetParentExpendituresForDate(DateTime date, IEnumerable<Expenditure> mainTableDataSource)
        {
            KeyValuePair<DateTime, decimal> kv = new KeyValuePair<DateTime, decimal>();

            IEnumerable<Expenditure> recordsForDate = mainTableDataSource.Where(exp => exp.HasDetails == false && exp.DateRecordUpdated.Date == date.Date);

            if (recordsForDate.Any())
            {
                decimal totalSumChanges = 0, sumTransactions = 0, sumNoTransactions = 0;

                IEnumerable<Expenditure> recordsWithTransactions = recordsForDate.Where(exp => exp.Transactions.Any());

                if (recordsWithTransactions.Any())
                {
                    IEnumerable<Transaction> recordsTransactionsForDate = recordsWithTransactions.SelectMany(exp => exp.Transactions).Where(t => t.DateModified.Date == date.Date);
                    sumTransactions = recordsTransactionsForDate.Sum(t => t.NewValue - t.OldValue);
                }

                IEnumerable<Expenditure> recordsWithoutTransactions = recordsForDate.Except(recordsWithTransactions);

                if (recordsWithoutTransactions.Any())
                {
                    sumNoTransactions = recordsWithoutTransactions.Sum(exp => exp.FieldValue);
                }

                totalSumChanges = sumTransactions + sumNoTransactions;

                kv = new KeyValuePair<DateTime, decimal>(date, totalSumChanges);
            }

            return kv;
        }
    }
}