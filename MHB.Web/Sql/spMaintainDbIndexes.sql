
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Rusev, Dimitar
-- Create date: 2012-10-02
-- Description:	Rebuild NONCLUSTERED user-defined indexes if fragmentation percentage is higher than 28%
-- =============================================
CREATE PROCEDURE spMaintainDbIndexes
	
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @indexFragmentationPercent INT
	DECLARE @indexID SMALLINT

	DECLARE @indexName NVARCHAR(200)
	DECLARE @columnName NVARCHAR(200)
	DECLARE @tableName NVARCHAR(200)

	DECLARE MyCursor CURSOR FOR 
							SELECT 
							
							i.name AS IndexName,
							COL_NAME(ic.OBJECT_ID,ic.column_id) AS ColumnName,
							OBJECT_NAME(ic.OBJECT_ID) AS TableName												

							FROM sys.indexes AS i

							INNER JOIN sys.index_columns AS ic ON i.OBJECT_ID = ic.OBJECT_ID AND i.index_id = ic.index_id
							INNER JOIN sys.objects AS o ON o.object_id = i.object_id

							WHERE i.is_primary_key = 0 AND i.type = 2 AND o.type = 'U'
							order by TableName
				
	OPEN MyCursor

	FETCH NEXT FROM MyCursor INTO @indexName, @columnName, @tableName

		WHILE @@fetch_status = 0 
		BEGIN
		
			--#####################################
			--			Get Index ID
			--#####################################
			
			SET @indexID = (SELECT index_id FROM sys.indexes WHERE name = @indexName)  				

			--#####################################
			--			Get Index Info
			--#####################################
			
			SET @indexFragmentationPercent = (SELECT avg_fragmentation_in_percent AS PercentFragment

												FROM sys.dm_db_index_physical_stats(DB_ID(),

												OBJECT_ID(@tableName), @indexID, NULL , 'DETAILED')

												WHERE avg_fragmentation_in_percent > 0)
			
			IF @indexFragmentationPercent > 28
			BEGIN
			
				DECLARE @qry AS NVARCHAR(400)
				
				SET @qry = 'DROP INDEX ' + @indexName + ' ON ' + @tableName
				
				EXEC(@qry)
												
				SET @qry = 'CREATE INDEX ' + @indexName + ' ON ' + @tableName + ' (' + @columnName + ')'
				
				EXEC(@qry)		
				
			END
				
			FETCH NEXT FROM MyCursor INTO @indexName, @columnName, @tableName
		END

	CLOSE MyCursor
	DEALLOCATE MyCursor	
	
END

GO
