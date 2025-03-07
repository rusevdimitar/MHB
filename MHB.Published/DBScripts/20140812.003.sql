USE [Test01Db]
GO
/****** Object:  StoredProcedure [dbo].[spGetAllUsersCategorySums]    Script Date: 08/12/2014 14:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Dimitar Rusev
-- Create date: 2012-06-13 13:04
-- Modify date: 2014-08-12 14:45
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
																			   WHERE dt.CategoryID = ' + CAST(@categoryID AS NVARCHAR) + ' AND mt.[Year] = ' + CAST(@year AS NVARCHAR) + ' AND dt.IsDeleted = 0)'				
							END
							ELSE 
							BEGIN
								SET @qry = N'SET @currentDetailsCategorySum = (SELECT SUM(DetailValue) FROM ' + CAST(@detailsTableName AS NVARCHAR) + ' dt
																			   INNER JOIN ' + CAST(@mainTableName AS NVARCHAR) + ' mt ON mt.ID = dt.ExpenditureID
																			   WHERE dt.CategoryID = ' + CAST(@categoryID AS NVARCHAR) + ' AND mt.[Year] = ' + CAST(@year AS NVARCHAR) + ' AND mt.UserID = ' + CAST(@userID AS NVARCHAR) + ' AND dt.IsDeleted = 0)'								
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
