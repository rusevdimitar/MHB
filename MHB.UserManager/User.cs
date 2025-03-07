using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using MHB.DAL;

namespace MHB.UserManager
{
    [DataContract]
    public class User
    {
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

        [IgnoreDataMemberAttribute()]
        public SqlConnection Connection { get; set; }

        private byte[] _password = new byte[] { };

        public byte[] Password
        {
            get
            {
                return this._password;
            }
            set
            {
                this._password = value;
            }
        }

        public int UserID { get; set; }

        public string Email { get; set; }

        public string Currency { get; set; }

        public Language SelectedLanguage { get; set; }

        public bool HasSetLang { get; set; }

        public DateTime RegistrationDate { get; set; }

        public int AttachmentSize { get; set; }

        public DateTime LastLoginTime { get; set; }

        private IPAddress _lastIPAddress = IPAddress.Any;

        public IPAddress LastIPAddress
        {
            get
            {
                return this._lastIPAddress;
            }
            set
            {
                this._lastIPAddress = value;
            }
        }

        public string UserAgent { get; set; }

        public TimeSpan AutoLoginStartTime { get; set; }

        public TimeSpan AutoLoginEndTime { get; set; }

        public bool AutoLoginIsAllowed { get; set; }

        private IPAddress _autoLoginHomeAddress = IPAddress.Any;

        public IPAddress AutoLoginHomeAddress
        {
            get
            {
                return this._autoLoginHomeAddress;
            }
            set
            {
                this._autoLoginHomeAddress = value;
            }
        }

        #endregion Properties

        #region Constructors

        public User(SqlConnection connection, int userID)
        {
            try
            {
                this._connectionString = connection.ConnectionString;

                this.Connection = connection;

                this.UserID = userID;

                IDataReader reader = SQLHelper.GetUser(this.UserID, connection);

                while (reader.Read())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("Password")))
                        this.Password = (byte[])reader["Password"];

                    if (!reader.IsDBNull(reader.GetOrdinal("UserID")))
                        this.UserID = (int)reader["UserID"];

                    if (!reader.IsDBNull(reader.GetOrdinal("Email")))
                        this.Email = reader["Email"].ToString();

                    if (!reader.IsDBNull(reader.GetOrdinal("Currency")))
                        this.Currency = reader["Currency"].ToString();

                    if (!reader.IsDBNull(reader.GetOrdinal("Language")))
                        this.SelectedLanguage = (Language)reader["Language"];

                    if (!reader.IsDBNull(reader.GetOrdinal("HasSetLang")))
                        this.HasSetLang = (bool)reader["HasSetLang"];

                    if (!reader.IsDBNull(reader.GetOrdinal("RegistrationDate")))
                        this.RegistrationDate = (DateTime)reader["RegistrationDate"];

                    if (!reader.IsDBNull(reader.GetOrdinal("AttachmentSize")))
                        this.AttachmentSize = (int)reader["AttachmentSize"];

                    if (!reader.IsDBNull(reader.GetOrdinal("LastLoginTime")))
                        this.LastLoginTime = (DateTime)reader["LastLoginTime"];

                    if (!reader.IsDBNull(reader.GetOrdinal("LastIPAddress")))
                        IPAddress.TryParse(reader["LastIPAddress"].ToString(), out this._lastIPAddress);

                    if (!reader.IsDBNull(reader.GetOrdinal("UserAgent")))
                        this.UserAgent = reader["UserAgent"].ToString();

                    if (!reader.IsDBNull(reader.GetOrdinal("AutoLoginStartTime")))
                        this.AutoLoginStartTime = (TimeSpan)reader["AutoLoginStartTime"];

                    if (!reader.IsDBNull(reader.GetOrdinal("AutoLoginEndTime")))
                        this.AutoLoginEndTime = (TimeSpan)reader["AutoLoginEndTime"];

                    if (!reader.IsDBNull(reader.GetOrdinal("AutoLoginIsAllowed")))
                        this.AutoLoginIsAllowed = (bool)reader["AutoLoginIsAllowed"];

