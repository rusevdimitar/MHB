USE [Test01Db]
GO
/****** Object:  StoredProcedure [dbo].[spAddNewUserCategory]    Script Date: 05/15/2012 15:43:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Rusev, Dimitar
-- Create date: 2012-05-15
-- Description:	Adds a new user-defined cost category
-- =============================================

CREATE PROCEDURE [dbo].[spAddNewUserCategory]
	@languageID INT,
	@categoryName NVARCHAR(200),
	@categoryKeyWords NVARCHAR(MAX),	
	@categoryIconPath NVARCHAR(MAX),
	@categoryIsPayIconVisible BIT,
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
           ,UserCategoryID)
     VALUES
           (@categoryID
           ,@categoryName
           ,@categoryIconPath
           ,@categoryIsPayIconVisible
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

