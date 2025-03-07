using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHB.BL
{
    public class TranslationBase
    {
        public static IEnumerable<Translation> GetTranslations(string connectionString)
        {
            List<Translation> languageTranslations = new List<Translation>();

            // Same table is used for categories translations where ControlID is the ID of the Category
            string qry = "SELECT ControlID, ControlTextEN, ControlTextBG, ControlTextDE FROM dbo.tbLanguage WHERE ISNUMERIC(ControlID) = 0";

            try
            {
                IDataReader reader = MHB.DAL.DataBaseConnector.GetDataReader(qry, connectionString);

                while (reader.Read())
                {
                    Translation translation = new Translation();

                    if (!reader.IsDBNull(reader.GetOrdinal("ControlID")))
                    {
                        translation.ControlID = reader["ControlID"].ToString();
                    }

                    if (!reader.IsDBNull(reader.GetOrdinal("ControlTextEN")))
                    {
                        translation.English = reader["ControlTextEN"].ToString();
                    }

                    if (!reader.IsDBNull(reader.GetOrdinal("ControlTextBG")))
                    {
                        translation.Bulgarian = reader["ControlTextBG"].ToString();
                    }

                    if (!reader.IsDBNull(reader.GetOrdinal("ControlTextDE")))
                    {
                        translation.German = reader["ControlTextDE"].ToString();
                    }

                    languageTranslations.Add(translation);
                }
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(ex, "MHB.BL.TranslationBase.GetTranslations", qry, 0, connectionString);
            }

            return languageTranslations;
        }

        //  Dim qry As String = String.Empty

        //Try

        //    Dim controlID As String = TextBoxControlID.Text

        //    If Not String.IsNullOrWhiteSpace(controlID) Then

        //        Select Case Me.CurrentLanguage

        //            Case Language.Bulgarian
        //                qry = "UPDATE tbLanguage SET ControlTextBG = @Translation WHERE ControlID = @ControlID"
        //                Exit Select
        //            Case Language.English
        //                qry = "UPDATE tbLanguage SET ControlTextEN = @Translation WHERE ControlID = @ControlID "
        //                Exit Select
        //            Case Language.German
        //                qry = "UPDATE tbLanguage SET ControlTextDE = @Translation WHERE ControlID = @ControlID"
        //                Exit Select

        //        End Select

        //        Dim parTranslation As SqlParameter = New SqlParameter("Translation", TextBoxNewTranslation.Text)
        //        Dim parControlID As SqlParameter = New SqlParameter("ControlID", controlID)

        //        DatabaseConnector.ExecuteQuery(qry, Me.GetConnectionString, parTranslation, parControlID)

        //        HttpRuntime.Cache.Remove("translations")

        //        Response.Redirect(Request.RawUrl)

        //    End If

        public static void UpdateControlTranslation(string controlID, string newTranslation, Enums.Language language, string connectionString)
        {
            if (string.IsNullOrWhiteSpace(controlID)) throw new ArgumentNullException("[controlID] parameter is missing ! ! !");
            if (string.IsNullOrWhiteSpace(newTranslation)) throw new ArgumentNullException("[newTranslation] parameter is missing ! ! !");
            if (string.IsNullOrWhiteSpace(connectionString)) throw new ArgumentNullException("[connectionString] parameter is missing ! ! !");

            string qry = string.Empty;

            try
            {
                switch (language)
                {
                    case Enums.Language.Bulgarian:
                        qry = "UPDATE tbLanguage SET ControlTextBG = @Translation WHERE ControlID = @ControlID";
                        break;

                    case Enums.Language.English:
                        qry = "UPDATE tbLanguage SET ControlTextEN = @Translation WHERE ControlID = @ControlID";
                        break;

                    case Enums.Language.German:
                        qry = "UPDATE tbLanguage SET ControlTextDE = @Translation WHERE ControlID = @ControlID";
                        break;
                }

                SqlParameter parTranslation = new SqlParameter("Translation", newTranslation);
                SqlParameter parControlID = new SqlParameter("ControlID", controlID);

                MHB.DAL.DataBaseConnector.ExecuteQuery(qry, connectionString, parTranslation, parControlID);
            }
            catch (Exception ex)
            {
                Logging.Logger.Log(ex, "MHB.BL.TranslationBase.UpdateControlTranslation", qry, 0, connectionString);
            }
        }
    }
}