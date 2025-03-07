using System;
using System.Collections.Generic;
using System.Linq;

namespace MHB.BL
{
    public class ExpenditureBase
    {
        public Int32 ID { get; set; }

        public Int32 UserID { get; set; }

        public UserManager.User User { get; set; }

        public Int32 Month { get; set; }

        public Int32 Year { get; set; }

        public String FieldName { get; set; }

        public String FieldDescription { get; set; }

        public Decimal FieldValue { get; set; }

        public Decimal FieldPreviousValue { get; set; }

        public Decimal FieldExpectedValue { get; set; }

        public Decimal FieldInitialValue { get; set; }

        public DateTime DueDate { get; set; }

        public DateTime DateRecordUpdated { get; set; }

        public DateTime DateRecordCreated { get; set; }

        public Boolean IsPaid { get; set; }

        public Boolean HasDetails { get; set; }

        public Byte[] Attachment { get; set; }

        public String AttachmentFileType { get; set; }

        public Boolean HasAttachment { get; set; }

        public Int32 OrderID { get; set; }

        public Int32 CategoryID { get; set; }

        public Category Category { get; set; }

        public Boolean Notified { get; set; }

        public DateTime NotificationDate { get; set; }

        public Boolean Flagged { get; set; }

        public Boolean IsDeleted { get; set; }

        public Boolean IsShared { get; set; }

        public Decimal FieldOldValue { get; set; }

        private IEnumerable<ExpenditureDetail> _details = Enumerable.Empty<ExpenditureDetail>();

        public IEnumerable<ExpenditureDetail> Details
        {
            get
            {
                return this._details;
            }
            set
            {
                this._details = value;
            }
        }

        public IEnumerable<Transaction> Transactions { get; set; }

        public Product Product { get; set; }

        public int ProductID { get; set; }

        public ExpenditureBase()
        {
        }
    }
}