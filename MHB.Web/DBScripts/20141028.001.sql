ALTER TABLE tbMainTable01 ADD CONSTRAINT FK_CostCategory FOREIGN KEY (CostCategory) REFERENCES tbCategories (ID)
ALTER TABLE tbMainTable02 ADD CONSTRAINT FK_CostCategory2 FOREIGN KEY (CostCategory) REFERENCES tbCategories (ID)
ALTER TABLE tbMainTable03 ADD CONSTRAINT FK_CostCategory3 FOREIGN KEY (CostCategory) REFERENCES tbCategories (ID)
ALTER TABLE tbMainTable04 ADD CONSTRAINT FK_CostCategory4 FOREIGN KEY (CostCategory) REFERENCES tbCategories (ID)
ALTER TABLE tbMainTable05 ADD CONSTRAINT FK_CostCategory5 FOREIGN KEY (CostCategory) REFERENCES tbCategories (ID)

USE [Test01Db]
GO
/****** Object:  StoredProcedure [dbo].[spAddNewUserCategory]    Script Date: 10/28/2014 11:55:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Rusev, Dimitar
-- Create date: 2012-05-15
-- Modified:	2013-07-08, 2013-10-28
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
--SET @categoryKeyWords = REPLACE(@categoryKeyWords, ' ', '')

DECLARE @index INT = CHARINDEX(',', @categoryKeyWords, 0)
DECLARE @keyWord NVARCHAR(200)

WHILE @index > 0
BEGIN	
	SET @keyWord = LTRIM(LEFT(@categoryKeyWords, @index - 1))
	
	IF LEN(@keyWord) > 0
	BEGIN	
	
		INSERT INTO tbCostCategories
			   (CostCategoryID
			   ,CostNames
			   ,[Language])
		 VALUES
			   (@categoryID
			   ,UPPER(@keyWord)
			   ,@languageID)
			   
	END
	
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

USE [Test01Db]
GO
/****** Object:  StoredProcedure [dbo].[spGetCategoriesTable]    Script Date: 10/28/2014 12:03:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Dimitar Rusev
-- Create date: 13.12.2009
-- Modified date: 08.07.2013, 28.10.2014
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
	DECLARE @commentsCount INT
	DECLARE @keyWords NVARCHAR(MAX)
	DECLARE @myCursor CURSOR
	
		
	DECLARE @costCategories TABLE(
	CategoryID INT,
	CategoryName NVARCHAR(50),
	CategoryKeyWords NVARCHAR(MAX),
	IconPath NVARCHAR(300),
	IsPayIconVisible BIT,
	IsShared BIT,
	UserCategoryID INT,
	CommentsCount INT
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
				
		SET @commentsCount = (SELECT COUNT(ID) FROM tbCategoryComments WHERE CategoryID = @categoryID AND IsDeleted = 0)

		-- Get keywords for category		
		SET @keyWords = ''
		
		SELECT @keyWords = COALESCE(@keyWords + ', ', '') + CostNames
		FROM tbCostCategories WHERE CostCategoryID = @categoryID
		
		INSERT INTO @costCategories SELECT @categoryID, @categoryName, @keyWords, IconPath, IsPayIconVisible, IsShared, UserCategoryID, @commentsCount FROM dbo.tbCategories WHERE ID = @categoryID AND (UserCategoryID = @userID OR UserCategoryID = 0)
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
						
				SET @commentsCount = (SELECT COUNT(ID) FROM tbCategoryComments WHERE CategoryID = @categoryID AND IsDeleted = 0)						
						
				-- Get keywords for category		
				SET @keyWords = ''
				
				SELECT @keyWords = COALESCE(@keyWords + ', ', '') + CostNames
				FROM tbCostCategories WHERE CostCategoryID = @categoryID
				
				-- Remove comma at the beggining and trim whitespaces
				IF CHARINDEX(',', @keyWords) = 1
				BEGIN								
					SET @keyWords = SUBSTRING(@keyWords, 2, LEN(@keyWords))
					--SET @keyWords = REPLACE(@keyWords, ' ', '')
				END
								
				INSERT INTO @costCategories SELECT @categoryID, @categoryName, @keyWords, IconPath, IsPayIconVisible, IsShared, UserCategoryID, @commentsCount FROM dbo.tbCategories WHERE ID = @categoryID
												
				FETCH NEXT FROM @myCursor INTO @categoryID
			END

		CLOSE @myCursor
		DEALLOCATE @myCursor
				
	END

	SELECT * FROM @costCategories 
END

USE [Test01Db]
GO
/****** Object:  StoredProcedure [dbo].[spUpdateCategory]    Script Date: 10/28/2014 12:04:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Rusev, Dimitar
-- Create date: 2014-10-28
-- Description:	Update category keywords
-- =============================================
ALTER PROCEDURE [dbo].[spUpdateCategory]
	-- Add the parameters for the stored procedure here
	@categoryId INT,
	@categoryKeyWordsCommaDelimited NVARCHAR(MAX),
	@languageID INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF CHARINDEX(',', @categoryKeyWordsCommaDelimited, len(@categoryKeyWordsCommaDelimited)) = 0
	BEGIN
		SET @categoryKeyWordsCommaDelimited = @categoryKeyWordsCommaDelimited + ','
	END

	DECLARE @index INT = CHARINDEX(',', @categoryKeyWordsCommaDelimited, 0)
	DECLARE @keyWord NVARCHAR(200)

	DELETE FROM tbCostCategories WHERE CostCategoryID = @categoryId AND [Language] = @languageID

	WHILE @index > 0
	BEGIN	
		SET @keyWord = LTRIM(LEFT(@categoryKeyWordsCommaDelimited, @index - 1))
		
		IF LEN(@keyWord) > 0
		BEGIN
			INSERT INTO tbCostCategories
			   (CostCategoryID
			   ,CostNames
			   ,[Language])
			VALUES
			   (@categoryID
			   ,UPPER(@keyWord)
			   ,@languageID)		
		END
				
		SET @categoryKeyWordsCommaDelimited = RIGHT(@categoryKeyWordsCommaDelimited, LEN(@categoryKeyWordsCommaDelimited) - @index)
		SET @index = CHARINDEX(',', @categoryKeyWordsCommaDelimited, 0)
	END
END

