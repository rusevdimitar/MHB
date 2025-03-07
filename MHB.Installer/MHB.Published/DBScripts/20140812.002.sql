USE [Test01Db]
GO
/****** Object:  StoredProcedure [dbo].[spGetSumsForCategoryPerYear]    Script Date: 08/12/2014 12:29:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:			Dimitar Rusev
-- Create date:		17.02.2012
-- Modified date:	12.08.2014
-- Description:		Returns the sums spent for each category for a given year
-- =============================================
ALTER PROCEDURE [dbo].[spGetSumsForCategoryPerYear]
	@UserID INT,	
	@StartYear INT,
	@Lang INT,
	@CategoryID INT
AS
BEGIN
	
	SET NOCOUNT ON;
	
	DECLARE @sum MONEY	
	DECLARE @costCategoryName NVARCHAR(50)	
	
	DECLARE @sums TABLE  
	(
		CostSum MONEY,
		CostCategory INT,
		CostCategoryName NVARCHAR(50),
		[Year] INT 
	)

	-- Get category name translation
	IF @Lang = 0
	BEGIN
		SET @costCategoryName = (SELECT ControlTextBG FROM dbo.tbLanguage WHERE ControlID = CAST(@CategoryID AS VARCHAR))
	END
	ELSE IF @Lang = 1
	BEGIN
		SET @costCategoryName = (SELECT ControlTextEN FROM dbo.tbLanguage WHERE ControlID = CAST(@CategoryID AS VARCHAR))
	END
	ELSE IF @Lang = 2
	BEGIN
		SET @costCategoryName = (SELECT ControlTextDE FROM dbo.tbLanguage WHERE ControlID = CAST(@CategoryID AS VARCHAR))
	END
	
	
	WHILE @StartYear <= YEAR(GETDATE())
	BEGIN
	
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



		DECLARE @sumMain MONEY
		DECLARE @sumDetailsCategory MONEY
		DECLARE @sumDetailsProductCategory MONEY

		DECLARE @query NVARCHAR(MAX);
		DECLARE @paramsDefinition nvarchar(500);

		SET @paramsDefinition = N'
		@sumMain MONEY OUTPUT, 
		@sumDetailsCategory MONEY OUTPUT, 
		@sumDetailsProductCategory MONEY OUTPUT
		';

		SET @query =
		'SET @sumMain = 
			(SELECT ISNULL(SUM(FieldValue), 0) FROM ' + CAST(@mainTableName AS NVARCHAR) + ' mt
					WHERE 
						mt.UserID = ' + CAST(@UserID AS NVARCHAR) + ' AND mt.[Year] = ' + CAST(@StartYear AS NVARCHAR) + ' AND mt.CostCategory = ' + CAST(@CategoryID AS NVARCHAR) + ' AND mt.IsDeleted = 0)

		SET @sumDetailsCategory = 
			(SELECT ISNULL(SUM(dt.DetailValue), 0) FROM ' + CAST(@detailsTableName AS NVARCHAR) + ' dt
						INNER JOIN ' + CAST(@mainTableName AS NVARCHAR) + ' mt ON dt.ExpenditureID = mt.ID
					WHERE
						mt.UserID = ' + CAST(@UserID AS NVARCHAR) + ' AND mt.[Year] = ' + CAST(@StartYear AS NVARCHAR) + ' AND dt.IsDeleted = 0			
						AND dt.CategoryID = ' + CAST(@CategoryID AS NVARCHAR) + ')

		SET @sumDetailsProductCategory = 					
			(SELECT ISNULL(SUM(dt.DetailValue), 0) FROM ' + CAST(@detailsTableName AS NVARCHAR) + ' dt
						INNER JOIN ' + CAST(@mainTableName AS NVARCHAR) + ' mt ON dt.ExpenditureID = mt.ID
						INNER JOIN tbProducts p ON p.ID = dt.ProductID
					WHERE
						mt.UserID = ' + CAST(@UserID AS NVARCHAR) + ' AND mt.[Year] = ' + CAST(@StartYear AS NVARCHAR) + ' AND dt.IsDeleted = 0			
						AND p.CategoryID <> dt.CategoryID
						AND p.CategoryID = ' + CAST(@CategoryID AS NVARCHAR) + ')'
					
				
		EXEC sp_executesql @query, @paramsDefinition, @sumMain OUTPUT, @sumDetailsCategory OUTPUT, @sumDetailsProductCategory OUTPUT

		SET @sum = @sumMain + @sumDetailsCategory + @sumDetailsProductCategory
															
		INSERT INTO @sums (CostSum, CostCategory, CostCategoryName, [Year]) VALUES (@sum, @CategoryID, @costCategoryName, @StartYear)
		
		SET @StartYear = @StartYear + 1
	END

	SELECT * FROM @sums 
   
END

