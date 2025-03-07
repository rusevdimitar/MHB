using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHB.BL
{
    /// <summary>
    /// Used as a dto object easily serializable
    /// </summary>
    public class ExpenditureDetailInfo
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public ExpenditureDetailInfo()
        {
        }
    }
}