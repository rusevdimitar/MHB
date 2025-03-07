using System;
using System.Collections.Generic;

namespace MHB.BL
{
    public class IncomeBase
    {
        public enum IncomeAction
        {
            Add = 1,
            Update = 2,
            Delete = 3
        }

        #region [ Properties ]

        private string _connectionString = string.Empty;

        public string ConnectionString
        {
            get
            {
                return _connectionString;
            }
            set
            {
                _connectionString = value;
            }
        }

        private int _id = 0;

        public int ID
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

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

        private int _month = 0;

        public int Month
        {
            get
            {
                return _month;
            }
            set
            {
                _month = value;
            }
        }

        private int _year = 0;

        public int Year
        {
            get
            {
                return _year;
            }
            set
            {
                _year = value;
            }
        }

        private string _name = string.Empty;

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        private decimal _value = 0.0M;

        public decimal Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }

        private DateTime _date;

        public DateTime Date
        {
            get
            {
                return _date;
            }
            set
            {
                _date = value;
            }
        }

        #endregion [ Properties ]

        #region [ Constructors ]

        public IncomeBase() { }

        #endregion [ Constructors ]

        #region [ Public Methods ]

        public void Delete(int id)
        {
            SQLHelper.DeleteIncome(id, _connectionString);
        }

        public void Update()
        {
            SQLHelper.UpdateIncome(this, _connectionString);
        }

        public void Add()
        {
            SQLHelper.AddIncome(this, _connectionString);
        }

        public List<IncomeLog> GetLogs()
        {
            return SQLHelper.GetIncomeLogs(this._userID, this._month, this._year, this._connectionString);
        }
        #endregion [ Public Methods ]
    }
}