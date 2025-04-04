USE [Test01Db]
GO
/****** Object:  StoredProcedure [dbo].[spGetUsersAverageExpensesForCategory]    Script Date: 08/13/2014 10:04:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Rusev, Dimitar
-- Create date: 08.10.2009
-- Description:	Gets the average user expenses for a category
-- Modified:	13.08.2014
-- =============================================
ALTER PROCEDURE [dbo].[spGetUsersAverageExpensesForCategory]
	@userID INT,
	@costCategory INT,
	@year INT,
	@month INT
AS
BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @maxValue NVARCHAR(100)
	DECLARE @minValue MONEY
	DECLARE @sumThisYear MONEY
	DECLARE @sum MONEY
	DECLARE @count MONEY
	DECLARE @max MONEY
	DECLARE @previousMonth INT
	DECLARE @previousYear INT
	DECLARE @difference MONEY
	DECLARE @query NVARCHAR(MAX);			
	DECLARE @paramsDefinition nvarchar(500);
	DECLARE @detailsTableName NVARCHAR(50);
	DECLARE @mainTableName NVARCHAR(50);

	IF @userID < 1000 
	BEGIN
		SELECT @mainTableName = N'tbMainTable01'  
		SELECT @detailsTableName = N'tbDetailsTable01'  
	END
	ELSE IF @userID >= 1000 AND @userID < 2000
	BEGIN
		SELECT @mainTableName = N'tbMainTable02'  
		SELECT @detailsTableName = N'tbDetailsTable02'
	END
	ELSE IF @userID >= 2000 AND @userID < 3000
	BEGIN
		SELECT @mainTableName = N'tbMainTable03'
		SELECT @detailsTableName = N'tbDetailsTable03'
	END
	ELSE IF @userID >= 3000 AND @userID < 4000
	BEGIN
		SELECT @mainTableName = N'tbMainTable04'
		SELECT @detailsTableName = N'tbDetailsTable04' 
	END
	ELSE IF @userID >= 4000 AND @userID < 5000
	BEGIN
		SELECT @mainTableName = N'tbMainTable05'
		SELECT @detailsTableName = N'tbDetailsTable05'
	END
	ELSE IF @userID >= 5000 AND @userID < 6000
	BEGIN
		SELECT @mainTableName = N'tbMainTable06'
		SELECT @detailsTableName = N'tbDetailsTable06'
	END
	
	IF @month = 1 
		BEGIN
			SET @previousMonth = 12
			SET @previousYear = @year - 1
		END
	ELSE 							
		BEGIN			
			SET @previousMonth = @month - 1		
			SET @previousYear = @year
		END
	
	
			--|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
			--+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
			
		    DECLARE @currentSumForCategoryMain MONEY = (SELECT ISNULL(SUM(FieldValue), 0) FROM tbMainTable01 WHERE [Month] = @month AND UserID = @userID AND [Year] = @year AND IsDeleted = 0 AND CostCategory = @costCategory)
			DECLARE @currentSumForCategoryDetails MONEY = (SELECT ISNULL(SUM(DetailValue), 0) FROM tbDetailsTable01 dt INNER JOIN tbMainTable01 mt ON dt.ExpenditureID = mt.ID WHERE DATEPART(YEAR, dt.DetailDate) = @year AND DATEPART(MONTH, dt.DetailDate) = @month AND dt.CategoryID = @costCategory AND mt.CostCategory <> @costCategory AND mt.UserID = @userID AND dt.IsDeleted = 0)
									
			DECLARE @currentSumForCategory MONEY = @currentSumForCategoryMain + @currentSumForCategoryDetails
			
					
			DECLARE @previousSumForCategoryMain MONEY = (SELECT ISNULL(SUM(FieldValue), 0) FROM tbMainTable01 WHERE [Month] = @previousMonth AND UserID = @userID AND [Year] = @previousYear AND IsDeleted = 0 AND CostCategory = @costCategory)
			DECLARE @previousSumForCategoryDetails MONEY = (SELECT ISNULL(SUM(DetailValue), 0) FROM tbDetailsTable01 dt INNER JOIN tbMainTable01 mt ON dt.ExpenditureID = mt.ID WHERE DATEPART(YEAR, dt.DetailDate) = @previousYear AND DATEPART(MONTH, dt.DetailDate) = @previousMonth AND dt.CategoryID = @costCategory AND mt.CostCategory <> @costCategory AND mt.UserID = @userID AND dt.IsDeleted = 0)
			
			DECLARE @previuosSumForCategory MONEY = @previousSumForCategoryMain + @previousSumForCategoryDetails
			  
			  
			SET @difference = @currentSumForCategory - @previuosSumForCategory            
			--=============================================================================================================================
			
			
			--+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
			--	Get MAX value (and string with date i.e: 177.99 (12, 2012)) for category from either tbDetailsTableXX or tbMainTableXX
			--	depending on where the highest value is - details or main
			--+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
			SET @paramsDefinition = N'
					@max MONEY OUTPUT, 
					@maxValue NVARCHAR(100) OUTPUT, 
					@costCategory INT, 
					@userID INT'
					
			SET @query =
		   'DECLARE @maxMain MONEY = (SELECT ISNULL(MAX(FieldValue), 0) FROM ' + CAST(@mainTableName AS NVARCHAR) + ' WHERE CostCategory = @costCategory AND UserID = @userID AND IsDeleted = 0)						
			DECLARE @maxDetails MONEY = (SELECT ISNULL(MAX(DetailValue), 0) FROM ' + CAST(@detailsTableName AS NVARCHAR) + ' dt INNER JOIN ' + CAST(@mainTableName AS NVARCHAR) + ' mt ON dt.ExpenditureID = mt.ID WHERE dt.CategoryID = @costCategory AND mt.UserID = @userID AND dt.IsDeleted = 0)
								
			IF @maxMain > @maxDetails
			BEGIN
				SET @max = @maxMain
				SET @maxValue = (SELECT TOP 1 CAST(FieldValue AS NVARCHAR) + '' ('' + CAST([Month] AS NVARCHAR) + '', '' + CAST([Year] AS NVARCHAR) + '')''
								 FROM ' + CAST(@mainTableName AS NVARCHAR) + ' 
								 WHERE IsDeleted = 0 AND CostCategory = @costCategory AND UserID = @userID AND FieldValue = @maxMain)
			END
			ELSE
			BEGIN
				SET @max = @maxDetails
				SET @maxValue = (SELECT TOP 1 CAST(dt.DetailValue AS NVARCHAR) + '' ('' + CAST(DATEPART(MONTH, DetailDate) AS NVARCHAR) + '', '' + CAST(DATEPART(YEAR, DetailDate) AS NVARCHAR) + '')'' 
								 FROM ' + CAST(@detailsTableName AS NVARCHAR) + ' dt INNER JOIN ' + CAST(@mainTableName AS NVARCHAR) + ' mt ON mt.ID = dt.ExpenditureID
								 WHERE dt.IsDeleted = 0 AND dt.CategoryID = @costCategory AND mt.UserID = @userID AND DetailValue = @maxDetails)
			END'
			--=============================================================================================================================
			EXEC sp_executesql @query, @paramsDefinition, @max OUTPUT, @maxValue OUTPUT, @costCategory, @userID
			
			--+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
			--	Get MIN value for category from either tbDetailsTableXX or tbMainTableXX
			--	depending on where the lowest value is - details or main
			--+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
			SET @paramsDefinition = N'
					@minValue MONEY OUTPUT, 				
					@costCategory INT, 
					@userID INT'
					
			SET @query =
		   'DECLARE @minMain MONEY = (SELECT ISNULL(MIN(FieldValue), 0) FROM dbo.' + CAST(@mainTableName AS NVARCHAR) + ' WHERE CostCategory = @costCategory AND UserID = @userID AND IsDeleted = 0)
			DECLARE @minDetails MONEY = (SELECT ISNULL(MIN(DetailValue), 0) FROM ' + CAST(@detailsTableName AS NVARCHAR) + ' dt INNER JOIN ' + CAST(@mainTableName AS NVARCHAR) + ' mt ON dt.ExpenditureID = mt.ID WHERE dt.CategoryID = @costCategory AND mt.UserID = @userID AND dt.IsDeleted = 0)
					
			SET @minValue = (SELECT CASE
								WHEN @minMain > 0 AND @minDetails = 0 THEN @minMain																				
								WHEN @minDetails > 0 AND @minMain = 0 THEN @minDetails
								WHEN @minMain < @minDetails THEN @minMain
								WHEN @minDetails < @minMain THEN @minDetails
							 END)'														
			--=============================================================================================================================
			EXEC sp_executesql @query, @paramsDefinition, @minValue OUTPUT, @costCategory, @userID
			
			--+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
			--	Get SUM value for category from either tbDetailsTableXX or tbMainTableXX		
			--+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
			SET @paramsDefinition = N'
					@sum MONEY OUTPUT, 				
					@costCategory INT, 
					@userID INT,
					@max MONEY'
					
			SET @query =	
		   'DECLARE @sumMain MONEY = (SELECT ISNULL(SUM(FieldValue), 0) FROM dbo.' + CAST(@mainTableName AS NVARCHAR) + ' WHERE FieldValue <> 0 AND UserID = @userID AND CostCategory = @costCategory AND IsDeleted = 0)
			DECLARE @sumDetails MONEY = (SELECT ISNULL(SUM(DetailValue), 0) FROM ' + CAST(@detailsTableName AS NVARCHAR) + ' dt INNER JOIN ' + CAST(@mainTableName AS NVARCHAR) + ' mt ON dt.ExpenditureID = mt.ID WHERE dt.CategoryID = @costCategory AND mt.CostCategory <> @costCategory AND mt.UserID = @userID AND dt.IsDeleted = 0)
			
			SET @sum = @sumMain + @sumDetails'
			--=============================================================================================================================
			EXEC sp_executesql @query, @paramsDefinition, @sum OUTPUT, @costCategory, @userID, @max
			
			--+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
			SET @paramsDefinition = N'
					@count MONEY OUTPUT, 				
					@costCategory INT, 
					@userID INT,
					@max MONEY'
					
			SET @query =
		   'DECLARE @countMain INT = (SELECT ISNULL(COUNT(ID), 0) from ' + CAST(@mainTableName AS NVARCHAR) + ' WHERE FieldValue <> 0 AND FieldValue <= @max AND CostCategory = @costCategory AND IsDeleted = 0)
			DECLARE @countDetails INT = (SELECT ISNULL(COUNT(dt.ID), 0) FROM ' + CAST(@detailsTableName AS NVARCHAR) + ' dt INNER JOIN ' + CAST(@mainTableName AS NVARCHAR) + ' mt ON dt.ExpenditureID = mt.ID WHERE dt.CategoryID = @costCategory AND mt.UserID = @userID AND dt.IsDeleted = 0)
			
			SET @count = @countMain + @countDetails'
			--=============================================================================================================================
			EXEC sp_executesql @query, @paramsDefinition, @count OUTPUT, @costCategory, @userID, @max
			
			--+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
			SET @paramsDefinition = N'
					@sumThisYear MONEY OUTPUT, 				
					@costCategory INT, 
					@userID INT,
					@year INT'
					
					
			SET @query =
		   'DECLARE @sumThisYearMain MONEY = (SELECT ISNULL(SUM(FieldValue), 0) FROM ' + CAST(@mainTableName AS NVARCHAR) + ' WHERE UserID = @userID AND CostCategory = @costCategory AND [Year] = @year AND IsDeleted = 0)
			DECLARE @sumThisYearDetails MONEY = (SELECT ISNULL(SUM(dt.DetailValue), 0) FROM ' + CAST(@detailsTableName AS NVARCHAR) + ' dt INNER JOIN ' + CAST(@mainTableName AS NVARCHAR) + ' mt ON dt.ExpenditureID = mt.ID WHERE DATEPART(YEAR, dt.DetailDate) = @year AND dt.CategoryID = @costCategory AND mt.CostCategory <> @costCategory AND mt.UserID = @userID AND dt.IsDeleted = 0)
			
			SET @sumThisYear = @sumThisYearMain + @sumThisYearDetails'
			--=============================================================================================================================
			EXEC sp_executesql @query, @paramsDefinition, @sumThisYear OUTPUT, @costCategory, @userID, @year			
			--|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
	
	
	







select @maxValue AS MaxString, @max AS MaxValue, @minValue AS MinValue, @sum / @count AS UsersAverage, @sumThisYear AS SumThisYear, @difference AS DifferenceToPrevMonth, @currentSumForCategory AS SumThisMonth, @previuosSumForCategory AS SumPreviousMonth
	
	--EXECUTE [dbo].[spGetAverageCostsForCategory] @costCategory, 0, @maxValue, @userID, @mainTableName


END