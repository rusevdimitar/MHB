USE [Test01Db]
GO
/****** Object:  StoredProcedure [dbo].[spGetCategoriesTable]    Script Date: 05/17/2012 11:59:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Dimitar Rusev
-- Create date: 13.12.2009
-- Modified date: 18.05.2012
-- Description:	Get categories table
-- =============================================
ALTER PROCEDURE [dbo].[spGetCategoriesTable]
	@language INT,
	@userID INT,
	@optionalcategoryID INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


	DECLARE @categoryID INT
	DECLARE @categoryName NVARCHAR(50)
	DECLARE @categoriesCount INT
	DECLARE @keyWords NVARCHAR(MAX)
	DECLARE @myCursor CURSOR
	
		
	DECLARE @costCategories TABLE(
	CategoryID INT,
	CategoryName NVARCHAR(50),
	CategoryKeyWords NVARCHAR(MAX),
	IconPath NVARCHAR(300),
	IsPayIconVisible BIT,
	UserCategoryID INT	
	)

	IF @optionalcategoryID IS NOT NULL
	BEGIN
		SET @categoryID = @optionalcategoryID
		
		IF @language = 0
		BEGIN
			SET @categoryName = (SELECT [ControlTextBG] FROM [tbLanguage] WHERE [ControlID] = CAST(@categoryID AS NVARCHAR))
		END
		ELSE IF @language = 1
		BEGIN
			SET @categoryName = (SELECT [ControlTextEN] FROM [tbLanguage] WHERE [ControlID] = CAST(@categoryID AS NVARCHAR))
		END
		ELSE IF @language = 2
		BEGIN
			SET @categoryName = (SELECT [ControlTextDE] FROM [tbLanguage] WHERE [ControlID] = CAST(@categoryID AS NVARCHAR))
		END
				
		-- Get keywords for category		
		SET @keyWords = ''
		
		SELECT @keyWords = COALESCE(@keyWords + ', ', '') + CostNames
		FROM tbCostCategories WHERE CostCategoryID = @categoryID
		
		INSERT INTO @costCategories SELECT @categoryID, @categoryName, @keyWords, IconPath, IsPayIconVisible, UserCategoryID FROM dbo.tbCategories WHERE ID = @categoryID AND (UserCategoryID = @userID OR UserCategoryID = 0)
	END
	ELSE
	BEGIN			
				
		SET @myCursor = CURSOR FOR SELECT ID FROM tbCategories
		
		IF @userID = 0
		BEGIN
			SET @myCursor = CURSOR FOR SELECT ID FROM tbCategories
		END
		ELSE
		BEGIN
			SET @myCursor = CURSOR FOR SELECT ID FROM tbCategories WHERE UserCategoryID = @userID OR UserCategoryID = 0
		END
		
		
		OPEN @myCursor

		FETCH NEXT FROM @myCursor INTO @categoryID

			WHILE @@fetch_status = 0 
				BEGIN		
					
				IF @language = 0
				BEGIN
					SET @categoryName = (SELECT [ControlTextBG] FROM [tbLanguage] WHERE [ControlID] = CAST(@categoryID AS NVARCHAR))
				END
				ELSE IF @language = 1
				BEGIN
					SET @categoryName = (SELECT [ControlTextEN] FROM [tbLanguage] WHERE [ControlID] = CAST(@categoryID AS NVARCHAR))
				END
				ELSE IF @language = 2
				BEGIN
					SET @categoryName = (SELECT [ControlTextDE] FROM [tbLanguage] WHERE [ControlID] = CAST(@categoryID AS NVARCHAR))
				END
						
				-- Get keywords for category		
				SET @keyWords = ''
				
				SELECT @keyWords = COALESCE(@keyWords + ', ', '') + CostNames
				FROM tbCostCategories WHERE CostCategoryID = @categoryID
								
				INSERT INTO @costCategories SELECT @categoryID, @categoryName, @keyWords, IconPath, IsPayIconVisible, UserCategoryID FROM dbo.tbCategories WHERE ID = @categoryID
												
				FETCH NEXT FROM @myCursor INTO @categoryID
			END

		CLOSE @myCursor
		DEALLOCATE @myCursor
		
		--SET @categoryID = 1

		--WHILE @categoryID <= @categoriesCount
		--BEGIN
		--	IF @language = 0
		--	BEGIN
		--		SET @categoryName = (SELECT [ControlTextBG] FROM [tbLanguage] WHERE [ControlID] = CAST(@categoryID AS NVARCHAR))
		--	END
		--	ELSE IF @language = 1
		--	BEGIN
		--		SET @categoryName = (SELECT [ControlTextEN] FROM [tbLanguage] WHERE [ControlID] = CAST(@categoryID AS NVARCHAR))
		--	END
		--	ELSE IF @language = 2
		--	BEGIN
		--		SET @categoryName = (SELECT [ControlTextDE] FROM [tbLanguage] WHERE [ControlID] = CAST(@categoryID AS NVARCHAR))
		--	END
					
		--	-- Get keywords for category		
		--	SET @keyWords = ''
			
		--	SELECT @keyWords = COALESCE(@keyWords + ', ', '') + CostNames
		--	FROM tbCostCategories WHERE CostCategoryID = @categoryID
			
		--	INSERT INTO @costCategories SELECT @categoryID, @categoryName, @keyWords, IconPath, IsPayIconVisible, UserCategoryID FROM dbo.tbCategories WHERE ID = @categoryID AND (UserCategoryID = @userID OR UserCategoryID = 0)
			
		--	SET @categoryID = @categoryID + 1
		--END	
	END

	SELECT * FROM @costCategories 
END
