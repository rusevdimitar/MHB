USE [Test01Db]
GO
/****** Object:  StoredProcedure [dbo].[spGetYearlySavingsSum]    Script Date: 10/15/2009 16:19:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Rusev, Dimitar
-- Create date: 15.10.2009
-- Description:	Gets the sum of the savings for a given user and year
-- =============================================
CREATE PROCEDURE [dbo].[spGetYearlySavingsSum]
	@userID INT, 
	@year INT 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT 
	ISNULL([SavingsJan],0) + 
      ISNULL([SavingsFeb],0) +
      ISNULL([SavingsMar],0) +
      ISNULL([SavingsApr],0) +
      ISNULL([SavingsMay],0) +
      ISNULL([SavingsJune],0) +
      ISNULL([SavingsJuly],0) +
      ISNULL([SavingsAug],0) +
      ISNULL([SavingsSept],0) +
      ISNULL([SavingsOct],0) +
      ISNULL([SavingsNov],0) +
      ISNULL([SavingsDec],0)
  FROM [dbo].[tbMonthlySavings] WHERE [UserID] = @userID AND [Year] = @year

END
