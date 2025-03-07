using MHB.DAL;
using System;
using System.Data.SqlClient;
using System.Runtime.Serialization;

namespace MHB.BL
{
    public class SupplierBase
    {
        public int ID { get; set; }

        public const int SUPPLIER_DEFAULT_ID = 1;

        private string _accountNumber = string.Empty;

        public string AccountNumber
        {
            get
            {
                return this._accountNumber;
            }
            set
            {
                this._accountNumber = value;
            }
        }

        private string _name = string.Empty;

        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
            }
        }

        private string _description = string.Empty;

        public string Description
        {
            get
            {
                return this._description;
            }
            set
            {
                this._description = value;
            }
        }

        private string _address = string.Empty;

        public string Address
        {
            get
            {
                return this._address;
            }
            set
            {
                this._address = value;
            }
        }

        private int _creditRating = 0;

        public int CreditRating
        {
            get
            {
                return this._creditRating;
            }
            set
            {
                this._creditRating = value;
            }
        }

        public bool PreferredVendorStatus { get; set; }

        public bool ActiveFlag { get; set; }

        private string _purchasingWebServiceURL = string.Empty;

        public string PurchasingWebServiceURL
        {
            get
            {
                return this._purchasingWebServiceURL;
            }
            set
            {
                this._purchasingWebServiceURL = value;
            }
        }

        private string _webSiteURL = string.Empty;

        public string WebSiteURL
        {
            get
            {
                return this._webSiteURL;
            }
            set
            {
                this._webSiteURL = value;
            }
        }

        public int UserID { get; set; }

        public DateTime DateModified { get; set; }

        public bool IsDeleted { get; set; }

        public string ConnectionString { get; set; }

        [IgnoreDataMemberAttribute()]
        public SqlConnection Connection { get; set; }

        public SupplierBase()
        {
        }

        public SupplierBase(int id, int userID, string connectionString)
        {
            this.ID = id;
            this.UserID = userID;
            this.ConnectionString = connectionString;
            this.Load();
        }

        public SupplierBase(int id, int userID, SqlConnection connection)
        {
            this.ID = id;
            this.UserID = userID;
            this.Connection = connection;
            this.Load();
        }

        public void Load()
        {
            Supplier result = null;

            if (this.Connection != null)
                result = SQLHelper.GetSupplier(this.ID, this.UserID, this.Connection);
            else
                result = SQLHelper.GetSupplier(this.ID, this.UserID, new SqlConnection(this.ConnectionString));

            this.Name = result.Name;
            this.AccountNumber = result.AccountNumber;
            this.Description = result.Description;
            this.CreditRating = result.CreditRating;
            this.PreferredVendorStatus = result.PreferredVendorStatus;
            this.ActiveFlag = result.ActiveFlag;
            this.PurchasingWebServiceURL = result.PurchasingWebServiceURL;
            this.WebSiteURL = result.WebSiteURL;
            this.UserID = result.UserID;
            this.DateModified = result.DateModified;
            this.IsDeleted = result.IsDeleted;
            this.ConnectionString = result.ConnectionString;
            this.Connection = result.Connection;
        }

        internal int Add()
        {
            Tuple<string, string> tableNames = ExpenditureManager.GetUserMainTableNames(this.UserID);

            return SQLHelper.AddSupplier(this, tableNames.Item1, tableNames.Item2);
        }

        internal bool Update()
        {
            return SQLHelper.UpdateSupplier(this);
        }

        [Obsolete("Use ExpenditureManagerBase.DeleteSupplier()")]
        public static bool Delete(int id, string connectionString)
        {
            return SQLHelper.DeleteSupplier(id, connectionString);
        }
    }
}