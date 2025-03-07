using MHB.DAL;
using MHB.UserManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;

namespace MHB.BL
{
    public class ExpenditureManagerBase
    {
        #region Constants

        public const string MAINTABLE_NAME_01 = "tbMainTable01";
        public const string MAINTABLE_NAME_02 = "tbMainTable02";
        public const string MAINTABLE_NAME_03 = "tbMainTable03";
        public const string MAINTABLE_NAME_04 = "tbMainTable04";
        public const string MAINTABLE_NAME_05 = "tbMainTable05";
        public const string MAINTABLE_NAME_06 = "tbMainTable06";
        public const string MAINTABLE_NAME_07 = "tbMainTable07";
        public const string MAINTABLE_NAME_08 = "tbMainTable08";
        public const string MAINTABLE_NAME_09 = "tbMainTable09";

        public const string DETAILS_TABLE_NAME_01 = "tbDetailsTable01";
        public const string DETAILS_TABLE_NAME_02 = "tbDetailsTable02";
        public const string DETAILS_TABLE_NAME_03 = "tbDetailsTable03";
        public const string DETAILS_TABLE_NAME_04 = "tbDetailsTable04";
        public const string DETAILS_TABLE_NAME_05 = "tbDetailsTable05";
        public const string DETAILS_TABLE_NAME_06 = "tbDetailsTable06";
        public const string DETAILS_TABLE_NAME_07 = "tbDetailsTable07";
        public const string DETAILS_TABLE_NAME_08 = "tbDetailsTable08";
        public const string DETAILS_TABLE_NAME_09 = "tbDetailsTable09";

        #endregion Constants

        #region Enums

        public enum Months
        {
            NotSet = 0,
            January = 1,
            February = 2,
            March = 3,
            April = 4,
            May = 5,
            June = 6,
            July = 7,
            August = 8,
            September = 9,
            October = 10,
            November = 11,
            December = 12
        }

        #endregion Enums

        #region Properties

        private string _connectionString = string.Empty;

        public string ConnectionString
        {
            get
            {
                return this._connectionString;
            }
            set
            {
                this._connectionString = value;
            }
        }

        private string _mainTableName = string.Empty;

        public string MainTableName
        {
            get
            {
                return this._mainTableName;
            }
            set
            {
                this._mainTableName = value;
            }
        }

        private string _detailsTableName = string.Empty;

        public string DetailsTableName
        {
            get
            {
                return this._detailsTableName;
            }
            set
            {
                this._detailsTableName = value;
            }
        }

        private int _userID = 0;

        public int UserID
        {
            get
            {
                return this._userID;
            }
            set
            {
                this._userID = value;
            }
        }

        private MHB.BL.Enums.Language _language = MHB.BL.Enums.Language.Bulgarian;

        public MHB.BL.Enums.Language Language
        {
            get
            {
                return this._language;
            }
            set
            {
                this._language = value;
            }
        }

        private int _month = 0;

        public int Month
        {
            get
            {
                return this._month;
            }
            set
            {
                this._month = value;
            }
        }

        private int _year = 0;

        public int Year
        {
            get
            {
                return this._year;
            }
            set
            {
                this._year = value;
            }
        }

        public IEnumerable<Category> Categories { get; set; }

        public IEnumerable<Product> Products { get; set; }

        public IEnumerable<Supplier> Suppliers { get; set; }

        #endregion Properties

        #region Constructors

        public ExpenditureManagerBase(string connectionString, int userID, int month, int year, MHB.BL.Enums.Language language)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException("ExpenditureManagerBase: required [connectionString] constructor parameter is null or empty!");

            if (userID <= 0)
                throw new ArgumentNullException("ExpenditureManagerBase: required [userID] constructor parameter is null or empty!");

            if (month <= 0)
                throw new ArgumentNullException("ExpenditureManagerBase: required [month] constructor parameter is null or empty!");

            if (year <= 0)
                throw new ArgumentNullException("ExpenditureManagerBase: required [year] constructor parameter is null or empty!");

            if (language < 0)
                throw new ArgumentNullException("ExpenditureManagerBase: required [language] constructor parameter is null or empty!");

            this.ConnectionString = connectionString;
            this.MainTableName = GetUserMainTableNames(userID).Item1;
            this.DetailsTableName = GetUserMainTableNames(userID).Item2;
            this.UserID = userID;
            this.Month = month;
            this.Year = year;
            this.Language = language;

            this.Suppliers = this.GetSuppliers();
            this.Categories = this.GetCategories();
            this.Products = this.GetProducts();
        }

        #endregion Constructors

        #region Private Members

        #region FillExpenditureList

        protected IEnumerable<Expenditure> FillExpenditureList(IDataReader reader, SqlConnection connection, bool loadDetails)
        {
            List<Expenditure> expenses = new List<Expenditure>();

            User user = null;

            while (reader.Read())
            {
                Expenditure e = new Expenditure();

                e.AttachmentFileType = reader.Get<string>("AttachmentFileType");

                e.CategoryID = reader.Get<int>("CostCategory");

                e.DateRecordUpdated = reader.Get<DateTime>("DateRecordUpdated");

                e.DateRecordCreated = reader.Get<DateTime>("DateRecordCreated");

                e.DueDate = reader.Get<DateTime>("DueDate");

                e.FieldDescription = reader.Get<string>("FieldDescription");

                e.FieldExpectedValue = reader.Get<decimal>("FieldExpectedValue");

                e.FieldInitialValue = reader.Get<decimal>("FieldInitialValue");

                e.FieldName = reader.Get<string>("FieldName");

                e.FieldOldValue = reader.Get<decimal>("FieldOldValue");

                e.FieldValue = reader.Get<decimal>("FieldValue");

                e.Flagged = reader.Get<bool>("Flagged");

                e.HasAttachment = reader.Get<bool>("HasAttachment");

                e.HasDetails = reader.Get<bool>("HasDetails");

                e.ID = reader.Get<int>("ID");

                e.IsDeleted = reader.Get<bool>("IsDeleted");

                e.IsShared = reader.Get<bool>("IsShared");

                e.IsPaid = reader.Get<bool>("IsPaid");

                e.Month = reader.Get<int>("Month");

                e.NotificationDate = reader.Get<DateTime>("NotificationDate");

                e.Notified = reader.Get<bool>("Notified");

                e.OrderID = reader.Get<int>("OrderID");

                e.UserID = reader.Get<int>("UserID");

                e.Year = reader.Get<int>("Year");

                if (e.HasDetails && loadDetails)
                {
                    e.Details = this.GetExpenditureDetails(e, connection);
                }

                e.ProductID = reader.Get<int>("ProductID");

                e.Transactions = Transaction.GetAll(e.ID, connection);

                if (user == null)
                {
                    user = new User(connection, e.UserID);
                }

                e.User = user;

                if (this.Categories.Any(c => c.ID == e.CategoryID))
                {
                    e.Category = this.Categories.Single(c => c.ID == e.CategoryID);
                }
                else
                {
                    e.Category = new Category(e.CategoryID, (short)e.User.SelectedLanguage, e.UserID, connection);
                }

                expenses.Add(e);
            }

            if (!reader.IsClosed)
            {
                reader.Close();
                reader.Dispose();
            }

            return expenses;
        }

