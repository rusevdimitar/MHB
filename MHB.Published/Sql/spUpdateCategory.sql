USE [Test01Db]
GO
/****** Object:  StoredProcedure [dbo].[spUpdateCategory]    Script Date: 10/28/2014 11:25:47 ******/
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
