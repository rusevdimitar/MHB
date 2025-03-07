using MHB.DAL;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace MHB.BL
{
    internal class SQLHelper
    {
        #region [ GetExpense ]

        public static IDataReader GetExpense(DateTime[] dates, SqlConnection connection, string mainTableName, string detailsTableName, int userID)
        {
            IDataReader reader = null;

            string qry = string.Format(@"SELECT * FROM {0} mt
                WHERE mt.IsDeleted = 0 AND mt.Month IN (@months) AND mt.Year IN (@years) AND mt.UserID = @UserID", mainTableName);

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("UserID", userID),
                new SqlParameter("months", string.Join(",", dates.Select(d => d.Month).Distinct())),
                new SqlParameter("years", string.Join(",", dates.Select(d => d.Year).Distinct()))
            };

            try
            {
                reader = MHB.DAL.DataBaseConnector.GetDataReader(qry, connection, parameters);
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(ex, "GetExpense", qry, -1, connection.ConnectionString);
            }

            return reader;
        }

        public static IDataReader GetExpense(Expression<Func<Expenditure, bool>> filter, ExpressionQueryArgs args, SqlConnection connection)
        {
            IDataReader reader = null;

            string whereClause = QueryManager.GetQuery<Expenditure>(filter);

            string qry = string.Format(@"SELECT * FROM [dbo].[{0}] {1} WHERE {2}",

                                        args["MainTableName"],
                                        filter.Parameters[0].Type.Name,
                                        whereClause);
            try
            {
                reader = MHB.DAL.DataBaseConnector.GetDataReader(qry, connection, args.SqlParameters);
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(ex, "GetExpense", qry, -1, connection.ConnectionString);
            }

            return reader;
        }

        public static IDataReader GetExpense(int userID, SqlConnection connection, string mainTableName, bool flaggedexpensessOnly = false, bool hidePaid = false, int month = -1, int year = -1, int[] expenditureIDs = null)
        {
            if (userID <= 0) throw new ArgumentNullException("[userID] parameter is null ! ! !");

            string qry = string.Empty;
            IDataReader reader = null;

            try
            {
                qry = string.Format("SELECT * FROM [dbo].[{0}] WHERE UserID = @UserID AND IsDeleted = 0", mainTableName);

                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(new SqlParameter("UserID", userID));

                if (year > 0)
                {
                    qry += " AND [Year] = @Year";
                    parameters.Add(new SqlParameter("Year", year));
                }
                if (month > 0)
                {
                    qry += " AND [Month] = @Month";
                    parameters.Add(new SqlParameter("Month", month));
                }
                if (flaggedexpensessOnly)
                {
                    qry += " AND Flagged = 1";
                }
                if (hidePaid)
                {
                    qry += " AND (IsPaid = 0 OR IsPaid IS NULL)";
                }
                if (expenditureIDs != null)
                {
                    qry += string.Format(" AND ID IN ({0})", string.Join(",", expenditureIDs));
                }
                if (hidePaid)
                {
                    qry += " AND (IsPaid = 0 OR IsPaid IS NULL)";
                }

                qry += " ORDER BY OrderID DESC";

                reader = MHB.DAL.DataBaseConnector.GetDataReader(qry, connection, parameters.ToArray());
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(ex, "GetExpense", qry, userID, connection.ConnectionString);
            }

            return reader;
        }

        #endregion [ GetExpense ]

        #region [ SearchExpenses ]

        public static IDataReader SearchExpenses(string qry, SqlConnection connection)
        {
            if (connection == null) throw new ArgumentNullException("MHB.BL.SQLHelper.SearchExpenses: ConnectionString is null!");

            IDataReader reader = null;

            try
            {
                reader = MHB.DAL.DataBaseConnector.GetDataReader(qry, connection);
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(ex, "SearchExpenses", qry, 0, connection.ConnectionString);
            }

            return reader;
        }

        #endregion [ SearchExpenses ]

        #region [ GetExpensesDataSource ]

        public static DataTable GetExpensesDataSource(int month, int year, int userID, string connectionString, string mainTableName, bool flaggedexpensessOnly)
        {
            string qry = string.Empty;
            DataTable table = null;

            try
            {
                if (flaggedexpensessOnly)
                {
                    qry = string.Format("SELECT * FROM [dbo].[{0}] WHERE Flagged = 1 AND UserID = {1} AND IsDeleted = 0 ORDER BY OrderID DESC", mainTableName, userID);
                }
                else
                {
                    qry = string.Format("SELECT * FROM [dbo].[{0}] WHERE Month = {1} AND UserID = {2} AND year = {3} AND IsDeleted = 0 ORDER BY OrderID DESC", mainTableName, month, userID, year); ;
                }

                table = MHB.DAL.DataBaseConnector.GetDataTable(qry, connectionString);
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(ex, "GetExpensesDataSource", qry, userID, connectionString);
            }

            return table;
        }

        #endregion [ GetExpensesDataSource ]

        #region [ GetParentExpensesIDs ]

        public static IDataReader GetParentExpensesIDs(int month, int year, int userID, string connectionString, string mainTableName, bool flaggedexpensessOnly)
        {
            string qry = string.Empty;
            IDataReader reader = null;

            try
            {
                qry = string.Format("SELECT [ID] FROM [dbo].[{0}] WHERE Month = {1} AND UserID = {2} AND year = {3} ORDER BY OrderID DESC", mainTableName, month, userID, year); ;

                reader = MHB.DAL.DataBaseConnector.GetDataReader(qry, connectionString);
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(ex, "GetParentExpensesIDs", qry, userID, connectionString);
            }

            return reader;
        }

        #endregion [ GetParentExpensesIDs ]

        #region [ GetChildExpenses ]

        public static IDataReader GetChildExpenses(Expression<Func<ExpenditureDetail, bool>> filter, ExpressionQueryArgs args, SqlConnection connection)
        {
            string qry = string.Empty;
            IDataReader reader = null;

            string whereClause = QueryManager.GetQuery<ExpenditureDetail>(filter);

            qry = string.Format(@"SELECT * FROM [dbo].[{0}] {1} WHERE {2}",

                                      args["DetailsTableName"],
                                      filter.Parameters[0].Type.Name,
                                      whereClause);
            try
            {
                reader = MHB.DAL.DataBaseConnector.GetDataReader(qry, connection, args.SqlParameters);
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(ex, "GetChildExpenses", qry, -1, connection.ConnectionString);
            }

            return reader;
        }

        public static IDataReader GetChildExpenses(int userID, string connectionString, string detailsTableName, string mainTableName, SqlConnection connection = null, int parentID = -1, int month = -1, int year = -1, int categoryID = -1, int productID = -1, Enums.MeasureType measureType = Enums.MeasureType.NotSet)
        {
            string qry = string.Empty;
            IDataReader reader = null;

            if (connection == null)
                connection = new SqlConnection(connectionString);

            List<SqlParameter> parameters = new List<SqlParameter>();

            try
            {
                qry = string.Format(@"SELECT * FROM dbo.{0} dt
                                      INNER JOIN {1} mt ON mt.ID = dt.ExpenditureID AND mt.UserID = @UserID
                                      WHERE dt.IsDeleted = 0", detailsTableName, mainTableName);

                parameters.Add(new SqlParameter("UserID", userID));

                if (parentID != -1)
                {
                    qry += " AND dt.ExpenditureID = @ParentID";
                    parameters.Add(new SqlParameter("ParentID", parentID));
                }

                if (month != -1)
                {
                    qry += " AND mt.Month = @Month";
                    parameters.Add(new SqlParameter("Month", month));
                }

                if (year != -1)
                {
                    qry += " AND mt.Year = @Year";
                    parameters.Add(new SqlParameter("Year", year));
                }

                if (categoryID != -1)
                {
                    qry += " AND dt.CategoryID = @CategoryID";
                    parameters.Add(new SqlParameter("CategoryID", categoryID));
                }

                if (productID != -1)
                {
                    qry += " AND dt.ProductID = @ProductID";
                    parameters.Add(new SqlParameter("ProductID", productID));
                }

                if (measureType != Enums.MeasureType.NotSet)
                {
                    qry += " AND dt.MeasureTypeID = @MeasureTypeID";
                    parameters.Add(new SqlParameter("MeasureTypeID", (int)measureType));
                }

                reader = MHB.DAL.DataBaseConnector.GetDataReader(qry, connection, parameters.ToArray());
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(ex, "GetChildExpenses", qry, userID, connectionString);
            }

            return reader;
        }

        #endregion [ GetChildExpenses ]

        #region [ GetChildExpensesDataSource ]

        public static DataTable GetChildExpensesDataSource(int parentID, int userID, string connectionString, string detailsTableName)
        {
            string qry = string.Empty;
            DataTable table = null;

            try
            {
                qry = string.Format("SELECT * FROM [dbo].[{0}] WHERE [ExpenditureID] = {1} AND IsDeleted = 0", detailsTableName, parentID);

                table = MHB.DAL.DataBaseConnector.GetDataTable(qry, connectionString);
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(ex, "GetChildExpensesDataSource", qry, userID, connectionString);
            }

            return table;
        }

        #endregion [ GetChildExpensesDataSource ]

        #region [ UpdateParentExpense ]

        public static void UpdateParentExpense(int id, string connectionString, string mainTableName, string fieldName, string fieldDescription, string fieldExpectedValue, string fieldValue, string fieldPreviousValue, string dueDate, string isPaid)
        {
            string qry = string.Empty;

            try
            {
                qry = String.Format(@"
UPDATE [dbo].[{0}] SET
[FieldName] = '{1}',
[FieldDescription] = '{2}',
[FieldExpectedValue] = {3},
[FieldValue] = {4},
[FieldOldValue] = {5},
[DueDate] = {6},
[IsPaid] = {7}
WHERE ID = {8}", mainTableName, fieldName, fieldDescription, fieldExpectedValue, fieldValue, fieldPreviousValue, dueDate, isPaid, id);

                MHB.DAL.DataBaseConnector.ExecuteQuery(qry, connectionString);
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(ex, "UpdateParentExpense", qry, 0, connectionString);
            }
        }

        #endregion [ UpdateParentExpense ]

        #region [ UpdateDateRecordLastUpdated ]

        public static void UpdateDateRecordLastUpdated(string connectionString, string mainTableName, int id)
        {
            string qry = string.Empty;

            try
            {
                qry = String.Format(@"
UPDATE [dbo].[{0}] SET [DateRecordUpdated] = GETDATE() WHERE [ID] = {1} ", mainTableName, id);

                MHB.DAL.DataBaseConnector.ExecuteQuery(qry, connectionString);
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(ex, "UpdateDateRecordLastUpdated", qry, 0, connectionString);
            }
        }

        #endregion [ UpdateDateRecordLastUpdated ]

        #region [ GetCategoryID ]

        public static int GetCategoryID(int id, string mainTableName, string fieldName, string connectionString)
        {
            string qry = string.Empty;
            int cat = 0;

            try
            {
                object result = MHB.DAL.DataBaseConnector.GetSingleValue(String.Format("EXECUTE [dbo].[spSetCostCategory] {0}, '{1}', '{2}'", id, mainTableName, fieldName), connectionString);

                if (int.TryParse(result.ToString(), out cat))
                    return cat;
                else
                    return 0;
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(ex, "GetCategoryID", qry, 0, connectionString);
            }

            return cat;
        }

        #endregion [ GetCategoryID ]

        #region [ GetCategoriesKeyWords ]

        [Obsolete]
        public static string[] GetCategoriesKeyWords(int language, int userID, string connectionString)
        {
            string qry = string.Format("EXECUTE spGetGategoryMatchingKeywords {0}, {1}", language, userID);

            List<string> result = new List<string>();

            try
            {
                IDataReader reader = MHB.DAL.DataBaseConnector.GetDataReader(qry, connectionString);

                while (reader.Read())
                {
                    result.Add(reader.Get<string>("CostNames"));
                }
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(ex, "GetCategoriesKeyWords", qry, userID, connectionString);
            }

            return result.ToArray();
        }

        #endregion [ GetCategoriesKeyWords ]

        #region [ AddNewCategory ]

        public static void AddNewCategory(int language, string categoryName, string categoryKeyWords, string categoryIconPath, bool categoryIsPayIconVisible, bool categoryIsShared, int userID, string connectionString)
        {
            string qry = string.Empty;

            try
            {
                qry = string.Format(@"EXECUTE dbo.spAddNewUserCategory
                                       @languageID
                                      ,@categoryName
                                      ,@categoryKeyWords
                                      ,@categoryIconPath
                                      ,@categoryIsPayIconVisible
                                      ,@categoryIsShared
                                      ,@userID");

                SqlParameter parLanguageID = new SqlParameter("@languageID", language);
                SqlParameter parCategoryName = new SqlParameter("@categoryName", categoryName);
                SqlParameter parCategoryKeyWords = new SqlParameter("@categoryKeyWords", categoryKeyWords);
                SqlParameter parCategoryIconPath = new SqlParameter("@categoryIconPath", categoryIconPath);
                SqlParameter parCategoryIsPayIconVisible = new SqlParameter("@categoryIsPayIconVisible", categoryIsPayIconVisible);
                SqlParameter parCategoryIsShared = new SqlParameter("@categoryIsShared", categoryIsShared);
                SqlParameter parUserID = new SqlParameter("@userID", userID);

                MHB.DAL.DataBaseConnector.ExecuteQuery(qry, connectionString, parLanguageID, parCategoryName, parCategoryKeyWords, parCategoryIconPath, parCategoryIsPayIconVisible, parCategoryIsShared, parUserID);
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(ex, "AddNewCategory", qry, userID, connectionString);
            }
        }

        #endregion [ AddNewCategory ]

        #region [ AddChildExpense ]

        private static int AddChildExpenseInternal(ExpenditureDetail e, string detailsTableName, string mainTableName, string connectionString, int userID)
        {
            if (string.IsNullOrEmpty(detailsTableName)) throw new ArgumentNullException("detailsTableName");
            if (string.IsNullOrEmpty(mainTableName)) throw new ArgumentNullException("mainTableName");
            if (string.IsNullOrEmpty(connectionString) && e.Connection == null) throw new ArgumentNullException("both connectionString and connection parameters are null ! ! !");
            if (userID <= 0) throw new ArgumentNullException("userID");

            if (e.Connection == null)
            {
                e.Connection = new SqlConnection(connectionString);
            }

            int newDetailID = -1;

            string qry = string.Empty;

            try
            {
                qry =
    string.Format(@"INSERT INTO dbo.{0} (ExpenditureID, DetailName, DetailDescription, DetailValue, DetailDate, ProductID, SupplierID, MeasureTypeID, Amount, DetailDateCreated, DetailInitialValue, CategoryID, InitialAmount, InitialMeasureTypeID, IsOcrScanned)
                    VALUES (@ExpenditureID, @DetailName, @DetailDescription, @DetailValue, GETDATE(), @ProductID, @SupplierID, @MeasureTypeID, @Amount, @DetailDateCreated, @DetailValue, @CategoryID, @Amount, @MeasureTypeID, @IsOcrScanned)

                    UPDATE {1} SET FieldOldValue = FieldValue WHERE ID = @ExpenditureID
                    DECLARE @sumDetails AS MONEY = (SELECT SUM(DetailValue) FROM {0} WHERE ExpenditureID = @ExpenditureID AND IsDeleted = 0)
                    UPDATE {1} SET FieldValue = ISNULL(@sumDetails, 0) WHERE ID = @ExpenditureID SELECT SCOPE_IDENTITY()", detailsTableName, mainTableName);

                decimal amount = 0;

                if (e.Product != null && e.Product.IsFixedMeasureType)
                {
                    e.MeasureType = e.Product.DefaultMeasureType;

                    switch (e.Product.DefaultMeasureType)
                    {
                        case Enums.MeasureType.Volume:
                            amount = e.Product.Volume;
                            break;

                        case Enums.MeasureType.Weight:
                            amount = e.Product.Weight;
                            break;

                        default:
                            amount = e.Product.PackageUnitsCount;
                            break;
                    }
                }
                else
                {
                    amount = e.Amount;
                }

                SqlParameter parExpenditureID = new SqlParameter("@ExpenditureID", e.ExpenditureID);
                SqlParameter parDetailName = new SqlParameter("@DetailName", e.DetailName ?? string.Empty);
                SqlParameter parDetailDescription = new SqlParameter("@DetailDescription", e.DetailDescription ?? string.Empty);
                SqlParameter parDetailValue = new SqlParameter("@DetailValue", e.DetailValue);
                SqlParameter parDetailDateCreated = new SqlParameter("@DetailDateCreated", e.DetailDateCreated == DateTime.MinValue ? DateTime.Now : e.DetailDateCreated);
                SqlParameter parProductID = new SqlParameter("@ProductID", e.ProductID);
                SqlParameter parSupplierID = new SqlParameter("@SupplierID", e.SupplierID);
                SqlParameter parMeasureTypeID = new SqlParameter("@MeasureTypeID", e.MeasureType);
                SqlParameter parIsOcrScanned = new SqlParameter("@IsOcrScanned", e.IsOcrScanned);
                SqlParameter parAmount = new SqlParameter("@Amount", amount);
                SqlParameter parCategoryID = new SqlParameter("@CategoryID", e.Product == null ? e.CategoryID : e.Product.CategoryID);

                object result = MHB.DAL.DataBaseConnector.GetSingleValue(qry, e.Connection, parExpenditureID, parDetailName, parDetailDescription, parDetailValue, parDetailDateCreated, parProductID, parSupplierID, parMeasureTypeID, parAmount, parCategoryID, parIsOcrScanned);

                if (int.TryParse(result.ToString(), out newDetailID))
                {
                    e.ID = newDetailID;
                }

                SQLHelper.AddChildExpenseProductParameters(e);
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(ex, "MHB.BL.SQLHelper.AddChildExpense", qry, e.UserID, e.Connection.ConnectionString);
            }

            return newDetailID;
        }

        internal static int AddChildExpense(ExpenditureDetail e)
        {
            return SQLHelper.AddChildExpenseInternal(e, e.DetailsTableName, e.MainTableName, e.ConnectionString, e.UserID);
        }

        internal static int AddChildExpense(ExpenditureDetail e, string detailsTableName, string mainTableName, string connectionString, int userID)
        {
            return SQLHelper.AddChildExpenseInternal(e, detailsTableName, mainTableName, connectionString, userID);
        }

        #endregion [ AddChildExpense ]

        #region [ AddChildExpenseProductParameters ]

        internal static void AddChildExpenseProductParameters(ExpenditureDetail detail)
        {
            if (detail == null) throw new ArgumentNullException("parameter detail is null ! ! !");

            string qry = string.Empty;

            try
            {
                if (detail.Product != null && detail.Product.Parameters != null && detail.Product.Parameters.Any())
                {
                    ProductParameter[] productParameters = detail.Product.Parameters.ToArray();

                    List<SqlParameter> parameters = new List<SqlParameter>();

                    for (int i = 0; i < productParameters.Length; i++)
                    {
                        qry += string.Format("INSERT INTO tbProductParameters (ParentID, ProductID, [Key], Value, ProductParameterType) VALUES (@ParentID{0}, @ProductID{0}, @Key{0}, @Value{0}, @ProductParameterType{0})", i);

                        parameters.Add(new SqlParameter(string.Format("ParentID{0}", i), productParameters[i].ParentID));
                        parameters.Add(new SqlParameter(string.Format("ProductID{0}", i), productParameters[i].ProductID));
                        parameters.Add(new SqlParameter(string.Format("Key{0}", i), productParameters[i].Key));
                        parameters.Add(new SqlParameter(string.Format("Value{0}", i), productParameters[i].Value));
                        parameters.Add(new SqlParameter(string.Format("ProductParameterType{0}", i), productParameters[i].ProductParameterTypeID));
                    }

                    MHB.DAL.DataBaseConnector.ExecuteQuery(qry, detail.Connection, parameters.ToArray());
                }
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(ex, "MHB.BL.SQLHelper.AddChildExpenseProductParameters", qry, detail.UserID, detail.ConnectionString);
            }
        }

        #endregion [ AddChildExpenseProductParameters ]

        // Product parameters ---->

        #region [ UpdateProductParameters ]

        internal static int UpdateProductParameters(IEnumerable<ExpenditureDetail> parentExpenseDetails, SqlConnection connection, string detailsTableName, string connectionString = "")
        {
            if (string.IsNullOrEmpty(connectionString) && connection == null) throw new ArgumentNullException("Both connection and connectionString parameters are null ! ! !");

            if (connection == null)
                connection = new SqlConnection(connectionString);

            int result = -1;

            try
            {
                string qry = string.Empty;

                List<SqlParameter> parameters = new List<SqlParameter>();

                foreach (ExpenditureDetail expenditureDetail in parentExpenseDetails)
                {
                    bool hasProductParameters = false;

                    if (expenditureDetail.ProductID != Product.PRODUCT_DEFAULT_ID && expenditureDetail.Product.Parameters != null && expenditureDetail.Product.Parameters.Any())
                    {
                        var productParameters = expenditureDetail.Product.Parameters.ToArray();

                        for (int i = 0; i < productParameters.Count(); i++)
                        {
                            int sqlParameterUniqueIdentifier = i + expenditureDetail.ID;

                            if (productParameters[i].ID == 0) // if Product.Parameters array is newly initialized delete all existing of that type and insert;
                            {
                                qry +=
string.Format("DELETE FROM tbProductParameters WHERE ParentID = @ParentID{0} AND ProductParameterType = @ProductParameterType{0} ", sqlParameterUniqueIdentifier);

                                qry +=
string.Format("INSERT INTO tbProductParameters (ParentID, ProductID, [Key], Value, ProductParameterType) VALUES (@ParentID{0}, @ProductID{0}, @Key{0}, @Value{0}, @ProductParameterType{0}) ", sqlParameterUniqueIdentifier);
                            }
                            else
                            {
                                qry +=
        string.Format("UPDATE tbProductParameters SET ParentID = @ParentID{0}, ProductID = @ProductID{0}, [Key] = @Key{0}, Value = @Value{0}, ProductParameterType = @ProductParameterType{0} WHERE ID = @ID{0} ", sqlParameterUniqueIdentifier);
                            }

                            parameters.Add(new SqlParameter(string.Format("ParentID{0}", sqlParameterUniqueIdentifier), productParameters[i].ParentID));
                            parameters.Add(new SqlParameter(string.Format("ProductID{0}", sqlParameterUniqueIdentifier), productParameters[i].ProductID));
                            parameters.Add(new SqlParameter(string.Format("Key{0}", sqlParameterUniqueIdentifier), productParameters[i].Key));
                            parameters.Add(new SqlParameter(string.Format("Value{0}", sqlParameterUniqueIdentifier), productParameters[i].Value));
                            parameters.Add(new SqlParameter(string.Format("ID{0}", sqlParameterUniqueIdentifier), productParameters[i].ID));
                            parameters.Add(new SqlParameter(string.Format("ProductParameterType{0}", sqlParameterUniqueIdentifier), productParameters[i].ProductParameterTypeID));
                        }

                        hasProductParameters = true;
                    }
                    else
                    {
                        qry += string.Format("DELETE FROM tbProductParameters WHERE ParentID = @ParentID1{0} ", expenditureDetail.ID);
                    }

                    qry += string.Format("UPDATE {0} SET HasProductParameters = @HasProductParameters{1} WHERE ID = @ParentID1{1} ", detailsTableName, expenditureDetail.ID);

                    parameters.Add(new SqlParameter(string.Format("ParentID1{0}", expenditureDetail.ID), expenditureDetail.ID));
                    parameters.Add(new SqlParameter(string.Format("HasProductParameters{0}", expenditureDetail.ID), hasProductParameters));
                }

                if (!string.IsNullOrEmpty(qry))
                    result = MHB.DAL.DataBaseConnector.ExecuteQuery(qry, connection, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("MHB.BL.SQLHelper.UpdateProductParameters: {0}", ex.Message), ex);
            }

            return result;
        }

        #endregion [ UpdateProductParameters ]

        #region [ GetProductParameters ]

        internal static IEnumerable<ProductParameter> GetProductParameters(int parentID, int productID, int userID, SqlConnection connection, string connectionString = "")
        {
            if (connection == null && string.IsNullOrEmpty(connectionString)) throw new ArgumentNullException("both connection and connectionString parameters are null ! ! !");

            if (connection == null) connection = new SqlConnection(connectionString);

            string qry = string.Empty;

            IDataReader reader = null;

            List<ProductParameter> results = new List<ProductParameter>();

            try
            {
                qry = "SELECT * FROM tbProductParameters WHERE ParentID = (CASE WHEN @ParentID = 0 THEN ParentID ELSE @ParentID END) AND ProductID = @ProductID";

                SqlParameter parParentID = new SqlParameter("ParentID", parentID);
                SqlParameter parProductID = new SqlParameter("ProductID", productID);

                reader = MHB.DAL.DataBaseConnector.GetDataReader(qry, connection, parParentID, parProductID);

                while (reader.Read())
                {
                    ProductParameter parameter = new ProductParameter();

                    int index = -1;

                    index = reader.GetOrdinal("ID");

                    if (!reader.IsDBNull(index))
                        parameter.ID = (int)reader[index];

                    index = reader.GetOrdinal("Key");

                    if (!reader.IsDBNull(index))
                        parameter.Key = reader[index].ToString();

                    index = reader.GetOrdinal("Value");

                    if (!reader.IsDBNull(index))
                        parameter.Value = reader[index].ToString();

                    index = reader.GetOrdinal("ProductParameterType");

                    if (!reader.IsDBNull(index))
                        parameter.ProductParameterTypeID = (int)reader[index];

                    parameter.ParentID = parentID;

                    parameter.ProductID = productID;

                    //parameter.Product = new Product(productID, userID, connection);

                    results.Add(parameter);
                }
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(ex, "MHB.BL.SQLHelper.GetProductParameters", qry, userID, connection.ConnectionString);
            }
            finally
            {
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }

            return results;
        }

        #endregion [ GetProductParameters ]

        #region [ AddProductParameter ]

        internal static void AddProductParameter(string key, string value, int parentID, int productID, int productParameterType, SqlConnection connection, string detailsTableName, string connectionString = "")
        {
            if (connection == null && string.IsNullOrEmpty(connectionString)) throw new ArgumentNullException("both connection and connectionString parameters are null ! ! !");

            if (connection == null) connection = new SqlConnection(connectionString);

            string qry = string.Empty;

            try
            {
                qry = string.Format(
@"INSERT INTO tbProductParameters (ParentID, ProductID, Key, Value, ProductParameterType) VALUES (@ParentID, @ProductID, @Key, @Value, @ProductParameterType)
 UPDATE {0} SET HasProductParameters = 1 WHERE ID = @ParentID", detailsTableName);

                SqlParameter parParentID = new SqlParameter("ParentID", parentID);

                SqlParameter parProductID = new SqlParameter("ProductID", productID);

                SqlParameter parKey = new SqlParameter("Key", key);

                SqlParameter parValue = new SqlParameter("Value", value);

                SqlParameter parProductParameterType = new SqlParameter("@ProductParameterType", value);

                MHB.DAL.DataBaseConnector.ExecuteQuery(qry, connection, parParentID, parProductID, parKey, parValue, parProductParameterType);
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(ex, "MHB.BL.SQLHelper.AddProductParameter", qry, 0, connection.ConnectionString);
            }
        }

        #endregion [ AddProductParameter ]

        #region [ DeleteProductParameter ]

        internal static bool DeleteProductParameter(ProductParameter productParameter)
        {
            if (productParameter == null) throw new ArgumentNullException("productParameter ! ! !");

            if (productParameter.Connection == null) throw new ArgumentNullException("Connection ! ! !");

            bool result = false;

            string qry = string.Empty;

            try
            {
                qry = "DELETE FROM tbProductParameters WHERE ID = @ID";

                SqlParameter parID = new SqlParameter("ID", productParameter.ID);

                int rowsAffected = MHB.DAL.DataBaseConnector.ExecuteQuery(qry, productParameter.Connection, parID);

                if (rowsAffected > 0)
                    result = true;
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(ex, "MHB.BL.SQLHelper.DeleteProductParameter", qry, 0, productParameter.Connection.ConnectionString);
            }

            return result;
        }

        #endregion [ DeleteProductParameter ]

        // Product parameters <----

        #region [ DeleteChildExpense ]

        [Obsolete]
        public static int DeleteChildExpense(List<ExpenditureDetail> expensesToDelete, string detailsTableName, string connectionString, int userID)
        {
            int result = -1;

            string qry = string.Empty;

            try
            {
                foreach (ExpenditureDetail expense in expensesToDelete)
                {
                    qry += string.Format(@"DELETE FROM tbProductParameters WHERE ParentID = {1} DELETE FROM {0} WHERE ID = {1} ", detailsTableName, expense.ID);
                }

                result = MHB.DAL.DataBaseConnector.ExecuteQuery(qry, connectionString);
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(ex, "DeleteChildExpense", qry, userID, connectionString);
            }

            return result;
        }

        #endregion [ DeleteChildExpense ]

        #region [ DeleteCategory ]

        public static void DeleteCategory(string connectionString, int categoryID)
        {
            string qry = string.Empty;

            try
            {
                qry = "EXECUTE spDeleteUserDefinedCategory @categoryID";

                SqlParameter parCategoryID = new SqlParameter("@categoryID", categoryID);

                MHB.DAL.DataBaseConnector.ExecuteQuery(qry, connectionString, parCategoryID);
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(ex, "DeleteCategory", qry, 0, connectionString);
            }
        }

        #endregion [ DeleteCategory ]

        #region [ UpdateBudgets ]

        [Obsolete("This table is no longer used; see: http://mantis.myhomebills.info/view.php?id=92#c2196")]
        public static void UpdateBudgets(string connectionString, decimal expenses, decimal budget, decimal savings, int month, int year, int userID)
        {
            string qry = string.Empty;

            try
            {
                switch (month)
                {
                    case 1:
                        qry += String.Format(@"
UPDATE [dbo].[tbMonthlyExpenses] SET [SumJanuary] = {0} WHERE [UserID] = {1} AND year = {2} ", expenses, userID, year);

                        qry += String.Format(@"
UPDATE [dbo].[tbMonthlyBudget] SET [BudgetJan] = {0} WHERE [UserID] = {1} AND year = {2} ", budget, userID, year);

                        qry += String.Format(@"
UPDATE [dbo].[tbMonthlysavings] SET [SavingsJan] = {0} WHERE [UserID] = {1} AND [year] = {2} ", savings, userID, year);

                        break;

                    case 2:
                        qry += String.Format(@"
UPDATE [dbo].[tbMonthlyExpenses] SET [SumFebruary] = {0} WHERE [UserID] = {1} AND year = {2}", expenses, userID, year);

                        qry += String.Format(@"
UPDATE [dbo].[tbMonthlyBudget] SET [BudgetFeb] = {0} WHERE [UserID] = {1} AND year = {2} ", budget, userID, year);

                        qry += String.Format(@"
UPDATE [dbo].[tbMonthlysavings] SET [SavingsFeb] = {0} WHERE [UserID] = {1} AND [year] = {2} ", savings, userID, year);

                        break;

                    case 3:
                        qry += String.Format(@"
UPDATE [dbo].[tbMonthlyExpenses] SET [SumMarch] = {0} WHERE [UserID] = {1} AND year = {2} ", expenses, userID, year);

                        qry += String.Format(@"
UPDATE [dbo].[tbMonthlyBudget] SET [BudgetMar] = {0} WHERE [UserID] = {1} AND year = {2} ", budget, userID, year);

                        qry += String.Format(@"
UPDATE [dbo].[tbMonthlysavings] SET [SavingsMar] = {0} WHERE [UserID] = {1} AND [year] = {2} ", savings, userID, year);

                        break;

                    case 4:
                        qry += String.Format(@"
UPDATE [dbo].[tbMonthlyExpenses] SET [SumApril] = {0} WHERE [UserID] = {1} AND year = {2} ", expenses, userID, year);

                        qry += String.Format(@"
UPDATE [dbo].[tbMonthlyBudget] SET [BudgetApr] = {0} WHERE [UserID] = {1} AND year = {2} ", budget, userID, year);

                        qry += String.Format(@"
UPDATE [dbo].[tbMonthlysavings] SET [SavingsApr] = {0} WHERE [UserID] = {1} AND [year] = {2} ", savings, userID, year);

                        break;

                    case 5:
                        qry += String.Format(@"
UPDATE [dbo].[tbMonthlyExpenses] SET [SumMay] = {0} WHERE [UserID] = {1} AND year = {2} ", expenses, userID, year);

                        qry += String.Format(@"
UPDATE [dbo].[tbMonthlyBudget] SET [BudgetMay] = {0} WHERE [UserID] = {1} AND year = {2} ", budget, userID, year);

                        qry += String.Format(@"
UPDATE [dbo].[tbMonthlysavings] SET [SavingsMay] = {0} WHERE [UserID] = {1} AND [year] = {2} ", savings, userID, year);

                        break;

                    case 6:
                        qry += String.Format(@"
UPDATE [dbo].[tbMonthlyExpenses] SET [SumJune] = {0} WHERE [UserID] = {1} AND year = {2} ", expenses, userID, year);

                        qry += String.Format(@"
UPDATE [dbo].[tbMonthlyBudget] SET [BudgetJune] = {0} WHERE [UserID] = {1} AND year = {2} ", budget, userID, year);

                        qry += String.Format(@"
UPDATE [dbo].[tbMonthlysavings] SET [SavingsJune] = {0} WHERE [UserID] = {1} AND [year] = {2} ", savings, userID, year);

                        break;

                    case 7:
                        qry += String.Format(@"
UPDATE [dbo].[tbMonthlyExpenses] SET [SumJuly] = {0} WHERE [UserID] = {1} AND year = {2} ", expenses, userID, year);

                        qry += String.Format(@"
UPDATE [dbo].[tbMonthlyBudget] SET [BudgetJuly] = {0} WHERE [UserID] = {1} AND year = {2} ", budget, userID, year);

                        qry += String.Format(@"
UPDATE [dbo].[tbMonthlysavings] SET [SavingsJuly] = {0} WHERE [UserID] = {1} AND [year] = {2} ", savings, userID, year);

                        break;

                    case 8:
                        qry += String.Format(@"
UPDATE [dbo].[tbMonthlyExpenses] SET [SumAugust] = {0} WHERE [UserID] = {1} AND year = {2} ", expenses, userID, year);

                        qry += String.Format(@"
UPDATE [dbo].[tbMonthlyBudget] SET [BudgetAug] = {0} WHERE [UserID] = {1} AND year = {2} ", budget, userID, year);

                        qry += String.Format(@"
UPDATE [dbo].[tbMonthlysavings] SET [savingsAug] = {0} WHERE [UserID] = {1} AND [year] = {2} ", savings, userID, year);

                        break;

                    case 9:
                        qry += String.Format(@"
UPDATE [dbo].[tbMonthlyExpenses] SET [SumSeptember] = {0} WHERE [UserID] = {1} AND year = {2} ", expenses, userID, year);

                        qry += String.Format(@"
UPDATE [dbo].[tbMonthlyBudget] SET [BudgetSept] = {0} WHERE [UserID] = {1} AND year = {2} ", budget, userID, year);

                        qry += String.Format(@"
UPDATE [dbo].[tbMonthlysavings] SET [SavingsAug] = {0} WHERE [UserID] = {1} AND [year] = {2} ", savings, userID, year);

                        break;

                    case 10:
                        qry += String.Format(@"
UPDATE [dbo].[tbMonthlyExpenses] SET [SumOctober] = {0} WHERE [UserID] = {1} AND year = {2}", expenses, userID, year);

                        qry += String.Format(@"
UPDATE [dbo].[tbMonthlyBudget] SET [BudgetOct] = {0} WHERE [UserID] = {1} AND year = {2}", budget, userID, year);

                        qry += String.Format(@"
UPDATE [dbo].[tbMonthlysavings] SET [SavingsOct] = {0} WHERE [UserID] = {1} AND [year] = {2}", savings, userID, year);

                        break;

                    case 11:
                        qry += String.Format(@"
UPDATE [dbo].[tbMonthlyExpenses] SET [SumNovember] = {0} WHERE [UserID] = {1} AND year = {2} ", expenses, userID, year);

                        qry += String.Format(@"
UPDATE [dbo].[tbMonthlyBudget] SET [BudgetNov] = {0} WHERE [UserID] = {1} AND year = {2} ", budget, userID, year);

                        qry += String.Format(@"
UPDATE [dbo].[tbMonthlysavings] SET [SavingsNov] = {0} WHERE [UserID] = {1} AND [year] = {2} ", savings, userID, year);

                        break;

                    case 12:
                        qry += String.Format(@"
UPDATE [dbo].[tbMonthlyExpenses] SET [SumDecember] = {0} WHERE [UserID] = {1} AND year = {2} ", expenses, userID, year);

                        qry += String.Format(@"
UPDATE [dbo].[tbMonthlyBudget] SET [BudgetDec] = {0} WHERE [UserID] = {1} AND year = {2} ", budget, userID, year);

                        qry += String.Format(@"
UPDATE [dbo].[tbMonthlysavings] SET [SavingsDec] = {0} WHERE [UserID] = {1} AND [year] = {2} ", savings, userID, year);

                        break;
                }

                MHB.DAL.DataBaseConnector.ExecuteQuery(qry, connectionString);
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(ex, "UpdateBudgets", qry, userID, connectionString);
            }
        }

        #endregion [ UpdateBudgets ]

        #region [ GetMonthlySavings ]

        public static void GetMonthlySavingsAndBudget(int month, int year, int userID, string connectionString, out double monthlyBudget, out double monthlySaving, out string qryToLog)
        {
            string qry = string.Empty;

            double savings = 0;
            double budget = 0;

            try
            {
                switch (month)
                {
                    case 1:
                        qry = String.Format(@"
    SELECT ISNULL([SavingsJan], 0) AS Savings, (SELECT ISNULL([BudgetJan], 0) FROM [dbo].[tbMonthlyBudget] WHERE [UserID] = {0} AND Year = {1}) AS Budget
    FROM [dbo].[tbMonthlySavings] WHERE [UserID] = {0} AND [Year] = {1}", userID, year);

                        break;

                    case 2:
                        qry = String.Format(@"
    SELECT ISNULL([SavingsFeb], 0) AS Savings, (SELECT ISNULL([BudgetFeb], 0) FROM [dbo].[tbMonthlyBudget] WHERE [UserID] = {0} AND Year = {1}) AS Budget
    FROM [dbo].[tbMonthlySavings] WHERE [UserID] = {0} AND [Year] = {1}", userID, year);

                        break;

                    case 3:
                        qry = String.Format(@"
    SELECT ISNULL([SavingsMar], 0) AS Savings, (SELECT ISNULL([BudgetMar], 0) FROM [dbo].[tbMonthlyBudget] WHERE [UserID] = {0} AND Year = {1}) AS Budget
    FROM [dbo].[tbMonthlySavings] WHERE [UserID] = {0} AND [Year] = {1}", userID, year);

                        break;

                    case 4:
                        qry = String.Format(@"
    SELECT ISNULL([SavingsApr], 0) AS Savings, (SELECT ISNULL([BudgetApr], 0) FROM [dbo].[tbMonthlyBudget] WHERE [UserID] = {0} AND Year = {1}) AS Budget
    FROM [dbo].[tbMonthlySavings] WHERE [UserID] = {0} AND [Year] = {1}", userID, year);

                        break;

                    case 5:
                        qry = String.Format(@"
    SELECT ISNULL([SavingsMay], 0) AS Savings, (SELECT ISNULL([BudgetMay], 0) FROM [dbo].[tbMonthlyBudget] WHERE [UserID] = {0} AND Year = {1}) AS Budget
    FROM [dbo].[tbMonthlySavings] WHERE [UserID] = {0} AND [Year] = {1}", userID, year);

                        break;

                    case 6:
                        qry = String.Format(@"
    SELECT ISNULL([SavingsJune], 0) AS Savings, (SELECT ISNULL([BudgetJune], 0) FROM [dbo].[tbMonthlyBudget] WHERE [UserID] = {0} AND Year = {1}) AS Budget
    FROM [dbo].[tbMonthlySavings] WHERE [UserID] = {0} AND [Year] = {1}", userID, year);

                        break;

                    case 7:
                        qry = String.Format(@"
    SELECT ISNULL([SavingsJuly], 0) AS Savings, (SELECT ISNULL([BudgetJuly], 0) FROM [dbo].[tbMonthlyBudget] WHERE [UserID] = {0} AND Year = {1}) AS Budget
    FROM [dbo].[tbMonthlySavings] WHERE [UserID] = {0} AND [Year] = {1}", userID, year);

                        break;

                    case 8:
                        qry = String.Format(@"
    SELECT ISNULL([SavingsAug], 0) AS Savings,(SELECT ISNULL([BudgetAug], 0) FROM [dbo].[tbMonthlyBudget] WHERE [UserID] = {0} AND Year = {1}) AS Budget
    FROM [dbo].[tbMonthlySavings] WHERE [UserID] = {0} AND [Year] = {1}", userID, year);

                        break;

                    case 9:
                        qry = String.Format(@"
    SELECT ISNULL([SavingsSept], 0) AS Savings, (SELECT ISNULL([BudgetSept], 0) FROM [dbo].[tbMonthlyBudget] WHERE [UserID] = {0} AND Year = {1}) AS Budget
    FROM [dbo].[tbMonthlySavings] WHERE [UserID] = {0} AND [Year] = {1}", userID, year);

                        break;

                    case 10:
                        qry = String.Format(@"
    SELECT ISNULL([SavingsOct], 0) AS Savings, (SELECT ISNULL([BudgetOct], 0) FROM [dbo].[tbMonthlyBudget] WHERE [UserID] = {0} AND Year = {1}) AS Budget
    FROM [dbo].[tbMonthlySavings] WHERE [UserID] = {0} AND [Year] = {1}", userID, year);

                        break;

                    case 11:
                        qry = String.Format(@"
    SELECT ISNULL([SavingsNov], 0) AS Savings, (SELECT ISNULL([BudgetNov], 0) FROM [dbo].[tbMonthlyBudget] WHERE [UserID] = {0} AND Year = {1}) AS Budget
    FROM [dbo].[tbMonthlySavings] WHERE [UserID] = {0} AND [Year] = {1}", userID, year);

                        break;

                    case 12:
                        qry = String.Format(@"
    SELECT ISNULL([SavingsDec], 0) AS Savings, (SELECT ISNULL([BudgetDec], 0) FROM [dbo].[tbMonthlyBudget] WHERE [UserID] = {0} AND Year = {1}) AS Budget
    FROM [dbo].[tbMonthlySavings] WHERE [UserID] = {0} AND [Year] = {1}", userID, year);

                        break;
                }

                IDataReader reader = MHB.DAL.DataBaseConnector.GetDataReader(qry, connectionString);

                while (reader.Read())
                {
                    savings = Convert.ToDouble(reader["Savings"]);
                    budget = Convert.ToDouble(reader["Budget"]);
                }
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(ex, "GetMonthlySavingsAndBudget", qry, userID, connectionString);
            }

            monthlySaving = savings;
            monthlyBudget = budget;
            qryToLog = qry;
        }

        #endregion [ GetMonthlySavings ]

        #region [ GetCategories ]

        public static IEnumerable<Category> GetCategories(string connectionString, int languageID, int userID)
        {
            string qry = string.Empty;
            List<Category> dataSource = new List<Category>();

            try
            {
                qry = @"EXECUTE spGetCategoriesTable @LanguageID, @UserID, NULL";

                SqlParameter parLanguageID = new SqlParameter("@LanguageID", languageID);
                SqlParameter parUserID = new SqlParameter("@UserID", userID);

                IDataReader reader = MHB.DAL.DataBaseConnector.GetDataReader(qry, connectionString, parLanguageID, parUserID);

                while (reader.Read())
                {
                    Category c = new Category();

                    if (!reader.IsDBNull(reader.GetOrdinal("CategoryID")))
                        c.ID = (int)reader["CategoryID"];

                    if (!reader.IsDBNull(reader.GetOrdinal("CategoryName")))
                        c.Name = (string)reader["CategoryName"];

                    if (!reader.IsDBNull(reader.GetOrdinal("CategoryKeyWords")))
                        c.CategoryKeywords = (string)reader["CategoryKeyWords"];

                    if (!reader.IsDBNull(reader.GetOrdinal("IconPath")))
                        c.IconPath = (string)reader["IconPath"];

                    if (!reader.IsDBNull(reader.GetOrdinal("IsPayIconVisible")))
                        c.IsPayIconVisible = (bool)reader["IsPayIconVisible"];

                    if (!reader.IsDBNull(reader.GetOrdinal("IsShared")))
                        c.IsShared = (bool)reader["IsShared"];

                    if (!reader.IsDBNull(reader.GetOrdinal("UserCategoryID")))
                        c.UserID = (int)reader["UserCategoryID"];

                    if (!reader.IsDBNull(reader.GetOrdinal("CommentsCount")))
                        c.CommentsCount = (int)reader["CommentsCount"];

                    dataSource.Add(c);
                }
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(ex, "GetCategories", qry, userID, connectionString);
            }

            return dataSource;
        }

        #endregion [ GetCategories ]

        #region [ GetCategoryComments ]

        public static List<CategoryComment> GetCategoryComments(int categoryID, int userID, string connectionString)
        {
            string qry = string.Empty;

            List<CategoryComment> comments = new List<CategoryComment>();

            try
            {
                qry = "SELECT * FROM tbCategoryComments WHERE CategoryID = @CategoryID";

                SqlParameter parCategoryID = new SqlParameter("@CategoryID", categoryID);

                IDataReader reader = MHB.DAL.DataBaseConnector.GetDataReader(qry, connectionString, parCategoryID);

                while (reader.Read())
                {
                    CategoryComment comment = new CategoryComment();

                    if (!reader.IsDBNull(reader.GetOrdinal("ID")))
                        comment.ID = (int)reader["ID"];

                    if (!reader.IsDBNull(reader.GetOrdinal("UserID")))
                        comment.UserID = (int)reader["UserID"];

                    if (!reader.IsDBNull(reader.GetOrdinal("CategoryID")))
                        comment.CategoryID = (int)reader["CategoryID"];

                    if (!reader.IsDBNull(reader.GetOrdinal("Comment")))
                        comment.Comment = (string)reader["Comment"];

                    if (!reader.IsDBNull(reader.GetOrdinal("Poster")))
                        comment.Poster = (string)reader["Poster"];

                    if (!reader.IsDBNull(reader.GetOrdinal("PositiveVotes")))
                        comment.PositiveVotesCount = (int)reader["PositiveVotes"];

                    if (!reader.IsDBNull(reader.GetOrdinal("NegativeVotes")))
                        comment.NegativeVotesCount = (int)reader["NegativeVotes"];

                    if (!reader.IsDBNull(reader.GetOrdinal("UsersVoted")))
                        comment.UsersVoted = reader["UsersVoted"].ToString().Split(',').Select(item => Convert.ToInt32(item)).ToArray();

                    if (!reader.IsDBNull(reader.GetOrdinal("IsDeleted")))
                        comment.IsDeleted = (bool)reader["IsDeleted"];

                    if (!reader.IsDBNull(reader.GetOrdinal("DateModified")))
                        comment.DateModified = (DateTime)reader["DateModified"];

                    comments.Add(comment);
                }
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(ex, "GetCategoryComments", qry, userID, connectionString);
            }

            return comments;
        }

        #endregion [ GetCategoryComments ]

        #region [ AddCategoryComment ]

        public static void AddCategoryComment(CategoryCommentBase categoryComment)
        {
            string qry = string.Empty;

            try
            {
                qry = @"INSERT INTO dbo.tbCategoryComments (UserID, CategoryID, Comment, Poster, UsersVoted, DateModified)
                        VALUES (@UserID, @CategoryID, @Comment, @Poster, @UsersVoted, GETDATE())";

                SqlParameter parUserID = new SqlParameter("@UserID", categoryComment.UserID);
                SqlParameter parCategoryID = new SqlParameter("@CategoryID", categoryComment.CategoryID);
                SqlParameter parComment = new SqlParameter("@Comment", categoryComment.Comment);
                SqlParameter parPoster = new SqlParameter("@Poster", categoryComment.Poster);
                SqlParameter parUsersVoted = new SqlParameter("@UsersVoted", categoryComment.UserID);

                MHB.DAL.DataBaseConnector.ExecuteQuery(qry, categoryComment.ConnectionString, parUserID, parCategoryID, parComment, parPoster, parUsersVoted);
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(ex, "AddCategoryComment", qry, categoryComment.UserID, categoryComment.ConnectionString);
            }
        }

        #endregion [ AddCategoryComment ]

        #region [ VoteOnCategoryComment ]

        public static void VoteOnCategoryComment(CategoryCommentBase categoryComment, bool voteUp, int voterUserID)
        {
            string qry = string.Empty;

            try
            {
                if (voteUp)
                {
                    qry = string.Format(@"UPDATE dbo.tbCategoryComments
                            SET PositiveVotes = (SELECT MAX(PositiveVotes) FROM dbo.tbCategoryComments WHERE ID = @CategoryCommentID) + 1,
                                UsersVoted = (SELECT ISNULL(UsersVoted, '') FROM dbo.tbCategoryComments WHERE ID = @CategoryCommentID) + ', {0}'
                            WHERE ID = @CategoryCommentID", voterUserID);
                }
                else
                {
                    qry = string.Format(@"UPDATE dbo.tbCategoryComments
                            SET NegativeVotes = (SELECT MAX(NegativeVotes) FROM dbo.tbCategoryComments WHERE ID = @CategoryCommentID) + 1,
                                UsersVoted = (SELECT ISNULL(UsersVoted, '') FROM dbo.tbCategoryComments WHERE ID = @CategoryCommentID) + ', {0}'
                            WHERE ID = @CategoryCommentID", voterUserID);
                }

                SqlParameter parCategoryCommentID = new SqlParameter("@CategoryCommentID", categoryComment.ID);

                MHB.DAL.DataBaseConnector.ExecuteQuery(qry, categoryComment.ConnectionString, parCategoryCommentID);
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(ex, "VoteOnCategoryComment", qry, categoryComment.UserID, categoryComment.ConnectionString);
            }
        }

        #endregion [ VoteOnCategoryComment ]

        #region [ LoadCategoryComment ]

        public static CategoryComment LoadCategoryComment(int commentID, string connectionString)
        {
            string qry = string.Empty;

            CategoryComment comment = new CategoryComment();

            try
            {
                qry = "SELECT * FROM tbCategoryComments WHERE ID = @CommentID";

                SqlParameter parCommentID = new SqlParameter("@CommentID", commentID);

                IDataReader reader = MHB.DAL.DataBaseConnector.GetDataReader(qry, connectionString, parCommentID);

                while (reader.Read())
                {
                    comment.ID = commentID;

                    if (!reader.IsDBNull(reader.GetOrdinal("UserID")))
                        comment.UserID = (int)reader["UserID"];

                    if (!reader.IsDBNull(reader.GetOrdinal("CategoryID")))
                        comment.CategoryID = (int)reader["CategoryID"];

                    if (!reader.IsDBNull(reader.GetOrdinal("Poster")))
                        comment.Poster = (string)reader["Poster"];

                    if (!reader.IsDBNull(reader.GetOrdinal("Comment")))
                        comment.Comment = (string)reader["Comment"];

                    if (!reader.IsDBNull(reader.GetOrdinal("PositiveVotes")))
                        comment.PositiveVotesCount = (int)reader["PositiveVotes"];

                    if (!reader.IsDBNull(reader.GetOrdinal("NegativeVotes")))
                        comment.NegativeVotesCount = (int)reader["NegativeVotes"];

                    if (!reader.IsDBNull(reader.GetOrdinal("UsersVoted")))
                        comment.UsersVoted = reader["UsersVoted"].ToString().Split(',').Select(item => Convert.ToInt32(item)).ToArray();

                    if (!reader.IsDBNull(reader.GetOrdinal("IsDeleted")))
                        comment.IsDeleted = (bool)reader["IsDeleted"];

                    if (!reader.IsDBNull(reader.GetOrdinal("DateModified")))
                        comment.DateModified = (DateTime)reader["DateModified"];

                    comment.ConnectionString = connectionString;
                }
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(ex, "LoadCategoryComment", qry, comment.UserID, connectionString);
            }

            return comment;
        }

        #endregion [ LoadCategoryComment ]

        #region [ DeleteCategoryComment ]

        public static void DeleteCategoryComment(int commentID, string connectionString)
        {
            string qry = string.Empty;

            try
            {
                qry = "UPDATE tbCategoryComments SET IsDeleted = 1 WHERE ID = @CommentID";

                SqlParameter parCommentID = new SqlParameter("@CommentID", commentID);

                MHB.DAL.DataBaseConnector.ExecuteQuery(qry, connectionString, parCommentID);
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(ex, "DeleteCategoryComment", qry, 0, connectionString);
            }
        }

        #endregion [ DeleteCategoryComment ]

        #region [ CopyCategory ]

        public static int CopyCategory(string connectionString, int sourceCategoryID, int destinationUserID)
        {
            string qry = string.Empty;
            int newCategoryID = 0;

            try
            {
                qry = @"EXECUTE spCopyCategory @sourceCategoryID, @destinationCategoryID";

                SqlParameter parSourceCategoryID = new SqlParameter("@sourceCategoryID", sourceCategoryID);
                SqlParameter parDestinationUserID = new SqlParameter("@destinationCategoryID", destinationUserID);

                object result = MHB.DAL.DataBaseConnector.GetSingleValue(qry, connectionString, parSourceCategoryID, parDestinationUserID);

                int.TryParse(result.ToString(), out newCategoryID);
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(ex, "CopyCategory", qry, 0, connectionString);
            }

            return newCategoryID;
        }

        #endregion [ CopyCategory ]

        #region [ SetCategoryShareFlag ]

        public static void SetCategoryShareFlag(string connectionString, int categoryID, bool isShared)
        {
            string qry = string.Empty;

            try
            {
                qry = @"UPDATE tbCategories SET IsShared = @isShared WHERE ID = @categoryID";

                SqlParameter parIsShared = new SqlParameter("@isShared", isShared);
                SqlParameter parCategoryID = new SqlParameter("@categoryID", categoryID);

                MHB.DAL.DataBaseConnector.ExecuteQuery(qry, connectionString, parIsShared, parCategoryID);
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(ex, "SetCategoryShareFlag", qry, 0, connectionString);
            }
        }

        #endregion [ SetCategoryShareFlag ]

        #region [ GetCategory ]

        public static Category GetCategory(int categoryID, int languageID, int userID, SqlConnection connection)
        {
            string qry = @"EXECUTE spGetCategoriesTable @languageID, @userID, @optionalCategoryID";

            Category c = new Category();

            try
            {
                SqlParameter parLanguageID = new SqlParameter("@languageID", languageID);
                SqlParameter parUserID = new SqlParameter("@userID", userID);
                SqlParameter parCategoryID = new SqlParameter("@optionalCategoryID", categoryID);

                IDataReader reader = MHB.DAL.DataBaseConnector.GetDataReader(qry, connection, parLanguageID, parUserID, parCategoryID);

                while (reader.Read())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("CategoryID")))
                        c.ID = (int)reader["CategoryID"];

                    if (!reader.IsDBNull(reader.GetOrdinal("CategoryName")))
                        c.Name = (string)reader["CategoryName"];

                    if (!reader.IsDBNull(reader.GetOrdinal("CategoryKeyWords")))
                        c.CategoryKeywords = (string)reader["CategoryKeyWords"];

                    if (!reader.IsDBNull(reader.GetOrdinal("IconPath")))
                        c.IconPath = (string)reader["IconPath"];

                    if (!reader.IsDBNull(reader.GetOrdinal("IsPayIconVisible")))
                        c.IsPayIconVisible = (bool)reader["IsPayIconVisible"];

                    if (!reader.IsDBNull(reader.GetOrdinal("IsShared")))
                        c.IsShared = (bool)reader["IsShared"];

                    if (!reader.IsDBNull(reader.GetOrdinal("UserCategoryID")))
                        c.UserID = (int)reader["UserCategoryID"];
                }
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(ex, "LoadCategory", qry, userID, connection.ConnectionString);
            }

            return c;
        }

        #endregion [ GetCategory ]

        #region [ GetSortOptions ]

        public static List<SortOption> GetSortOptions(string connectionString, int language)
        {
            string qry = string.Empty;
            List<SortOption> dataSource = new List<SortOption>();

            try
            {
                qry = string.Format(@"
SELECT s.ID, t.Name, s.Enabled FROM SortOptions s
INNER JOIN SortOptionsTranslations t ON t.SortOptionID = s.ID
WHERE t.LanguageID = {0}", language);

                IDataReader reader = MHB.DAL.DataBaseConnector.GetDataReader(qry, connectionString);

                while (reader.Read())
                {
                    SortOption s = new SortOption();

                    s.ID = (int)reader["ID"];
                    s.Name = (string)reader["Name"];
                    s.Enabled = (bool)reader["Enabled"];

                    dataSource.Add(s);
                }

                return dataSource;
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(ex, "GetSortOptions", qry, 0, connectionString);
            }

            return dataSource;
        }

        #endregion [ GetSortOptions ]

        #region [ InsertTransaction ]

        public static void InsertTransaction(string connectionString, TransactionBase transaction)
        {
            string qry = string.Empty;

            try
            {
                qry = string.Format(@"
INSERT INTO dbo.tbTransactionLog
           (UserID
           ,ExpenseID
           ,NewValue
           ,OldValue
           ,TransactionText
           ,DateModified)
VALUES
           ({0}
           ,{1}
           ,{2}
           ,{3}
           ,'{4}'
           ,'{5}')",
                  transaction.UserID,
                  transaction.ExpenseID,
                  transaction.NewValue,
                  transaction.OldValue,
                  transaction.TransactionText,
                  transaction.DateModified);

                MHB.DAL.DataBaseConnector.ExecuteQuery(qry, connectionString);
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(ex, "InsertTransaction", qry, transaction.UserID, connectionString);
            }
        }

        #endregion [ InsertTransaction ]

        #region [ InsertTransaction ]

        public static void InsertTransactions(string connectionString, List<Transaction> transactions)
        {
            if (transactions.Count == 0) return;

            string qry = string.Empty;

            try
            {
                foreach (Transaction transaction in transactions)
                {
                    qry += string.Format(@"
INSERT INTO dbo.tbTransactionLog
           (UserID
           ,ExpenseID
           ,NewValue
           ,OldValue
           ,TransactionText
           ,DateModified)
VALUES
           ({0}
           ,{1}
           ,{2}
           ,{3}
           ,'{4}'
           ,'{5}') ",
                      transaction.UserID,
                      transaction.ExpenseID,
                      transaction.NewValue,
                      transaction.OldValue,
                      transaction.TransactionText,
                      transaction.DateModified);
                }

                MHB.DAL.DataBaseConnector.ExecuteQuery(qry, connectionString);
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(ex, "InsertTransactions", qry, transactions[0].UserID, connectionString);
            }
        }

        #endregion [ InsertTransaction ]

        #region [ GetTransactions ]

        public static IEnumerable<Transaction> GetTransactions(string connectionString, int expenseID)
        {
            return SQLHelper.GetTransactionsInternal(new SqlConnection(connectionString), expenseID);
        }

        public static IEnumerable<Transaction> GetTransactions(SqlConnection connection, int expenseID)
        {
            return SQLHelper.GetTransactionsInternal(connection, expenseID);
        }

        public static IEnumerable<Transaction> GetTransactionsInternal(SqlConnection connection, int expenseID)
        {
            string qry = string.Empty;
            List<Transaction> dataSource = new List<Transaction>();

            try
            {
                qry = string.Format(@"
SELECT ID
      ,UserID
      ,ExpenseID
      ,NewValue
      ,OldValue
      ,TransactionText
      ,DateModified
  FROM dbo.tbTransactionLog WHERE ExpenseID = {0}", expenseID);

                IDataReader reader = MHB.DAL.DataBaseConnector.GetDataReader(qry, connection);

                while (reader.Read())
                {
                    Transaction t = new Transaction();

                    t.ID = int.Parse(reader["ID"].ToString());
                    t.ExpenseID = int.Parse(reader["ExpenseID"].ToString());

                    decimal newValue = 0;
                    if (decimal.TryParse(reader["NewValue"].ToString(), out newValue))
                        t.NewValue = newValue;
                    else
                        t.NewValue = 0;

                    decimal oldValue = 0;
                    if (decimal.TryParse(reader["OldValue"].ToString(), out oldValue))
                        t.OldValue = oldValue;
                    else
                        t.OldValue = 0;

                    t.TransactionText = reader["TransactionText"].ToString();

                    if (!string.IsNullOrEmpty(reader["DateModified"].ToString()))
                        t.DateModified = DateTime.Parse(reader["DateModified"].ToString());
                    else
                        t.DateModified = DateTime.Now;

                    dataSource.Add(t);
                }

                return dataSource;
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(ex, "GetTransactions", qry, 0, connection.ConnectionString);
            }

            return dataSource;
        }

        #endregion [ GetTransactions ]

        #region [ GetMaximumExpenditureForCategory ]

        public static IDataReader GetMaximumExpenditureForCategory(string mainTableName, int userID, int categoryID, SqlConnection connection)
        {
            if (connection == null) throw new ArgumentNullException("MHB.BL.SQLHelper.GetMaximumExpenditureForCategory: connection is null!");

            string qry = string.Empty;
            IDataReader reader = null;

            try
            {
                qry =
string.Format("SELECT * FROM {0} WHERE FieldValue = (SELECT MAX(FieldValue) FROM {0} WHERE UserID = @UserID AND CostCategory = @CategoryID AND IsDeleted = 0)", mainTableName);

                SqlParameter parUserID = new SqlParameter("@UserID", userID);
                SqlParameter parCategoryID = new SqlParameter("@CategoryID", categoryID);

                reader = MHB.DAL.DataBaseConnector.GetDataReader(qry, connection, parUserID, parCategoryID);

                return reader;
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(ex, "GetMaximumExpenditureForCategory", qry, 0, connection.ConnectionString);
            }

            return reader;
        }

        #endregion [ GetMaximumExpenditureForCategory ]

        #region [ GetUserIncome ]

        public static List<Income> GetUserIncome(int userID, int year, int month, string connectionString)
        {
            List<Income> userIncome = new List<Income>();

            string qry = string.Empty;

            try
            {
                qry = @"SELECT [ID], [IncomeName], [IncomeValue], [IncomeDate], [UserID], [Month], [Year]
                        FROM [dbo].[tbIncomes]
                        WHERE [UserID] = @UserID
                        AND [Month] = @Month
                        AND [Year] = @Year
                        AND IsDeleted = 0";

                SqlParameter parUserID = new SqlParameter("@UserID", userID);
                SqlParameter parMonth = new SqlParameter("@Month", month);
                SqlParameter parYear = new SqlParameter("@Year", year);

                IDataReader reader = MHB.DAL.DataBaseConnector.GetDataReader(qry, connectionString, parUserID, parMonth, parYear);

                while (reader.Read())
                {
                    Income i = new Income();

                    if (!reader.IsDBNull(reader.GetOrdinal("ID")))
                        i.ID = Convert.ToInt32(reader["ID"]);

                    i.UserID = userID;

                    if (!reader.IsDBNull(reader.GetOrdinal("IncomeName")))
                        i.Name = reader["IncomeName"].ToString();

                    if (!reader.IsDBNull(reader.GetOrdinal("IncomeValue")))
                        i.Value = Convert.ToDecimal(reader["IncomeValue"]);

                    if (!reader.IsDBNull(reader.GetOrdinal("IncomeDate")))
                        i.Date = DateTime.Parse(reader["IncomeDate"].ToString());

                    userIncome.Add(i);
                }
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(ex, "GetUserIncome", qry, userID, connectionString);
            }

            return userIncome;
        }

        #endregion [ GetUserIncome ]

        #region [ GetIncomeLogs ]

        public static List<IncomeLog> GetIncomeLogs(int userID, int month, int year, string connectionString)
        {
            string qry = string.Empty;

            List<IncomeLog> logs = new List<IncomeLog>();

            try
            {
                qry =
"SELECT ID, Name, Value, UserID, [Month], [Year], IncomeDate, DateModified, IncomeAction FROM dbo.tbIncomeLog WHERE UserID = @UserID AND [Month] = @Month AND [Year] = @Year";

                SqlParameter parUserID = new SqlParameter("@UserID", userID);
                SqlParameter parMonth = new SqlParameter("@Month", month);
                SqlParameter parYear = new SqlParameter("@Year", year);

                IDataReader reader = MHB.DAL.DataBaseConnector.GetDataReader(qry, connectionString, parUserID, parMonth, parYear);

                while (reader.Read())
                {
                    IncomeLog log = new IncomeLog();

                    if (!reader.IsDBNull(reader.GetOrdinal("ID")))
                        log.ID = (int)reader["ID"];

                    if (!reader.IsDBNull(reader.GetOrdinal("IncomeID")))
                        log.IncomeID = (int)reader["IncomeID"];

                    if (!reader.IsDBNull(reader.GetOrdinal("Name")))
                        log.Name = (string)reader["Name"];

                    if (!reader.IsDBNull(reader.GetOrdinal("Value")))
                        log.Value = (decimal)reader["Value"];

                    if (!reader.IsDBNull(reader.GetOrdinal("Month")))
                        log.Month = (int)reader["Month"];

                    if (!reader.IsDBNull(reader.GetOrdinal("Year")))
                        log.Year = (int)reader["Year"];

                    if (!reader.IsDBNull(reader.GetOrdinal("IncomeDate")))
                        log.IncomeDate = (DateTime)reader["IncomeDate"];

                    if (!reader.IsDBNull(reader.GetOrdinal("DateModified")))
                        log.DateModified = (DateTime)reader["DateModified"];

                    if (!reader.IsDBNull(reader.GetOrdinal("IncomeAction")))
                        log.Action = (Income.IncomeAction)reader["IncomeAction"];

                    logs.Add(log);
                }
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(ex, "GetIncomeLogs", qry, userID, connectionString);
            }

            return logs;
        }

        #endregion [ GetIncomeLogs ]

        #region [ DeleteIncome ]

        public static void DeleteIncome(int id, string connectionString)
        {
            string qry = string.Empty;

            try
            {
                qry =
@"INSERT INTO tbIncomeLog (IncomeID, Name, Value, UserID, [Month], [Year], IncomeDate, DateModified, IncomeAction)
SELECT ID, IncomeName, IncomeValue, UserID, [Month], [Year], IncomeDate, GETDATE(), @IncomeAction FROM tbIncomes WHERE ID = @id
    UPDATE tbIncomes SET IsDeleted = 1, DateModified = GETDATE() WHERE ID = @id";

                SqlParameter parID = new SqlParameter("@id", id);
                SqlParameter parIncomeAction = new SqlParameter("@IncomeAction", Income.IncomeAction.Delete);

                MHB.DAL.DataBaseConnector.ExecuteQuery(qry, connectionString, parID, parIncomeAction);
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(ex, "DeleteIncome", qry, 0, connectionString);
            }
        }

        #endregion [ DeleteIncome ]

        #region [ UpdateIncome ]

        public static void UpdateIncome(IncomeBase income, string connectionString)
        {
            string qry = string.Empty;

            try
            {
                qry =
   @"UPDATE [dbo].[tbIncomes] SET [IncomeName] = @IncomeName, [IncomeValue] = @IncomeValue, [IncomeDate] = @IncomeDate, DateModified = GETDATE() WHERE [ID] = @id
        INSERT INTO tbIncomeLog (IncomeID, Name, Value, UserID, [Month], [Year], IncomeDate, DateModified, IncomeAction)
        VALUES (@id, @IncomeName, @IncomeValue, @UserID, @Month, @Year, @IncomeDate, GETDATE(), @IncomeAction)";

                SqlParameter parID = new SqlParameter("@id", income.ID);
                SqlParameter parIncomeName = new SqlParameter("@IncomeName", income.Name);
                SqlParameter parIncomeValue = new SqlParameter("@IncomeValue", income.Value);
                SqlParameter parIncomeUserID = new SqlParameter("@UserID", income.UserID);
                SqlParameter parIncomeDate = new SqlParameter("@IncomeDate", income.Date > DateTime.MinValue ? income.Date : new DateTime(1900, 1, 1));
                SqlParameter parIncomeMonth = new SqlParameter("@Month", income.Month);
                SqlParameter parIncomeYear = new SqlParameter("@Year", income.Year);
                SqlParameter parIncomeAction = new SqlParameter("@IncomeAction", Income.IncomeAction.Update);

                MHB.DAL.DataBaseConnector.ExecuteQuery(qry, connectionString, parID, parIncomeName, parIncomeValue, parIncomeUserID, parIncomeDate, parIncomeMonth, parIncomeYear, parIncomeAction);
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(ex, "DeleteIncome", qry, 0, connectionString);
            }
        }

        #endregion [ UpdateIncome ]

        #region [ AddIncome ]

        public static void AddIncome(IncomeBase income, string connectionString)
        {
            string qry = string.Empty;

            try
            {
                qry =
@"INSERT INTO [dbo].[tbIncomes] ([IncomeName], [IncomeValue], [IncomeDate], [UserID], [Month], [Year]) VALUES (@IncomeName, @IncomeValue, @IncomeDate, @UserID, @Month, @Year)
    INSERT INTO tbIncomeLog (IncomeID, Name, Value, UserID, [Month], [Year], IncomeDate, DateModified, IncomeAction)
    VALUES (SCOPE_IDENTITY(), @IncomeName, @IncomeValue, @UserID, @Month, @Year, @IncomeDate, GETDATE(), @IncomeAction)";

                SqlParameter parIncomeName = new SqlParameter("@IncomeName", income.Name);
                SqlParameter parIncomeValue = new SqlParameter("@IncomeValue", income.Value);
                SqlParameter parIncomeDate = new SqlParameter("@IncomeDate", income.Date > DateTime.MinValue ? income.Date : new DateTime(1900, 1, 1));
                SqlParameter parIncomeUserID = new SqlParameter("@UserID", income.UserID);
                SqlParameter parIncomeMonth = new SqlParameter("@Month", income.Month);
                SqlParameter parIncomeYear = new SqlParameter("@Year", income.Year);
                SqlParameter parIncomeAction = new SqlParameter("@IncomeAction", Income.IncomeAction.Add);

                MHB.DAL.DataBaseConnector.ExecuteQuery(qry, connectionString, parIncomeName, parIncomeValue, parIncomeDate, parIncomeUserID, parIncomeMonth, parIncomeYear, parIncomeAction);
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(ex, "AddIncome", qry, income.UserID, connectionString);
            }
        }

        #endregion [ AddIncome ]

        #region [ GetUsersYearlyExpensesProMonth ]

        public static IDataReader GetUsersYearlyExpensesProMonth(int userID, int year, string connectionString)
        {
            string qry = string.Empty;

            IDataReader reader = null;

            try
            {
                qry = @"spGetUsersYearlyExpenses @UserID, @Year";

                SqlParameter parUserID = new SqlParameter("@UserID", userID);
                SqlParameter parYear = new SqlParameter("@Year", year);

                reader = MHB.DAL.DataBaseConnector.GetDataReader(qry, connectionString, parUserID, parYear);
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(ex, "GetUsersYearlyExpensesProMonth", qry, userID, connectionString);
            }

            return reader;
        }

        #endregion [ GetUsersYearlyExpensesProMonth ]

        #region [ GetUsersYearlyBudgetsProMonth ]

        public static IDataReader GetUsersYearlyBudgetsProMonth(int userID, int year, string connectionString)
        {
            string qry = string.Empty;

            IDataReader reader = null;

            try
            {
                qry = @"SELECT BudgetJan AS SumJanuary, BudgetFeb AS SumFebruary, BudgetMar AS SumMarch, BudgetApr AS SumApril, BudgetMay AS SumMay, BudgetJune AS SumJune, BudgetJuly AS SumJuly, BudgetAug AS SumAugust, BudgetSept AS SumSeptember, BudgetOct AS SumOctober, BudgetNov AS SumNovember, BudgetDec AS SumDecember
                               FROM dbo.tbMonthlyBudget
                               WHERE UserID = @UserID AND [Year] = @Year";

                SqlParameter parUserID = new SqlParameter("@UserID", userID);
                SqlParameter parYear = new SqlParameter("@Year", year);

                reader = MHB.DAL.DataBaseConnector.GetDataReader(qry, connectionString, parUserID, parYear);
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(ex, "GetUsersYearlyBudgetsProMonth", qry, userID, connectionString);
            }

            return reader;
        }

        #endregion [ GetUsersYearlyBudgetsProMonth ]

        #region [ GetUsersYearlySavingsProMonth ]

        public static IDataReader GetUsersYearlySavingsProMonth(int userID, int year, string connectionString)
        {
            string qry = string.Empty;

            IDataReader reader = null;

            try
            {
                qry = @"SELECT SavingsJan AS SumJanuary, SavingsFeb AS SumFebruary, SavingsMar AS SumMarch, SavingsApr AS SumApril, SavingsMay AS SumMay, SavingsJune AS SumJune, SavingsJuly AS SumJuly, SavingsAug AS SumAugust, SavingsSept AS SumSeptember, SavingsOct AS SumOctober, SavingsNov AS SumNovember, SavingsDec AS SumDecember
                        FROM  dbo.tbMonthlySavings
                        WHERE UserID = @UserID AND [Year] = @Year";

                SqlParameter parUserID = new SqlParameter("@UserID", userID);
                SqlParameter parYear = new SqlParameter("@Year", year);

                reader = MHB.DAL.DataBaseConnector.GetDataReader(qry, connectionString, parUserID, parYear);
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(ex, "GetUsersYearlySavingsProMonth", qry, userID, connectionString);
            }

            return reader;
        }

        #endregion [ GetUsersYearlySavingsProMonth ]

        #region [ class SearchQueries ]

        // Always leave trailing white spaces to string;
        internal class SearchQueries
        {
            public static string AND_MainTable_Name_OR_Description_CONTAINS(string searchText)
            {
                string qry = string.Empty;

                if (searchText.Trim().Length > 0)
                {
                    qry += string.Format("AND (mt.FieldName LIKE '%{0}%' OR mt.FieldDescription LIKE '%{0}%') ", searchText);
                }

                return qry;
            }

            public static string AND_MainTable_Value_COMPARE(string sum, string comparisonOperator)
            {
                string qry = string.Empty;

                decimal searchSum = 0;

                if (decimal.TryParse(sum, out searchSum))
                {
                    qry += string.Format("AND mt.FieldValue {0} {1} ", comparisonOperator, sum);
                }

                return qry;
            }

            public static string AND_DetailsTable_Name_OR_Description_CONTAINS(string searchText)
            {
                string qry = string.Empty;

                if (searchText.Trim().Length > 0)
                {
                    qry += string.Format("AND (dt.DetailName LIKE '%{0}%' OR dt.DetailDescription LIKE '%{0}%') ", searchText);
                }

                return qry;
            }

            public static string AND_MainTable_Name_OR_Description_OR_DetailsTable_Name_OR_Description_CONTAINS(string searchText)
            {
                string qry = string.Empty;

                if (searchText.Trim().Length > 0)
                {
                    qry += string.Format(
                    "AND ((mt.FieldName LIKE '%{0}%' OR mt.FieldDescription LIKE '%{0}%') OR (dt.DetailName LIKE '%{0}%' OR dt.DetailDescription LIKE '%{0}%') ",
                    searchText);
                }

                return qry;
            }

            public static string LEFT_JOIN_DetailsTable_TO_MainTable(string detailsTableName)
            {
                string qry = string.Empty;

                if (detailsTableName.Trim().Length > 0)
                {
                    qry = string.Format("LEFT JOIN dbo.{0} dt ON dt.ExpenditureID = mt.ID ", detailsTableName);
                }

                return qry;
            }

            public static string AND_MainTable_Year_EQUALS(string year)
            {
                string qry = string.Empty;

                if (year.Trim().Length > 0)
                {
                    qry = string.Format("AND mt.Year = {0} ", year);
                }

                return qry;
            }

            public static string AND_MainTable_CostCategory_EQUALS(string categoryId)
            {
                string qry = string.Empty;

                if (categoryId.Trim().Length > 0)
                {
                    qry = string.Format("AND mt.CostCategory = {0} ", categoryId);
                }

                return qry;
            }

            public static string ORDER_BY_MainTable_FieldValue(string sortDirection)
            {
                string qry = string.Empty;

                if (sortDirection.Trim().Length > 0)
                {
                    qry = string.Format("ORDER BY mt.FieldValue {0} ", sortDirection);
                }

                return qry;
            }

            public static string ORDER_BY_MainTable_Year_Month(string sortDirection)
            {
                string qry = string.Empty;

                if (sortDirection.Trim().Length > 0)
                {
                    qry = string.Format("ORDER BY mt.Year {0}, mt.Month {0} ", sortDirection);
                }

                return qry;
            }

            public static string ORDER_BY_MainTable_CostCategory(string sortDirection)
            {
                string qry = string.Empty;

                if (sortDirection.Trim().Length > 0)
                {
                    qry = string.Format("ORDER BY mt.CostCategory {0} ", sortDirection);
                }

                return qry;
            }
        }

        #endregion [ class SearchQueries ]

        #region [ MergeProducts ]

        public static void MergeProducts(int newProductID, int oldProductID, int userID, string mainTableName, string detailsTableName, string connectionString)
        {
            if (newProductID <= 1) throw new ArgumentOutOfRangeException("newProductID");
            if (oldProductID <= 1) throw new ArgumentOutOfRangeException("oldProductID");
            if (newProductID == oldProductID) throw new ArgumentException("newProductID and oldProductID parameters cannot be equal!");

            StringBuilder sbQry = new StringBuilder();

            try
            {
                sbQry.AppendLine("IF EXISTS (SELECT ID FROM tbProducts WHERE ID IN (@NewProductID, @OldProductID))");
                sbQry.AppendLine("BEGIN");
                sbQry.AppendFormat("UPDATE {0} SET ProductID = @NewProductID WHERE ProductID = @OldProductID AND IsDeleted = 0", mainTableName);
                sbQry.AppendFormat("UPDATE {0} SET ProductID = @NewProductID WHERE ProductID = @OldProductID AND IsDeleted = 0", detailsTableName);
                sbQry.AppendLine("UPDATE tbProducts SET IsDeleted = 1 WHERE ID = @OldProductID");
                sbQry.AppendLine("END");

                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("NewProductID", newProductID),
                    new SqlParameter("OldProductID", oldProductID)
                };

                DataBaseConnector.ExecuteQuery(sbQry.ToString(), connectionString, sqlParameters);
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(ex, "MHB.BL.SQLHelper.MergeProducts", sbQry.ToString(), userID, connectionString);
            }
        }

        #endregion [ MergeProducts ]

        #region [ GetProduct ]

        internal static Product GetProduct(int id, int userID, SqlConnection connection, bool loadSupplier = false, bool loadCategory = false)
        {
            string qry = string.Empty;

            Product product = new Product();

            try
            {
                qry = @"SELECT ID, Name, [Description], KeyWords, OcrKeyWords, StandardCost, ListPrice, Color, Picture, [Weight], Volume, PackageUnitsCount, UserID, DateModified, IsDeleted, VendorID, CategoryID, IsFixedMeasureType, DefaultMeasureType
FROM tbProducts WHERE ID = @ID AND IsDeleted = 0";

                SqlParameter parID = new SqlParameter("@ID", id);

                IDataReader reader = MHB.DAL.DataBaseConnector.GetDataReader(qry, connection, parID);

                while (reader.Read())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("ID")))
                        product.ID = (int)reader["ID"];

                    if (!reader.IsDBNull(reader.GetOrdinal("Name")))
                        product.Name = (string)reader["Name"];

                    if (!reader.IsDBNull(reader.GetOrdinal("Description")))
                        product.Description = (string)reader["Description"];

                    if (!reader.IsDBNull(reader.GetOrdinal("KeyWords")))
                        product.KeyWords = reader["KeyWords"].ToString().Split(',');

                    if (!reader.IsDBNull(reader.GetOrdinal("OcrKeyWords")))
                        product.OcrKeyWords = reader["OcrKeyWords"].ToString().Split(',');

                    if (!reader.IsDBNull(reader.GetOrdinal("StandardCost")))
                        product.StandardCost = (decimal)reader["StandardCost"];

                    if (!reader.IsDBNull(reader.GetOrdinal("ListPrice")))
                        product.ListPrice = (decimal)reader["ListPrice"];

                    if (!reader.IsDBNull(reader.GetOrdinal("Color")))
                        product.Color = (string)reader["Color"];

                    if (!reader.IsDBNull(reader.GetOrdinal("Picture")))
                        product.Picture = (byte[])reader["Picture"];

                    if (!reader.IsDBNull(reader.GetOrdinal("Weight")))
                        product.Weight = (decimal)reader["Weight"];

                    if (!reader.IsDBNull(reader.GetOrdinal("Volume")))
                        product.Volume = (decimal)reader["Volume"];

                    if (!reader.IsDBNull(reader.GetOrdinal("PackageUnitsCount")))
                        product.PackageUnitsCount = (decimal)reader["PackageUnitsCount"];

                    if (!reader.IsDBNull(reader.GetOrdinal("UserID")))
                        product.UserID = (int)reader["UserID"];

                    if (!reader.IsDBNull(reader.GetOrdinal("DateModified")))
                        product.DateModified = (DateTime)reader["DateModified"];

                    if (!reader.IsDBNull(reader.GetOrdinal("IsDeleted")))
                        product.IsDeleted = (bool)reader["IsDeleted"];

                    if (!reader.IsDBNull(reader.GetOrdinal("VendorID")))
                        product.VendorID = (int)reader["VendorID"];

                    if (!reader.IsDBNull(reader.GetOrdinal("CategoryID")))
                        product.CategoryID = (int)reader["CategoryID"];

                    if (!reader.IsDBNull(reader.GetOrdinal("IsFixedMeasureType")))
                        product.IsFixedMeasureType = (bool)reader["IsFixedMeasureType"];

                    if (!reader.IsDBNull(reader.GetOrdinal("DefaultMeasureType")))
                        product.DefaultMeasureType = (Enums.MeasureType)reader["DefaultMeasureType"];

                    product.Connection = connection;

                    if (loadSupplier)
                    {
                        product.Supplier = SQLHelper.GetSupplier(product.VendorID, userID, connection);
                    }

                    if (loadCategory)
                    {
                        product.Category = SQLHelper.GetCategory(product.CategoryID, 1, userID, connection);
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(ex, "MHB.BL.SQLHelper.GetProduct", qry, userID, connection.ConnectionString);
            }

            return product;
        }

        #endregion [ GetProduct ]

        #region [ GetProducts ]

        private static Dictionary<int, Tuple<int, MHB.BL.Enums.MeasureType>> GetProductTotalEntries(string detailsTableName, string connectionString, int year = 0, int month = 0)
        {
            string qry = string.Empty;

            Dictionary<int, Tuple<int, MHB.BL.Enums.MeasureType>> productCounts = new Dictionary<int, Tuple<int, MHB.BL.Enums.MeasureType>>();

            try
            {
                qry = string.Format(
@"SELECT ProductID, [Count], MeasureTypeID FROM
(
	SELECT ProductID, COUNT(ProductID) AS [Count], MeasureTypeID,

	RANK() OVER (PARTITION BY ProductID ORDER BY COUNT(*) DESC) AS Rnk

	FROM {0}

	WHERE IsDeleted = 0 AND ProductID > 1 AND

	MONTH(DetailDate) = CASE WHEN @Month > 0 THEN @Month ELSE MONTH(DetailDate) END AND

	YEAR(DetailDate) = CASE WHEN @Year > 0 THEN @Year ELSE YEAR(DetailDate) END

	GROUP BY ProductID, MeasureTypeID
) x
WHERE Rnk = 1
ORDER BY x.[Count] DESC", detailsTableName);

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("Year", year),
                    new SqlParameter("Month", month)
                };

                IDataReader reader = MHB.DAL.DataBaseConnector.GetDataReader(qry, connectionString, parameters);

                while (reader.Read())
                {
                    int productID = reader.Get<int>("ProductID");
                    if (!productCounts.ContainsKey(productID))
                    {
                        productCounts.Add(productID, Tuple.Create(reader.Get<int>("Count"), reader.Get<MHB.BL.Enums.MeasureType>("MeasureTypeID")));
                    }
                    else
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(ex, "MHB.BL.SQLHelper.GetProductTotalEntries", qry, 0, connectionString);
            }

            return productCounts;
        }

        internal static IEnumerable<Product> GetProducts(int userID, SqlConnection connection, IEnumerable<Category> categoriesCache = null, IEnumerable<Supplier> suppliersCache = null)
        {
            var productOccurances = GetProductTotalEntries(ExpenditureManager.GetUserMainTableNames(userID).Item2, connection.ConnectionString);

            string qry = string.Empty;

            List<Product> products = new List<Product>();

            IDataReader reader = null;

            try
            {
                qry = @"SELECT ID, Name, [Description], KeyWords, OcrKeyWords, StandardCost, ListPrice, Color, Picture, [Weight], Volume, PackageUnitsCount, UserID, DateModified, IsDeleted, VendorID, CategoryID, IsFixedMeasureType, DefaultMeasureType FROM tbProducts WHERE IsDeleted = 0 AND UserID = @userID";

                SqlParameter parUserID = new SqlParameter("@userID", userID);

                reader = MHB.DAL.DataBaseConnector.GetDataReader(qry, connection, parUserID);

                while (reader.Read())
                {
                    Product product = new Product();

                    if (!reader.IsDBNull(reader.GetOrdinal("ID")))
                        product.ID = (int)reader["ID"];

                    if (!reader.IsDBNull(reader.GetOrdinal("Name")))
                        product.Name = (string)reader["Name"];

                    if (!reader.IsDBNull(reader.GetOrdinal("Description")))
                        product.Description = (string)reader["Description"];

                    if (!reader.IsDBNull(reader.GetOrdinal("KeyWords")))
                        product.KeyWords = reader["KeyWords"].ToString().Split(',');

                    if (!reader.IsDBNull(reader.GetOrdinal("OcrKeyWords")))
                        product.OcrKeyWords = reader["OcrKeyWords"].ToString().Split(',');

                    if (!reader.IsDBNull(reader.GetOrdinal("StandardCost")))
                        product.StandardCost = (decimal)reader["StandardCost"];

                    if (!reader.IsDBNull(reader.GetOrdinal("ListPrice")))
                        product.ListPrice = (decimal)reader["ListPrice"];

                    if (!reader.IsDBNull(reader.GetOrdinal("Color")))
                        product.Color = (string)reader["Color"];

                    if (!reader.IsDBNull(reader.GetOrdinal("Picture")))
                        product.Picture = (byte[])reader["Picture"];

                    if (!reader.IsDBNull(reader.GetOrdinal("Weight")))
                        product.Weight = (decimal)reader["Weight"];

                    if (!reader.IsDBNull(reader.GetOrdinal("Volume")))
                        product.Volume = (decimal)reader["Volume"];

                    if (!reader.IsDBNull(reader.GetOrdinal("PackageUnitsCount")))
                        product.PackageUnitsCount = (decimal)reader["PackageUnitsCount"];

                    if (!reader.IsDBNull(reader.GetOrdinal("UserID")))
                        product.UserID = (int)reader["UserID"];

                    if (!reader.IsDBNull(reader.GetOrdinal("DateModified")))
                        product.DateModified = (DateTime)reader["DateModified"];

                    if (!reader.IsDBNull(reader.GetOrdinal("IsDeleted")))
                        product.IsDeleted = (bool)reader["IsDeleted"];

                    if (!reader.IsDBNull(reader.GetOrdinal("VendorID")))
                        product.VendorID = (int)reader["VendorID"];

                    if (!reader.IsDBNull(reader.GetOrdinal("CategoryID")))
                        product.CategoryID = (int)reader["CategoryID"];

                    if (!reader.IsDBNull(reader.GetOrdinal("IsFixedMeasureType")))
                        product.IsFixedMeasureType = (bool)reader["IsFixedMeasureType"];

                    if (!reader.IsDBNull(reader.GetOrdinal("DefaultMeasureType")))
                        product.DefaultMeasureType = (Enums.MeasureType)reader["DefaultMeasureType"];

                    Tuple<int, MHB.BL.Enums.MeasureType> prevailingOccurancesMeasureType;

                    productOccurances.TryGetValue(product.ID, out prevailingOccurancesMeasureType);

                    if (prevailingOccurancesMeasureType != null)
                    {
                        product.Priority = prevailingOccurancesMeasureType.Item1;

                        product.PrevailingMeasureType = prevailingOccurancesMeasureType.Item2;
                    }

                    product.Connection = connection;

                    if (suppliersCache != null && suppliersCache.Any(s => s.ID == product.VendorID))
                    {
                        product.Supplier = suppliersCache.Single(s => s.ID == product.VendorID);
                    }
                    else if (product.VendorID != Supplier.SUPPLIER_DEFAULT_ID)
                    {
                        product.Supplier = SQLHelper.GetSupplier(product.VendorID, userID, connection);
                    }

                    if (categoriesCache != null && categoriesCache.Any(c => c.ID == product.CategoryID))
                    {
                        product.Category = categoriesCache.Single(c => c.ID == product.CategoryID);
                    }
                    else if (product.CategoryID != Category.CATEGORY_DEFAULT_ID)
                    {
                        product.Category = SQLHelper.GetCategory(product.CategoryID, 1, userID, connection);
                    }

                    product.Parameters = Enumerable.Empty<ProductParameter>();

                    products.Add(product);
                }

                //foreach (Product product in products)
                //{
                //    product.Parameters = SQLHelper.GetProductParameters(0, product.ID, userID, connection);
                //}
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(ex, "MHB.BL.SQLHelper.GetProducts", qry, userID, connection.ConnectionString);
            }
            finally
            {
                if (!reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            return products;
        }

        #endregion [ GetProducts ]

        #region [ DeleteProduct ]

        internal static bool DeleteProduct(int id, string connectionString, string mainTableName, string detailsTableName)
        {
            string qry = string.Empty;

            bool result = false;

            try
            {
                qry = string.Format(@"UPDATE tbProducts SET IsDeleted = 1, DateModified = GETDATE() WHERE ID = @ProductID
                                      UPDATE {0} SET ProductID = @DefaultProductID WHERE IsDeleted = 0 AND ProductID = @ProductID
                                      UPDATE {1} SET ProductID = @DefaultProductID WHERE IsDeleted = 0 AND ProductID = @ProductID", mainTableName, detailsTableName);

                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@ProductID", id),
                    new SqlParameter("@DefaultProductID", Product.PRODUCT_DEFAULT_ID)
                };

                MHB.DAL.DataBaseConnector.ExecuteQuery(qry, connectionString, sqlParameters);

                result = true;
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(ex, "MHB.BL.SQLHelper.DeleteProduct", qry, 0, connectionString);
            }

            return result;
        }

        #endregion [ DeleteProduct ]

        #region [ AddProduct ]

        internal static int AddProduct(ProductBase p)
        {
            string qry = string.Empty;

            int productID = 1;

            try
            {
                qry = @"INSERT INTO dbo.tbProducts
                                       (Name
                                       ,[Description]
                                       ,KeyWords, OcrKeyWords
                                       ,StandardCost
                                       ,ListPrice
                                       ,Color
                                       ,Picture
                                       ,[Weight]
                                       ,Volume
                                       ,PackageUnitsCount
                                       ,UserID
                                       ,DateModified
                                       ,IsDeleted
                                       ,VendorID
                                       ,CategoryID)
                                 VALUES
                                       (@Name
                                       ,@Description
                                       ,@KeyWords, @OcrKeyWords
                                       ,@StandardCost
                                       ,@ListPrice
                                       ,@Color
                                       ,@Picture
                                       ,@Weight
                                       ,@Volume
                                       ,@PackageUnitsCount
                                       ,@UserID
                                       ,GETDATE()
                                       ,@IsDeleted
                                       ,@VendorID
                                       ,@CategoryID) SELECT SCOPE_IDENTITY()";

                if (p.Volume > 0)
                {
                    p.DefaultMeasureType = Enums.MeasureType.Volume;
                    p.IsFixedMeasureType = true;
                    p.Weight = 0;
                }
                else if (p.Weight > 0)
                {
                    p.DefaultMeasureType = Enums.MeasureType.Weight;
                    p.IsFixedMeasureType = true;
                    p.Volume = 0;
                }
                else
                {
                    p.DefaultMeasureType = Enums.MeasureType.NotSet;
                    p.IsFixedMeasureType = false;
                    p.Weight = 0;
                    p.Volume = 0;
                }

                SqlParameter parName = new SqlParameter("@Name", p.Name);
                SqlParameter parDescription = new SqlParameter("@Description", p.Description);
                SqlParameter parKeyWords = new SqlParameter("@KeyWords", string.Join(",", p.KeyWords ?? new string[] { }));
                SqlParameter parOcrKeyWords = new SqlParameter("@OcrKeyWords", string.Join(",", p.OcrKeyWords ?? new string[] { }));
                SqlParameter parStandardCost = new SqlParameter("@StandardCost", p.StandardCost);
                SqlParameter parListPrice = new SqlParameter("@ListPrice", p.ListPrice);
                SqlParameter parColor = new SqlParameter("@Color", p.Color);
                SqlParameter parPicture = new SqlParameter("@Picture", p.Picture);
                SqlParameter parWeight = new SqlParameter("@Weight", p.Weight);
                SqlParameter parVolume = new SqlParameter("@Volume", p.Volume);
                SqlParameter parPackageUnitsCount = new SqlParameter("@PackageUnitsCount", p.PackageUnitsCount);
                SqlParameter parUserID = new SqlParameter("@UserID", p.UserID);
                SqlParameter parIsDeleted = new SqlParameter("@IsDeleted", p.IsDeleted);
                SqlParameter parVendorID = new SqlParameter("@VendorID", p.VendorID);
                SqlParameter parCategoryID = new SqlParameter("@CategoryID", p.CategoryID);

                object result = MHB.DAL.DataBaseConnector.GetSingleValue(qry, p.ConnectionString, parName, parDescription, parKeyWords, parOcrKeyWords, parStandardCost, parListPrice, parColor, parPicture, parWeight, parVolume, parPackageUnitsCount, parUserID, parIsDeleted, parVendorID, parCategoryID);

                int.TryParse(result.ToString(), out productID);
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(ex, "MHB.BL.SQLHelper.AddProduct", qry, p.UserID, p.ConnectionString);
            }

            return productID;
        }

        #endregion [ AddProduct ]

        #region [ UpdateProduct ]

        internal static bool UpdateProduct(ProductBase p)
        {
            string qry = string.Empty;

            bool result = false;

            try
            {
                qry = @"UPDATE dbo.tbProducts
                           SET Name = @Name
                              ,[Description] = @Description
                              ,KeyWords = @KeyWords
                              ,OcrKeyWords = @OcrKeyWords
                              ,StandardCost = @StandardCost
                              ,ListPrice = @ListPrice
                              ,Color = @Color
                              ,Picture = @Picture
                              ,[Weight] = @Weight
                              ,Volume = @Volume
                              ,PackageUnitsCount = @PackageUnitsCount
                              ,UserID = @UserID
                              ,DateModified = GETDATE()
                              ,IsDeleted = @IsDeleted
                              ,VendorID = @VendorID
                              ,CategoryID = @CategoryID
                              ,IsFixedMeasureType = @IsFixedMeasureType
                              ,DefaultMeasureType = @DefaultMeasureType
                              WHERE ID = @ID
";

                // update all existing detail product entries with the new category id
                qry += string.Format(@"UPDATE details SET

                            details.CategoryID = @CategoryID,
                            details.MeasureTypeID = (CASE WHEN @DefaultMeasureType = 0 THEN details.InitialMeasureTypeID ELSE @DefaultMeasureType END),
                            details.Amount = (CASE WHEN @Amount = 0 THEN details.InitialAmount ELSE @Amount END)

                            FROM dbo.{0} AS details
                            INNER JOIN dbo.{1} AS main
                                    ON main.ID = details.ExpenditureID
                            WHERE main.IsDeleted = 0
                                AND main.UserID = @UserID
                                AND details.ProductID = @ID
                                AND details.IsDeleted = 0
", ExpenditureManager.GetUserMainTableNames(p.UserID).Item2, ExpenditureManager.GetUserMainTableNames(p.UserID).Item1);

                // update all existing main product entries with the new category id
                qry += string.Format("UPDATE {0} SET CostCategory = @CategoryID WHERE UserID = @UserID AND ProductID = @ID AND IsDeleted = 0", ExpenditureManager.GetUserMainTableNames(p.UserID).Item1);

                decimal amount = 0;

                if (p.Volume > 0)
                {
                    p.DefaultMeasureType = Enums.MeasureType.Volume;
                    p.IsFixedMeasureType = true;
                    amount = p.Volume;
                    p.Weight = 0;
                }
                else if (p.Weight > 0)
                {
                    p.DefaultMeasureType = Enums.MeasureType.Weight;
                    p.IsFixedMeasureType = true;
                    amount = p.Weight;
                    p.Volume = 0;
                }
                else
                {
                    p.DefaultMeasureType = Enums.MeasureType.NotSet;
                    p.IsFixedMeasureType = false;
                    p.Weight = 0;
                    p.Volume = 0;
                }

                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@ID", p.ID),
                    new SqlParameter("@Name", p.Name),
                    new SqlParameter("@Description", p.Description),
                    new SqlParameter("@KeyWords", string.Join(",", p.KeyWords)),
                    new SqlParameter("@OcrKeyWords", string.Join(",", p.OcrKeyWords)),
                    new SqlParameter("@StandardCost", p.StandardCost),
                    new SqlParameter("@ListPrice", p.ListPrice),
                    new SqlParameter("@Color", p.Color),
                    new SqlParameter("@Picture", p.Picture),
                    new SqlParameter("@Weight", p.Weight),
                    new SqlParameter("@Volume", p.Volume),
                    new SqlParameter("@PackageUnitsCount", p.PackageUnitsCount),
                    new SqlParameter("@UserID", p.UserID),
                    new SqlParameter("@IsDeleted", p.IsDeleted),
                    new SqlParameter("@VendorID", p.VendorID),
                    new SqlParameter("@CategoryID", p.CategoryID),
                    new SqlParameter("@IsFixedMeasureType", p.IsFixedMeasureType),
                    new SqlParameter("@DefaultMeasureType", p.DefaultMeasureType),
                    new SqlParameter("@Amount", amount)
                };

                MHB.DAL.DataBaseConnector.ExecuteQuery(qry, p.Connection ?? new SqlConnection(p.ConnectionString), sqlParameters);

                SQLHelper.UpdateSupplier(p.Supplier);

                result = true;
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(ex, "MHB.BL.SQLHelper.UpdateProduct", qry, p.UserID, p.ConnectionString);
            }

            return result;
        }

        #endregion [ UpdateProduct ]

        #region [ GetSupplier ]

        internal static Supplier GetSupplier(int ID, int userID, SqlConnection connection)
        {
            string qry = string.Empty;

            Supplier supplier = new Supplier();

            try
            {
                qry = @"SELECT VendorID, AccountNumber, Name, [Description], [Address], CreditRating, PreferredVendorStatus, ActiveFlag, PurchasingWebServiceURL, WebsiteURL, UserID, ModifiedDate, IsDeleted
FROM tbVendors WHERE VendorID = @VendorID AND IsDeleted = 0";

                SqlParameter parID = new SqlParameter("@VendorID", ID);

                IDataReader reader = MHB.DAL.DataBaseConnector.GetDataReader(qry, connection, parID);

                while (reader.Read())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("VendorID")))
                        supplier.ID = (int)reader["VendorID"];

                    if (!reader.IsDBNull(reader.GetOrdinal("AccountNumber")))
                        supplier.AccountNumber = (string)reader["AccountNumber"];

                    if (!reader.IsDBNull(reader.GetOrdinal("Name")))
                        supplier.Name = (string)reader["Name"];

                    if (!reader.IsDBNull(reader.GetOrdinal("Description")))
                        supplier.Description = (string)reader["Description"];

                    if (!reader.IsDBNull(reader.GetOrdinal("Address")))
                        supplier.Address = (string)reader["Address"];

                    if (!reader.IsDBNull(reader.GetOrdinal("CreditRating")))
                        supplier.CreditRating = (int)(byte)reader["CreditRating"];

                    if (!reader.IsDBNull(reader.GetOrdinal("PreferredVendorStatus")))
                        supplier.PreferredVendorStatus = (bool)reader["PreferredVendorStatus"];

                    if (!reader.IsDBNull(reader.GetOrdinal("ActiveFlag")))
                        supplier.ActiveFlag = (bool)reader["ActiveFlag"];

                    if (!reader.IsDBNull(reader.GetOrdinal("PurchasingWebServiceURL")))
                        supplier.PurchasingWebServiceURL = (string)reader["PurchasingWebServiceURL"];

                    if (!reader.IsDBNull(reader.GetOrdinal("WebsiteURL")))
                        supplier.WebSiteURL = (string)reader["WebsiteURL"];

                    if (!reader.IsDBNull(reader.GetOrdinal("UserID")))
                        supplier.UserID = (int)reader["UserID"];

                    if (!reader.IsDBNull(reader.GetOrdinal("ModifiedDate")))
                        supplier.DateModified = (DateTime)reader["ModifiedDate"];

                    if (!reader.IsDBNull(reader.GetOrdinal("IsDeleted")))
                        supplier.IsDeleted = (bool)reader["IsDeleted"];

                    supplier.Connection = connection;
                }
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(ex, "MHB.BL.SQLHelper.GetSupplier", qry, userID, connection.ConnectionString);
            }

            return supplier;
        }

        #endregion [ GetSupplier ]

        #region [ GetSuppliers ]

        internal static IEnumerable<Supplier> GetSuppliers(int userID, SqlConnection connection)
        {
            string qry = string.Empty;

            List<Supplier> suppliers = new List<Supplier>();

            try
            {
                //                qry = @"SELECT VendorID, AccountNumber, Name, [Description], [Address], CreditRating, PreferredVendorStatus, ActiveFlag, PurchasingWebServiceURL, WebsiteURL, UserID, ModifiedDate, IsDeleted
                //FROM tbVendors WHERE IsDeleted = 0 AND UserID = @UserID";

                Tuple<string, string> tables = ExpenditureManager.GetUserMainTableNames(userID);

                qry = string.Format(@"SELECT SUM(dt.DetailValue) AS TotalSumSpent,
COUNT(dt.SupplierID) + SUM(dt.DetailValue) AS [Priority],
v.AccountNumber, v.Name, v.Description, v.Address, v.CreditRating, v.PreferredVendorStatus, v.ActiveFlag, v.PurchasingWebServiceURL, v.WebsiteURL, v.UserID, v.ModifiedDate, v.IsDeleted, v.VendorID,
COUNT(dt.SupplierID) AS TotalCount, dt.SupplierID FROM {0} dt
INNER JOIN {1} mt ON dt.ExpenditureID = mt.ID
INNER JOIN tbVendors v ON dt.SupplierID = v.VendorID
WHERE mt.IsDeleted = 0 AND mt.UserID = @UserID AND dt.IsDeleted = 0 AND v.IsDeleted = 0
GROUP BY dt.SupplierID,
v.AccountNumber, v.Name, v.Description, v.Address, v.CreditRating, v.PreferredVendorStatus, v.ActiveFlag, v.PurchasingWebServiceURL, v.WebsiteURL, v.UserID, v.ModifiedDate, v.IsDeleted, v.VendorID
ORDER BY [Priority] DESC", tables.Item2, tables.Item1);

                IDataReader reader = MHB.DAL.DataBaseConnector.GetDataReader(qry, connection, new SqlParameter("UserID", userID));

                while (reader.Read())
                {
                    Supplier supplier = new Supplier();

                    supplier.ID = reader.Get<int>("VendorID");
                    supplier.AccountNumber = reader.Get<string>("AccountNumber");
                    supplier.Name = reader.Get<string>("Name");
                    supplier.Description = reader.Get<string>("Description");
                    supplier.Address = reader.Get<string>("Address");
                    supplier.CreditRating = (int)reader.Get<byte>("CreditRating");
                    supplier.PreferredVendorStatus = reader.Get<bool>("PreferredVendorStatus");
                    supplier.ActiveFlag = reader.Get<bool>("ActiveFlag");
                    supplier.PurchasingWebServiceURL = reader.Get<string>("PurchasingWebServiceURL");
                    supplier.WebSiteURL = reader.Get<string>("WebsiteURL");
                    supplier.UserID = reader.Get<int>("UserID");
                    supplier.DateModified = reader.Get<DateTime>("ModifiedDate");
                    supplier.IsDeleted = reader.Get<bool>("IsDeleted");
                    supplier.TotalPurchasesCount = reader.Get<int>("TotalCount");
                    supplier.TotalPurchasesSum = reader.Get<decimal>("TotalSumSpent");

                    supplier.Connection = connection;

                    suppliers.Add(supplier);
                }

                decimal countTotalPurchases = suppliers.Sum(s => s.TotalPurchasesCount);

                decimal sumTotalPurchases = suppliers.Sum(s => s.TotalPurchasesSum);

                int index = 0;

                foreach (Supplier supplier in suppliers)
                {
                    supplier.TotalPurchasesSharePercentage = (supplier.TotalPurchasesCount / countTotalPurchases) * 100;

                    supplier.TotalPurchasesSumPercentage = (supplier.TotalPurchasesSum / sumTotalPurchases) * 100;

                    if (index <= 5 || supplier.DateModified > DateTime.Now.Subtract(TimeSpan.FromDays(10)) || supplier.PreferredVendorStatus)
                    {
                        supplier.Opacity = 1;
                    }
                    else if (index > 5 && index <= 8)
                    {
                        supplier.Opacity = 0.8M;
                    }
                    else if (index > 8 && index <= 12)
                    {
                        supplier.Opacity = 0.65M;
                    }
                    else if (index > 12 && index <= 16)
                    {
                        supplier.Opacity = 0.4M;
                    }
                    else
                    {
                        supplier.Opacity = 0.1M;
                    }

                    index++;
                }
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(ex, "MHB.BL.SQLHelper.GetSuppliers", qry, userID, connection.ConnectionString);
            }

            return suppliers;
        }

        #endregion [ GetSuppliers ]

        #region [ AddSupplier ]

        internal static int AddSupplier(SupplierBase s, string mainTableName, string detailsTableName)
        {
            string qry = string.Empty;

            int supplierID = -1;

            try
            {
                qry = @"INSERT INTO dbo.tbVendors (AccountNumber, Name, [Description], [Address], CreditRating, PreferredVendorStatus, ActiveFlag, PurchasingWebServiceURL, WebsiteURL, UserID, ModifiedDate, IsDeleted)
VALUES (@AccountNumber, @Name, @Description, @Address, @CreditRating, @PreferredVendorStatus, @ActiveFlag, @PurchasingWebServiceURL, @WebsiteURL, @UserID, GETDATE(), @IsDeleted)";

                qry += string.Format(@"DECLARE @SupplierID AS INT = (SELECT SCOPE_IDENTITY())
INSERT INTO {0} (UserID, Flagged, IsDeleted, FieldOldValue, ProductID, FieldInitialValue, IsShared) VALUES(@UserID, 0, 0, 0, 1, 0, 0)
DECLARE @RowID AS INT = (SELECT SCOPE_IDENTITY())
INSERT INTO {1} (ExpenditureID, IsDeleted, ProductID, SupplierID, MeasureTypeID, Amount, DetailInitialValue, IsShared, InitialAmount, InitialMeasureTypeID, HasProductParameters, IsSurplus)
VALUES(@RowID, 0, 1, @SupplierID, 0, 0, 0, 0, 0, 0, 0, 0)", mainTableName, detailsTableName);

                SqlParameter parAccountNumber = new SqlParameter("@AccountNumber", s.AccountNumber);
                SqlParameter parName = new SqlParameter("@Name", s.Name);
                SqlParameter parDescription = new SqlParameter("@Description", s.Description);
                SqlParameter parAddress = new SqlParameter("@Address", s.Address);
                SqlParameter parCreditRating = new SqlParameter("@CreditRating", s.CreditRating);
                SqlParameter parPreferredVendorStatus = new SqlParameter("@PreferredVendorStatus", s.PreferredVendorStatus);
                SqlParameter parActiveFlag = new SqlParameter("@ActiveFlag", s.ActiveFlag);
                SqlParameter parPurchasingWebServiceURL = new SqlParameter("@PurchasingWebServiceURL", s.PurchasingWebServiceURL);
                SqlParameter parWebSiteURL = new SqlParameter("@WebsiteURL", s.WebSiteURL);
                SqlParameter parUserID = new SqlParameter("@UserID", s.UserID);
                SqlParameter parIsDeleted = new SqlParameter("@IsDeleted", s.IsDeleted);

                object result =
MHB.DAL.DataBaseConnector.GetSingleValue(qry, s.Connection, parAccountNumber, parName, parDescription, parAddress, parCreditRating, parPreferredVendorStatus, parActiveFlag, parPurchasingWebServiceURL, parWebSiteURL, parUserID, parIsDeleted);

                int.TryParse(result.ToString(), out supplierID);
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(ex, "MHB.BL.SQLHelper.AddSupplier", qry, s.UserID, s.ConnectionString);
            }

            return supplierID;
        }

        #endregion [ AddSupplier ]

        #region [ UpdateSupplier ]

        internal static bool UpdateSupplier(SupplierBase s)
        {
            if (s.ID == 0) return false;

            string qry = string.Empty;

            bool result = false;

            try
            {
                qry = @"UPDATE tbVendors SET
                            AccountNumber = @AccountNumber,
                            Name = @Name,
                            [Description] = @Description,
                            [Address] = @Address,
                            CreditRating = @CreditRating,
                            PreferredVendorStatus = @PreferredVendorStatus,
                            ActiveFlag = @ActiveFlag,
                            PurchasingWebServiceURL = @PurchasingWebServiceURL,
                            WebsiteURL = @WebsiteURL,
                            UserID = @UserID,
                            ModifiedDate = GETDATE(),
                            IsDeleted = @IsDeleted
                            WHERE VendorID = @VendorID";

                SqlParameter parID = new SqlParameter("@VendorID", s.ID);
                SqlParameter parAccountNumber = new SqlParameter("@AccountNumber", s.AccountNumber);
                SqlParameter parName = new SqlParameter("@Name", s.Name);
                SqlParameter parDescription = new SqlParameter("@Description", s.Description);
                SqlParameter parAddress = new SqlParameter("@Address", s.Address);
                SqlParameter parCreditRating = new SqlParameter("@CreditRating", s.CreditRating);
                SqlParameter parPreferredVendorStatus = new SqlParameter("@PreferredVendorStatus", s.PreferredVendorStatus);
                SqlParameter parActiveFlag = new SqlParameter("@ActiveFlag", s.ActiveFlag);
                SqlParameter parPurchasingWebServiceURL = new SqlParameter("@PurchasingWebServiceURL", s.PurchasingWebServiceURL);
                SqlParameter parWebSiteURL = new SqlParameter("@WebsiteURL", s.WebSiteURL);
                SqlParameter parUserID = new SqlParameter("@UserID", s.UserID);
                SqlParameter parIsDeleted = new SqlParameter("@IsDeleted", s.IsDeleted);

                MHB.DAL.DataBaseConnector.ExecuteQuery(qry, s.Connection, parID, parAccountNumber, parName, parDescription, parAddress, parCreditRating, parPreferredVendorStatus, parActiveFlag, parPurchasingWebServiceURL, parWebSiteURL, parUserID, parIsDeleted);

                result = true;
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(ex, "MHB.BL.SQLHelper.UpdateSupplier", qry, s.UserID, s.ConnectionString);
            }

            return result;
        }

        #endregion [ UpdateSupplier ]

        #region [ DeleteSupplier ]

        internal static bool DeleteSupplier(int id, string connectionString)
        {
            string qry = string.Empty;

            bool result = false;

            try
            {
                qry = "UPDATE tbVendors SET IsDeleted = 1, ModifiedDate = GETDATE() WHERE VendorID = @ID";

                SqlParameter parID = new SqlParameter("@ID", id);

                MHB.DAL.DataBaseConnector.ExecuteQuery(qry, connectionString, parID);

                result = true;
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(ex, "MHB.BL.SQLHelper.DeleteSupplier", qry, 0, connectionString);
            }

            return result;
        }

        #endregion [ DeleteSupplier ]

        #region [ GetProductPriceStatistics ]

        internal static IEnumerable<Tuple<decimal, DateTime, int>> GetProductPriceStatistics(int productID, Enums.MeasureType measureType, string connectionString)
        {
            return SQLHelper.FillProductPriceStatisticsList(productID, measureType, connectionString);
        }

        internal static IEnumerable<Tuple<decimal, DateTime, int>> GetProductPriceStatistics(ProductBase product, Enums.MeasureType measureType)
        {
            return SQLHelper.FillProductPriceStatisticsList(product.ID, measureType, product.ConnectionString, product.Connection);
        }

        #endregion [ GetProductPriceStatistics ]

        #region [ FillProductPriceStatisticsList ]

        private static IEnumerable<Tuple<decimal, DateTime, int>> FillProductPriceStatisticsList(int productID, Enums.MeasureType measureType, string connectionString, SqlConnection connection = null)
        {
            List<Tuple<decimal, DateTime, int>> result = new List<Tuple<decimal, DateTime, int>>();

            try
            {
                IDataReader reader = SQLHelper.GetProductPriceStatisticsReader(productID, measureType, connectionString, connection);

                while (reader.Read())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("Price")) && !reader.IsDBNull(reader.GetOrdinal("DetailDate")))
                    {
                        result.Add(new Tuple<decimal, DateTime, int>((decimal)reader["Price"], (DateTime)reader["DetailDate"], (int)reader["SupplierID"]));
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(ex, "MHB.BL.SQLHelper.GetProductPriceStatistics", string.Empty, 0, connectionString);
            }

            return result;
        }

        #endregion [ FillProductPriceStatisticsList ]

        #region [ GetProductPriceStatisticsReader ]

        private static IDataReader GetProductPriceStatisticsReader(int productID, Enums.MeasureType measureType, string connectionString, SqlConnection connection = null)
        {
            if (connection == null)
                connection = new SqlConnection(connectionString);

            string qry = string.Empty;

            IDataReader reader = null;

            try
            {
                qry = "EXECUTE spGetDetailsProductPriceStatistics @ProductID, @MeasureType";

                SqlParameter parProductID = new SqlParameter("@ProductID", productID);
                SqlParameter parMeasureTypeID = new SqlParameter("@MeasureType", (int)measureType);

                reader = MHB.DAL.DataBaseConnector.GetDataReader(qry, connection, parProductID, parMeasureTypeID);
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(ex, "MHB.BL.SQLHelper.GetProductPriceStatisticsReader", qry, 0, connectionString);
            }

            return reader;
        }

        #endregion [ GetProductPriceStatisticsReader ]

        #region [ GetFrequentlyPurchasedItems ]

        internal static IDataReader GetFrequentlyPurchasedItems(int recordsReturnedCount, int month, int year, int userID, string connectionString, SqlConnection connection = null)
        {
            if (connection == null)
                connection = new SqlConnection(connectionString);

            string qry = string.Empty;

            IDataReader reader = null;

            try
            {
                qry = "EXECUTE spGetFrequentlyPurchasedProductsByDate @top, @month, @year, @userID";

                SqlParameter parTop = new SqlParameter("@top", recordsReturnedCount);
                SqlParameter parMonth = new SqlParameter("@month", month);
                SqlParameter parYear = new SqlParameter("@year", year);
                SqlParameter parUserID = new SqlParameter("@userID", userID);

                reader = MHB.DAL.DataBaseConnector.GetDataReader(qry, connection, parTop, parMonth, parYear, parUserID);
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(ex, "MHB.BL.SQLHelper.GetFrequentlyPurchasedItems", qry, userID, connectionString);
            }

            return reader;
        }

        #endregion [ GetFrequentlyPurchasedItems ]

        internal static void UpdateCategory(int categoryID, string categoryKeywords, int languageID, int userID, string connectionString, SqlConnection connection = null)

        {
            if (connection == null)
                connection = new SqlConnection(connectionString);

            string qry = string.Empty;

            try
            {
                qry = "EXECUTE spUpdateCategory @categoryId, @categoryKeyWordsCommaDelimited, @languageID";

                SqlParameter parCategoryID = new SqlParameter("categoryId", categoryID);
                SqlParameter parCategoryKeywordsCommaDelimited = new SqlParameter("categoryKeyWordsCommaDelimited", categoryKeywords);
                SqlParameter parLanguageID = new SqlParameter("languageID", languageID);

                MHB.DAL.DataBaseConnector.ExecuteQuery(qry, connection, parCategoryID, parCategoryKeywordsCommaDelimited, parLanguageID);
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(ex, "MHB.BL.SQLHelper.UpdateCategory", qry, userID, connectionString);
            }
        }
    }
}