USE [smetkieu_db1]
GO
/****** Object:  StoredProcedure [dbo].[spDuplicateMonthRecords]    Script Date: 02/15/2012 21:35:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Rusev, Dimitar
-- Create date: 28.10.2011
-- Description:	Duplicates all the records of a given month.
-- Modified:	15.02.2012
-- =============================================
ALTER PROCEDURE [dbo].[spDuplicateMonthRecords]
	
	@UserID INT,
	@CurrentMonth INT, 
	@CurrentYear INT,
	@DestinationMonth INT,
	@DestinationYear INT,
	@MainTableName NVARCHAR(20),
	@ReplaceDestinationRecords BIT,
	@FlaggedOnly BIT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


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
DECLARE @qry NVARCHAR(MAX)


IF @FlaggedOnly = 1
	BEGIN
		SET @qry = 'INSERT INTO tbMainTableTemp(UserID, Month, Year, FieldName, FieldDescription, FieldValue, FieldExpectedValue, DueDate, IsPaid, HasDetails, AttachmentFileType, HasAttachment, CostCategory, Notified, NotificationDate, Flagged, IsDeleted, FieldOldValue) 
		(SELECT UserID, Month, Year, FieldName, FieldDescription, FieldValue, FieldExpectedValue, DueDate, IsPaid, HasDetails, AttachmentFileType, HasAttachment, CostCategory, Notified, NotificationDate, Flagged, IsDeleted, FieldOldValue
		FROM ' + CAST(@MainTableName AS NVARCHAR) + ' WHERE UserID = ' + CAST(@UserID AS NVARCHAR) + ' AND Month = ' + CAST(@CurrentMonth AS NVARCHAR) + ' AND Year = ' + CAST(@CurrentYear AS NVARCHAR) + ' AND Flagged = 1 AND IsDeleted = 0)'
	END
ELSE
BEGIN
	SET @qry = 'INSERT INTO tbMainTableTemp(UserID, Month, Year, FieldName, FieldDescription, FieldValue, FieldExpectedValue, DueDate, IsPaid, HasDetails, AttachmentFileType, HasAttachment, CostCategory, Notified, NotificationDate, Flagged, IsDeleted, FieldOldValue) 
	(SELECT UserID, Month, Year, FieldName, FieldDescription, FieldValue, FieldExpectedValue, DueDate, IsPaid, HasDetails, AttachmentFileType, HasAttachment, CostCategory, Notified, NotificationDate, Flagged, IsDeleted, FieldOldValue
	FROM ' + CAST(@MainTableName AS NVARCHAR) + ' WHERE UserID = ' + CAST(@UserID AS NVARCHAR) + ' AND Month = ' + CAST(@CurrentMonth AS NVARCHAR) + ' AND Year = ' + CAST(@CurrentYear AS NVARCHAR) + ' AND IsDeleted = 0)'
END



EXECUTE sp_sqlexec @qry

SET @qry = ''

IF @ReplaceDestinationRecords = 1
BEGIN 
	SET @qry = 'DELETE FROM ' + CAST(@MainTableName AS NVARCHAR) + ' WHERE Month = ' + CAST(@DestinationMonth AS NVARCHAR) + ' AND Year = ' + CAST(@DestinationYear AS NVARCHAR) + ' AND UserID = ' + CAST(@UserID AS NVARCHAR)
	EXECUTE sp_sqlexec @qry
END

SET @qry = ''

DECLARE MyCursor CURSOR FOR 
SELECT FieldName, FieldDescription, FieldValue, FieldExpectedValue, DueDate, IsPaid, HasDetails, AttachmentFileType, HasAttachment, CostCategory, Notified, NotificationDate, Flagged, IsDeleted, FieldOldValue
FROM tbMainTableTemp WHERE UserID = @UserID

OPEN MyCursor

FETCH NEXT FROM MyCursor INTO @FieldName, @FieldDescription, @FieldValue, @FieldExpectedValue, @DueDate, @IsPaid, @HasDetails, @AttachmentFileType, @HasAttachment, @CostCategory, @Notified, @NotificationDate, @Flagged, @IsDeleted, @FieldOldValue

	WHILE @@fetch_status = 0 
	BEGIN		
		
		SET @qry = 'INSERT INTO ' + @MainTableName + ' (UserID, Month, Year, FieldName, FieldDescription, FieldValue, FieldExpectedValue, DueDate, IsPaid, HasDetails, AttachmentFileType, HasAttachment, CostCategory, Notified, NotificationDate, Flagged, IsDeleted, FieldOldValue)
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
		CAST(ISNULL(@IsPaid, 0) AS NVARCHAR) + ', ' + 
		CAST(ISNULL(@HasDetails, 0) AS NVARCHAR) + ', ' + 		
		+ '''' + CAST(ISNULL(@AttachmentFileType, '') AS NVARCHAR) + ''', ' + 
		CAST(ISNULL(@HasAttachment, 0) AS NVARCHAR) + ', ' + 
		CAST(ISNULL(@CostCategory, 0) AS NVARCHAR) + ', ' + 
		CAST(ISNULL(@Notified, 0) AS NVARCHAR) + ', ' + 		
		+ '''' + CONVERT(VARCHAR, ISNULL(@NotificationDate, '19000101'), 126) + ''', ' + 			
		CAST(ISNULL(@Flagged, 0) AS NVARCHAR) + ', ' + 
		CAST(ISNULL(@IsDeleted, 0) AS NVARCHAR) + ', ' + 
		CAST(ISNULL(@FieldOldValue, 0) AS NVARCHAR) + ')'
						
		
		EXECUTE sp_sqlexec @qry
								
	
		FETCH NEXT FROM MyCursor INTO @FieldName, @FieldDescription, @FieldValue, @FieldExpectedValue, @DueDate, @IsPaid, @HasDetails, @AttachmentFileType, @HasAttachment, @CostCategory, @Notified, @NotificationDate, @Flagged, @IsDeleted, @FieldOldValue
	END

CLOSE MyCursor
DEALLOCATE MyCursor


DELETE FROM tbMainTableTemp WHERE UserID = @UserID



END