        #endregion FillExpenditureList

        protected ExpensesProMonth FillExpensesProMonth(IDataReader reader)
        {
            ExpensesProMonth exp = new ExpensesProMonth();

            while (reader.Read())
            {
                if (!reader.IsDBNull(reader.GetOrdinal("SumJanuary")))
                    exp.SumJanuary = (decimal)reader["SumJanuary"];

                if (!reader.IsDBNull(reader.GetOrdinal("SumFebruary")))
                    exp.SumFebruary = (decimal)reader["SumFebruary"];

                if (!reader.IsDBNull(reader.GetOrdinal("SumMarch")))
                    exp.SumMarch = (decimal)reader["SumMarch"];

                if (!reader.IsDBNull(reader.GetOrdinal("SumApril")))
                    exp.SumApril = (decimal)reader["SumApril"];

                if (!reader.IsDBNull(reader.GetOrdinal("SumMay")))
                    exp.SumMay = (decimal)reader["SumMay"];

                if (!reader.IsDBNull(reader.GetOrdinal("SumJune")))
                    exp.SumJune = (decimal)reader["SumJune"];

                if (!reader.IsDBNull(reader.GetOrdinal("SumJuly")))
                    exp.SumJuly = (decimal)reader["SumJuly"];

                if (!reader.IsDBNull(reader.GetOrdinal("SumAugust")))
                    exp.SumAugust = (decimal)reader["SumAugust"];

                if (!reader.IsDBNull(reader.GetOrdinal("SumSeptember")))
                    exp.SumSeptember = (decimal)reader["SumSeptember"];

                if (!reader.IsDBNull(reader.GetOrdinal("SumOctober")))
                    exp.SumOctober = (decimal)reader["SumOctober"];

                if (!reader.IsDBNull(reader.GetOrdinal("SumNovember")))
                    exp.SumNovember = (decimal)reader["SumNovember"];

                if (!reader.IsDBNull(reader.GetOrdinal("SumDecember")))
                    exp.SumDecember = (decimal)reader["SumDecember"];

                break;
            }

            return exp;
        }

        #endregion Private Members

        #region Public Members

        #region GetProducts

        public IEnumerable<Product> GetProducts()
        {
            this.Products = SQLHelper.GetProducts(this._userID, new SqlConnection(this._connectionString), this.Categories, this.Suppliers);
            return this.Products;
        }

        #endregion GetProducts

        #region AddProduct

        public int AddProduct(Product product)
        {
            int result = product.Add();
            this.GetProducts();
            return result;
        }

        #endregion AddProduct

        #region UpdateProduct

        public bool UpdateProduct(Product product)
        {
            bool result = product.Update();
            this.GetProducts();
            return result;
        }

        #endregion UpdateProduct

        #region GetSuppliers

        public IEnumerable<Supplier> GetSuppliers()
        {
            this.Suppliers = SQLHelper.GetSuppliers(this._userID, new SqlConnection(this._connectionString));
            return this.Suppliers;
        }

        #endregion GetSuppliers

        #region AddSupplier

        public int AddSupplier(Supplier supplier)
        {
            if (supplier.Connection == null)
                supplier.Connection = new SqlConnection(this._connectionString);

            int supplierID = supplier.Add();
            this.GetSuppliers();
            return supplierID;
        }

        #endregion AddSupplier

        #region UpdateSupplier

        public bool UpdateSupplier(Supplier supplier)
        {
            bool result = supplier.Update();
            this.GetSuppliers();
            return result;
        }

        #endregion UpdateSupplier

        #region DeleteSupplier

        public bool DeleteSupplier(int supplierID)
        {
            bool result = SQLHelper.DeleteSupplier(supplierID, this._connectionString);

            this.GetSuppliers();

            return result;
        }

        #endregion DeleteSupplier

        #region GetCategories

        public void AddCategory(Category newCategory)
        {
            if (newCategory == null) throw new ArgumentNullException("newCategory");

            newCategory.Add();
            this.GetCategories();
        }

        public void DeleteCategory(int categoryID)
        {
            Category.Delete(this._connectionString, categoryID);
            this.GetCategories();
        }

        public void UpdateCategory(Category category)
        {
            if (category == null) throw new ArgumentNullException("category");
            category.Update();
            this.GetCategories();
        }

        public IEnumerable<Category> GetCategories()
        {
            this.Categories = SQLHelper.GetCategories(this._connectionString, (int)this._language, this._userID);
            return this.Categories;
        }

        public IEnumerable<Category> GetUserCategories(int userID)
        {
            return SQLHelper.GetCategories(this._connectionString, (int)this._language, this._userID);
        }

        public IEnumerable<Category> GetAllCategories()
        {
            return SQLHelper.GetCategories(this._connectionString, (int)this._language, 0);
        }

        #endregion GetCategories

        #region GetUserExpenditures

        public IEnumerable<Expenditure> GetUserExpenditures(DateTime[] dates, SqlConnection connection = null)
        {
            if (string.IsNullOrWhiteSpace(this._connectionString)) throw new ArgumentNullException("MHB.BL.ExpenditureManagerBase.GetUserExpenditures: ConnectionString is null!!");

            if (connection == null)
                connection = new SqlConnection(this._connectionString);

            IDataReader reader = SQLHelper.GetExpense(dates, connection, this.MainTableName, this.DetailsTableName, this.UserID);

            return this.FillExpenditureList(reader, connection, true);
        }

        public IEnumerable<Expenditure> GetUserExpenditures(Expression<Func<Expenditure, bool>> filter, ExpressionQueryArgs args, SqlConnection connection = null)
        {
            if (string.IsNullOrWhiteSpace(this._connectionString)) throw new ArgumentNullException("MHB.BL.ExpenditureManagerBase.GetUserExpenditures: ConnectionString is null!!");

            if (connection == null)
                connection = new SqlConnection(this._connectionString);

            if (!args.ContainsKey("MainTableName"))
                args.Add("MainTableName", this._mainTableName);

            IDataReader reader = SQLHelper.GetExpense(filter, args, connection);

            return this.FillExpenditureList(reader, connection, true);
        }

        public IEnumerable<Expenditure> GetUserExpenditures(bool allFlaggedOnly, SqlConnection connection = null)
        {
            if (string.IsNullOrWhiteSpace(this._connectionString)) throw new ArgumentNullException("MHB.BL.ExpenditureManagerBase.GetUserExpenditures: ConnectionString is null!!");

            if (connection == null)
                connection = new SqlConnection(this._connectionString);

            IDataReader reader = SQLHelper.GetExpense(this._userID, connection, this._mainTableName, allFlaggedOnly, false, -1, -1, null);

            return this.FillExpenditureList(reader, connection, true);
        }

        public IEnumerable<Expenditure> GetUserExpenditures(bool flaggedOnly, bool loadDetails, bool hidePaid, int[] expenditureIDs = null, SqlConnection connection = null)
        {
            if (string.IsNullOrWhiteSpace(this._connectionString)) throw new ArgumentNullException("MHB.BL.ExpenditureManagerBase.GetUserExpenditures: ConnectionString is null!!");

            if (connection == null)
                connection = new SqlConnection(this._connectionString);

            IDataReader reader = SQLHelper.GetExpense(this._userID, connection, this._mainTableName, flaggedOnly, hidePaid, this._month, this._year, expenditureIDs);

            return this.FillExpenditureList(reader, connection, loadDetails);
        }

