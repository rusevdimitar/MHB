using System.Collections.Generic;

namespace MHB.BL
{
    public class SortOptionBase
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public int Language { get; set; }

        public bool Enabled { get; set; }

        public SortOptionBase()
        { }

        public static List<SortOption> GetAll(string connectionString, int languageID)
        {
            return SQLHelper.GetSortOptions(connectionString, languageID);
        }
    }
}