using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHB.BL
{
    public class ProductParameter: ProductParameterBase
    {
        public bool Delete()
        {
            return SQLHelper.DeleteProductParameter(this);
        }

    }
}
