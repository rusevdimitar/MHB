using MHB.BL;
using MHB.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.ServiceModel;

namespace MHB.API
{
    [ServiceContract]
    public interface IMhbApiService
    {
        [OperationContract]
        int DeleteParentExpenditure(string userName, string password, string key, int[] expenditureIDs, out string executedQry);

        [OperationContract]
        int DeleteChildExpenditures(string userName, string password, string key, int[] expenditureIDs, out string executedQry);

        [OperationContract]
        bool AddParentExpenditure(string userName, string password, string key, int currentYear, int currentMonth, string expectedValue, bool reccuringEveryMonthOfTheYear, bool reccuringForFollowingMonthsOnly, string dueDate, string name, string description, int productID, out bool februaryException, out string executedQry);

        [OperationContract]
        IEnumerable<Expenditure> SearchUserExpenditures(string userName, string password, string key, int currentYear, int currentMonth, Enums.SearchOptions searchOption, bool searchByYearToo, string year, string searchText, string sum, string comarisonOperator, bool searchByCategoryToo, string category, Enums.SortOptions sortOption, Enums.SortDirection sortDirection);

        [OperationContract]
        IEnumerable<ExpenditureDetail> GetExpenditureDetails(int parentID, string userName, string password, string key);

        [OperationContract]
        bool UpdateParentExpenses(string userName, string password, string key, List<Expenditure> expensesToUpdate);

        [OperationContract]
        decimal GetUsersAverageSumForCategory(string userName, string password, string key, int categoryID);

        [OperationContract]
        int CopyParentExpense(string userName, string password, string key, int parentExpenseID, out string qryToLog);

        [OperationContract]
        int DeleteAttachment(string userName, string password, string key, int parentExpenseID, out string qryToLog);

        [OperationContract]
        Expenditure GetMaximumExpenditureForCategory(string userName, string password, string key, int categoryID);

        [OperationContract]
        IEnumerable<Income> GetUserIncome(string userName, string password, string key, int currentYear, int currentMonth);

        [OperationContract]
        bool DuplicateExpenditures(string userName, string password, string key, int destinationMonth, int destinationYear, int currentMonth, int currentYear, bool deleteExistingData, bool copyFlaggedOnly, bool markUnpaid, bool zeroActualSum, out string qryToLog);

        [OperationContract]
        ExpensesProMonth GetYearlyExpensesProMonth(string userName, string password, string key, int currentYear);

        [OperationContract]
        ExpensesProMonth GetYearlyBudgetsProMonth(string userName, string password, string key, int currentYear);

        [OperationContract]
        ExpensesProMonth GetYearlySavingsProMonth(string userName, string password, string key, int currentYear);

        [OperationContract]
        IEnumerable<Expenditure> GetUserExpenditures(string mainTableName, string detailsTableName, string userName, string password, string key, int month, int year, bool flaggedOnly, bool loadDetails, bool hidePaid, int userIDToLookUp);

        [OperationContract]
        IEnumerable<Expenditure> GetExpenditures(string userName, string password, string key, int month, int year, bool flaggedOnly, bool loadDetails, bool hidePaid);

        [OperationContract]
        IEnumerable<ActionLog> GetActionLogs(string userName, string password, string key, DateTime startDate);

        [OperationContract]
        IEnumerable<ExceptionLog> GetExceptionLogs(string userName, string password, string key, DateTime startDate);

        [OperationContract]
        bool BlockUser(string userName, string password, string key, int userToBlockID);

        [OperationContract]
        void BanIP(string userName, string password, string key, string ipAddressV4, string ipAddressV6, int userToBlockID);

        [OperationContract]
        object GetSingleValue(string userName, string password, string key, string qry, string connectionString, params SqlParameter[] parameters);

        [OperationContract]
        int ExecuteQuery(string userName, string password, string key, string qry, params SqlParameter[] parameters);

        [OperationContract]
        IDataReader GetDataReader(string userName, string password, string key, string qry, params SqlParameter[] parameters);

        [OperationContract]
        DataTable GetDataTable(string userName, string password, string key, string qry, params SqlParameter[] parameters);
    }
}