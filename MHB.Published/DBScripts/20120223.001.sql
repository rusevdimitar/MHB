USE smetkieu_db1
GO
/****** Object:  StoredProcedure [dbo].[spGetUsersAverageExpensesForCategory]    Script Date: 02/23/2012 13:28:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Rusev, Dimitar
-- Create date: 08.10.2009
-- Description:	Gets the average user expenses for a category
-- Modified:	23.02.2012
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
	DECLARE @currentSumForCategory MONEY
	DECLARE @previuosSumForCategory MONEY
	DECLARE @previousMonth INT
	DECLARE @previousYear INT
	DECLARE @difference MONEY
	
	IF @userID < 1000 
		BEGIN
		
			SET @currentSumForCategory = (SELECT ISNULL(SUM(FieldValue), 0) FROM tbMainTable01 WHERE [Month] = @month AND UserID = @userID AND [Year] = @year AND IsDeleted = 0 AND CostCategory = @costCategory)
			
			  IF @month = 1 
					BEGIN
						SET @previousMonth = 12
						SET @previousYear = @year - 1
					END
			  ELSE 					
					SET @previousMonth = @month - 1		
					
			SET @previuosSumForCategory = (SELECT ISNULL(SUM(FieldValue), 0) FROM tbMainTable01 WHERE [Month] = @previousMonth AND UserID = @userID AND [Year] = @previousYear AND IsDeleted = 0 AND CostCategory = @costCategory)					
			  
            SET @difference = @currentSumForCategory - @previuosSumForCategory                 
		
			SET @max = (SELECT MAX(FieldValue) FROM tbMainTable01 WHERE CostCategory = @costCategory AND UserID = @userID AND IsDeleted = 0)
			SET @maxValue = (SELECT TOP 1 CAST(FieldValue AS NVARCHAR) + ' (' + CAST([Month] AS NVARCHAR) + ', ' + CAST([Year] AS NVARCHAR) + ')' FROM dbo.tbMainTable01 WHERE IsDeleted = 0 AND CostCategory = @costCategory AND UserID = @userID AND FieldValue = (SELECT MAX(FieldValue) FROM tbMainTable01 WHERE CostCategory = @costCategory AND UserID = @userID AND IsDeleted = 0))		
			SET @minValue = (SELECT MIN(FieldValue) FROM dbo.tbMainTable01 WHERE CostCategory = @costCategory AND UserID = @userID AND IsDeleted = 0)
			SET @sum = (SELECT SUM(FieldValue) FROM dbo.tbMainTable01 WHERE FieldValue <> 0 AND FieldValue <= @max AND CostCategory = @costCategory AND IsDeleted = 0)
			SET @count = (SELECT COUNT(UserID) from dbo.tbMainTable01 WHERE FieldValue <> 0 AND FieldValue <= @max AND CostCategory = @costCategory AND IsDeleted = 0)
			SET @sumThisYear = (SELECT SUM(FieldValue) FROM tbMainTable01 WHERE UserID = @userID AND CostCategory = @costCategory AND [Year] =  @year AND IsDeleted = 0)
		END
	ELSE IF @userID >= 1000 AND @userID < 2000
		BEGIN
		
			SET @currentSumForCategory = (SELECT ISNULL(SUM(FieldValue), 0) FROM tbMainTable02 WHERE [Month] = @month AND UserID = @userID AND [Year] = @year AND IsDeleted = 0 AND CostCategory = @costCategory)
			
			  IF @month = 1 
					BEGIN
						SET @previousMonth = 12
						SET @previousYear = @year - 1
					END
			  ELSE 					
					SET @previousMonth = @month - 1		
					
			SET @previuosSumForCategory = (SELECT ISNULL(SUM(FieldValue), 0) FROM tbMainTable02 WHERE [Month] = @previousMonth AND UserID = @userID AND [Year] = @previousYear AND IsDeleted = 0 AND CostCategory = @costCategory)					
			  
            SET @difference = @currentSumForCategory - @previuosSumForCategory            
			SET @max = (SELECT MAX(FieldValue) FROM tbMainTable02 WHERE CostCategory = @costCategory AND UserID = @userID AND IsDeleted = 0)
			SET @maxValue = (SELECT TOP 1 CAST(FieldValue AS NVARCHAR) + ' (' + CAST([Month] AS NVARCHAR) + ', ' + CAST([Year] AS NVARCHAR) + ')' FROM dbo.tbMainTable02 WHERE IsDeleted = 0 AND CostCategory = @costCategory AND UserID = @userID AND FieldValue = (SELECT MAX(FieldValue) FROM tbMainTable02 WHERE CostCategory = @costCategory AND UserID = @userID AND IsDeleted = 0))		
			SET @minValue = (SELECT MIN(FieldValue) FROM dbo.tbMainTable02 WHERE CostCategory = @costCategory AND UserID = @userID AND IsDeleted = 0)
			SET @sum = (SELECT SUM(FieldValue) FROM dbo.tbMainTable02 WHERE FieldValue <> 0 AND FieldValue <= @max AND CostCategory = @costCategory AND IsDeleted = 0)
			SET @count = (SELECT COUNT(UserID) from dbo.tbMainTable02 WHERE FieldValue <> 0 AND FieldValue <= @max AND CostCategory = @costCategory AND IsDeleted = 0)
			SET @sumThisYear = (SELECT SUM(FieldValue) FROM tbMainTable02 WHERE UserID = @userID AND CostCategory = @costCategory AND [Year] =  @year AND IsDeleted = 0)
		END
	ELSE IF @userID >= 2000 AND @userID < 3000
		BEGIN
		
			SET @currentSumForCategory = (SELECT ISNULL(SUM(FieldValue), 0) FROM tbMainTable03 WHERE [Month] = @month AND UserID = @userID AND [Year] = @year AND IsDeleted = 0 AND CostCategory = @costCategory)
			
			  IF @month = 1 
					BEGIN
						SET @previousMonth = 12
						SET @previousYear = @year - 1
					END
			  ELSE 					
					SET @previousMonth = @month - 1		
					
			SET @previuosSumForCategory = (SELECT ISNULL(SUM(FieldValue), 0) FROM tbMainTable03 WHERE [Month] = @previousMonth AND UserID = @userID AND [Year] = @previousYear AND IsDeleted = 0 AND CostCategory = @costCategory)					
			
			SET @max = (SELECT MAX(FieldValue) FROM tbMainTable03 WHERE CostCategory = @costCategory AND UserID = @userID AND IsDeleted = 0)
			SET @maxValue = (SELECT TOP 1 CAST(FieldValue AS NVARCHAR) + ' (' + CAST([Month] AS NVARCHAR) + ', ' + CAST([Year] AS NVARCHAR) + ')' FROM dbo.tbMainTable03 WHERE IsDeleted = 0 AND CostCategory = @costCategory AND UserID = @userID AND FieldValue = (SELECT MAX(FieldValue) FROM tbMainTable03 WHERE CostCategory = @costCategory AND UserID = @userID AND IsDeleted = 0))		
			SET @minValue = (SELECT MIN(FieldValue) FROM dbo.tbMainTable03 WHERE CostCategory = @costCategory AND UserID = @userID AND IsDeleted = 0)
			SET @sum = (SELECT SUM(FieldValue) FROM dbo.tbMainTable03 WHERE FieldValue <> 0 AND FieldValue <= @max AND CostCategory = @costCategory AND IsDeleted = 0)
			SET @count = (SELECT COUNT(UserID) from dbo.tbMainTable03 WHERE FieldValue <> 0 AND FieldValue <= @max AND CostCategory = @costCategory AND IsDeleted = 0)
			SET @sumThisYear = (SELECT SUM(FieldValue) FROM tbMainTable03 WHERE UserID = @userID AND CostCategory = @costCategory AND [Year] =  @year AND IsDeleted = 0)
		END	
	ELSE IF @userID >= 3000 AND @userID < 4000
		BEGIN
		
			SET @currentSumForCategory = (SELECT ISNULL(SUM(FieldValue), 0) FROM tbMainTable04 WHERE [Month] = @month AND UserID = @userID AND [Year] = @year AND IsDeleted = 0 AND CostCategory = @costCategory)
			
			  IF @month = 1 
					BEGIN
						SET @previousMonth = 12
						SET @previousYear = @year - 1
					END
			  ELSE 					
					SET @previousMonth = @month - 1		
					
			SET @previuosSumForCategory = (SELECT ISNULL(SUM(FieldValue), 0) FROM tbMainTable04 WHERE [Month] = @previousMonth AND UserID = @userID AND [Year] = @previousYear AND IsDeleted = 0 AND CostCategory = @costCategory)					
			
			SET @max = (SELECT MAX(FieldValue) FROM tbMainTable04 WHERE CostCategory = @costCategory AND UserID = @userID AND IsDeleted = 0)
			SET @maxValue = (SELECT TOP 1 CAST(FieldValue AS NVARCHAR) + ' (' + CAST([Month] AS NVARCHAR) + ', ' + CAST([Year] AS NVARCHAR) + ')' FROM dbo.tbMainTable04 WHERE IsDeleted = 0 AND CostCategory = @costCategory AND UserID = @userID AND FieldValue = (SELECT MAX(FieldValue) FROM tbMainTable04 WHERE CostCategory = @costCategory AND UserID = @userID AND IsDeleted = 0))		
			SET @minValue = (SELECT MIN(FieldValue) FROM dbo.tbMainTable04 WHERE CostCategory = @costCategory AND UserID = @userID AND IsDeleted = 0)
			SET @sum = (SELECT SUM(FieldValue) FROM dbo.tbMainTable04 WHERE FieldValue <> 0 AND FieldValue <= @max AND CostCategory = @costCategory AND IsDeleted = 0)
			SET @count = (SELECT COUNT(UserID) from dbo.tbMainTable04 WHERE FieldValue <> 0 AND FieldValue <= @max AND CostCategory = @costCategory AND IsDeleted = 0)
			SET @sumThisYear = (SELECT SUM(FieldValue) FROM tbMainTable04 WHERE UserID = @userID AND CostCategory = @costCategory AND [Year] =  @year AND IsDeleted = 0)
		END	
	ELSE IF @userID >= 4000 AND @userID < 5000
		BEGIN
		
			SET @currentSumForCategory = (SELECT ISNULL(SUM(FieldValue), 0) FROM tbMainTable05 WHERE [Month] = @month AND UserID = @userID AND [Year] = @year AND IsDeleted = 0 AND CostCategory = @costCategory)
			
			  IF @month = 1 
					BEGIN
						SET @previousMonth = 12
						SET @previousYear = @year - 1
					END
			  ELSE 					
					SET @previousMonth = @month - 1		
					
			SET @previuosSumForCategory = (SELECT ISNULL(SUM(FieldValue), 0) FROM tbMainTable05 WHERE [Month] = @previousMonth AND UserID = @userID AND [Year] = @previousYear AND IsDeleted = 0 AND CostCategory = @costCategory)					
			
			SET @max = (SELECT MAX(FieldValue) FROM tbMainTable05 WHERE CostCategory = @costCategory AND UserID = @userID AND IsDeleted = 0)
			SET @maxValue = (SELECT TOP 1 CAST(FieldValue AS NVARCHAR) + ' (' + CAST([Month] AS NVARCHAR) + ', ' + CAST([Year] AS NVARCHAR) + ')' FROM dbo.tbMainTable05 WHERE IsDeleted = 0 AND CostCategory = @costCategory AND UserID = @userID AND FieldValue = (SELECT MAX(FieldValue) FROM tbMainTable05 WHERE CostCategory = @costCategory AND UserID = @userID AND IsDeleted = 0))		
			SET @minValue = (SELECT MIN(FieldValue) FROM dbo.tbMainTable05 WHERE CostCategory = @costCategory AND UserID = @userID AND IsDeleted = 0)
			SET @sum = (SELECT SUM(FieldValue) FROM dbo.tbMainTable05 WHERE FieldValue <> 0 AND FieldValue <= @max AND CostCategory = @costCategory AND IsDeleted = 0)
			SET @count = (SELECT COUNT(UserID) from dbo.tbMainTable05 WHERE FieldValue <> 0 AND FieldValue <= @max AND CostCategory = @costCategory AND IsDeleted = 0)
			SET @sumThisYear = (SELECT SUM(FieldValue) FROM tbMainTable05 WHERE UserID = @userID AND CostCategory = @costCategory AND [Year] =  @year AND IsDeleted = 0)
		END	
	ELSE IF @userID >= 5000 AND @userID < 6000
	BEGIN
	
			SET @currentSumForCategory = (SELECT ISNULL(SUM(FieldValue), 0) FROM tbMainTable06 WHERE [Month] = @month AND UserID = @userID AND [Year] = @year AND IsDeleted = 0 AND CostCategory = @costCategory)
			
			  IF @month = 1 
					BEGIN
						SET @previousMonth = 12
						SET @previousYear = @year - 1
					END
			  ELSE 					
					SET @previousMonth = @month - 1		
					
			SET @previuosSumForCategory = (SELECT ISNULL(SUM(FieldValue), 0) FROM tbMainTable06 WHERE [Month] = @previousMonth AND UserID = @userID AND [Year] = @previousYear AND IsDeleted = 0 AND CostCategory = @costCategory)					
			
			SET @max = (SELECT MAX(FieldValue) FROM tbMainTable06 WHERE CostCategory = @costCategory AND UserID = @userID AND IsDeleted = 0)
			SET @maxValue = (SELECT TOP 1 CAST(FieldValue AS NVARCHAR) + ' (' + CAST([Month] AS NVARCHAR) + ', ' + CAST([Year] AS NVARCHAR) + ')' FROM dbo.tbMainTable06 WHERE IsDeleted = 0 AND CostCategory = @costCategory AND UserID = @userID AND FieldValue = (SELECT MAX(FieldValue) FROM tbMainTable06 WHERE CostCategory = @costCategory AND UserID = @userID AND IsDeleted = 0))		
			SET @minValue = (SELECT MIN(FieldValue) FROM dbo.tbMainTable06 WHERE CostCategory = @costCategory AND UserID = @userID AND IsDeleted = 0)
			SET @sum = (SELECT SUM(FieldValue) FROM dbo.tbMainTable06 WHERE FieldValue <> 0 AND FieldValue <= @max AND CostCategory = @costCategory AND IsDeleted = 0)
			SET @count = (SELECT COUNT(UserID) from dbo.tbMainTable06 WHERE FieldValue <> 0 AND FieldValue <= @max AND CostCategory = @costCategory AND IsDeleted = 0)
			SET @sumThisYear = (SELECT SUM(FieldValue) FROM tbMainTable06 WHERE UserID = @userID AND CostCategory = @costCategory AND [Year] =  @year AND IsDeleted = 0)
	END	
	
	
	







select @maxValue AS MaxString, @max AS MaxValue, @minValue AS MinValue, @sum / @count AS UsersAverage, @sumThisYear AS SumThisYear, @difference AS DifferenceToPrevMonth
	
	--EXECUTE [dbo].[spGetAverageCostsForCategory] @costCategory, 0, @maxValue, @userID, @mainTableName


END