UPDATE dt SET dt.CategoryID = p.CategoryID
FROM tbDetailsTable01 dt
INNER JOIN tbProducts p ON p.ID = dt.ProductID
WHERE dt.CategoryID IS NULL AND dt.ProductID <> 1

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

USE [Test01Db]
GO
/****** Object:  StoredProcedure [dbo].[spGetAllUsersCategorySums]    Script Date: 08/13/2014 14:39:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Dimitar Rusev
-- Create date: 2012-06-13 13:04
-- Modify date: 2014-08-13 14:48
-- Description:	Gets the sum spent by all users for each category
-- =============================================
ALTER PROCEDURE [dbo].[spGetAllUsersCategorySums]
	-- Add the parameters for the stored procedure here
	@language INT = 0,
	@year INT,
	@endYear INT,
	@userID INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

   

DECLARE @mainTableName NVARCHAR(100)
DECLARE @detailsTableName NVARCHAR(100)
DECLARE @usersCount INT = 0
DECLARE @qry NVARCHAR(MAX)
DECLARE @paramDefinition NVARCHAR(500)
DECLARE @categoryID INT = 1
DECLARE @categorySum MONEY = 0
DECLARE @categoryName NVARCHAR(100)

DECLARE @sums TABLE  
(
	UsersCount MONEY,
	CategoryID INT,
	CategoryName NVARCHAR(100),
	CategorySum MONEY,
	CategoryAverageSum MONEY,
	[Year] INT
)

DECLARE CursorMainTableNames CURSOR FOR 
	SELECT name
	FROM sysobjects
	WHERE type = 'U' AND name LIKE 'tbMainTable%' AND name <> 'tbMainTableTemp'
	ORDER BY name

OPEN CursorMainTableNames

FETCH NEXT FROM CursorMainTableNames INTO @mainTableName

	WHILE @@fetch_status = 0 
	BEGIN		
		
		DECLARE @currentUsersCount INT	
				
		SET @qry = N'SET @currentUsersCount = (SELECT COUNT(DISTINCT UserID) FROM ' + CAST(@mainTableName AS NVARCHAR) + ')'
		SET @paramDefinition = N'@currentUsersCount INT OUTPUT, @mainTableName NVARCHAR'
					
		EXEC sp_executesql
		@qry,
		@paramDefinition,
		@currentUsersCount OUTPUT, @mainTableName = @mainTableName
								
		SET @usersCount = @usersCount + @currentUsersCount																		
										
		FETCH NEXT FROM CursorMainTableNames INTO @mainTableName
	END

CLOSE CursorMainTableNames
DEALLOCATE CursorMainTableNames

WHILE @year <= @endYear
BEGIN
	DECLARE CursorCategoriesIDs CURSOR FOR 
	SELECT ID FROM tbCategories WHERE UserCategoryID = @userID OR UserCategoryID = 0

	OPEN CursorCategoriesIDs

	FETCH NEXT FROM CursorCategoriesIDs INTO @categoryID

		WHILE @@fetch_status = 0 
		BEGIN	
		
		DECLARE CursorCategoriesSum CURSOR FOR 
		SELECT name
		FROM sysobjects
		WHERE type = 'U' AND name LIKE 'tbMainTable%' AND name <> 'tbMainTableTemp'
		ORDER BY name

		OPEN CursorCategoriesSum

		FETCH NEXT FROM CursorCategoriesSum INTO @mainTableName

			WHILE @@fetch_status = 0
			BEGIN
				
				DECLARE @currentCategorySum MONEY = 0
				IF @userID = 0
				BEGIN
					SET @qry = N'SET @currentCategorySum = (SELECT SUM(FieldValue) FROM ' + CAST(@mainTableName AS NVARCHAR) + ' WHERE CostCategory = ' + CAST(@categoryID AS NVARCHAR) + ' AND [Year] = ' + CAST(@year AS NVARCHAR) + ')'				
				END
				ELSE 
				BEGIN
					SET @qry = N'SET @currentCategorySum = (SELECT SUM(FieldValue) FROM ' + CAST(@mainTableName AS NVARCHAR) + ' WHERE CostCategory = ' + CAST(@categoryID AS NVARCHAR) + ' AND [Year] = ' + CAST(@year AS NVARCHAR) + ' AND UserID = ' + CAST(@userID AS NVARCHAR) + ')'
				END
				
				SET @paramDefinition = N'@currentCategorySum INT OUTPUT, @mainTableName NVARCHAR'
							
				EXEC sp_executesql
				@qry,
				@paramDefinition,
				@currentCategorySum OUTPUT, @mainTableName = @mainTableName
																				
				SET @categorySum = @categorySum + ISNULL(@currentCategorySum, 0)

--++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
--++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
--++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
					DECLARE CursorDetailsCategoriesSum CURSOR FOR 
					SELECT name
					FROM sysobjects
					WHERE type = 'U' AND name LIKE 'tbDetailsTable%' AND name <> 'tbDetailsTableTemp'
					ORDER BY name

					OPEN CursorDetailsCategoriesSum

					FETCH NEXT FROM CursorDetailsCategoriesSum INTO @detailsTableName

						WHILE @@fetch_status = 0
						BEGIN
							
							DECLARE @currentDetailsCategorySum MONEY = 0
							IF @userID = 0
							BEGIN
								SET @qry = N'SET @currentDetailsCategorySum = (SELECT SUM(DetailValue) FROM ' + CAST(@detailsTableName AS NVARCHAR) + ' dt
																			   INNER JOIN ' + CAST(@mainTableName AS NVARCHAR) + ' mt ON mt.ID = dt.ExpenditureID
																			   WHERE dt.CategoryID = ' + CAST(@categoryID AS NVARCHAR) + ' AND DATEPART(YEAR, dt.DetailDate) = ' + CAST(@year AS NVARCHAR) + ' AND dt.IsDeleted = 0 AND mt.CostCategory <> ' + CAST(@categoryID AS NVARCHAR) + ')'				
							END
							ELSE 
							BEGIN
								SET @qry = N'SET @currentDetailsCategorySum = (SELECT SUM(DetailValue) FROM ' + CAST(@detailsTableName AS NVARCHAR) + ' dt
																			   INNER JOIN ' + CAST(@mainTableName AS NVARCHAR) + ' mt ON mt.ID = dt.ExpenditureID
																			   WHERE dt.CategoryID = ' + CAST(@categoryID AS NVARCHAR) + ' AND DATEPART(YEAR, dt.DetailDate) = ' + CAST(@year AS NVARCHAR) + ' AND mt.UserID = ' + CAST(@userID AS NVARCHAR) + ' AND dt.IsDeleted = 0 AND mt.CostCategory <> ' + CAST(@categoryID AS NVARCHAR) + ')'								
							END
							
							SET @paramDefinition = N'@currentDetailsCategorySum INT OUTPUT, @detailsTableName NVARCHAR'
										
							EXEC sp_executesql
							@qry,
							@paramDefinition,
							@currentDetailsCategorySum OUTPUT, @detailsTableName = @detailsTableName
																							
							SET @categorySum = @categorySum + ISNULL(@currentDetailsCategorySum, 0)

							FETCH NEXT FROM CursorDetailsCategoriesSum INTO @detailsTableName
						END

					CLOSE CursorDetailsCategoriesSum
					DEALLOCATE CursorDetailsCategoriesSum			
--++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
--++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
--++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++				
				

				FETCH NEXT FROM CursorCategoriesSum INTO @mainTableName
			END

		CLOSE CursorCategoriesSum
		DEALLOCATE CursorCategoriesSum			
		
		SET @categoryName = 
			CASE @language 
				WHEN 0 THEN (SELECT ControlTextBG FROM tbLanguage WHERE ControlID = CAST(@categoryID AS NVARCHAR))
				WHEN 1 THEN (SELECT ControlTextEN FROM tbLanguage WHERE ControlID = CAST(@categoryID AS NVARCHAR))
				WHEN 2 THEN (SELECT ControlTextDE FROM tbLanguage WHERE ControlID = CAST(@categoryID AS NVARCHAR))			
			END		
					
		INSERT INTO @sums SELECT @usersCount, ID, @categoryName, @categorySum, @categorySum / @usersCount, @year FROM dbo.tbCategories WHERE ID = @categoryID
				
		
		SET @categorySum = 0
		
		SET @categoryID = @categoryID + 1
	FETCH NEXT FROM CursorCategoriesIDs INTO @categoryID
	END

	CLOSE CursorCategoriesIDs
	DEALLOCATE CursorCategoriesIDs	
	
	SET @year = @year + 1
	SET @categoryID = 1
END

SELECT * FROM @sums ORDER BY CategoryName DESC

END


USE [Test01Db]
GO
/****** Object:  StoredProcedure [dbo].[spGetSumsForCategoryPerYear]    Script Date: 08/13/2014 14:53:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:			Dimitar Rusev
-- Create date:		17.02.2012
-- Modified date:	13.08.2014
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

		DECLARE @query NVARCHAR(MAX);
		DECLARE @paramsDefinition nvarchar(500);

		SET @paramsDefinition = N'
		@sumMain MONEY OUTPUT, 
		@sumDetailsCategory MONEY OUTPUT';

		SET @query =
		'SET @sumMain = 
			(SELECT ISNULL(SUM(FieldValue), 0) FROM ' + CAST(@mainTableName AS NVARCHAR) + ' mt
					WHERE 
						mt.UserID = ' + CAST(@UserID AS NVARCHAR) + ' AND mt.[Year] = ' + CAST(@StartYear AS NVARCHAR) + ' AND mt.CostCategory = ' + CAST(@CategoryID AS NVARCHAR) + ' AND mt.IsDeleted = 0)

		SET @sumDetailsCategory = 
			(SELECT ISNULL(SUM(dt.DetailValue), 0) FROM ' + CAST(@detailsTableName AS NVARCHAR) + ' dt
						INNER JOIN ' + CAST(@mainTableName AS NVARCHAR) + ' mt ON dt.ExpenditureID = mt.ID
					WHERE
						mt.UserID = ' + CAST(@UserID AS NVARCHAR) + ' AND DATEPART(YEAR, dt.DetailDate) = ' + CAST(@StartYear AS NVARCHAR) + ' AND dt.IsDeleted = 0			
						AND dt.CategoryID = ' + CAST(@CategoryID AS NVARCHAR) + ' AND mt.CostCategory <> ' + CAST(@CategoryID AS NVARCHAR) + ')'
					
				
		EXEC sp_executesql @query, @paramsDefinition, @sumMain OUTPUT, @sumDetailsCategory OUTPUT

		SET @sum = @sumMain + @sumDetailsCategory
															
		INSERT INTO @sums (CostSum, CostCategory, CostCategoryName, [Year]) VALUES (@sum, @CategoryID, @costCategoryName, @StartYear)
		
		SET @StartYear = @StartYear + 1
	END

	SELECT * FROM @sums 
   
END