                    if (!reader.IsDBNull(reader.GetOrdinal("AutoLoginHomeAddress")))
                        IPAddress.TryParse(reader["AutoLoginHomeAddress"].ToString(), out this._autoLoginHomeAddress);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("UserManager.User.ctor:{0}", ex.Message), ex);
            }
        }

        #endregion Constructors

        public enum Language
        {
            Bulgarian = 0,
            English = 1,
            German = 2
        }

        [Obsolete("@RegistrationDate")]
        public bool UpdateUser()
        {
            string qry = string.Empty;

            try
            {
                qry = @"UPDATE tbUsers SET
                        [Password] = CAST(@Password AS VARBINARY),
                        Email = @Email,
                        Currency = @Currency,
                        [Language] = @Language,
                        HasSetLang = @HasSetLang,
                        RegistrationDate = RegistrationDate, -- DO NOT UPDATE that's why remove @
                        AttachmentSize = @AttachmentSize,
                        LastLoginTime = @LastLoginTime,
                        LastIPAddress = @LastIPAddress,
                        UserAgent = @UserAgent,
                        AutoLoginStartTime = @AutoLoginStartTime,
                        AutoLoginEndTime = @AutoLoginEndTime,
                        AutoLoginIsAllowed = @AutoLoginIsAllowed,
                        AutoLoginHomeAddress = @AutoLoginHomeAddress
                        WHERE UserID = @UserID;";

                SqlParameter[] parameters = new SqlParameter[15];

                parameters[0] = new SqlParameter("Password", this.Password);
                parameters[1] = new SqlParameter("Email", this.Email);
                parameters[2] = new SqlParameter("Currency", this.Currency);
                parameters[3] = new SqlParameter("Language", (int)this.SelectedLanguage);
                parameters[4] = new SqlParameter("HasSetLang", 1);
                parameters[5] = new SqlParameter("RegistrationDate", this.RegistrationDate);
                parameters[6] = new SqlParameter("AttachmentSize", this.AttachmentSize);
                parameters[7] = new SqlParameter("LastLoginTime", this.LastLoginTime);
                parameters[8] = new SqlParameter("LastIPAddress", this.LastIPAddress.ToString());
                parameters[9] = new SqlParameter("UserAgent", this.UserAgent);
                parameters[10] = new SqlParameter("AutoLoginStartTime", this.AutoLoginStartTime);
                parameters[11] = new SqlParameter("AutoLoginEndTime", this.AutoLoginEndTime);
                parameters[12] = new SqlParameter("AutoLoginIsAllowed", this.AutoLoginIsAllowed);
                parameters[13] = new SqlParameter("AutoLoginHomeAddress", this.AutoLoginHomeAddress.ToString());
                parameters[14] = new SqlParameter("UserID", this.UserID);

                MHB.DAL.DataBaseConnector.ExecuteQuery(qry, this._connectionString, parameters);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("MHB.UserManager.User.UpdateUser: {0} {1}", qry, ex.Message), ex);
            }
        }

        #region Static members

        #region SetUsersCurrentLanguage

        public static bool SetUsersCurrentLanguage(Language language, int userID, string connectionString)
        {
            string qry = string.Empty;
            string lang = string.Empty;

            try
            {
                switch (language)
                {
                    case Language.English:
                        lang = "1";
                        break;

                    case Language.Bulgarian:
                        lang = "0";
                        break;

                    case Language.German:
                        lang = "2";
                        break;
                }

                qry = "UPDATE [dbo].[tbUsers] SET [language] = @language WHERE [userID] = @userid";

                SqlParameter[] parameters = new SqlParameter[2];

                parameters[0] = new SqlParameter("userid", userID);
                parameters[1] = new SqlParameter("language", lang);

                MHB.DAL.DataBaseConnector.ExecuteQuery(qry, connectionString, parameters);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("MHB.UserManager.User.SetUsersCurrentLanguage(CurrentLanguage language, Int32 userID) threw an exception: " + qry + ex.Message, ex);
            }
        }

        #endregion SetUsersCurrentLanguage

        #region GetUserID

        public static int GetUserID(string email, string password, SqlConnection connection)
        {
            return User.GetUserIDInternal(email, password, connection, null);
        }

        public static int GetUserID(string email, string password, string connectionString)
        {
            return User.GetUserIDInternal(email, password, null, connectionString);
        }

        private static int GetUserIDInternal(string email, string password, SqlConnection connection, string connectionString)
        {
            int userID = 0;

            string qry = string.Empty;

            if (connection == null && string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException("MHB.UserManager.User.GetUserID: connectionString parameter is null or empty! Either provide a valid connection string or pass an existing SqlConnection!");

            if (connection == null)
                connection = new SqlConnection(connectionString);

            try
            {
                object result = null;

                qry = "SELECT UserID FROM dbo.tbUsers WHERE Email = @Email AND Password = CAST(@Password AS VARBINARY)";

                SqlParameter parEmail = new SqlParameter("Email", SqlDbType.VarChar, 100);
                parEmail.Value = email;

                SqlParameter parPassword = new SqlParameter("Password", SqlDbType.VarChar, 50);
                parPassword.Value = password;

                result = MHB.DAL.DataBaseConnector.GetSingleValue(qry, connection, parEmail, parPassword);

                if (result != null)
                {
                    int.TryParse(result.ToString(), out userID);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("MHB.UserManager.User.GetUserID: {0} {1}", ex.Message, qry), ex);
            }

            return userID;
        }

        #endregion GetUserID

        #region GetMonthlyBudge

        public static Double GetMonthlyBudget(int userID, int month, int year, string connectionString)
        {
            string qry = string.Empty;

            switch (month)
            {
                case 1:
                    qry = string.Format("SELECT ISNULL([BudgetJan], 0) FROM [dbo].[tbMonthlyBudget] WHERE [UserID] = {0} AND Year = {1}", userID, year);
                    break;

                case 2:
                    qry = string.Format("SELECT ISNULL([BudgetFeb], 0) FROM [dbo].[tbMonthlyBudget] WHERE [UserID] = {0} AND Year = {1}", userID, year);
                    break;

                case 3:
                    qry = string.Format("SELECT ISNULL([BudgetMar], 0) FROM [dbo].[tbMonthlyBudget] WHERE [UserID] = {0} AND Year = {1}", userID, year);
                    break;

                case 4:
                    qry = string.Format("SELECT ISNULL([BudgetApr], 0) FROM [dbo].[tbMonthlyBudget] WHERE [UserID] = {0} AND Year = {1}", userID, year);
                    break;

                case 5:
                    qry = string.Format("SELECT ISNULL([BudgetMay], 0) FROM [dbo].[tbMonthlyBudget] WHERE [UserID] = {0} AND Year = {1}", userID, year);
                    break;

                case 6:
                    qry = string.Format("SELECT ISNULL([BudgetJune], 0) FROM [dbo].[tbMonthlyBudget] WHERE [UserID] = {0} AND Year = {1}", userID, year);
                    break;

                case 7:
                    qry = string.Format("SELECT ISNULL([BudgetJuly], 0) FROM [dbo].[tbMonthlyBudget] WHERE [UserID] = {0} AND Year = {1}", userID, year);
                    break;

                case 8:
                    qry = string.Format("SELECT ISNULL([BudgetAug], 0) FROM [dbo].[tbMonthlyBudget] WHERE [UserID] = {0} AND Year = {1}", userID, year);
                    break;

                case 9:
                    qry = string.Format("SELECT ISNULL([BudgetSept], 0) FROM [dbo].[tbMonthlyBudget] WHERE [UserID] = {0} AND Year = {1}", userID, year);
                    break;

                case 10:
                    qry = string.Format("SELECT ISNULL([BudgetOct], 0) FROM [dbo].[tbMonthlyBudget] WHERE [UserID] = {0} AND Year = {1}", userID, year);
                    break;

                case 11:
                    qry = string.Format("SELECT ISNULL([BudgetNov], 0) FROM [dbo].[tbMonthlyBudget] WHERE [UserID] = {0} AND Year = {1}", userID, year);
                    break;

                case 12:
                    qry = string.Format("SELECT ISNULL([BudgetDec], 0) FROM [dbo].[tbMonthlyBudget] WHERE [UserID] = {0} AND Year = {1}", userID, year);
                    break;
            }

            object result = MHB.DAL.DataBaseConnector.GetSingleValue(qry, connectionString);

            double val = 0;

            if (double.TryParse(result.ToString(), out val))
            {
                return val;
            }
            else
                return 0;
        }

        #endregion GetMonthlyBudge

        #region GetLastIPAddress

        public static void GetLastIPAddress(Int32 userID, string connectionString, out string lastLoginTime, out string lastIPAddress, out string userAgent)
        {
            string qry = string.Format("SELECT [lastlogintime], [lastipaddress], [useragent] FROM [dbo].[tbUsers] WHERE [userID] = {0}", userID);

            IDataReader reader = null;

            lastLoginTime = string.Empty;
            lastIPAddress = string.Empty;
            userAgent = string.Empty;

            try
            {
                reader = MHB.DAL.DataBaseConnector.GetDataReader(qry, connectionString);

                while (reader.Read())
                {
                    if (!reader.IsDBNull(0))
                    {
                        lastLoginTime = reader["lastlogintime"].ToString();
                    }
                    if (!reader.IsDBNull(1))
                    {
                        lastIPAddress = reader["lastipaddress"].ToString();
                    }
                    if (!reader.IsDBNull(2))
                    {
                        userAgent = reader["useragent"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MHB.Logging.Logger.Log(ex, "MHB.UserManager.User.cs.GetLastIPAddress(Int32 userID, string connectionString, out string lastLoginTime, out string lastIPAddress, out string userAgent)", qry, userID, connectionString);
            }
        }

        #endregion GetLastIPAddress

        #region GetUserEmail

        public static IDataReader GetUserEmails(string connectionString, int userID)
        {
            string qry = string.Empty;
            IDataReader reader = null;

            qry = @"SELECT email FROM dbo.tbUsers";

            try
            {
                reader = MHB.DAL.DataBaseConnector.GetDataReader(qry, connectionString);
            }
            catch (Exception ex)
            {
                MHB.Logging.Logger.Log(ex, "MHB.UserManager.User.cs.GetUserEmails()", qry, userID, connectionString);
            }

            return reader;
        }

        #endregion GetUserEmail

        #region UpdateUserGeoLocationInfo

        public static void UpdateUserGeoLocationInfo(int userId, string connectionString, string ipAddress)
        {
            string qry = string.Empty;

            try
            {
                MHB.IPGeoLocation.GeoLocation geo = new MHB.IPGeoLocation.GeoLocation();

                MHB.IPGeoLocation.GeoData geoData = geo.GetGeoData(ipAddress);

                qry = string.Format(@"EXECUTE [dbo].[spAddUserGeoLocationInfo]
                               '{0}'
                               ,{1}
                              ,'{2}'
                              ,'{3}'
                              ,'{4}'
                              ,'{5}'
                              ,'{6}'
                              ,'{7}'
                              ,{8}
                              ,{9}",
                            geoData.IpAddress,
                            userId,
                            geoData.CountryCode,
                            geoData.CountryName,
                            geoData.RegionCode,
                            geoData.RegionName,
                            geoData.City,
                            geoData.ZipCode,
                            geoData.Latitude,
                            geoData.Longitude);

                MHB.DAL.DataBaseConnector.ExecuteQuery(qry, connectionString);
            }
            catch (Exception ex)
            {
                MHB.Logging.Logger.Log(ex, "UpdateUserGeoLocationInfo", qry, userId, connectionString);
            }
        }

        #endregion UpdateUserGeoLocationInfo

        #region GenerateAPIKey

        public static string GenerateAPIKey(string email, string password, bool isAdmin, string connectionString)
        {
            string qry = string.Empty;

            int userId = 0;
            string key = string.Empty;

            try
            {
                userId = GetUserID(email, password, connectionString);

                if (userId != 0)
                {
                    qry = "EXECUTE GenerateAPIKey @userId, @isAdmin";

                    object result = MHB.DAL.DataBaseConnector.GetSingleValue(qry, connectionString, new SqlParameter("@userId", userId), new SqlParameter("@isAdmin", isAdmin));

                    if (!string.IsNullOrEmpty(result.ToString()))
                    {
                        key = result.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MHB.Logging.Logger.Log(ex, "GenerateAPIKey", qry, userId, connectionString);
            }

            return key;
        }

        #endregion GenerateAPIKey

        #region GetAPIKey

        public static APIKey GetAPIKey(int userId, SqlConnection connection)
        {
            return User.GetAPIKeyInternal(userId, connection, null);
        }

        public static APIKey GetAPIKey(int userId, string connectionString)
        {
            return User.GetAPIKeyInternal(userId, null, connectionString);
        }

        public static APIKey GetAPIKey(string email, string connectionString)
        {
            return User.GetAPIKeyInternal(-1, null, connectionString, email);
        }

        public static APIKey GetAPIKeyInternal(int userId, SqlConnection connection, string connectionString, string email = "")
        {
            string qry = string.Empty;
            APIKey key = new APIKey();

            if (connection == null && string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException("MHB.UserManager.User.GetUserID: connectionString parameter is null or empty! Either provide a valid connection string or pass an existing SqlConnection!");

            if (connection == null)
                connection = new SqlConnection(connectionString);

            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[] { };

                if (userId >= 0)
                {
                    qry = "SELECT APIKey, IsAdmin, UserID FROM dbo.tbAPIUsers WHERE UserID = @userId";

                    sqlParameters = new SqlParameter[] { new SqlParameter("@userId", userId) };
                }
                else
                {
                    qry = "SELECT APIKey, IsAdmin, au.UserID FROM dbo.tbAPIUsers au INNER JOIN tbUsers u ON u.UserID = au.UserID WHERE u.Email = @email";

                    sqlParameters = new SqlParameter[] { new SqlParameter("@email", email) };
                }

                IDataReader reader = MHB.DAL.DataBaseConnector.GetDataReader(qry, connection, sqlParameters);

                while (reader.Read())
                {
                    key.Key = reader.Get<string>("APIKey");

                    key.IsAdmin = reader.Get<bool>("IsAdmin");

                    key.UserID = reader.Get<int>("UserID");

                    break;
                }

                if (!reader.IsClosed)
                    reader.Close();
            }
            catch (Exception ex)
            {
                MHB.Logging.Logger.Log(ex, "GetAPIKey", qry, userId, connectionString);
            }

            return key;
        }

        #endregion GetAPIKey

        #region IsInBlackList

        public static bool IsInBlackList(string ipAddressV4, string ipAddressV6, string connectionString, out int userID)
        {
            string qry = string.Empty;

            bool isBanned = false;

            // TODO: Get user id from procedure
            userID = -1;

            try
            {
                qry = "EXECUTE spCheckIPBlacklisted @ipAddressV4, @ipAddressV6";

                SqlParameter[] parameters = new SqlParameter[2];

                parameters[0] = new SqlParameter("@ipAddressV4", ipAddressV4);
                parameters[1] = new SqlParameter("@ipAddressV6", ipAddressV6);

                isBanned = MHB.DAL.DataBaseConnector.GetSingleValue<bool>(qry, connectionString, parameters);
            }
            catch (Exception ex)
            {
                MHB.Logging.Logger.Log(ex, "IsInBlackList", qry, 0, connectionString);
            }
            finally
            {
                if (isBanned)
                {
                    MHB.Logging.Logger.LogAction(Logging.Logger.HistoryAction.BlackListCheckCheck_BANNED, userID, connectionString, ipAddressV4);
                }
                else
                {
                    MHB.Logging.Logger.LogAction(Logging.Logger.HistoryAction.BlackListCheckCheckOK, userID, connectionString, ipAddressV4);
                }
            }

            return isBanned;
        }

        #endregion IsInBlackList

        #region AddToBlackList

        public static void AddToBlackList(int userID, string ipAddressV4, string ipAddressV6, string connectionString)
        {
            string qry = string.Empty;

            try
            {
                qry = "EXECUTE spAddUserToBlackList @userID, @ipAddressV4, @ipAddressV6";

                SqlParameter[] parameters = new SqlParameter[3];

                parameters[0] = new SqlParameter("@userID", userID);
                parameters[1] = new SqlParameter("@ipAddressV4", ipAddressV4 ?? string.Empty);
                parameters[2] = new SqlParameter("@ipAddressV6", ipAddressV6 ?? string.Empty);

                MHB.DAL.DataBaseConnector.ExecuteQuery(qry, connectionString, parameters);
            }
            catch (Exception ex)
            {
                MHB.Logging.Logger.Log(ex, "AddToBlackList", qry, userID, connectionString);
            }
        }

        #endregion AddToBlackList

        public string GetVisitorLanguageCodeByAddress(string ipAddress)
        {
            if (string.IsNullOrEmpty(ipAddress))
            {
                throw new ArgumentNullException("User.GetVisitorLanguageCodeByAddress: required ipAddres parameter is null!");
            }

            IPAddress ipCheck = null;

            if (!IPAddress.TryParse(ipAddress, out ipCheck))
            {
                throw new ArgumentOutOfRangeException("User.GetVisitorLanguageCodeByAddress: required ipAddres parameter is out of range!");
            }

            string qry = string.Empty, result = string.Empty;

            try
            {
                qry = @"SELECT TOP 1 CountryCode, COUNT(CountryCode) AS CountryCodeCccurrence
                            FROM tbUsersGeoLocationData
                            WHERE Ip = @IpAddress
                            GROUP BY CountryCode
                            ORDER BY CountryCodeCccurrence DESC";

                result = MHB.DAL.DataBaseConnector.GetSingleValue<string>(qry, this.Connection, new SqlParameter("IpAddress", ipAddress));
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(ex, "GetVisitorLanguageCodeByAddress", qry, this.UserID, this.ConnectionString);
            }

            return result ?? "";
        }

        #endregion Static members
    }

    public struct APIKey
    {
        public string Key { get; set; }

        public int UserID { get; set; }

        public bool IsAdmin { get; set; }
    }
}