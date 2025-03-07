USE [Test01Db]
GO
/****** Object:  StoredProcedure [dbo].[spDuplicateMonthRecords]    Script Date: 2/10/2017 1:56:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Rusev, Dimitar
-- Create date: 28.10.2011
-- Description:	Duplicates all the records of a given month.
-- Modified:	13.08.2015
-- =============================================
ALTER PROCEDURE [dbo].[spDuplicateMonthRecords]
	
	@UserID INT,
	@CurrentMonth INT, 
	@CurrentYear INT,
	@DestinationMonth INT,
	@DestinationYear INT,
	@MainTableName NVARCHAR(13),
	@DetailsTableName NVARCHAR(16),
	@ReplaceDestinationRecords BIT,
	@FlaggedOnly BIT,
	@MarkUnpaid BIT,
	@ZeroCopiedActualSum BIT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

DECLARE @ID INT
DECLARE @FieldName NVARCHAR(50)
DECLARE @FieldDescription NVARCHAR(500)
DECLARE @FieldValue MONEY
DECLARE @FieldExpectedValue MONEY
DECLARE @DueDate DATETIME
DECLARE @IsPaid BIT
DECLARE @HasDetails BIT 
DECLARE @AttachmentFileType NCHAR(5)
DECLARE @HasAttachment BIT
DECLARE @CostCategory INT
DECLARE @Notified BIT
DECLARE @NotificationDate DATETIME 
DECLARE @Flagged BIT
DECLARE @IsDeleted BIT
DECLARE @FieldOldValue MONEY
DECLARE @ProductID INT
DECLARE @qry NVARCHAR(MAX)


IF @FlaggedOnly = 1
	BEGIN
		SET @qry = 'INSERT INTO tbMainTableTemp (ID, UserID, Month, Year, FieldName, FieldDescription, FieldValue, FieldExpectedValue, DueDate, IsPaid, HasDetails, AttachmentFileType, HasAttachment, CostCategory, Notified, NotificationDate, Flagged, IsDeleted, FieldOldValue, ProductID) 
		(SELECT ID, UserID, Month, Year, FieldName, FieldDescription, FieldValue, FieldExpectedValue, DueDate, IsPaid, HasDetails, AttachmentFileType, HasAttachment, CostCategory, Notified, NotificationDate, Flagged, IsDeleted, FieldOldValue, ProductID
		FROM ' + CAST(@MainTableName AS NVARCHAR) + ' WHERE UserID = ' + CAST(@UserID AS NVARCHAR) + ' AND Month = ' + CAST(@CurrentMonth AS NVARCHAR) + ' AND Year = ' + CAST(@CurrentYear AS NVARCHAR) + ' AND Flagged = 1 AND IsDeleted = 0)'
	END
ELSE
	BEGIN
		SET @qry = 'INSERT INTO tbMainTableTemp (ID, UserID, Month, Year, FieldName, FieldDescription, FieldValue, FieldExpectedValue, DueDate, IsPaid, HasDetails, AttachmentFileType, HasAttachment, CostCategory, Notified, NotificationDate, Flagged, IsDeleted, FieldOldValue, ProductID) 
		(SELECT ID, UserID, Month, Year, FieldName, FieldDescription, FieldValue, FieldExpectedValue, DueDate, IsPaid, HasDetails, AttachmentFileType, HasAttachment, CostCategory, Notified, NotificationDate, Flagged, IsDeleted, FieldOldValue, ProductID
		FROM ' + CAST(@MainTableName AS NVARCHAR) + ' WHERE UserID = ' + CAST(@UserID AS NVARCHAR) + ' AND Month = ' + CAST(@CurrentMonth AS NVARCHAR) + ' AND Year = ' + CAST(@CurrentYear AS NVARCHAR) + ' AND IsDeleted = 0)'
	END



EXECUTE sp_sqlexec @qry

SET @qry = ''

IF @ReplaceDestinationRecords = 1
BEGIN 
	SET @qry = 'UPDATE ' + CAST(@DetailsTableName AS NVARCHAR) + ' SET IsDeleted = 1 WHERE ExpenditureID IN (SELECT ID FROM ' + CAST(@MainTableName AS NVARCHAR) + ' WHERE Month = ' + CAST(@DestinationMonth AS NVARCHAR) + ' AND Year = ' + CAST(@DestinationYear AS NVARCHAR) + ' AND UserID = ' + CAST(@UserID AS NVARCHAR) + ')'
	EXECUTE sp_sqlexec @qry
	SET @qry = 'UPDATE ' + CAST(@MainTableName AS NVARCHAR) + ' SET IsDeleted = 1 WHERE Month = ' + CAST(@DestinationMonth AS NVARCHAR) + ' AND Year = ' + CAST(@DestinationYear AS NVARCHAR) + ' AND UserID = ' + CAST(@UserID AS NVARCHAR)
	EXECUTE sp_sqlexec @qry
END

SET @qry = ''

DECLARE MyCursor CURSOR FOR 
SELECT ID, FieldName, FieldDescription, FieldValue, FieldExpectedValue, DueDate, IsPaid, HasDetails, AttachmentFileType, HasAttachment, CostCategory, Notified, NotificationDate, Flagged, IsDeleted, FieldOldValue, ProductID
FROM tbMainTableTemp WHERE UserID = @UserID

OPEN MyCursor

FETCH NEXT FROM MyCursor INTO @ID, @FieldName, @FieldDescription, @FieldValue, @FieldExpectedValue, @DueDate, @IsPaid, @HasDetails, @AttachmentFileType, @HasAttachment, @CostCategory, @Notified, @NotificationDate, @Flagged, @IsDeleted, @FieldOldValue, @ProductID

	WHILE @@fetch_status = 0 
	BEGIN			
	
		IF ISDATE(@DueDate) = 1
		BEGIN			
			SET @DueDate = DATEADD(M, @DestinationMonth - @CurrentMonth, @DueDate)			
			SET @DueDate = DATEADD(Y, @DestinationYear - @CurrentYear, @DueDate)
		END
		
		IF @MarkUnpaid = 1
		BEGIN
			SET @IsPaid = 0			
		END
		
		IF @ZeroCopiedActualSum = 1
		BEGIN
			SET @FieldValue = 0
		END		
		
		SET @qry = 'INSERT INTO ' + @MainTableName + ' (UserID, Month, Year, FieldName, FieldDescription, FieldValue, FieldExpectedValue, DueDate, DateRecordUpdated, IsPaid, HasDetails, AttachmentFileType, HasAttachment, CostCategory, Notified, NotificationDate, Flagged, IsDeleted, FieldOldValue, ProductID)
		VALUES 
		(' + 
		CAST(@UserID AS NVARCHAR) + ', ' + 
		CAST(@DestinationMonth AS NVARCHAR) + ', ' +  
		CAST(@DestinationYear AS NVARCHAR) + ', ' +  
		+ '''' + CAST(ISNULL(@FieldName, '') AS NVARCHAR) + ''', ' +  
		+ '''' + CAST(ISNULL(@FieldDescription, '') AS NVARCHAR) + ''', ' + 
		CAST(ISNULL(@FieldValue, 0) AS NVARCHAR) + ', ' + 
		CAST(ISNULL(@FieldExpectedValue, 0) AS NVARCHAR) + ', ' + 
		+ '''' + CONVERT(VARCHAR, ISNULL(@DueDate, '19000101'), 126) + ''', ' + 	
		+ 'GETDATE(), ' +
		CAST(ISNULL(@IsPaid, 0) AS NVARCHAR) + ', ' + 
		CAST(ISNULL(@HasDetails, 0) AS NVARCHAR) + ', ' + 		
		+ '''' + CAST(ISNULL(@AttachmentFileType, '') AS NVARCHAR) + ''', ' + 
		CAST(ISNULL(@HasAttachment, 0) AS NVARCHAR) + ', ' + 
		CAST(ISNULL(@CostCategory, 0) AS NVARCHAR) + ', ' + 
		CAST(ISNULL(@Notified, 0) AS NVARCHAR) + ', ' + 		
		+ '''' + CONVERT(VARCHAR, ISNULL(@NotificationDate, '19000101'), 126) + ''', ' + 			
		CAST(ISNULL(@Flagged, 0) AS NVARCHAR) + ', ' + 
		CAST(ISNULL(@IsDeleted, 0) AS NVARCHAR) + ', ' + 
		CAST(ISNULL(@FieldOldValue, 0) AS NVARCHAR) + ', ' +
		CAST(ISNULL(@ProductID, 0) AS NVARCHAR) + ')
		IF ' + CAST(ISNULL(@HasDetails, 0) AS NVARCHAR) + ' = 1 AND ' + CAST(ISNULL(@ZeroCopiedActualSum, 0) AS NVARCHAR) + ' = 0
		BEGIN					
			INSERT INTO ' + @DetailsTableName + '	
			SELECT SCOPE_IDENTITY(), DetailName, DetailDescription, DetailValue, DetailDate, Attachment, AttachmentFileType, HasAttachment, IsDeleted, ProductID, SupplierID, MeasureTypeID, Amount, DetailDateCreated, DetailInitialValue, 0, CategoryID, InitialAmount, InitialMeasureTypeID, HasProductParameters, IsSurplus, IsOcrScanned
			FROM ' + @DetailsTableName + ' WHERE ExpenditureID = ' + CAST(@ID AS NVARCHAR) + '
		END'
								print @qry
		EXECUTE sp_sqlexec @qry

		FETCH NEXT FROM MyCursor INTO @ID, @FieldName, @FieldDescription, @FieldValue, @FieldExpectedValue, @DueDate, @IsPaid, @HasDetails, @AttachmentFileType, @HasAttachment, @CostCategory, @Notified, @NotificationDate, @Flagged, @IsDeleted, @FieldOldValue, @ProductID
	END

CLOSE MyCursor
DEALLOCATE MyCursor

DELETE FROM tbMainTableTemp WHERE UserID = @UserID

END


