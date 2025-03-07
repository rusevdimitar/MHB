using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHB.BL
{
    public class ExpressionQueryArgs : Dictionary<string, object>
    {
        public static T Parameter<T>(string name)
        {
            return default(T);
        }

        public SqlParameter[] SqlParameters
        {
            get
            {
                return this.Select(kv => new SqlParameter(kv.Key, kv.Value)).ToArray();
            }
        }
    }
}