        public IEnumerable<Expenditure> GetUserExpenditures(int[] expenditureIDs = null, SqlConnection connection = null)
        {
            if (string.IsNullOrWhiteSpace(this._connectionString)) throw new ArgumentNullException("MHB.BL.ExpenditureManagerBase.GetUserExpenditures: ConnectionString is null!!");

            if (connection == null)
                connection = new SqlConnection(this._connectionString);

            IDataReader reader = SQLHelper.GetExpense(this._userID, connection, this._mainTableName, false, false, -1, -1, expenditureIDs);

            return this.FillExpenditureList(reader, connection, false);
        }

        #endregion GetUserExpenditures

        #region SearchUserExpenditures

        public IEnumerable<Expenditure> SearchUserExpenditures(Enums.SearchOptions searchOption, bool searchByYearToo, string year, string searchText, string sum, string comparisonOperator, bool searchByCategoryToo, string category, Enums.SortOptions sortOption, Enums.SortDirection sortDirection)
        {
            StringBuilder qry = new StringBuilder();
            StringBuilder whereClause = new StringBuilder();

            qry.AppendFormat(@"SELECT * FROM dbo.{0} mt ", this._mainTableName);

            whereClause.AppendFormat("WHERE mt.IsDeleted = 0 AND mt.UserID = {0} ", this._userID);

            switch (searchOption)
            {
                case Enums.SearchOptions.SearchBySum:
                    whereClause.Append(SQLHelper.SearchQueries.AND_MainTable_Value_COMPARE(sum, comparisonOperator));
                    break;

                case Enums.SearchOptions.SearchByText:
                    whereClause.Append(SQLHelper.SearchQueries.AND_MainTable_Name_OR_Description_CONTAINS(searchText));
                    break;

                case Enums.SearchOptions.SearchByDetails:
                    qry.Append(SQLHelper.SearchQueries.LEFT_JOIN_DetailsTable_TO_MainTable(this._detailsTableName));
                    whereClause.Append(SQLHelper.SearchQueries.AND_DetailsTable_Name_OR_Description_CONTAINS(searchText));
                    break;

                case Enums.SearchOptions.AllOfAbove:
                    whereClause.Append(SQLHelper.SearchQueries.AND_MainTable_Value_COMPARE(sum, comparisonOperator));
                    whereClause.Append(SQLHelper.SearchQueries.AND_MainTable_Name_OR_Description_OR_DetailsTable_Name_OR_Description_CONTAINS(searchText));
                    break;
            }

            if (searchByYearToo)
            {
                whereClause.Append(SQLHelper.SearchQueries.AND_MainTable_Year_EQUALS(year));
            }

            if (searchByCategoryToo)
            {
                whereClause.Append(SQLHelper.SearchQueries.AND_MainTable_CostCategory_EQUALS(category));
            }

            // Append WHERE clause to search query;
            qry.Append(whereClause);

            switch (sortOption)
            {
                case Enums.SortOptions.Price:
                    qry.Append(SQLHelper.SearchQueries.ORDER_BY_MainTable_FieldValue(sortDirection.ToString()));
                    break;

                case Enums.SortOptions.Date:
                    qry.Append(SQLHelper.SearchQueries.ORDER_BY_MainTable_Year_Month(sortDirection.ToString()));
                    break;

                case Enums.SortOptions.Category:
                    qry.Append(SQLHelper.SearchQueries.ORDER_BY_MainTable_CostCategory(sortDirection.ToString()));
                    break;
            }

            SqlConnection connection = new SqlConnection(this._connectionString);

            IDataReader reader = SQLHelper.SearchExpenses(qry.ToString(), connection);

            return this.FillExpenditureList(reader, connection, true);
        }

        #endregion SearchUserExpenditures

        #region AddNewParentExpense

