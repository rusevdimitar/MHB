USE [Test01Db]
GO
/****** Object:  StoredProcedure [dbo].[spDuplicateMonthRecords]    Script Date: 01/27/2012 15:24:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Rusev, Dimitar
-- Create date: 27.07.2012
-- Description:	Copies a parent expense
-- Modified:	
-- =============================================
CREATE PROCEDURE [dbo].[spCopyParentExpense]
	
	@ID INT,
	@MainTableName NVARCHAR(20)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @qry NVARCHAR(MAX)

	SET @qry = 'INSERT INTO ' + CAST(@MainTableName AS NVARCHAR) + ' (UserID, [Month], [Year], FieldName, FieldDescription, FieldValue, FieldExpectedValue, DueDate, DateRecordUpdated, IsPaid, HasDetails, Attachment, AttachmentFileType, HasAttachment, OrderID, CostCategory, Notified, NotificationDate, Flagged, IsDeleted, FieldOldValue)
	SELECT UserID, [Month], [Year], FieldName, FieldDescription, FieldValue, FieldExpectedValue, DueDate, DateRecordUpdated, IsPaid, HasDetails, Attachment, AttachmentFileType, HasAttachment, OrderID, CostCategory, Notified, NotificationDate, Flagged, IsDeleted, FieldOldValue FROM ' + CAST(@MainTableName AS NVARCHAR) + ' WHERE ID = ' + CAST(@ID AS NVARCHAR)



	EXECUTE sp_sqlexec @qry


END


