

UPDATE tbDetailsTable01 SET Amount=1.00	WHERE ID=12366
UPDATE tbDetailsTable01 SET Amount=1.00	WHERE ID=12367
UPDATE tbDetailsTable01 SET Amount=1.00	WHERE ID=12368
UPDATE tbDetailsTable01 SET Amount=1.00	WHERE ID=14141
UPDATE tbDetailsTable01 SET Amount=1.00	WHERE ID=14159
UPDATE tbDetailsTable01 SET Amount=1.00	WHERE ID=14179
UPDATE tbDetailsTable01 SET Amount=1.00	WHERE ID=14342
UPDATE tbDetailsTable01 SET Amount=1.00	WHERE ID=14343
UPDATE tbDetailsTable01 SET Amount=0.30	WHERE ID=14715
UPDATE tbDetailsTable01 SET Amount=0.08	WHERE ID=14976
UPDATE tbDetailsTable01 SET Amount=0.08	WHERE ID=14977
UPDATE tbDetailsTable01 SET Amount=0.15	WHERE ID=15922
UPDATE tbDetailsTable01 SET Amount=0.10	WHERE ID=16242
UPDATE tbDetailsTable01 SET Amount=0.102	WHERE ID=16253
UPDATE tbDetailsTable01 SET Amount=0.25	WHERE ID=18778
UPDATE tbDetailsTable01 SET Amount=0.25	WHERE ID=18779
UPDATE tbDetailsTable01 SET Amount=0.25	WHERE ID=18782
UPDATE tbDetailsTable01 SET Amount=0.15	WHERE ID=19782
UPDATE tbDetailsTable01 SET Amount=0.15	WHERE ID=19783
UPDATE tbDetailsTable01 SET Amount=0.10	WHERE ID=19788
UPDATE tbDetailsTable01 SET Amount=1.00	WHERE ID=19858
UPDATE tbDetailsTable01 SET Amount=0.15	WHERE ID=20210
UPDATE tbDetailsTable01 SET Amount=0.20	WHERE ID=20438
UPDATE tbDetailsTable01 SET Amount=0.20	WHERE ID=20439
UPDATE tbDetailsTable01 SET Amount=0.20	WHERE ID=20649
UPDATE tbDetailsTable01 SET Amount=0.15	WHERE ID=23442
UPDATE tbDetailsTable01 SET Amount=0.07	WHERE ID=23452
UPDATE tbDetailsTable01 SET Amount=0.08	WHERE ID=23456
UPDATE tbDetailsTable01 SET Amount=0.08	WHERE ID=23457
UPDATE tbDetailsTable01 SET Amount=0.156	WHERE ID=23509
UPDATE tbDetailsTable01 SET Amount=0.10	WHERE ID=24114
UPDATE tbDetailsTable01 SET Amount=0.10	WHERE ID=24115
UPDATE tbDetailsTable01 SET Amount=0.10	WHERE ID=26047
UPDATE tbDetailsTable01 SET Amount=0.10	WHERE ID=26233
UPDATE tbDetailsTable01 SET Amount=0.10	WHERE ID=26234
UPDATE tbDetailsTable01 SET Amount=0.10	WHERE ID=26349
UPDATE tbDetailsTable01 SET Amount=0.10	WHERE ID=26350
UPDATE tbDetailsTable01 SET Amount=0.10	WHERE ID=26537
UPDATE tbDetailsTable01 SET Amount=0.106	WHERE ID=27113
UPDATE tbDetailsTable01 SET Amount=0.102	WHERE ID=27114
UPDATE tbDetailsTable01 SET Amount=0.138	WHERE ID=27115
UPDATE tbDetailsTable01 SET Amount=0.10	WHERE ID=27371
UPDATE tbDetailsTable01 SET Amount=0.102	WHERE ID=27372
UPDATE tbDetailsTable01 SET Amount=0.148	WHERE ID=27382
UPDATE tbDetailsTable01 SET Amount=0.10	WHERE ID=27697
UPDATE tbDetailsTable01 SET Amount=0.10	WHERE ID=28207
UPDATE tbDetailsTable01 SET Amount=0.10	WHERE ID=28208
UPDATE tbDetailsTable01 SET Amount=0.05	WHERE ID=28210
UPDATE tbDetailsTable01 SET Amount=0.05	WHERE ID=28211
UPDATE tbDetailsTable01 SET Amount=0.16	WHERE ID=28214
UPDATE tbDetailsTable01 SET Amount=0.242	WHERE ID=28405

