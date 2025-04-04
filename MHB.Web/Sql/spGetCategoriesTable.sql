USE [Test01Db]
GO
/****** Object:  StoredProcedure [dbo].[spGetCategoriesTable]    Script Date: 10/28/2014 11:22:45 ******/
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