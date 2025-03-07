USE [Test01Db]
GO
/****** Object:  StoredProcedure [dbo].[spCopyParentExpense]    Script Date: 9/7/2015 3:04:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Rusev, Dimitar
-- Create date: 27.07.2012
-- Description:	Copies a parent expense
-- Modified:	07.09.2015
-- =============================================
ALTER PROCEDURE [dbo].[spCopyParentExpense]
	
	@ID INT,
	@MainTableName NVARCHAR(20),
	@DetailsTableName NVARCHAR(20)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @qry NVARCHAR(MAX)

	SET @qry = 'INSERT INTO ' + CAST(@MainTableName AS NVARCHAR) + ' (UserID, [Month], [Year], FieldName, FieldDescription, FieldValue, FieldExpectedValue, DueDate, DateRecordUpdated, IsPaid, HasDetails, Attachment, AttachmentFileType, HasAttachment, OrderID, CostCategory, Notified, NotificationDate, Flagged, IsDeleted, FieldOldValue)
	SELECT UserID, [Month], [Year], FieldName, FieldDescription, FieldValue, FieldExpectedValue, DueDate, DateRecordUpdated, IsPaid, HasDetails, Attachment, AttachmentFileType, HasAttachment, OrderID, CostCategory, Notified, NotificationDate, Flagged, IsDeleted, FieldOldValue FROM ' + CAST(@MainTableName AS NVARCHAR) + ' WHERE ID = ' + CAST(@ID AS NVARCHAR) + '
	IF (SELECT HasDetails FROM ' + CAST(@MainTableName AS NVARCHAR) + ' WHERE ID =' + CAST(@ID AS NVARCHAR) + ') = 1
	BEGIN					
		INSERT INTO ' + @DetailsTableName + '	
		SELECT SCOPE_IDENTITY(), DetailName, DetailDescription, DetailValue, DetailDate, Attachment, AttachmentFileType, HasAttachment, IsDeleted, ProductID, SupplierID, MeasureTypeID, Amount, DetailDateCreated, DetailInitialValue, 0, CategoryID, InitialAmount, InitialMeasureTypeID, HasProductParameters, 0
		FROM ' + @DetailsTableName + ' WHERE ExpenditureID = ' + CAST(@ID AS NVARCHAR) + '
	END'


	EXECUTE sp_sqlexec @qry


END


