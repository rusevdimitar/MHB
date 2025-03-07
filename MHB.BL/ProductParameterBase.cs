using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Runtime.Serialization;

namespace MHB.BL
{
    public class ProductParameterBase
    {
        public const int PRODUCT_PARAMETER_DEFAULT_ID = 0;

        public int ID { get; set; }

        public int ProductParameterTypeID { get; set; }

        public ProductParameterType ProductParameterType { get; set; }

        public int ProductID { get; set; }

        public Product Product { get; set; }

        public int ParentID { get; set; }

        public object Parent { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }

        [IgnoreDataMemberAttribute()]
        public SqlConnection Connection { get; set; }

        public ProductParameterBase()
        {
           
        }     
    }
}
