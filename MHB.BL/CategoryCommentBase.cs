using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHB.BL
{
    public class CategoryCommentBase
    {
        private int _id = 0;
        public int ID
        {
            get
            {
                return this._id;
            }
            set
            {
                this._id = value;
            }
        }

        private int _userID = 0;
        public int UserID
        {
            get
            {
                return this._userID;
            }
            set
            {
                this._userID = value;
            }
        }

        private int _categoryID = 0;
        public int CategoryID
        {
            get
            {
                return this._categoryID;
            }
            set
            {
                this._categoryID = value;
            }
        }

        private string _poster = string.Empty;
        public string Poster
        {
            get
            {
                return this._poster;
            }
            set
            {
                this._poster = value;
            }
        }

        private string _comment = string.Empty;
        public string Comment
        {
            get
            {
                return this._comment;
            }
            set
            {
                this._comment = value;
            }
        }

        private int _positiveVotesCount = 0;
        public int PositiveVotesCount
        {
            get
            {
                return this._positiveVotesCount;
            }
            set
            {
                this._positiveVotesCount = value;
            }
        }

        private int _negativeVotesCount = 0;
        public int NegativeVotesCount
        {
            get
            {
                return this._negativeVotesCount;
            }
            set
            {
                this._negativeVotesCount = value;
            }
        }

        private string _connectionString = string.Empty;
        public string ConnectionString
        {
            get
            {
                return this._connectionString;
            }
            set
            {
                this._connectionString = value;
            }
        }

        private int[] _usersVoted = new int[0];
        public int[] UsersVoted
        {
            get
            {
                return this._usersVoted;
            }
            set
            {
                this._usersVoted = value;
            }
        }

        private bool _isDeleted;
        public bool IsDeleted
        {
            get
            {
                return this._isDeleted;
            }
            set
            {
                this._isDeleted = value;
            }
        }

        private DateTime _dateModified;
        public DateTime DateModified
        {
            get
            {
                return this._dateModified;
            }
            set
            {
                this._dateModified = value;
            }
        }


        public CategoryCommentBase()
        {

        }

        public CategoryCommentBase(int commentID, string connectionString)
        {
            this._id = commentID;
            this._connectionString = connectionString;
            this.Load();
        }

        public void Load()
        {
            CategoryComment comment = SQLHelper.LoadCategoryComment(this._id, this._connectionString);

            this._id = comment.ID;
            this._userID = comment.UserID;
            this._categoryID = comment.CategoryID;
            this._poster = comment.Poster;
            this._comment = comment.Comment;
            this._positiveVotesCount = comment.PositiveVotesCount;
            this._negativeVotesCount = comment.NegativeVotesCount;
            this._usersVoted = comment.UsersVoted;
            this._isDeleted = comment.IsDeleted;
            this._dateModified = comment.DateModified;
        }

        public void Add()
        {
            SQLHelper.AddCategoryComment(this);
        }

        public void Add(CategoryCommentBase newCategoryComment)
        {
            SQLHelper.AddCategoryComment(newCategoryComment);
        }

        public void Delete()
        {
            SQLHelper.DeleteCategoryComment(this._id, this._connectionString);
        }

        public void VoteUp(int voterUserID)
        {
            SQLHelper.VoteOnCategoryComment(this, true, voterUserID);
        }

        public void VoteDown(int voterUserID)
        {
            SQLHelper.VoteOnCategoryComment(this, false, voterUserID);
        }

        public void VoteUp(CategoryCommentBase newCategoryComment, int voterUserID)
        {
            SQLHelper.VoteOnCategoryComment(newCategoryComment, true, voterUserID);
        }

        public void VoteDown(CategoryCommentBase newCategoryComment, int voterUserID)
        {
            SQLHelper.VoteOnCategoryComment(newCategoryComment, false, voterUserID);
        }
    }
}
