ALTER TABLE tbCategories ADD IsShared BIT NOT NULL DEFAULT 0


/****** Object:  StoredProcedure [dbo].[spGetCategoriesTable]    Script Date: 06/12/2012 11:36:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Dimitar Rusev
-- Create date: 13.12.2009
-- Modified date: 12.06.2012
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
	IsShared BIT,
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
		
		INSERT INTO @costCategories SELECT @categoryID, @categoryName, @keyWords, IconPath, IsPayIconVisible, IsShared, UserCategoryID FROM dbo.tbCategories WHERE ID = @categoryID AND (UserCategoryID = @userID OR UserCategoryID = 0)
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
								
				INSERT INTO @costCategories SELECT @categoryID, @categoryName, @keyWords, IconPath, IsPayIconVisible, IsShared, UserCategoryID FROM dbo.tbCategories WHERE ID = @categoryID
												
				FETCH NEXT FROM @myCursor INTO @categoryID
			END

		CLOSE @myCursor
		DEALLOCATE @myCursor
				
	END

	SELECT * FROM @costCategories 
END


/****** Object:  StoredProcedure [dbo].[spCopyCategory]    Script Date: 06/12/2012 12:55:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Dimitar Rusev
-- Create date: 12.06.2012
-- Modified date: 12.06.2012
-- Description:	Duplicates a category and assignes a new userID
-- =============================================
CREATE PROCEDURE [dbo].[spCopyCategory]
	@sourceCategoryID INT,
	@destinationUserID INT	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DECLARE @categoryID INT = 0;
	
	SET @categoryID = (SELECT MAX(ID) FROM tbCategories) + 1
	
	INSERT INTO tbCategories
	(
		ID,
		CategoryName, 
		IconPath, 
		IsPayIconVisible, 
		UserCategoryID, 
		IsShared
	)
	SELECT @categoryID, CategoryName, IconPath, IsPayIconVisible, @destinationUserID, 0 FROM tbCategories WHERE ID = @sourceCategoryID
	
	INSERT INTO dbo.tbCostCategories
	(CostCategoryID, CostNames, [Language])
	SELECT @categoryID, CostNames, [Language] FROM tbCostCategories WHERE CostCategoryID = @sourceCategoryID
	
	
	INSERT INTO tbLanguage (ControlID, ControlTextEN, ControlTextBG, ControlTextDE) SELECT @categoryID, ControlTextEN, ControlTextBG, ControlTextDE FROM tbLanguage WHERE ControlID = CAST(@sourceCategoryID AS NVARCHAR)
	
	SELECT @categoryID
END

INSERT INTO dbo.tbLanguage
           (ControlID
           ,ControlTextEN
           ,ControlTextBG
           ,ControlTextDE)
     VALUES
           ('LinkButtonUse'
           ,'Use'
           ,'Използвай'
           ,'Verwenden')

INSERT INTO dbo.tbLanguage
           (ControlID
           ,ControlTextEN
           ,ControlTextBG
           ,ControlTextDE)
     VALUES
           ('LabelUsersCategoriesList'
           ,'Other users shared categories list'
           ,'Категории споделени публично от други потребители'
           ,'Öffentlichе Kategorien von anderen Benutzern')

INSERT INTO dbo.tbLanguage
           (ControlID
           ,ControlTextEN
           ,ControlTextBG
           ,ControlTextDE)
     VALUES
           ('ShareCategory'
           ,'Share (make public)'
           ,'Сподели (направи публична)'
           ,'Share (veröffentlichen)')
           
INSERT INTO dbo.tbLanguage
           (ControlID
           ,ControlTextEN
           ,ControlTextBG
           ,ControlTextDE)
     VALUES
           ('RemoveShareCategory'
           ,'Don''t share'
           ,'Не споделяй'
           ,'Nicht veröffentlichen')
		   
INSERT INTO dbo.tbLanguage
           (ControlID
           ,ControlTextEN
           ,ControlTextBG
           ,ControlTextDE)
     VALUES
           ('LabelShareNewCategory'
           ,'Share?'
           ,'Сподели с останалите?'
           ,'Veröffentlichen?') 		    



SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Rusev, Dimitar
-- Create date: 2012-05-15
-- Description:	Adds a new user-defined cost category
-- =============================================

ALTER PROCEDURE [dbo].[spAddNewUserCategory]
	@languageID INT,
	@categoryName NVARCHAR(200),
	@categoryKeyWords NVARCHAR(MAX),	
	@categoryIconPath NVARCHAR(MAX),
	@categoryIsPayIconVisible BIT,
	@categoryIsShared BIT,
	@userID INT

AS
BEGIN
	
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
DECLARE @categoryID INT = 0;

SET @categoryID = (SELECT MAX(ID) FROM tbCategories) + 1

INSERT INTO tbCategories
           (ID
           ,CategoryName
           ,IconPath
           ,IsPayIconVisible
           ,IsShared
           ,UserCategoryID)
     VALUES
           (@categoryID
           ,@categoryName
           ,@categoryIconPath
           ,@categoryIsPayIconVisible
           ,@categoryIsShared
           ,@userID)
           
-- if delimited string doesn't end with a comma we append one
IF CHARINDEX(',', @categoryKeyWords, len(@categoryKeyWords)) = 0
BEGIN
	SET @categoryKeyWords = @categoryKeyWords + ','
END

-- remove whitespaces
SET @categoryKeyWords = REPLACE(@categoryKeyWords, ' ', '')

DECLARE @index INT = CHARINDEX(',', @categoryKeyWords, 0)
DECLARE @category NVARCHAR(200)

WHILE @index > 0
BEGIN	
	SET @category = LEFT(@categoryKeyWords, @index - 1)
	
	INSERT INTO tbCostCategories
           (CostCategoryID
           ,CostNames
           ,[Language])
     VALUES
           (@categoryID
           ,UPPER(@category)
           ,@languageID)
	
	SET @categoryKeyWords = RIGHT(@categoryKeyWords, LEN(@categoryKeyWords) - @index)
	SET @index = CHARINDEX(',', @categoryKeyWords, 0)
END

IF @languageID = 0
BEGIN
	INSERT INTO tbLanguage (ControlID, ControlTextBG) VALUES (@categoryID, @categoryName)
END
ELSE IF @languageID = 1
BEGIN
	INSERT INTO tbLanguage (ControlID, ControlTextEN) VALUES (@categoryID, @categoryName)
END
ELSE IF @languageID = 2
BEGIN
	INSERT INTO tbLanguage (ControlID, ControlTextDE) VALUES (@categoryID, @categoryName)
END




	
END

		   