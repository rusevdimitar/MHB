using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHB.BL
{
    public class CategoryComment : CategoryCommentBase
    {
        public CategoryComment()
        {

        }
        public CategoryComment(int commentID, string connectionString)
            : base(commentID, connectionString)
        {

        }
    }
}
