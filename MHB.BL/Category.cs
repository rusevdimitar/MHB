using System.Data.SqlClient;
namespace MHB.BL
{
    public class Category : CategoryBase
    {
        public Category() : base() { }

        public Category(int categoryID, short languageID, int userID, string connectionString) : base(categoryID, languageID, userID, connectionString) { }

        public Category(int categoryID, short languageID, int userID, SqlConnection connection) : base(categoryID, languageID, userID, connection) { }
    }
}