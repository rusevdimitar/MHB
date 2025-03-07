using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace MHB.BL
{
    public class TransactionBase
    {
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

        private int _ID = 0;

        public int ID
        {
            get
            {
                return _ID;
            }
            set
            {
                _ID = value;
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

        private int _expenseID = 0;

        public int ExpenseID
        {
            get
            {
                return _expenseID;
            }
            set
            {
                _expenseID = value;
            }
        }

        private decimal _oldValue = 0;

        public decimal OldValue
        {
            get
            {
                return _oldValue;
            }
            set
            {
                _oldValue = value;
            }
        }

        private decimal _newValue = 0;

        public decimal NewValue
        {
            get
            {
                return _newValue;
            }
            set
            {
                _newValue = value;
            }
        }

        private string _transactionText = string.Empty;

        public string TransactionText
        {
            get
            {
                return _transactionText;
            }
            set
            {
                _transactionText = value;
            }
        }

        private DateTime _dateModified = DateTime.Now;

        public DateTime DateModified
        {
            get
            {
                return _dateModified;
            }
            set
            {
                _dateModified = value;
            }
        }

        #endregion [ Properties ]

        public void Add()
        {
            SQLHelper.InsertTransaction(this._connectionString, this);
        }

        public static IEnumerable<Transaction> GetAll(int expenseID, string connectionString)
        {
            return SQLHelper.GetTransactions(connectionString, expenseID);
        }

        public static IEnumerable<Transaction> GetAll(int expenseID, SqlConnection connection)
        {
            return SQLHelper.GetTransactions(connection, expenseID);
        }
    }
}