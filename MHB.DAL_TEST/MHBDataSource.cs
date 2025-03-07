using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MHB_DAL
{

    public class Expenditure : tbMainTable01 
    {
 
    }

    public class MHBDataSource
    {

        protected readonly MyHomeBillsDbDataContext _context = new MyHomeBillsDbDataContext();

        public MHBDataSource()
        { }

        public List<Expenditure> GetMonthExpenses(int month, int year, int userID, string mainTableName)
        {
            List<Expenditure> bills = new List<Expenditure>();

            

            switch (mainTableName)
            {
                case "tbMainTable01":



                    bills = (from b in new MyHomeBillsDbDataContext().tbMainTable01s
                             where b.Month == month && b.Year == year && b.UserID == userID
                             select new Expenditure
                             {
                                 ID = b.ID,
                                 UserID = b.UserID,
                                 Month = b.Month,
                                 Year = b.Year,
                                 FieldName = b.FieldName,
                                 FieldDescription = b.FieldDescription,
                                 FieldValue = b.FieldValue,
                                 FieldExpectedValue = b.FieldExpectedValue,
                                 DueDate = b.DueDate,
                                 DateRecordUpdated = b.DateRecordUpdated,
                                 IsPaid = b.IsPaid,
                                 HasDetails = b.HasDetails,
                                 Attachment = b.Attachment,
                                 AttachmentFileType = b.AttachmentFileType,
                                 HasAttachment = b.HasAttachment,
                                 OrderID = b.OrderID,
                                 CostCategory = b.CostCategory,
                                 Notified = b.Notified,
                                 NotificationDate = b.NotificationDate,
                                 Flagged = b.Flagged,
                                 IsDeleted = b.IsDeleted,
                                 FieldOldValue = b.FieldOldValue
                             }
                             ).ToList();
                    

                    break;
                case "tbMainTable02":
                    bills = (from b in new MyHomeBillsDbDataContext().tbMainTable02s
                             where b.Month == month && b.Year == year && b.UserID == userID
                             select new Expenditure
                             {
                                 ID = b.ID,
                                 UserID = b.UserID,
                                 Month = b.Month,
                                 Year = b.Year,
                                 FieldName = b.FieldName,
                                 FieldDescription = b.FieldDescription,
                                 FieldValue = b.FieldValue,
                                 FieldExpectedValue = b.FieldExpectedValue,
                                 DueDate = b.DueDate,
                                 DateRecordUpdated = b.DateRecordUpdated,
                                 IsPaid = b.IsPaid,
                                 HasDetails = b.HasDetails,
                                 Attachment = b.Attachment,
                                 AttachmentFileType = b.AttachmentFileType,
                                 HasAttachment = b.HasAttachment,
                                 OrderID = b.OrderID,
                                 CostCategory = b.CostCategory,
                                 Notified = b.Notified,
                                 NotificationDate = b.NotificationDate,
                                 Flagged = b.Flagged,
                                 IsDeleted = b.IsDeleted,
                                 FieldOldValue = b.FieldOldValue
                             }
                             ).ToList();

                    break;               
                case "tbMainTable03":
                    bills = (from b in new MyHomeBillsDbDataContext().tbMainTable03s
                             where b.Month == month && b.Year == year && b.UserID == userID
                             select new Expenditure
                             {
                                 ID = b.ID,
                                 UserID = b.UserID,
                                 Month = b.Month,
                                 Year = b.Year,
                                 FieldName = b.FieldName,
                                 FieldDescription = b.FieldDescription,
                                 FieldValue = b.FieldValue,
                                 FieldExpectedValue = b.FieldExpectedValue,
                                 DueDate = b.DueDate,
                                 DateRecordUpdated = b.DateRecordUpdated,
                                 IsPaid = b.IsPaid,
                                 HasDetails = b.HasDetails,
                                 Attachment = b.Attachment,
                                 AttachmentFileType = b.AttachmentFileType,
                                 HasAttachment = b.HasAttachment,
                                 OrderID = b.OrderID,
                                 CostCategory = b.CostCategory,
                                 Notified = b.Notified,
                                 NotificationDate = b.NotificationDate,
                                 Flagged = b.Flagged,
                                 IsDeleted = b.IsDeleted,
                                 FieldOldValue = b.FieldOldValue
                             }
                             ).ToList();

                    break;
                case "tbMainTable04":
                    bills = (from b in new MyHomeBillsDbDataContext().tbMainTable04s
                             where b.Month == month && b.Year == year && b.UserID == userID
                             select new Expenditure
                             {
                                 ID = b.ID,
                                 UserID = b.UserID,
                                 Month = b.Month,
                                 Year = b.Year,
                                 FieldName = b.FieldName,
                                 FieldDescription = b.FieldDescription,
                                 FieldValue = b.FieldValue,
                                 FieldExpectedValue = b.FieldExpectedValue,
                                 DueDate = b.DueDate,
                                 DateRecordUpdated = b.DateRecordUpdated,
                                 IsPaid = b.IsPaid,
                                 HasDetails = b.HasDetails,
                                 Attachment = b.Attachment,
                                 AttachmentFileType = b.AttachmentFileType,
                                 HasAttachment = b.HasAttachment,
                                 OrderID = b.OrderID,
                                 CostCategory = b.CostCategory,
                                 Notified = b.Notified,
                                 NotificationDate = b.NotificationDate,
                                 Flagged = b.Flagged,
                                 IsDeleted = b.IsDeleted,
                                 FieldOldValue = b.FieldOldValue
                             }
                             ).ToList();

                    break;
                case "tbMainTable05":
                    bills = (from b in new MyHomeBillsDbDataContext().tbMainTable05s
                             where b.Month == month && b.Year == year && b.UserID == userID
                             select new Expenditure
                             {
                                 ID = b.ID,
                                 UserID = b.UserID,
                                 Month = b.Month,
                                 Year = b.Year,
                                 FieldName = b.FieldName,
                                 FieldDescription = b.FieldDescription,
                                 FieldValue = b.FieldValue,
                                 FieldExpectedValue = b.FieldExpectedValue,
                                 DueDate = b.DueDate,
                                 DateRecordUpdated = b.DateRecordUpdated,
                                 IsPaid = b.IsPaid,
                                 HasDetails = b.HasDetails,
                                 Attachment = b.Attachment,
                                 AttachmentFileType = b.AttachmentFileType,
                                 HasAttachment = b.HasAttachment,
                                 OrderID = b.OrderID,
                                 CostCategory = b.CostCategory,
                                 Notified = b.Notified,
                                 NotificationDate = b.NotificationDate,
                                 Flagged = b.Flagged,
                                 IsDeleted = b.IsDeleted,
                                 FieldOldValue = b.FieldOldValue
                             }
                             ).ToList();

                    break;              
            }

            

            return bills;

        }

        public void Insert(Expenditure bill)
        {
            _context.tbMainTable01s.InsertOnSubmit(bill);
            _context.SubmitChanges();

        }
    }


}
