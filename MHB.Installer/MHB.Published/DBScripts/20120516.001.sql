USE [Test01Db]
GO
/****** Object:  StoredProcedure [dbo].[spDeleteUserDefinedCategory]    Script Date: 05/16/2012 10:09:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Rusev, Dimitar
-- Create date: 2012-05-16 09:06
-- Description:	Deletes an user-defined category from the database
-- =============================================
CREATE PROCEDURE [dbo].[spDeleteUserDefinedCategory]
		
	@categoryID INT
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    DELETE FROM dbo.tbCostCategories WHERE CostCategoryID = @categoryID
    DELETE FROM dbo.tbCategories WHERE ID = @categoryID
    DELETE FROM dbo.tbLanguage WHERE ControlID = CAST(@categoryID AS NVARCHAR)
	
END