        public bool AddNewParentExpense(string expectedValue, bool reccuringEveryMonth, bool recurrentExpenditure, string dueDate, string fieldName, string description, int productID, out bool februaryException, out string qryToLog)
        {
            string qry = string.Empty;

            try
            {
                fieldName = fieldName.Replace("'", string.Empty).Replace("--", string.Empty);
                description = description.Replace("'", string.Empty).Replace("--", string.Empty);

                expectedValue = Regex.Replace(expectedValue, "[A-Za-z;'%^!@#$%&*()_+-]", String.Empty);

                DateTime dueDateTime;

                double expVal = 0;
                if (!double.TryParse(expectedValue, out expVal))
                {
                    expectedValue = "0";
                }

                bool febException = false;

                qry = string.Empty;

                int startMonth = 1;

                if (reccuringEveryMonth)
                {
                    startMonth = this._month;
                }

                SqlParameter[] parameters = new SqlParameter[] { };

                if (recurrentExpenditure)
                {
                    List<SqlParameter> tempSqlParameters = new List<SqlParameter>();

                    for (int month = startMonth; month <= 12; month++)
                    {
                        if (DateTime.TryParse(dueDate, out dueDateTime))
                        {
                            switch ((Enums.Month)month)
                            {
                                case Enums.Month.February:
                                    if (dueDateTime.Day > 28)
                                    {
                                        dueDateTime = new DateTime(dueDateTime.Year, month, 28);
                                        febException = true;
                                    }

                                    break;

                                case Enums.Month.April:
                                case Enums.Month.June:
                                case Enums.Month.September:
                                case Enums.Month.November:
                                    if (dueDateTime.Day == 31)
                                    {
                                        dueDateTime = new DateTime(dueDateTime.Year, month, 30);
                                    }

                                    break;

                                default:
                                    dueDateTime = new DateTime(dueDateTime.Year, month, dueDateTime.Day);
                                    break;
                            }
                        }
                        else
                            dueDateTime = (DateTime)SqlDateTime.MinValue;

                        qry += string.Format(@"

INSERT INTO {1} (UserID, Month, Year, FieldName, FieldDescription, FieldValue, FieldExpectedValue, DueDate, DateRecordUpdated, OrderID, ProductID, DateRecordCreated) VALUES
(@UserID{0}, @Month{0}, @Year{0}, @FieldName{0}, @FieldDescription{0}, 0 , @FieldExpectedValue{0}, @DueDate{0}, GETDATE(), IDENT_CURRENT('{1}'), @ProductID{0}, GETDATE())

DECLARE @RowId{0} INT = SCOPE_IDENTITY();

EXECUTE [dbo].[spSetCostCategory] @RowId{0}, '{1}', @FieldName{0}", month.ToString(), this._mainTableName);

                        tempSqlParameters.Add(new SqlParameter(string.Format("UserID{0}", month), this._userID));
                        tempSqlParameters.Add(new SqlParameter(string.Format("Month{0}", month), month));
                        tempSqlParameters.Add(new SqlParameter(string.Format("Year{0}", month), this._year));
                        tempSqlParameters.Add(new SqlParameter(string.Format("FieldName{0}", month), fieldName));
                        tempSqlParameters.Add(new SqlParameter(string.Format("FieldDescription{0}", month), description));
                        tempSqlParameters.Add(new SqlParameter(string.Format("FieldExpectedValue{0}", month), expectedValue));
                        tempSqlParameters.Add(new SqlParameter(string.Format("DueDate{0}", month), dueDateTime));
                        tempSqlParameters.Add(new SqlParameter(string.Format("ProductID{0}", month), productID));
                    }

                    parameters = tempSqlParameters.ToArray();
                }
                else
                {
                    qry = string.Format(@"

INSERT INTO {0} (UserID, Month, Year, FieldName, FieldDescription, FieldValue, FieldExpectedValue, DueDate, DateRecordUpdated, OrderID, ProductID, DateRecordCreated) VALUES
(@UserID, @Month, @Year, @FieldName, @FieldDescription, 0 , @FieldExpectedValue, @DueDate, GETDATE(), IDENT_CURRENT('{0}'), @ProductID, GETDATE())

DECLARE @RowId INT = SCOPE_IDENTITY();

EXECUTE spSetCostCategory @RowId, '{0}', @FieldName", this._mainTableName);

                    parameters = new SqlParameter[]
                    {
                        new SqlParameter("UserID", this._userID),
                        new SqlParameter("Month", this._month),
                        new SqlParameter("Year", this._year),
                        new SqlParameter("FieldName", fieldName),
                        new SqlParameter("FieldDescription", description),
                        new SqlParameter("FieldExpectedValue", expectedValue),
                        new SqlParameter("DueDate", DateTime.TryParse(dueDate, out dueDateTime) ? dueDateTime : (DateTime)SqlDateTime.MinValue),
                        new SqlParameter("ProductID", productID),
                    };
                }

                MHB.DAL.DataBaseConnector.ExecuteQuery(qry, this._connectionString, parameters);

                februaryException = febException;
                qryToLog = qry;

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("AddNewParentExpense:{0}", ex.Message), ex);
            }
        }

        #endregion AddNewParentExpense

        #region AddChildExpense

        /// <summary>
        /// Adds a new ExpenditureDetail to tbDetailsTableXX; If either ExpenditureDetail.ConnectionString/MainTableName/DetailsTableName/UserID properties are undefined ExpenditureManager default settings would be used!
        /// </summary>
        /// <param name="expenseDetail"></param>
        /// <returns>Int</returns>
        public int AddNewChildExpense(ExpenditureDetail expenseDetail)
        {
            if (string.IsNullOrEmpty(expenseDetail.ConnectionString) ||
                string.IsNullOrEmpty(expenseDetail.MainTableName) ||
                string.IsNullOrEmpty(expenseDetail.DetailsTableName) ||
                expenseDetail.UserID == 0)
            {
                return SQLHelper.AddChildExpense(expenseDetail, this._detailsTableName, this._mainTableName, this._connectionString, this._userID);
            }
            else
            {
                return SQLHelper.AddChildExpense(expenseDetail);
            }
        }

        #endregion AddChildExpense

        #region DeleteParentExpense

        public int DeleteParentExpense(int[] parentExpenseID, out string qryToLog)
        {
            string qry = string.Empty;
            int recordsDeleted = 0;

            try
            {
                if (parentExpenseID != null && parentExpenseID.Any())
                {
                    for (int i = 0; i < parentExpenseID.Length; i++)
                    {
                        if (parentExpenseID[i] == 0) break;

                        qry += string.Format(@"
UPDATE {0} SET IsDeleted = 1 WHERE ExpenditureID = {1} AND ExpenditureID IN (SELECT ID FROM {2} WHERE UserID = {3}) ", this._detailsTableName, parentExpenseID[i], this._mainTableName, this._userID);

                        qry += string.Format(@"
UPDATE {0} SET IsDeleted = 1, DateRecordUpdated = GETDATE() WHERE ID = {1} AND UserID = {2} ", this._mainTableName, parentExpenseID[i], this._userID);
                    }

                    recordsDeleted = MHB.DAL.DataBaseConnector.ExecuteQuery(qry, this._connectionString);
                }
                qryToLog = qry;

                return recordsDeleted;
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(ex, "ExpenditureManagerBase.DeleteParentExpense", qry, this._userID, this._connectionString);
                throw;
            }
        }

        #endregion DeleteParentExpense

        #region

        public int DeleteParentExpenses(int year, int month)
        {
            string qry = string.Empty;
            int recordsDeleted = 0;

            try
            {
                qry = string.Format(@"UPDATE dt
                                          SET dt.IsDeleted = 1
                                          FROM {0} AS dt
                                          INNER JOIN {1} AS mt
                                          ON mt.ID = dt.ExpenditureID
                                          WHERE mt.Month = @Month AND mt.Year = @Year

                        UPDATE {1} SET IsDeleted = 1 WHERE IsDeleted = 0 AND [Month] = @Month AND [Year] = @Year AND UserID = @UserID", this._detailsTableName, this._mainTableName);

                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("Month", month),
                    new SqlParameter("Year", year),
                    new SqlParameter("UserID", this._userID)
                };

                recordsDeleted = MHB.DAL.DataBaseConnector.ExecuteQuery(qry, this._connectionString, sqlParameters);
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(ex, "ExpenditureManagerBase.DeleteParentExpenses", qry, this._userID, this._connectionString);
                throw;
            }

            return recordsDeleted;
        }

        #endregion Public Members

        #region DeleteChildExpenses

        public int DeleteChildExpenses(ExpenditureDetail[] expenditureDetailsToDelete, out string qryToLog)
        {
            string qry = string.Empty;
            int rowsDeleted = 0;

            try
            {
                qry =
string.Format("UPDATE {0} SET IsDeleted = 1 WHERE ID IN ({1}) AND ExpenditureID IN (SELECT ID FROM {2} WHERE UserID = {3}) DELETE FROM tbProductParameters WHERE ParentID IN ({1})",
                this._detailsTableName,
                string.Join(",", expenditureDetailsToDelete.Select(d => d.ID)),
                this._mainTableName,
                this._userID);

                int[] parents = expenditureDetailsToDelete.Select(d => d.ExpenditureID).Distinct().ToArray();

                for (int i = 0; i < parents.Length; i++)
                {
                    qry += string.Format(@"
                        UPDATE {0} SET FieldOldValue = FieldValue WHERE ID = {2}
                        DECLARE @sumDetails AS MONEY = (SELECT SUM(DetailValue) FROM {1} WHERE ExpenditureID = {2} AND IsDeleted = 0)
                        UPDATE {0} SET FieldValue = ISNULL(@sumDetails, 0) WHERE ID = {2}", this._mainTableName, this._detailsTableName, parents[i]);
                }

                rowsDeleted = MHB.DAL.DataBaseConnector.ExecuteQuery(qry, this._connectionString);

                qryToLog = qry;
                return rowsDeleted;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("MHB.BL.DeleteChildExpenses:{0}", ex.Message), ex);
            }
        }

        #endregion DeleteChildExpenses

        #region UpdateBudgets

        public void UpdateBudgets(decimal expenses, decimal budget, decimal savings)
        {
            SQLHelper.UpdateBudgets(_connectionString, expenses, budget, savings, this._month, this._year, this._userID);
        }

        #endregion UpdateBudgets

        #region GetExpenditureDetails

        public IEnumerable<ExpenditureDetail> GetExpenditureDetails(Expenditure parent, SqlConnection connection)
        {
            if (connection == null) throw new ArgumentNullException("connection is null ! ! !");

            return this.GetExpenditureDetailsInternal(connection, parent);
        }

        public IEnumerable<ExpenditureDetail> GetExpenditureDetails(SqlConnection connection, int year, int categoryID, Enums.MeasureType measureType = Enums.MeasureType.NotSet)
        {
            if (connection == null) throw new ArgumentNullException("connection is null ! ! !");

            return this.GetExpenditureDetailsInternal(connection, null, -1, year, categoryID, -1, measureType);
        }

        public IEnumerable<ExpenditureDetail> GetExpenditureDetails(SqlConnection connection, int month, int year)
        {
            if (connection == null) throw new ArgumentNullException("connection is null ! ! !");

            return this.GetExpenditureDetailsInternal(connection, null, month, year);
        }

        public IEnumerable<ExpenditureDetail> GetExpenditureDetails(SqlConnection connection, int month, int year, Category category)
        {
            if (connection == null) throw new ArgumentNullException("connection is null ! ! !");

            return this.GetExpenditureDetailsInternal(connection, null, month, year, category.ID);
        }

        public IEnumerable<ExpenditureDetail> GetExpenditureDetailsForProduct(int productID)
        {
            return this.GetExpenditureDetailsInternal(new SqlConnection(this._connectionString), null, -1, -1, -1, productID);
        }

        public IEnumerable<ExpenditureDetail> GetExpenditureDetailsForCategory(int categoryID)
        {
            return this.GetExpenditureDetailsInternal(new SqlConnection(this._connectionString), null, -1, -1, categoryID, -1);
        }

        public IEnumerable<ExpenditureDetail> GetExpenditureDetails(SqlConnection connection, int productID)
        {
            if (connection == null) throw new ArgumentNullException("connection is null ! ! !");

            return this.GetExpenditureDetailsInternal(connection, null, -1, -1, -1, productID);
        }

        public IEnumerable<ExpenditureDetail> GetExpenditureDetails(Expression<Func<ExpenditureDetail, bool>> filter, ExpressionQueryArgs args, SqlConnection connection = null, Expenditure parent = null)
        {
            if (connection == null)
            {
                connection = new SqlConnection(this._connectionString);
            }

            IDataReader reader = SQLHelper.GetChildExpenses(filter, args, connection);

            IEnumerable<ExpenditureDetail> details = this.FillExpenditureDetails(connection, parent, reader);

            return details;
        }

        private IEnumerable<ExpenditureDetail> GetExpenditureDetailsInternal(SqlConnection connection, Expenditure parent = null, int month = -1, int year = -1, int categoryID = -1, int productID = -1, Enums.MeasureType prevailingMeasureType = Enums.MeasureType.NotSet)
        {
            IDataReader reader = SQLHelper.GetChildExpenses(this._userID, this._connectionString, this._detailsTableName, this._mainTableName, connection, parent != null ? parent.ID : -1, month, year, categoryID, productID, prevailingMeasureType);

            IEnumerable<ExpenditureDetail> details = this.FillExpenditureDetails(connection, parent, reader);

            return details;
        }

        private IEnumerable<ExpenditureDetail> FillExpenditureDetails(SqlConnection connection, Expenditure parent, IDataReader reader, IEnumerable<Category> categoriesCache = null)
        {
            List<ExpenditureDetail> details = new List<ExpenditureDetail>();

            while (reader.Read())
            {
                ExpenditureDetail e = new ExpenditureDetail();

                if (!reader.IsDBNull(reader.GetOrdinal("AttachmentFileType")))
                    e.AttachmentFileType = (string)reader["AttachmentFileType"];

                if (!reader.IsDBNull(reader.GetOrdinal("DetailDate")))
                    e.DetailDate = (DateTime)reader["DetailDate"];

                if (!reader.IsDBNull(reader.GetOrdinal("DetailDateCreated")))
                    e.DetailDateCreated = (DateTime)reader["DetailDateCreated"];

                if (!reader.IsDBNull(reader.GetOrdinal("DetailDescription")))
                    e.DetailDescription = (string)reader["DetailDescription"];

                if (!reader.IsDBNull(reader.GetOrdinal("DetailName")))
                    e.DetailName = (string)reader["DetailName"];

                if (!reader.IsDBNull(reader.GetOrdinal("DetailValue")))
                    e.DetailValue = (decimal)reader["DetailValue"];

                if (!reader.IsDBNull(reader.GetOrdinal("DetailInitialValue")))
                    e.DetailInitialValue = (decimal)reader["DetailInitialValue"];

                if (!reader.IsDBNull(reader.GetOrdinal("ExpenditureID")))
                    e.ExpenditureID = (int)reader["ExpenditureID"];

                if (!reader.IsDBNull(reader.GetOrdinal("HasAttachment")))
                    e.HasAttachment = (bool)reader["HasAttachment"];

                if (!reader.IsDBNull(reader.GetOrdinal("ID")))
                    e.ID = (int)reader["ID"];

                if (!reader.IsDBNull(reader.GetOrdinal("IsDeleted")))
                    e.IsDeleted = (bool)reader["IsDeleted"];

                if (!reader.IsDBNull(reader.GetOrdinal("IsShared")))
                    e.IsShared = (bool)reader["IsShared"];

                if (!reader.IsDBNull(reader.GetOrdinal("HasProductParameters")))
                    e.HasProductParameters = (bool)reader["HasProductParameters"];

                if (!reader.IsDBNull(reader.GetOrdinal("ProductID")))
                {
                    e.ProductID = (int)reader["ProductID"];

                    if (e.ProductID != Product.PRODUCT_DEFAULT_ID)
                    {
                        if (this.Products.Any(p => p.ID == e.ProductID))
                        {
                            e.Product = new Product(this.Products.Single(p => p.ID == e.ProductID)); // create a new Product as this creates a copy of the object otherwise .Parameters property is overwritten;
                        }
                        else
                        {
                            e.Product = new Product(e.ProductID, this._userID, e.ID, e, connection, this.Categories, this.Suppliers);
                        }

                        if (e.HasProductParameters)
                            e.Product.Parameters = SQLHelper.GetProductParameters(e.ID, e.ProductID, e.UserID, connection);
                        else
                            e.Product.Parameters = Enumerable.Empty<ProductParameter>();
                    }
                }

                if (!reader.IsDBNull(reader.GetOrdinal("SupplierID")))
                {
                    e.SupplierID = (int)reader["SupplierID"];

                    if (this.Suppliers.Any(s => s.ID == e.SupplierID))
                    {
                        e.Supplier = this.Suppliers.Single(s => s.ID == e.SupplierID);
                    }
                    else
                    {
                        e.Supplier = new Supplier(e.SupplierID, this._userID, connection);
                    }
                }

                if (!reader.IsDBNull(reader.GetOrdinal("Amount")))
                    e.Amount = (decimal)reader["Amount"];

                if (!reader.IsDBNull(reader.GetOrdinal("MeasureTypeID")))
                    e.MeasureType = (Enums.MeasureType)reader["MeasureTypeID"];

                if (!reader.IsDBNull(reader.GetOrdinal("InitialMeasureTypeID")))
                    e.InitialMeasureType = (Enums.MeasureType)reader["InitialMeasureTypeID"];

                if (!reader.IsDBNull(reader.GetOrdinal("InitialAmount")))
                    e.InitialAmount = (decimal)reader["InitialAmount"];

                e.IsSurplus = reader.Get<bool>("IsSurplus");

                e.IsOcrScanned = reader.Get<bool>("IsOcrScanned");

                //if (parent == null)
                //{
                //    e.Parent = this.GetUserExpenditures(new int[] { e.ExpenditureID }, connection).FirstOrDefault();
                //}
                if (parent != null)
                {
                    e.Parent = parent;
                }

                details.Add(e);
            }
            return details;
        }

        #endregion GetExpenditureDetails

        #region UpdateParentExpenses

        public bool UpdateParentExpenses(List<Expenditure> expensesToUpdate, string qryUpdateLastDateUpdated)
        {
            if (expensesToUpdate.Count == 0) return false;

            string qry = qryUpdateLastDateUpdated;

            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                for (int i = 0; i < expensesToUpdate.Count; i++)
                {
                    qry += string.Format(@"UPDATE dbo.{0} SET
                                            FieldName = @FieldName{1},
                                            FieldDescription = @FieldDescription{1},
                                            FieldExpectedValue = (CASE WHEN @IsPaid{1} = 1 THEN @FieldValue{1} ELSE @FieldExpectedValue{1} END),
                                            FieldValue = @FieldValue{1},
                                            FieldOldValue = @FieldOldValue{1},
                                            DueDate = @DueDate{1},
                                            IsPaid = @IsPaid{1},
                                            Flagged = @Flagged{1},
                                            CostCategory = @CostCategory{1},
                                            IsShared = @IsShared{1},
                                            DateRecordUpdated = GETDATE()
                                            WHERE ID = @ID{1} AND UserID = @UserID{1}
                                            IF (SELECT COUNT(ID) FROM tbTransactionLog WHERE ExpenseID = @ID{1}) = 0
                                            BEGIN
	                                            UPDATE dbo.{0} SET FieldInitialValue = @FieldValue{1} WHERE ID = @ID{1}
                                            END
                                            ", this._mainTableName, i);

                    parameters.Add(new SqlParameter(string.Format("FieldName{0}", i), expensesToUpdate[i].FieldName));
                    parameters.Add(new SqlParameter(string.Format("FieldDescription{0}", i), expensesToUpdate[i].FieldDescription));
                    parameters.Add(new SqlParameter(string.Format("FieldExpectedValue{0}", i), expensesToUpdate[i].FieldExpectedValue));
                    parameters.Add(new SqlParameter(string.Format("FieldValue{0}", i), expensesToUpdate[i].FieldValue));
                    parameters.Add(new SqlParameter(string.Format("FieldOldValue{0}", i), expensesToUpdate[i].FieldOldValue));
                    parameters.Add(new SqlParameter(string.Format("DueDate{0}", i), expensesToUpdate[i].DueDate));
                    parameters.Add(new SqlParameter(string.Format("IsPaid{0}", i), expensesToUpdate[i].IsPaid));
                    parameters.Add(new SqlParameter(string.Format("Flagged{0}", i), expensesToUpdate[i].Flagged));
                    parameters.Add(new SqlParameter(string.Format("CostCategory{0}", i), expensesToUpdate[i].CategoryID));
                    parameters.Add(new SqlParameter(string.Format("IsShared{0}", i), expensesToUpdate[i].IsShared));
                    parameters.Add(new SqlParameter(string.Format("ID{0}", i), expensesToUpdate[i].ID));
                    parameters.Add(new SqlParameter(string.Format("UserID{0}", i), this._userID));
                }

                MHB.DAL.DataBaseConnector.ExecuteQuery(qry, this._connectionString, parameters.ToArray());

                return true;
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(ex, string.Format("MHB.BL.UpdateParentExpenses:{0}", ex.Message), qry, this._userID, this._connectionString);
                return false;
            }
        }

        #endregion UpdateParentExpenses

        #region UpdateChildExpenses

        public bool UpdateChildExpenses(List<ExpenditureDetail> expensesToUpdate)
        {
            if (expensesToUpdate.Count == 0) return false;

            string qry = string.Empty;

            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                for (int i = 0; i < expensesToUpdate.Count; i++)
                {
                    qry += string.Format(@"
                        DECLARE @originalExpenditureID{1} INT = (SELECT ExpenditureID FROM {0} WHERE ID = @ID{1})", this._detailsTableName, i);

                    qry += string.Format(@"UPDATE {0} SET
                                                ExpenditureID = @ExpenditureID{1},
                                                DetailName = @DetailName{1},
                                                DetailDescription = @DetailDescription{1},
                                                DetailValue = @DetailValue{1},
                                                Amount = @Amount{1},
                                                DetailDate = @DetailDate{1},
                                                ProductID = @ProductID{1},
                                                SupplierID = @SupplierID{1},
                                                MeasureTypeID = @MeasureTypeID{1},
                                                IsShared = @IsShared{1},
                                                --InitialAmount = @Amount{1},
                                                InitialMeasureTypeID = @MeasureTypeID{1},
                                                IsSurplus = @IsSurplus{1},
                                                IsOcrScanned = @IsOcrScanned{1}
                                                WHERE ID = @ID{1} ", this._detailsTableName, i);

                    qry += string.Format(@"
                        IF @ExpenditureID{2} <> @originalExpenditureID{2}
                        BEGIN
                            UPDATE {0} SET FieldOldValue = FieldValue WHERE ID = @originalExpenditureID{2}
                            DECLARE @updatedSumDetails{2} AS MONEY = (SELECT SUM(DetailValue) FROM {1} WHERE ExpenditureID = @originalExpenditureID{2} AND IsDeleted = 0)
                            UPDATE {0} SET FieldValue = ISNULL(@updatedSumDetails{2}, 0) WHERE ID = @originalExpenditureID{2}
                        END ", this._mainTableName, this._detailsTableName, i);

                    qry += string.Format(@"
                        UPDATE {0} SET FieldOldValue = FieldValue WHERE ID = @ExpenditureID{2}
                        DECLARE @sumDetails{2} AS MONEY = (SELECT SUM(DetailValue) FROM {1} WHERE ExpenditureID = @ExpenditureID{2} AND IsDeleted = 0)
                        UPDATE {0} SET FieldValue = ISNULL(@sumDetails{2}, 0) WHERE ID = @ExpenditureID{2} ", this._mainTableName, this._detailsTableName, i);

                    Product detailProduct = expensesToUpdate[i].Product;

                    decimal amount = 0;

                    if (detailProduct != null && detailProduct.IsFixedMeasureType)
                    {
                        switch (detailProduct.DefaultMeasureType)
                        {
                            case Enums.MeasureType.Volume:
                                amount = detailProduct.Volume;
                                break;

                            case Enums.MeasureType.Weight:
                                amount = detailProduct.Weight;
                                break;

                            default:
                                amount = detailProduct.PackageUnitsCount;
                                break;
                        }
                    }
                    else
                    {
                        amount = expensesToUpdate[i].Amount;
                    }

                    parameters.Add(new SqlParameter(string.Format("DetailName{0}", i), expensesToUpdate[i].DetailName));
                    parameters.Add(new SqlParameter(string.Format("DetailDescription{0}", i), expensesToUpdate[i].DetailDescription));
                    parameters.Add(new SqlParameter(string.Format("DetailValue{0}", i), expensesToUpdate[i].DetailValue));
                    parameters.Add(new SqlParameter(string.Format("Amount{0}", i), amount));
                    parameters.Add(new SqlParameter(string.Format("DetailDate{0}", i), expensesToUpdate[i].DetailDate));
                    parameters.Add(new SqlParameter(string.Format("ProductID{0}", i), expensesToUpdate[i].ProductID));
                    parameters.Add(new SqlParameter(string.Format("SupplierID{0}", i), expensesToUpdate[i].SupplierID));
                    parameters.Add(new SqlParameter(string.Format("MeasureTypeID{0}", i), (int)expensesToUpdate[i].MeasureType != 0 ? (int)expensesToUpdate[i].MeasureType : (int)MHB.BL.Enums.MeasureType.Count));
                    parameters.Add(new SqlParameter(string.Format("ID{0}", i), expensesToUpdate[i].ID));
                    parameters.Add(new SqlParameter(string.Format("ExpenditureID{0}", i), expensesToUpdate[i].ExpenditureID));
                    parameters.Add(new SqlParameter(string.Format("IsShared{0}", i), expensesToUpdate[i].IsShared));
                    parameters.Add(new SqlParameter(string.Format("IsSurplus{0}", i), expensesToUpdate[i].IsSurplus));
                    parameters.Add(new SqlParameter(string.Format("IsOcrScanned{0}", i), expensesToUpdate[i].IsOcrScanned));
                }

                MHB.DAL.DataBaseConnector.ExecuteQuery(qry, this._connectionString, parameters.ToArray());

                SQLHelper.UpdateProductParameters(expensesToUpdate, null, this.DetailsTableName, this._connectionString);

                return true;
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(ex, string.Format("MHB.BL.UpdateChildExpenses:{0}", ex.Message), qry, this._userID, this._connectionString);
                return false;
            }
        }

        #endregion UpdateChildExpenses

        #region GetUsersAverageSumForCategory

        public decimal GetUsersAverageSumForCategory(int userID, int categoryID)
        {
            try
            {
                string qry = "EXECUTE [dbo].[spGetUsersAverageExpensesForCategory] @CategoryID, @UserID";

                SqlParameter parCategoryID = new SqlParameter("@CategoryID", categoryID);
                SqlParameter parUserID = new SqlParameter("@UserID", userID);

                object result = MHB.DAL.DataBaseConnector.GetSingleValue(qry, this._connectionString, parCategoryID, parUserID);

                decimal avg = 0;

                if (decimal.TryParse(result.ToString(), out avg))
                    return avg;
                else
                    return 0;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("GetUsersAverageSumForCategory:{0}", ex.Message), ex);
            }
        }

        #endregion GetUsersAverageSumForCategory

        #region CopyParentExpense

        public int CopyParentExpense(int parentExpenseID, out string qryToLog)
        {
            string qry = string.Empty;
            int recordsCopied = 0;

            try
            {
                qry = "EXECUTE spCopyParentExpense @parentExpenseID, @mainTableName, @detailsTableName";
                //, parentExpenseID, this._mainTableName);

                recordsCopied = MHB.DAL.DataBaseConnector.ExecuteQuery(qry, this._connectionString,
                    new SqlParameter("parentExpenseID", parentExpenseID),
                    new SqlParameter("mainTableName", this._mainTableName),
                    new SqlParameter("detailsTableName", this._detailsTableName));

                qryToLog = qry;

                return recordsCopied;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("MHB.BL.CopyParentExpense:{0}", ex.Message), ex);
            }
        }

        #endregion CopyParentExpense

        #region DeleteAttachment

        public int DeleteAttachment(int parentExpenseID, out string qryToLog)
        {
            string qry = string.Empty;
            int recordsDeleted = 0;

            try
            {
                qry = String.Format(
"UPDATE [dbo].[{0}] SET [HasAttachment] = 0 WHERE ID = {1}", this._mainTableName, parentExpenseID);

                recordsDeleted = MHB.DAL.DataBaseConnector.ExecuteQuery(qry, this._connectionString);

                qryToLog = qry;

                return recordsDeleted;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("MHB.BL.DeletAttachment:{0}", ex.Message), ex);
            }
        }

        #endregion DeleteAttachment

        #region GetMaximumExpenditureForCategory

        public Expenditure GetMaximumExpenditureForCategory(int categoryID)
        {
            try
            {
                SqlConnection connection = new SqlConnection(this._connectionString);

                IDataReader reader = SQLHelper.GetMaximumExpenditureForCategory(this._mainTableName, this._userID, categoryID, connection);

                IEnumerable<Expenditure> expense = this.FillExpenditureList(reader, connection, true);

                if (expense.Count() > 0)
                    return expense.FirstOrDefault();
                else
                    return new Expenditure();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("GetMaximumExpenditureForCategory:{0}", ex.Message), ex);
            }
        }

        #endregion GetMaximumExpenditureForCategory

        #region GetUserIncome

        public IEnumerable<Income> GetUserIncome()
        {
            try
            {
                return SQLHelper.GetUserIncome(this._userID, this._year, this._month, this._connectionString);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("GetUserIncome:{0}", ex.Message), ex);
            }
        }

        public IEnumerable<Income> GetUserIncome(int month)
        {
            try
            {
                if (month > 12 || month < 1) throw new ArgumentOutOfRangeException("Month out of range!");

                return SQLHelper.GetUserIncome(this._userID, this._year, month, this._connectionString);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("GetUserIncome:{0}", ex.Message), ex);
            }
        }

