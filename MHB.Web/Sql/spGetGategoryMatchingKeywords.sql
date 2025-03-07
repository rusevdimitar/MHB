USE [Test01Db]
GO
/****** Object:  StoredProcedure [dbo].[spGetGategoryMatchingKeywords]    Script Date: 05/17/2012 11:15:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Rusev, Dimitar
-- Create date: 2010-12-11
-- Modified date: 2012-05-17
-- Description:	Returns a table of all category keywords
-- =============================================
ALTER PROCEDURE [dbo].[spGetGategoryMatchingKeywords]
	-- Add the parameters for the stored procedure here
	@language INT,
	@userID INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

   
SELECT tbCostCategories.ID
      ,tbCostCategories.CostCategoryID
      ,tbCostCategories.CostNames
      ,tbCostCategories.Language
      ,tbCategories.UserCategoryID
  FROM dbo.tbCostCategories 
  INNER JOIN tbCategories ON tbCategories.ID = tbCostCategories.CostCategoryID
  WHERE [Language] = 0 AND (tbCategories.UserCategoryID = 0 OR tbCategories.UserCategoryID = @userID)



END
