using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHB.DAL
{
    public static class Extensions
    {
        public static T Get<T>(this IDataReader reader, string columnName, T defaultValue = default(T))
        {
            if (!reader.IsDBNull(reader.GetOrdinal(columnName)))
            {
                return (T)reader[columnName];
            }
            else
            {
                return defaultValue;
            }
        }
    }
}