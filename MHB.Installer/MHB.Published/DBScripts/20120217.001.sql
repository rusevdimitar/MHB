USE smetkieu_db1
GO
/****** Object:  StoredProcedure [dbo].[spGetCategoriesTable]    Script Date: 02/17/2012 10:53:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Dimitar Rusev
-- Create date: 13.12.2009
-- Modified date: 17.02.2012
-- Description:	Get categories table
-- =============================================
ALTER PROCEDURE [dbo].[spGetCategoriesTable]
	@language INT,
	@userID INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


	DECLARE @categoryID INT
	DECLARE @categoryName NVARCHAR(50)
	


	DECLARE @costCategories TABLE(
	CategoryID INT,
	CategoryName NVARCHAR(50),
	IconPath NVARCHAR(300),
	IsPayIconVisible BIT,
	UserCategoryID INT
	)

	SET @categoryID = 1

	WHILE @categoryID < 11
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

				
		
		INSERT INTO @costCategories SELECT @categoryID, @categoryName, IconPath, IsPayIconVisible, UserCategoryID FROM dbo.tbCategories WHERE ID = @categoryID AND UserCategoryID = @userID
		
		SET @categoryID = @categoryID + 1
	END

	SELECT * FROM @costCategories 
END