        public IEnumerable<Income> GetUserIncome(int month, int year)
        {
            try
            {
                if (month > 12 || month < 1) throw new ArgumentOutOfRangeException("Month out of range!");

                if (year.ToString().Length != 4) throw new ArgumentOutOfRangeException("Year out of range!");

                return SQLHelper.GetUserIncome(this._userID, year, month, this._connectionString);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("GetUserIncome:{0}", ex.Message), ex);
            }
        }

        #endregion GetUserIncome

        #region Duplicate Expenditures

        public bool DuplicateExpenditures(int destinationMonth, int destinationYear, bool deleteExistingData, bool copyFlaggedOnly, bool markUnpaid, bool zeroActualSum, out string qryToLog)
        {
            string qry = string.Empty;

            bool result = false;

            try
            {
                qry = "EXECUTE spDuplicateMonthRecords @userID, @month, @year, @destinationMonth, @destinationYear, @mainTableName, @detailsTableName, @deleteExistingData, @copyFlaggedOnly, @markUnpaid, @zeroCopiedActualSum";

                qryToLog = qry;

                SqlParameter[] parameters = new SqlParameter[11];

                parameters[0] = new SqlParameter("userID", this._userID);
                parameters[1] = new SqlParameter("month", this._month);
                parameters[2] = new SqlParameter("year", this._year);
                parameters[3] = new SqlParameter("destinationMonth", destinationMonth);

                parameters[4] = new SqlParameter("destinationYear", destinationYear);
                parameters[5] = new SqlParameter("mainTableName", this._mainTableName);
                parameters[6] = new SqlParameter("detailsTableName", this._detailsTableName);
                parameters[7] = new SqlParameter("deleteExistingData", deleteExistingData);
                parameters[8] = new SqlParameter("copyFlaggedOnly", copyFlaggedOnly);

                parameters[9] = new SqlParameter("markUnpaid", markUnpaid);
                parameters[10] = new SqlParameter("zeroCopiedActualSum", zeroActualSum);

                MHB.DAL.DataBaseConnector.ExecuteQuery(qry, this._connectionString, parameters);

                result = true;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("DuplicateExpenditures:{0}", ex.Message), ex);
            }

            return result;
        }

        #endregion Duplicate Expenditures

        #region GetSearchKeywords

        public List<string> GetSearchKeywords()
        {
            List<string> result = new List<string>();

            string qry = string.Empty;

            try
            {
                qry = "EXECUTE spGetSearchKeywords @userID";

                SqlParameter parUserID = new SqlParameter("userID", this._userID);

                IDataReader reader = MHB.DAL.DataBaseConnector.GetDataReader(qry, this._connectionString, parUserID);

                while (reader.Read())
                {
                    result.Add(reader["Keywords"].ToString());
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("GetSearchKeywords:{0}", ex.Message), ex);
            }

            return result;
        }

        #endregion GetSearchKeywords

        #region GetYearlyExpensesProMonth

        public ExpensesProMonth GetYearlyExpensesProMonth()
        {
            ExpensesProMonth exp = new ExpensesProMonth();

            try
            {
                IDataReader reader = SQLHelper.GetUsersYearlyExpensesProMonth(this._userID, this._year, this._connectionString);

                exp = this.FillExpensesProMonth(reader);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("GetYearlyExpensesProMonth(): {0}", ex.Message), ex);
            }

            return exp;
        }

        #endregion GetYearlyExpensesProMonth

        #region GetYearlyBudgetsProMonth

        public ExpensesProMonth GetYearlyBudgetsProMonth()
        {
            ExpensesProMonth exp = new ExpensesProMonth();

            try
            {
                IDataReader reader = SQLHelper.GetUsersYearlyBudgetsProMonth(this._userID, this._year, this._connectionString);

                exp = this.FillExpensesProMonth(reader);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("GetYearlyBudgetsProMonth(): {0}", ex.Message), ex);
            }

            return exp;
        }

        #endregion GetYearlyBudgetsProMonth

        #region GetYearlySavingsProMonth

        public ExpensesProMonth GetYearlySavingsProMonth()
        {
            ExpensesProMonth exp = new ExpensesProMonth();

            try
            {
                IDataReader reader = SQLHelper.GetUsersYearlySavingsProMonth(this._userID, this._year, this._connectionString);

                exp = this.FillExpensesProMonth(reader);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("GetYearlySavingsProMonth(): {0}", ex.Message), ex);
            }

            return exp;
        }

        #endregion GetYearlySavingsProMonth

        #region GetYearlyExpensesProMonthDataTable

        public DataTable GetYearlyExpensesProMonthDataTable(ExpensesProMonth expenses)
        {
            DataTable table = new DataTable();

            table.Columns.Add("SumJanuary", typeof(decimal));
            table.Columns.Add("SumFebruary", typeof(decimal));
            table.Columns.Add("SumMarch", typeof(decimal));
            table.Columns.Add("SumApril", typeof(decimal));
            table.Columns.Add("SumMay", typeof(decimal));
            table.Columns.Add("SumJune", typeof(decimal));
            table.Columns.Add("SumJuly", typeof(decimal));
            table.Columns.Add("SumAugust", typeof(decimal));
            table.Columns.Add("SumSeptember", typeof(decimal));
            table.Columns.Add("SumOctober", typeof(decimal));
            table.Columns.Add("SumNovember", typeof(decimal));
            table.Columns.Add("SumDecember", typeof(decimal));

            table.Rows.Add(
                expenses.SumJanuary,
                expenses.SumFebruary,
                expenses.SumMarch,
                expenses.SumApril,
                expenses.SumMay,
                expenses.SumJune,
                expenses.SumJuly,
                expenses.SumAugust,
                expenses.SumSeptember,
                expenses.SumOctober,
                expenses.SumNovember,
                expenses.SumDecember);

            return table;
        }

        #endregion GetYearlyExpensesProMonthDataTable

        #region MergeDetails

        public int MergeDetails(int[] detailIDs)
        {
            string qry = string.Empty;

            int recordsCreated = 0;

            try
            {
                qry = "EXECUTE spMergeDetails @detailIDs, @detailsTableName";

                SqlParameter parDetailIds = new SqlParameter("@detailIDs", string.Join(",", detailIDs));
                SqlParameter parDetailsTableName = new SqlParameter("@detailsTableName", this._detailsTableName);

                recordsCreated = MHB.DAL.DataBaseConnector.ExecuteQuery(qry, this._connectionString, parDetailIds, parDetailsTableName);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("MHB.BL.MergeDetails:{0}", ex.Message), ex);
            }

            return recordsCreated;
        }

        #endregion MergeDetails

        #endregion Public Members

        #region Static Members

        #region GetUserMainTableNames

        public static Tuple<string, string> GetUserMainTableNames(int userID)
        {
            Tuple<string, string> mainTablesNames = new Tuple<string, string>(string.Empty, string.Empty);

            if (userID < 1000)
                mainTablesNames = new Tuple<string, string>(MAINTABLE_NAME_01, DETAILS_TABLE_NAME_01);
            else if (userID >= 1000 && userID < 2000)
                mainTablesNames = new Tuple<string, string>(MAINTABLE_NAME_02, DETAILS_TABLE_NAME_02);
            else if (userID >= 2000 && userID < 3000)
                mainTablesNames = new Tuple<string, string>(MAINTABLE_NAME_03, DETAILS_TABLE_NAME_03);
            else if (userID >= 3000 && userID < 4000)
                mainTablesNames = new Tuple<string, string>(MAINTABLE_NAME_04, DETAILS_TABLE_NAME_04);
            else if (userID >= 4000 && userID < 5000)
                mainTablesNames = new Tuple<string, string>(MAINTABLE_NAME_05, DETAILS_TABLE_NAME_05);
            else if (userID >= 5000 && userID < 6000)
                mainTablesNames = new Tuple<string, string>(MAINTABLE_NAME_06, DETAILS_TABLE_NAME_06);
            else if (userID >= 6000)
                mainTablesNames = new Tuple<string, string>(MAINTABLE_NAME_07, DETAILS_TABLE_NAME_07);

            return mainTablesNames;
        }

        #endregion GetUserMainTableNames

        #endregion Static Members
    }

    public struct ExpensesProMonth
    {
        public decimal SumJanuary;

        public decimal SumFebruary;

        public decimal SumMarch;

        public decimal SumApril;

        public decimal SumMay;

        public decimal SumJune;

        public decimal SumJuly;

        public decimal SumAugust;

        public decimal SumSeptember;

        public decimal SumOctober;

        public decimal SumNovember;

        public decimal SumDecember;
    }
}