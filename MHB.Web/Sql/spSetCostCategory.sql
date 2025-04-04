set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

-- =============================================
-- Author:		Rusev, Dimtitar
-- Create date: 08.10.2009
-- Description:	Sets the costs category, based on keywords in the name
-- Modified:	15.10.2009
-- =============================================
CREATE PROCEDURE [dbo].[spSetCostCategory]
	
	@ID INT,
	@mainTable NVARCHAR(20),
	@keyWord NVARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DECLARE @costCategory INT	
	DECLARE @qry AS NVARCHAR(MAX)
	

	SET @costCategory = (SELECT TOP 1 [CostCategoryID] 
	  FROM [dbo].[tbCostCategories]
	WHERE PATINDEX('%'+CostNames+'%', RTRIM(LTRIM(@keyWord))) <> 0)

	IF @costCategory IS NOT NULL
	BEGIN
		SET @qry = 'UPDATE ' + @mainTable + ' SET CostCategory = ' + CAST(@costCategory AS NVARCHAR) + ' WHERE ID = ' + CAST(@ID AS NVARCHAR)
		EXECUTE sp_sqlexec @qry
	END
	ELSE
	BEGIN
		SET @qry = 'UPDATE ' + @mainTable + ' SET CostCategory = 0 WHERE ID = ' + CAST(@ID AS NVARCHAR)
		EXECUTE sp_sqlexec @qry
	END 
	
	SELECT ISNULL(@costCategory,0) AS CostCategory
END
