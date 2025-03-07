using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHB.BL
{
    public class IncomeLogBase
    {
        public int ID { get; set; }
        public int IncomeID { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }
        public int UserID { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public DateTime IncomeDate { get; set; }
        public DateTime DateModified { get; set; }
        public Income.IncomeAction Action { get; set; }
    }
}
