using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MHB.BL
{
    public class QueryManager
    {
        public static string GetQuery<T>(Expression<Func<T, bool>> filter)
        {
            string qry = string.Empty;
           
            qry = Regex.Replace(((LambdaExpression)filter).Body.ToString(), @"Convert\(([^)]*)\)", "$1");

            qry = Regex.Replace(qry, @"Parameter\(([^)]*)\)", "@$1").Replace("\"", string.Empty);

            qry = Regex.Replace(qry, @"-ConvertChecked\(([^)]*)\)", "$1");

            // Replace lambda expressions
            qry = Regex.Replace(qry, @"\w*\.\w*.Any\(\w*.=>.\(([^)]*)\)\)", "$1");

            // Replace lambda parameter with actual type (i.e 'exp =>..' from lambda will become 'Expenditure.')
            qry = qry.Replace(string.Format("{0}.", filter.Parameters[0].Name), string.Format("{0}.", filter.Parameters[0].Type.Name));

            qry = Regex.Replace(qry, string.Format(@"\w*(?<!{0})\.", filter.Parameters[0].Type.Name), "joinedtable.");
            
            qry = qry.Replace("AndAlso", "AND");

            qry = qry.Replace("OrElse", "OR");

            qry = qry.Replace("==", "=");

            qry = Regex.Replace(qry, "true", "1", RegexOptions.IgnoreCase);

            qry = Regex.Replace(qry, "false", "0", RegexOptions.IgnoreCase);

           

            return qry;
        }
    }
}