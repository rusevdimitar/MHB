using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Serialization;
using System.Linq;

namespace MHB.BL
{
    public class CategoryBase
    {
        #region [ Constants ]

        public const int Fuel = 1;
        public const int Electricity = 2;
        public const int Phone = 3;
        public const int Rent = 4;
        public const int Savings = 5;
        public const int Internet = 6;
        public const int Food = 7;
        public const int Car = 8;
        public const int Loan = 9;
        public const int Medical = 10;

        #endregion [ Constants ]

        #region [ Properties ]

        public const int CATEGORY_DEFAULT_ID = 0;

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

        private int _languageID = 0;

        public int LanguageID
        {
            get
            {
                return this._languageID;
            }
            set
            {
                this._languageID = value;
            }
        }

        private string _name = string.Empty;

        public string Name
        {
            get
            {
                if (string.IsNullOrEmpty(this._name))
                    this._name = string.Empty;

                return _name;
            }
            set
            {
                this._name = value;
            }
        }

        private string _categoryKeywords = string.Empty;

        public string CategoryKeywords
        {
            get
            {
                if (string.IsNullOrEmpty(_categoryKeywords))
                    this._categoryKeywords = string.Empty;

                return string.Format("{0},{1}", this._categoryKeywords.Trim(), this.Name.ToUpper());
            }
            set
            {
                this._categoryKeywords = value;
            }
        }

        public IEnumerable<string> Keywords
        {
            get
            {
                return this.CategoryKeywords.Split(',').Where(k => !string.IsNullOrWhiteSpace(k)).Select(k => k.Trim().ToUpper());
            }
        }

        private string _iconPath = string.Empty;

        public string IconPath
        {
            get
            {
                if (string.IsNullOrEmpty(_iconPath))
                    this._iconPath = "../Images/warning_icon_small.gif";

                return this._iconPath;
            }
            set
            {
                this._iconPath = value;
            }
        }

        private bool _isPayIconVisible;

        public bool IsPayIconVisible
        {
            get
            {
                return this._isPayIconVisible;
            }
            set
            {
                this._isPayIconVisible = value;
            }
        }

        private bool _isShared;

        public bool IsShared
        {
            get
            {
                return this._isShared;
            }
            set
            {
                this._isShared = value;
            }
        }

        private List<CategoryComment> _comments = new List<CategoryComment>();

        public List<CategoryComment> Comments
        {
            get
            {
                return this._comments;
            }
            set
            {
                this._comments = value;
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

        private int _commentsCount = 0;

        public int CommentsCount
        {
            get
            {
                return this._commentsCount;
            }
            set
            {
                this._commentsCount = value;
            }
        }

        private string _connectionString = string.Empty;

        public string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(this._connectionString))
                    this._connectionString = string.Empty;

                return this._connectionString;
            }
            set
            {
                this._connectionString = value;
            }
        }

        [IgnoreDataMemberAttribute()]
        public SqlConnection Connection { get; set; }

        #endregion [ Properties ]

        #region [ Constructors ]

        public CategoryBase()
        { }

        public CategoryBase(int categoryID, short languageID, int userID, string connectionString)
        {
            this._id = categoryID;
            this._languageID = languageID;
            this._userID = userID;
            this._connectionString = connectionString;
            this.Load();
        }

        public CategoryBase(int categoryID, short languageID, int userID, SqlConnection connection)
        {
            this._id = categoryID;
            this._languageID = languageID;
            this._userID = userID;
            this._connectionString = connection.ConnectionString;
            this.Connection = connection;
            this.Load();
        }

        #endregion [ Constructors ]

        public void Load()

        {
            Category c = null;

            if (this.Connection != null)
                c = SQLHelper.GetCategory(this._id, this._languageID, this._userID, this.Connection);
            else
                c = SQLHelper.GetCategory(this._id, this._languageID, this._userID, new SqlConnection(this._connectionString));

            this._id = c.ID;
            this._languageID = c.LanguageID;
            this._name = c.Name;
            this._iconPath = c.IconPath;
            this._isPayIconVisible = c.IsPayIconVisible;
            this._isShared = c.IsShared;
            this._userID = c.UserID;
            this._categoryKeywords = c.CategoryKeywords;
            this._commentsCount = c.CommentsCount;
        }

        public void LoadComments()
        {
            this._comments = SQLHelper.GetCategoryComments(this._id, this._userID, this._connectionString);
        }

        [Obsolete]
        public static string[] GetCategoryKeyWords(int languageID, int userID, string connectionString)
        {
            return SQLHelper.GetCategoriesKeyWords(languageID, userID, connectionString);
        }

        internal void Add()
        {
            SQLHelper.AddNewCategory(this._languageID, this._name, this._categoryKeywords, this._iconPath, this._isPayIconVisible, this._isShared, this._userID, this._connectionString);
        }

        internal void Update()
        {
            SQLHelper.UpdateCategory(this._id, this._categoryKeywords, this._languageID, this._userID, this._connectionString);
        }

        internal static void Delete(string connectionString, int categoryID)
        {
            SQLHelper.DeleteCategory(connectionString, categoryID);
        }

        public static int CopyCategory(string connectionString, int sourceCategoryID, int destinationUserID)
        {
            return SQLHelper.CopyCategory(connectionString, sourceCategoryID, destinationUserID);
        }

        public void SetShareFlag(bool isShared)
        {
            this._isShared = isShared;

            SQLHelper.SetCategoryShareFlag(this._connectionString, this._id, this._isShared);
        }
    }
}