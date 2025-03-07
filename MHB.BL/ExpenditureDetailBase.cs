using System;
using System.Data.SqlClient;
using System.Runtime.Serialization;

namespace MHB.BL
{
    public class ExpenditureDetailBase
    {
        public int ID { get; set; }

        public int ExpenditureID { get; set; }

        private Expenditure _parent = null;

        public Expenditure Parent
        {
            get
            {
                return this._parent;
            }
            set
            {
                this._parent = value;
                this.ExpenditureID = this._parent.ID;
            }
        }

        public string DetailName { get; set; }

        private string _detailDescription = string.Empty;

        public string DetailDescription
        {
            get
            {
                if (this._detailDescription == null)
                    this._detailDescription = string.Empty;

                return this._detailDescription;
            }
            set
            {
                this._detailDescription = value;
            }
        }

        public decimal DetailValue { get; set; }

        public decimal DetailInitialValue { get; set; }

        public decimal Amount { get; set; }

        public Enums.MeasureType MeasureType { get; set; }

        public DateTime DetailDate { get; set; }

        public DateTime DetailDateCreated { get; set; }

        public byte[] Attachment { get; set; }

        public string AttachmentFileType { get; set; }

        public bool HasAttachment { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsShared { get; set; }

        private int _productID = Product.PRODUCT_DEFAULT_ID;

        public int ProductID
        {
            get
            {
                return this._productID;
            }
            set
            {
                this._productID = value;
            }
        }

        public Product Product { get; set; }

        private int _supplierID = Supplier.SUPPLIER_DEFAULT_ID;

        public int SupplierID
        {
            get
            {
                return this._supplierID;
            }
            set
            {
                this._supplierID = value;
            }
        }

        public Supplier Supplier { get; set; }

        public string MainTableName { get; set; }

        public string DetailsTableName { get; set; }

        public string ConnectionString { get; set; }

        public int UserID { get; set; }

        public int CategoryID
        {
            get
            {
                if (this.Product != null)
                {
                    return this.Product.CategoryID;
                }
                else
                {
                    return Category.CATEGORY_DEFAULT_ID;
                }
            }
        }

        public decimal InitialAmount { get; set; }

        public Enums.MeasureType InitialMeasureType { get; set; }

        [IgnoreDataMemberAttribute()]
        public SqlConnection Connection { get; set; }

        [Obsolete("Why is this in db?? and not calculated dynamically??")]
        public bool HasProductParameters { get; set; }

        public ExpenditureDetailBase()
        {
        }
    }
}