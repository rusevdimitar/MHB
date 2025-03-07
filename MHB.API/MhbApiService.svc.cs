using MHB.BL;
using MHB.Logging;
using MHB.UserManager;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Web;

namespace MHB.API
{
    public class MhbApiService : IMhbApiService
    {
        private readonly ExpenditureManager _expenditureManager;
        private string _connectionString;
        private string _ipAddress;

        public MhbApiService()
        {
            this._connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            //this._expenditureManager = new ExpenditureManager(this._connectionString, this.);

            OperationContext context = OperationContext.Current;
            MessageProperties prop = context.IncomingMessageProperties;
            RemoteEndpointMessageProperty endpoint = prop[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;

            this._ipAddress = endpoint.Address;
        }

        public IEnumerable<Expenditure> GetExpenditures(string userName, string password, string key, int month, int year, bool flaggedOnly, bool loadDetails, bool hidePaid)
        {
            IEnumerable<Expenditure> expenses = Enumerable.Empty<Expenditure>();

            int userID = UserManager.User.GetUserID(userName, password, this._connectionString);

            MHB.Logging.Logger.LogAction(Logger.HistoryAction.API_GetExpenditures, userID, this._connectionString, this._ipAddress);

            if (userID != 0)
            {
                APIKey apiKey = UserManager.User.GetAPIKey(userID, this._connectionString);

                if (apiKey.Key == key)
                {
                    this._expenditureManager.MainTableName = ExpenditureManager.GetUserMainTableNames(userID).Item1;
                    this._expenditureManager.DetailsTableName = ExpenditureManager.GetUserMainTableNames(userID).Item2;
                    this._expenditureManager.UserID = userID;
                    this._expenditureManager.Year = year;
                    this._expenditureManager.Month = month;

                    expenses = this._expenditureManager.GetUserExpenditures(flaggedOnly, loadDetails, hidePaid);
                }
            }

            return expenses;
        }

        public int DeleteParentExpenditure(string userName, string password, string key, int[] expenditureIDs, out string executedQry)
        {
            executedQry = string.Empty;
            int result = -1;
            int userID = UserManager.User.GetUserID(userName, password, this._connectionString);

            MHB.Logging.Logger.LogAction(Logger.HistoryAction.API_DeleteParentExpenditure, userID, this._connectionString, this._ipAddress);

            if (userID != 0)
            {
                APIKey apiKey = UserManager.User.GetAPIKey(userID, this._connectionString);

                if (apiKey.Key == key)
                {
                    this._expenditureManager.MainTableName = ExpenditureManager.GetUserMainTableNames(userID).Item1;
                    this._expenditureManager.DetailsTableName = ExpenditureManager.GetUserMainTableNames(userID).Item2;
                    this._expenditureManager.UserID = userID;

                    result = this._expenditureManager.DeleteParentExpense(expenditureIDs, out executedQry);
                }
            }

            return result;
        }

        public int DeleteChildExpenditures(string userName, string password, string key, int[] expenditureIDs, out string executedQry)
        {
            executedQry = string.Empty;
            int result = -1;
            int userID = UserManager.User.GetUserID(userName, password, this._connectionString);

            MHB.Logging.Logger.LogAction(Logger.HistoryAction.API_DeleteChildExpenditures, userID, this._connectionString, this._ipAddress);

            if (userID != 0)
            {
                APIKey apiKey = UserManager.User.GetAPIKey(userID, this._connectionString);

                if (apiKey.Key == key)
                {
                    this._expenditureManager.MainTableName = ExpenditureManager.GetUserMainTableNames(userID).Item1;
                    this._expenditureManager.DetailsTableName = ExpenditureManager.GetUserMainTableNames(userID).Item2;
                    this._expenditureManager.UserID = userID;

                    result = this._expenditureManager.DeleteChildExpenses(expenditureIDs.Select(id => new ExpenditureDetail() { ID = id }).ToArray(), out executedQry);
                }
            }

            return result;
        }

        public bool AddParentExpenditure(string userName, string password, string key, int currentYear, int currentMonth, string expectedValue, bool reccuringEveryMonthOfTheYear, bool reccuringForFollowingMonthsOnly, string dueDate, string name, string description, int productID, out bool februaryException, out string executedQry)
        {
            februaryException = false;
            executedQry = string.Empty;
            bool result = false;
            int userID = UserManager.User.GetUserID(userName, password, this._connectionString);

            MHB.Logging.Logger.LogAction(Logger.HistoryAction.API_AddParentExpenditure, userID, this._connectionString, this._ipAddress);

            if (userID != 0)
            {
                APIKey apiKey = UserManager.User.GetAPIKey(userID, this._connectionString);

                if (apiKey.Key == key)
                {
                    this._expenditureManager.MainTableName = ExpenditureManager.GetUserMainTableNames(userID).Item1;
                    this._expenditureManager.DetailsTableName = ExpenditureManager.GetUserMainTableNames(userID).Item2;
                    this._expenditureManager.UserID = userID;
                    this._expenditureManager.Year = currentYear;
                    this._expenditureManager.Month = currentMonth;

                    result = this._expenditureManager.AddNewParentExpense(expectedValue, reccuringEveryMonthOfTheYear, reccuringForFollowingMonthsOnly, dueDate, name, description, productID, out februaryException, out executedQry);
                }
            }

            return result;
        }

        public IEnumerable<Expenditure> SearchUserExpenditures(string userName, string password, string key, int currentYear, int currentMonth, Enums.SearchOptions searchOption, bool searchByYearToo, string year, string searchText, string sum, string comarisonOperator, bool searchByCategoryToo, string category, Enums.SortOptions sortOption, Enums.SortDirection sortDirection)
        {
            IEnumerable<Expenditure> result = Enumerable.Empty<Expenditure>();

            int userID = UserManager.User.GetUserID(userName, password, this._connectionString);

            MHB.Logging.Logger.LogAction(Logger.HistoryAction.API_SearchUserExpenditures, userID, this._connectionString, this._ipAddress);

            if (userID != 0)
            {
                APIKey apiKey = UserManager.User.GetAPIKey(userID, this._connectionString);

                if (apiKey.Key == key)
                {
                    this._expenditureManager.MainTableName = ExpenditureManager.GetUserMainTableNames(userID).Item1;
                    this._expenditureManager.DetailsTableName = ExpenditureManager.GetUserMainTableNames(userID).Item2;
                    this._expenditureManager.UserID = userID;
                    this._expenditureManager.Year = currentYear;
                    this._expenditureManager.Month = currentMonth;

                    result = this._expenditureManager.SearchUserExpenditures(searchOption, searchByYearToo, year, searchText, sum, comarisonOperator, searchByCategoryToo, category, sortOption, sortDirection);
                }
            }

            return result;
        }

        public IEnumerable<ExpenditureDetail> GetExpenditureDetails(int parentID, string userName, string password, string key)
        {
            IEnumerable<ExpenditureDetail> result = Enumerable.Empty<ExpenditureDetail>();

            int userID = UserManager.User.GetUserID(userName, password, this._connectionString);

            MHB.Logging.Logger.LogAction(Logger.HistoryAction.API_GetExpenditureDetails, userID, this._connectionString, this._ipAddress);

            if (userID != 0)
            {
                APIKey apiKey = UserManager.User.GetAPIKey(userID, this._connectionString);

                if (apiKey.Key == key)
                {
                    this._expenditureManager.MainTableName = ExpenditureManager.GetUserMainTableNames(userID).Item1;
                    this._expenditureManager.DetailsTableName = ExpenditureManager.GetUserMainTableNames(userID).Item2;
                    this._expenditureManager.UserID = userID;

                    this._expenditureManager.GetExpenditureDetails(new Expenditure() { ID = parentID }, new SqlConnection(this._connectionString));
                }
            }

            return result;
        }

        public bool UpdateParentExpenses(string userName, string password, string key, List<Expenditure> expensesToUpdate)
        {
            bool result = false;
            int userID = UserManager.User.GetUserID(userName, password, this._connectionString);

            MHB.Logging.Logger.LogAction(Logger.HistoryAction.API_UpdateParentExpenses, userID, this._connectionString, this._ipAddress);

            if (userID != 0)
            {
                APIKey apiKey = UserManager.User.GetAPIKey(userID, this._connectionString);

                if (apiKey.Key == key)
                {
                    this._expenditureManager.MainTableName = ExpenditureManager.GetUserMainTableNames(userID).Item1;
                    this._expenditureManager.DetailsTableName = ExpenditureManager.GetUserMainTableNames(userID).Item2;
                    this._expenditureManager.UserID = userID;

                    result = this._expenditureManager.UpdateParentExpenses(expensesToUpdate, string.Empty);
                }
            }

            return result;
        }

        public decimal GetUsersAverageSumForCategory(string userName, string password, string key, int categoryID)
        {
            decimal result = 0;
            int userID = UserManager.User.GetUserID(userName, password, this._connectionString);

            MHB.Logging.Logger.LogAction(Logger.HistoryAction.API_GetUsersAverageSumForCategory, userID, this._connectionString, this._ipAddress);

            if (userID != 0)
            {
                APIKey apiKey = UserManager.User.GetAPIKey(userID, this._connectionString);

                if (apiKey.Key == key)
                {
                    this._expenditureManager.UserID = userID;

                    result = this._expenditureManager.GetUsersAverageSumForCategory(userID, categoryID);
                }
            }

            return result;
        }

        public int CopyParentExpense(string userName, string password, string key, int parentExpenseID, out string qryToLog)
        {
            qryToLog = string.Empty;
            int result = 0;
            int userID = UserManager.User.GetUserID(userName, password, this._connectionString);

            MHB.Logging.Logger.LogAction(Logger.HistoryAction.API_CopyParentExpense, userID, this._connectionString, this._ipAddress);

            if (userID != 0)
            {
                APIKey apiKey = UserManager.User.GetAPIKey(userID, this._connectionString);

                if (apiKey.Key == key)
                    result = this._expenditureManager.CopyParentExpense(parentExpenseID, out qryToLog);
            }

            return result;
        }

        public int DeleteAttachment(string userName, string password, string key, int parentExpenseID, out string qryToLog)
        {
            qryToLog = string.Empty;
            int result = 0;
            int userID = UserManager.User.GetUserID(userName, password, this._connectionString);

            MHB.Logging.Logger.LogAction(Logger.HistoryAction.API_DeleteAttachment, userID, this._connectionString, this._ipAddress);

            if (userID != 0)
            {
                APIKey apiKey = UserManager.User.GetAPIKey(userID, this._connectionString);

                if (apiKey.Key == key)
                {
                    this._expenditureManager.MainTableName = ExpenditureManager.GetUserMainTableNames(userID).Item1;
                    result = this._expenditureManager.DeleteAttachment(parentExpenseID, out qryToLog);
                }
            }

            return result;
        }

        public Expenditure GetMaximumExpenditureForCategory(string userName, string password, string key, int categoryID)
        {
            Expenditure result = new Expenditure();
            int userID = UserManager.User.GetUserID(userName, password, this._connectionString);

            MHB.Logging.Logger.LogAction(Logger.HistoryAction.API_GetMaximumExpenditureForCategory, userID, this._connectionString, this._ipAddress);

            if (userID != 0)
            {
                APIKey apiKey = UserManager.User.GetAPIKey(userID, this._connectionString);

                if (apiKey.Key == key)
                {
                    this._expenditureManager.MainTableName = ExpenditureManager.GetUserMainTableNames(userID).Item1;
                    this._expenditureManager.UserID = userID;

                    result = this._expenditureManager.GetMaximumExpenditureForCategory(categoryID);
                }
            }

            return result;
        }

        public IEnumerable<Income> GetUserIncome(string userName, string password, string key, int currentYear, int currentMonth)
        {
            IEnumerable<Income> result = Enumerable.Empty<Income>();

            int userID = UserManager.User.GetUserID(userName, password, this._connectionString);

            MHB.Logging.Logger.LogAction(Logger.HistoryAction.API_GetUserIncome, userID, this._connectionString, this._ipAddress);

            if (userID != 0)
            {
                APIKey apiKey = UserManager.User.GetAPIKey(userID, this._connectionString);

                if (apiKey.Key == key)
                {
                    this._expenditureManager.UserID = userID;
                    this._expenditureManager.Year = currentYear;
                    this._expenditureManager.Month = currentMonth;

                    result = this._expenditureManager.GetUserIncome();
                }
            }

            return result;
        }

        public bool DuplicateExpenditures(string userName, string password, string key, int destinationMonth, int destinationYear, int currentMonth, int currentYear, bool deleteExistingData, bool copyFlaggedOnly, bool markUnpaid, bool zeroActualSum, out string qryToLog)
        {
            qryToLog = string.Empty;
            bool result = false;
            int userID = UserManager.User.GetUserID(userName, password, this._connectionString);

            MHB.Logging.Logger.LogAction(Logger.HistoryAction.API_DuplicateExpenditures, userID, this._connectionString, this._ipAddress);

            if (userID != 0)
            {
                APIKey apiKey = UserManager.User.GetAPIKey(userID, this._connectionString);

                if (apiKey.Key == key)
                {
                    this._expenditureManager.MainTableName = ExpenditureManager.GetUserMainTableNames(userID).Item1;
                    this._expenditureManager.DetailsTableName = ExpenditureManager.GetUserMainTableNames(userID).Item2;
                    this._expenditureManager.UserID = userID;
                    this._expenditureManager.Year = currentYear;
                    this._expenditureManager.Month = currentMonth;

                    result = this._expenditureManager.DuplicateExpenditures(destinationMonth, destinationYear, deleteExistingData, copyFlaggedOnly, markUnpaid, zeroActualSum, out qryToLog);
                }
            }

            return result;
        }

        public ExpensesProMonth GetYearlyExpensesProMonth(string userName, string password, string key, int currentYear)
        {
            ExpensesProMonth result = new ExpensesProMonth();
            int userID = UserManager.User.GetUserID(userName, password, this._connectionString);

            MHB.Logging.Logger.LogAction(Logger.HistoryAction.API_GetYearlyExpensesProMonth, userID, this._connectionString, this._ipAddress);

            if (userID != 0)
            {
                APIKey apiKey = UserManager.User.GetAPIKey(userID, this._connectionString);

                if (apiKey.Key == key)
                {
                    this._expenditureManager.MainTableName = ExpenditureManager.GetUserMainTableNames(userID).Item1;
                    this._expenditureManager.DetailsTableName = ExpenditureManager.GetUserMainTableNames(userID).Item2;
                    this._expenditureManager.UserID = userID;
                    this._expenditureManager.Year = currentYear;

                    result = this._expenditureManager.GetYearlyExpensesProMonth();
                }
            }

            return result;
        }

        public ExpensesProMonth GetYearlyBudgetsProMonth(string userName, string password, string key, int currentYear)
        {
            ExpensesProMonth result = new ExpensesProMonth();
            int userID = UserManager.User.GetUserID(userName, password, this._connectionString);

            MHB.Logging.Logger.LogAction(Logger.HistoryAction.API_GetYearlyBudgetsProMonth, userID, this._connectionString, this._ipAddress);

            if (userID != 0)
            {
                APIKey apiKey = UserManager.User.GetAPIKey(userID, this._connectionString);

                if (apiKey.Key == key)
                {
                    this._expenditureManager.MainTableName = ExpenditureManager.GetUserMainTableNames(userID).Item1;
                    this._expenditureManager.DetailsTableName = ExpenditureManager.GetUserMainTableNames(userID).Item2;
                    this._expenditureManager.UserID = userID;
                    this._expenditureManager.Year = currentYear;

                    result = this._expenditureManager.GetYearlyBudgetsProMonth();
                }
            }

            return result;
        }

        public ExpensesProMonth GetYearlySavingsProMonth(string userName, string password, string key, int currentYear)
        {
            ExpensesProMonth result = new ExpensesProMonth();
            int userID = UserManager.User.GetUserID(userName, password, this._connectionString);

            MHB.Logging.Logger.LogAction(Logger.HistoryAction.API_GetYearlySavingsProMonth, userID, this._connectionString, this._ipAddress);

            if (userID != 0)
            {
                APIKey apiKey = UserManager.User.GetAPIKey(userID, this._connectionString);

                if (apiKey.Key == key)
                {
                    this._expenditureManager.MainTableName = ExpenditureManager.GetUserMainTableNames(userID).Item1;
                    this._expenditureManager.DetailsTableName = ExpenditureManager.GetUserMainTableNames(userID).Item2;
                    this._expenditureManager.UserID = userID;
                    this._expenditureManager.Year = currentYear;

                    result = this._expenditureManager.GetYearlySavingsProMonth();
                }
            }

            return result;
        }

        // Admin
        public IEnumerable<Expenditure> GetUserExpenditures(string mainTableName, string detailsTableName, string userName, string password, string key, int month, int year, bool flaggedOnly, bool loadDetails, bool hidePaid, int userIDToLookUp)
        {
            IEnumerable<Expenditure> expenses = Enumerable.Empty<Expenditure>();

            int userID = UserManager.User.GetUserID(userName, password, this._connectionString);

            MHB.Logging.Logger.LogAction(Logger.HistoryAction.API_GetUserExpenditures, userID, this._connectionString, this._ipAddress);

            if (userID != 0)
            {
                APIKey apiKey = UserManager.User.GetAPIKey(userID, this._connectionString);

                if (apiKey.Key == key && apiKey.IsAdmin)
                {
                    this._expenditureManager.MainTableName = mainTableName;
                    this._expenditureManager.DetailsTableName = detailsTableName;
                    this._expenditureManager.UserID = userIDToLookUp;
                    this._expenditureManager.Year = year;
                    this._expenditureManager.Month = month;

                    expenses = this._expenditureManager.GetUserExpenditures(flaggedOnly, loadDetails, hidePaid);
                }
            }

            return expenses;
        }

        // Admin
        public IEnumerable<ActionLog> GetActionLogs(string userName, string password, string key, DateTime startDate)
        {
            IEnumerable<ActionLog> actionLogs = Enumerable.Empty<ActionLog>();

            SqlConnection connection = new SqlConnection(this._connectionString);

            int userID = UserManager.User.GetUserID(userName, password, connection);

            MHB.Logging.Logger.LogAction(Logger.HistoryAction.API_GetActionLogs, userID, null, this._ipAddress, connection);

            if (userID != 0)
            {
                APIKey apiKey = UserManager.User.GetAPIKey(userID, connection);

                if (apiKey.Key == key && apiKey.IsAdmin)
                {
                    ActionLog actionLog = new ActionLog(connection);

                    actionLogs = actionLog.LoadAll(startDate);
                }
            }

            if (connection.State != ConnectionState.Closed)
                connection.Close();

            if (connection != null)
                connection.Dispose();

            return actionLogs;
        }

        // Admin
        public IEnumerable<ExceptionLog> GetExceptionLogs(string userName, string password, string key, DateTime startDate)
        {
            IEnumerable<ExceptionLog> exceptionLogs = Enumerable.Empty<ExceptionLog>();

            SqlConnection connection = new SqlConnection(this._connectionString);

            int userID = UserManager.User.GetUserID(userName, password, connection);

            MHB.Logging.Logger.LogAction(Logger.HistoryAction.API_GetExceptionLogs, userID, null, this._ipAddress, connection);

            if (userID != 0)
            {
                APIKey apiKey = UserManager.User.GetAPIKey(userID, connection);

                if (apiKey.Key == key && apiKey.IsAdmin)
                {
                    ExceptionLog exception = new ExceptionLog(connection);

                    exceptionLogs = exception.LoadAll(startDate);
                }
            }

            if (connection.State != ConnectionState.Closed)
                connection.Close();

            if (connection != null)
                connection.Dispose();

            return exceptionLogs;
        }

        // Admin
        public bool BlockUser(string userName, string password, string key, int userToBlockID)
        {
            int userID = UserManager.User.GetUserID(userName, password, this._connectionString);

            MHB.Logging.Logger.LogAction(Logger.HistoryAction.API_BlockUser, userID, this._connectionString, this._ipAddress);

            if (userID != 0)
            {
                APIKey apiKey = UserManager.User.GetAPIKey(userID, this._connectionString);

                if (apiKey.Key == key && apiKey.IsAdmin)
                {
                    User usr = new User(new SqlConnection(this._connectionString), userToBlockID);

                    usr.Email = string.Format("deleted_{0}", usr.Email);

                    return usr.UpdateUser();
                }
            }

            return true;
        }

        // Admin
        public void BanIP(string userName, string password, string key, string ipAddressV4, string ipAddressV6, int userToBlockID)
        {
            int userID = UserManager.User.GetUserID(userName, password, this._connectionString);

            MHB.Logging.Logger.LogAction(Logger.HistoryAction.API_BanIP, userID, this._connectionString, this._ipAddress);

            if (userID != 0)
            {
                APIKey apiKey = UserManager.User.GetAPIKey(userID, this._connectionString);

                if (apiKey.Key == key && apiKey.IsAdmin)
                {
                    UserManager.User.AddToBlackList(userToBlockID, ipAddressV4, ipAddressV6, this._connectionString);
                }
            }
        }

        // Admin
        public object GetSingleValue(string userName, string password, string key, string qry, string connectionString, params SqlParameter[] parameters)
        {
            object result = new object();
            int userID = UserManager.User.GetUserID(userName, password, this._connectionString);

            MHB.Logging.Logger.LogAction(Logger.HistoryAction.API_GetSingleValue, userID, this._connectionString, this._ipAddress);

            if (userID != 0)
            {
                APIKey apiKey = UserManager.User.GetAPIKey(userID, this._connectionString);

                if (apiKey.Key == key && apiKey.IsAdmin)
                {
                    result = MHB.DAL.DataBaseConnector.GetSingleValue(qry, this._connectionString, parameters);
                }
            }

            return result;
        }

        // Admin
        public int ExecuteQuery(string userName, string password, string key, string qry, params SqlParameter[] parameters)
        {
            int result = 0;
            int userID = UserManager.User.GetUserID(userName, password, this._connectionString);

            MHB.Logging.Logger.LogAction(Logger.HistoryAction.API_ExecuteQuery, userID, this._connectionString, this._ipAddress);

            if (userID != 0)
            {
                APIKey apiKey = UserManager.User.GetAPIKey(userID, this._connectionString);

                if (apiKey.Key == key && apiKey.IsAdmin)
                {
                    result = MHB.DAL.DataBaseConnector.ExecuteQuery(qry, this._connectionString, parameters);
                }
            }

            return result;
        }

        // Admin
        public IDataReader GetDataReader(string userName, string password, string key, string qry, params SqlParameter[] parameters)
        {
            IDataReader result = null;
            int userID = UserManager.User.GetUserID(userName, password, this._connectionString);

            MHB.Logging.Logger.LogAction(Logger.HistoryAction.API_GetDataReader, userID, this._connectionString, this._ipAddress);

            if (userID != 0)
            {
                APIKey apiKey = UserManager.User.GetAPIKey(userID, this._connectionString);

                if (apiKey.Key == key && apiKey.IsAdmin)
                {
                    result = MHB.DAL.DataBaseConnector.GetDataReader(qry, this._connectionString, parameters);
                }
            }

            return result;
        }

        // Admin
        public DataTable GetDataTable(string userName, string password, string key, string qry, params SqlParameter[] parameters)
        {
            DataTable result = new DataTable();
            int userID = UserManager.User.GetUserID(userName, password, this._connectionString);

            MHB.Logging.Logger.LogAction(Logger.HistoryAction.API_GetDataTable, userID, this._connectionString, this._ipAddress);

            if (userID != 0)
            {
                APIKey apiKey = UserManager.User.GetAPIKey(userID, this._connectionString);

                if (apiKey.Key == key && apiKey.IsAdmin)
                {
                    result = MHB.DAL.DataBaseConnector.GetDataTable(qry, this._connectionString, parameters);
                }
            }

            return result;
        }
    }
}