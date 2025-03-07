using System;
using System.Collections.Generic;

namespace MHB.BL
{
    public class TransactionManagerBase
    {
        #region [ Properties ]

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

        #endregion [ Properties ]

        #region [ Constructors ]

        public TransactionManagerBase()
        {
        }

        public TransactionManagerBase(string connectionString)
        {
            this._connectionString = connectionString;
        }

        #endregion [ Constructors ]

        #region [ Public Members ]

        #region [ RecordTransactions ]

        public bool RecordTransactions(List<Transaction> transactions)
        {
            try
            {
                SQLHelper.InsertTransactions(this._connectionString, transactions);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("TransactionManagerBase.RecordTransactions:{0}", ex.Message), ex);
            }
        }

        #endregion [ RecordTransactions ]

        #endregion [ Public Members ]
    }
}