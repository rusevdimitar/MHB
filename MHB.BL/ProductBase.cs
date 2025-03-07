using MHB.DAL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace MHB.BL
{
    public class ProductBase
    {
        public const string PRODUCT_DEFAULT_COLOR = "FFFFFF";
        public const int PRODUCT_DEFAULT_ID = 1;

        private readonly IEnumerable<Category> _categoriesCache = Enumerable.Empty<Category>();
        public readonly IEnumerable<Supplier> _suppliersCache = Enumerable.Empty<Supplier>();

        #region Properties

        public int ID { get; set; }

        public int ParentID { get; set; }

        public object Parent { get; set; }

        public string Name { get; set; }

        private string _description = string.Empty;

        public string Description
        {
            get
            {
                if (this._description == null)
                    this._description = string.Empty;

                return this._description;
            }
            set
            {
                this._description = value;
            }
        }

        public string[] KeyWords { get; set; }

        public string[] OcrKeyWords { get; set; }

        public decimal StandardCost { get; set; }

        public decimal ListPrice { get; set; }

        private string _color = PRODUCT_DEFAULT_COLOR;

        public string Color
        {
            get
            {
                return this._color;
            }
            set
            {
                this._color = value;
            }
        }

        private byte[] _picture = new byte[0];

        public byte[] Picture
        {
            get
            {
                return this._picture;
            }
            set
            {
                this._picture = value;
            }
        }

        private decimal _weight = 0;

        public decimal Weight
        {
            get
            {
                return this._weight;
            }
            set
            {
                if (this._weight != value)
                {
                    this._weight = value;
                    this._volume = 0;
                }
            }
        }

        private decimal _volume = 0;

        public decimal Volume
        {
            get
            {
                return this._volume;
            }
            set
            {
                if (this._volume != value)
                {
                    this._volume = value;
                    this._weight = 0;
                }
            }
        }

        private decimal _packageUnitsCount = 1;

        public decimal PackageUnitsCount
        {
            get
            {
                return this._packageUnitsCount;
            }
            set
            {
                this._packageUnitsCount = value;
            }
        }

        public int UserID { get; set; }

        public DateTime DateModified { get; set; }

        public bool IsDeleted { get; set; }

        public string ConnectionString { get; set; }

        [IgnoreDataMemberAttribute()]
        public SqlConnection Connection { get; set; }

        private Supplier _supplier = new Supplier();

        public Supplier Supplier
        {
            get
            {
                return this._supplier;
            }
            set
            {
                this._supplier = value;
            }
        }

        private Category _category = new Category();

        public Category Category
        {
            get
            {
                return this._category;
            }
            set
            {
                this._category = value;
            }
        }

        private int _categoryID = Category.CATEGORY_DEFAULT_ID;

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

        private int _vendorID = 0;

        public int VendorID
        {
            get
            {
                return this._vendorID;
            }
            set
            {
                this._vendorID = value;
            }
        }

        public bool IsFixedMeasureType { get; set; }

        public Enums.MeasureType DefaultMeasureType { get; set; }

        public IEnumerable<ProductParameter> Parameters { get; set; }

        private Enums.MeasureType _prevailingMeasureType = Enums.MeasureType.Count;

        public Enums.MeasureType PrevailingMeasureType
        {
            get
            {
                return this._prevailingMeasureType;
            }
            set
            {
                this._prevailingMeasureType = value;
            }
        }

        #endregion Properties

        public ProductBase()
        {
        }

        public ProductBase(int id, int userID, string connectionString)
        {
            this.ID = id;
            this.UserID = userID;
            this.ConnectionString = connectionString;
            this.Load();
        }

        public ProductBase(int id, int userID, SqlConnection connection)
        {
            this.ID = id;
            this.UserID = userID;
            this.Connection = connection;
            this.Load();
        }

        public ProductBase(int id, int userID, int parentID, object parent, SqlConnection connection, IEnumerable<Category> categoriesCache = null, IEnumerable<Supplier> suppliersCache = null)
        {
            if (categoriesCache != null)
            {
                this._categoriesCache = categoriesCache;
            }

            if (suppliersCache != null)
            {
                this._suppliersCache = suppliersCache;
            }

            this.ID = id;
            this.UserID = userID;
            this.ParentID = parentID;
            this.Parent = parent;
            this.Connection = connection;
            this.Load();
        }

        public ProductBase(ProductBase product)
        {
            this.ID = product.ID;
            this.Name = product.Name;
            this.Description = product.Description;
            this.KeyWords = product.KeyWords;
            this.OcrKeyWords = product.OcrKeyWords;
            this.StandardCost = product.StandardCost;
            this.ListPrice = product.ListPrice;
            this.Color = product.Color;
            this.Picture = product.Picture;
            this.Weight = product.Weight;
            this.Volume = product.Volume;
            this.UserID = product.UserID;
            this.DateModified = product.DateModified;
            this.IsDeleted = product.IsDeleted;
            this.VendorID = product.VendorID;
            this.CategoryID = product.CategoryID;
            this.PackageUnitsCount = product.PackageUnitsCount;
            this.IsFixedMeasureType = product.IsFixedMeasureType;
            this.DefaultMeasureType = product.DefaultMeasureType;
            this.PrevailingMeasureType = product.PrevailingMeasureType;
            this.Supplier = product.Supplier;
            this.Category = product.Category;
        }

        public void Load()
        {
            SqlConnection cn = null;

            if (this.Connection != null)
                cn = this.Connection;
            else
                cn = new SqlConnection(this.ConnectionString);

            Product result = SQLHelper.GetProduct(this.ID, this.UserID, cn);

            this.ID = result.ID;
            this.Name = result.Name;
            this.Description = result.Description;
            this.KeyWords = result.KeyWords;
            this.OcrKeyWords = result.OcrKeyWords;
            this.StandardCost = result.StandardCost;
            this.ListPrice = result.ListPrice;
            this.Color = result.Color;
            this.Picture = result.Picture;
            this.Weight = result.Weight;
            this.Volume = result.Volume;
            this.UserID = result.UserID;
            this.DateModified = result.DateModified;
            this.IsDeleted = result.IsDeleted;
            this.VendorID = result.VendorID;
            this.CategoryID = result.CategoryID;
            this.PackageUnitsCount = result.PackageUnitsCount;
            this.IsFixedMeasureType = result.IsFixedMeasureType;
            this.DefaultMeasureType = result.DefaultMeasureType;
            this.PrevailingMeasureType = result.PrevailingMeasureType;

            if (this._suppliersCache.Any(s => s.ID == this.VendorID))
            {
                this._supplier = this._suppliersCache.Single(s => s.ID == this.VendorID);
            }
            else
            {
                this._supplier = SQLHelper.GetSupplier(this.VendorID, this.UserID, cn);
            }

            if (this._categoriesCache.Any(c => c.ID == this.CategoryID))
            {
                this._category = this._categoriesCache.Single(c => c.ID == this.CategoryID);
            }
            else
            {
                this._category = SQLHelper.GetCategory(this.CategoryID, 1, this.UserID, cn);
            }

            this.Parameters = SQLHelper.GetProductParameters(this.ParentID, this.ID, this.UserID, this.Connection, this.ConnectionString);

            if (this.Parameters.Any())
            {
                foreach (ProductParameter parameter in this.Parameters)
                {
                    parameter.Parent = this.Parent;
                    parameter.Product = result;
                }
            }
        }

        internal int Add()
        {
            return SQLHelper.AddProduct(this);
        }

        internal bool Update()
        {
            return SQLHelper.UpdateProduct(this);
        }

        public static bool Delete(int id, string connectionString, int userID)
        {
            return SQLHelper.DeleteProduct(id, connectionString, ExpenditureManager.GetUserMainTableNames(userID).Item1, ExpenditureManager.GetUserMainTableNames(userID).Item2);
        }

        public static void Merge(int newProductID, int oldProductID, int userID, string connectionString)
        {
            SQLHelper.MergeProducts(newProductID, oldProductID, userID, ExpenditureManager.GetUserMainTableNames(userID).Item1, ExpenditureManager.GetUserMainTableNames(userID).Item2, connectionString);
        }

        [Obsolete("This data can be obtained by loading all expenditure details by using a method similar to ExpenditureManager.GetExpenditureDetails")]
        public static IEnumerable<Tuple<decimal, DateTime, int>> GetProductPriceStatistics(int productID, Enums.MeasureType measureType, string connectionString)
        {
            return SQLHelper.GetProductPriceStatistics(productID, measureType, connectionString);
        }

        [Obsolete("This data can be obtained by loading all expenditure details by using a method similar to ExpenditureManager.GetExpenditureDetails")]
        public IEnumerable<Tuple<decimal, DateTime, int>> GetProductPriceStatistics(Enums.MeasureType measureType)
        {
            return SQLHelper.GetProductPriceStatistics(this, measureType);
        }

        [Obsolete("This data can be obtained by loading all expenditure details by using a method similar to ExpenditureManager.GetExpenditureDetails")]
        public IEnumerable<Tuple<decimal, DateTime, int>> GetProductPriceStatistics()
        {
            return SQLHelper.GetProductPriceStatistics(this, Enums.MeasureType.NotSet);
        }
    }
}