ALTER TABLE tbDetailsTable01 ADD InitialAmount MONEY NOT NULL DEFAULT 0
ALTER TABLE tbDetailsTable02 ADD InitialAmount MONEY NOT NULL DEFAULT 0
ALTER TABLE tbDetailsTable03 ADD InitialAmount MONEY NOT NULL DEFAULT 0
ALTER TABLE tbDetailsTable04 ADD InitialAmount MONEY NOT NULL DEFAULT 0
ALTER TABLE tbDetailsTable05 ADD InitialAmount MONEY NOT NULL DEFAULT 0

UPDATE tbDetailsTable01 SET InitialAmount = Amount WHERE IsDeleted = 0
UPDATE tbDetailsTable02 SET InitialAmount = Amount WHERE IsDeleted = 0
UPDATE tbDetailsTable03 SET InitialAmount = Amount WHERE IsDeleted = 0
UPDATE tbDetailsTable04 SET InitialAmount = Amount WHERE IsDeleted = 0
UPDATE tbDetailsTable05 SET InitialAmount = Amount WHERE IsDeleted = 0

ALTER TABLE tbDetailsTable01 ADD InitialMeasureTypeID INT NOT NULL DEFAULT 0 FOREIGN KEY REFERENCES tbMeasureTypes(ID)
ALTER TABLE tbDetailsTable02 ADD InitialMeasureTypeID INT NOT NULL DEFAULT 0 FOREIGN KEY REFERENCES tbMeasureTypes(ID)
ALTER TABLE tbDetailsTable03 ADD InitialMeasureTypeID INT NOT NULL DEFAULT 0 FOREIGN KEY REFERENCES tbMeasureTypes(ID)
ALTER TABLE tbDetailsTable04 ADD InitialMeasureTypeID INT NOT NULL DEFAULT 0 FOREIGN KEY REFERENCES tbMeasureTypes(ID)
ALTER TABLE tbDetailsTable05 ADD InitialMeasureTypeID INT NOT NULL DEFAULT 0 FOREIGN KEY REFERENCES tbMeasureTypes(ID)

UPDATE tbDetailsTable01 SET InitialMeasureTypeID = MeasureTypeID WHERE IsDeleted = 0
UPDATE tbDetailsTable02 SET InitialMeasureTypeID = MeasureTypeID WHERE IsDeleted = 0
UPDATE tbDetailsTable03 SET InitialMeasureTypeID = MeasureTypeID WHERE IsDeleted = 0
UPDATE tbDetailsTable04 SET InitialMeasureTypeID = MeasureTypeID WHERE IsDeleted = 0
UPDATE tbDetailsTable05 SET InitialMeasureTypeID = MeasureTypeID WHERE IsDeleted = 0

USE [Test01Db]
GO
/****** Object:  StoredProcedure [dbo].[spDuplicateMonthRecords]    Script Date: 3/18/2015 1:11:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Rusev, Dimitar
-- Create date: 28.10.2011
-- Description:	Duplicates all the records of a given month.
-- Modified:	18.03.2015
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
			SELECT SCOPE_IDENTITY(), DetailName, DetailDescription, DetailValue, DetailDate, Attachment, AttachmentFileType, HasAttachment, IsDeleted, ProductID, SupplierID, MeasureTypeID, Amount, DetailDateCreated, DetailInitialValue, 0, CategoryID, InitialAmount, InitialMeasureTypeID
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


