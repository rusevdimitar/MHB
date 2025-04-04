USE [Test01Db]
GO
/****** Object:  StoredProcedure [dbo].[spGetAverageCostsForCategory]    Script Date: 11/22/2009 16:55:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Rusev, Dimitar
-- Create date: 07.10.2009
-- Description:	Calculates the average cost for a specific cost category
-- Modified:	22.11.2009
-- =============================================
ALTER PROCEDURE [dbo].[spGetAverageCostsForCategory]
	-- Add the parameters for the stored procedure here	
	@costCategory INT,
	@minValue INT,
	@maxValue INT,
	@userID INT,
	@mainTableIndex INT
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

DECLARE @s NVARCHAR(50)
DECLARE @s2 INT
DECLARE @sum BIGINT
DECLARE @currentSum BIGINT
DECLARE @resultsCount INT


SET @sum = 0
SET @resultsCount = 0


DECLARE MyCursor CURSOR FOR SELECT CostNames FROM dbo.tbCostCategories WHERE CostCategoryID = @costCategory
OPEN MyCursor

FETCH NEXT FROM MyCursor INTO @s

	WHILE @@fetch_status = 0 
	BEGIN
		IF @userID < 1000 
		BEGIN
			DECLARE MyCursor2 CURSOR FOR SELECT ID FROM dbo.tbMainTable01 WHERE FieldName LIKE @s
		END
		ELSE IF @userID >= 1000 AND @userID < 2000
		BEGIN
			DECLARE MyCursor2 CURSOR FOR SELECT ID FROM dbo.tbMainTable02 WHERE FieldName LIKE @s
		END
		ELSE IF @userID >= 2000 AND @userID < 3000
		BEGIN
			DECLARE MyCursor2 CURSOR FOR SELECT ID FROM dbo.tbMainTable03 WHERE FieldName LIKE @s
		END
		ELSE IF @userID >= 3000 AND @userID < 4000
		BEGIN
			DECLARE MyCursor2 CURSOR FOR SELECT ID FROM dbo.tbMainTable04 WHERE FieldName LIKE @s
		END
		ELSE IF @userID >= 4000 AND @userID < 5000
		BEGIN
			DECLARE MyCursor2 CURSOR FOR SELECT ID FROM dbo.tbMainTable05 WHERE FieldName LIKE @s
		END
		ELSE IF @userID >= 5000 AND @userID < 6000
		BEGIN
			DECLARE MyCursor2 CURSOR FOR SELECT ID FROM dbo.tbMainTable06 WHERE FieldName LIKE @s
		END
		
		
		OPEN MyCursor2
			WHILE @@fetch_status = 0 
			BEGIN
				SET @resultsCount = 1 + @resultsCount				
				IF @mainTableIndex = 1
					BEGIN
						SET @currentSum = ISNULL((SELECT FieldValue FROM tbMainTable01 WHERE ID = @s2 AND FieldValue BETWEEN @minValue AND @maxValue),0)			
						SET @sum = @sum + @currentSum
					END
				ELSE IF @mainTableIndex = 2
					BEGIN
						SET @currentSum = ISNULL((SELECT FieldValue FROM tbMainTable02 WHERE ID = @s2 AND FieldValue BETWEEN @minValue AND @maxValue),0)			
						SET @sum = @sum + @currentSum
					END	
				ELSE IF @mainTableIndex = 3
					BEGIN
						SET @currentSum = ISNULL((SELECT FieldValue FROM tbMainTable03 WHERE ID = @s2 AND FieldValue BETWEEN @minValue AND @maxValue),0)			
						SET @sum = @sum + @currentSum
					END 
				ELSE IF @mainTableIndex = 4
					BEGIN 
						SET @currentSum = ISNULL((SELECT FieldValue FROM tbMainTable04 WHERE ID = @s2 AND FieldValue BETWEEN @minValue AND @maxValue),0)			
						SET @sum = @sum + @currentSum
					END				
				ELSE IF @mainTableIndex = 5
					BEGIN
						SET @currentSum = ISNULL((SELECT FieldValue FROM tbMainTable05 WHERE ID = @s2 AND FieldValue BETWEEN @minValue AND @maxValue),0)			
						SET @sum = @sum + @currentSum
					END
				ELSE IF @mainTableIndex = 6
					BEGIN
						SET @currentSum = ISNULL((SELECT FieldValue FROM tbMainTable06 WHERE ID = @s2 AND FieldValue BETWEEN @minValue AND @maxValue),0)			
						SET @sum = @sum + @currentSum
					END
											  																				
				FETCH NEXT FROM MyCursor2 INTO @s2
				
			END		
			
			CLOSE MyCursor2
			DEALLOCATE MyCursor2
		FETCH NEXT FROM MyCursor INTO @s
	END

CLOSE MyCursor
DEALLOCATE MyCursor


SELECT (@sum / @resultsCount) AS AverageCost






END





