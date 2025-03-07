using System.Collections.Generic;
using System.Linq;

namespace MHB.Entities
{
    public class ExpenseManager
    {
        private MHBEntities context = new MHBEntities();

        private int _userID = 0;

        public int UserID
        {
            get
            {
                return _userID;
            }
            set
            {
                _userID = value;
            }
        }

        public ExpenseManager()
        {
        }

        public List<tbMainTable01> GetUserExpenses()
        {
            List<tbMainTable01> expenses = new List<tbMainTable01>();

            var query = from m in context.tbMainTable01 where m.UserID == _userID select m;

            expenses = query.ToList<tbMainTable01>();

            return expenses;
        }
    }
}