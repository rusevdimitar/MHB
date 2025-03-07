// -----------------------------------------------------------------------
// <copyright file="SQLHelper.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MHB.UserManager
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    internal class SQLHelper
    {
 
        public static IDataReader GetUser(int userID, SqlConnection connection)
        {
            string qry = @"SELECT [Password], UserID, Email, Currency, [Language], HasSetLang, RegistrationDate, AttachmentSize, LastLoginTime, LastIPAddress, UserAgent, AutoLoginStartTime, AutoLoginEndTime, AutoLoginIsAllowed, AutoLoginHomeAddress
                            FROM tbUsers
                            WHERE UserID = @UserID";

            IDataReader reader = null;

            try
            {
                reader = MHB.DAL.DataBaseConnector.GetDataReader(qry, connection, new SqlParameter("@UserID", userID));
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(ex, string.Format("MHB.UserManager.SQLHelper.GetUser():{0}", ex.Message), qry, userID, connection.ConnectionString);
            }

            return reader;
        }
    }
}