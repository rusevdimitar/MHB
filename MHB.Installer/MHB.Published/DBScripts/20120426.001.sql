USE [Test01Db]
GO
/****** Object:  StoredProcedure [dbo].[spGetActionLogData]    Script Date: 04/26/2012 09:09:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Rusev, Dimitar
-- Create date: 2012-04-26
-- Description:	Gets the unique visitors since a given date
-- =============================================

ALTER PROCEDURE [dbo].[spGetUniqueVisitors]
	@startDate DATETIME

AS
BEGIN
	
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

SELECT DISTINCT tbUsers.email AS UserEmail, CAST(tbUsers.password AS VARCHAR) AS UserPassword, tbUsers.registrationdate AS UserRegistrationDate, tbUsers.lastipaddress AS UserLastIPAddress
FROM tbActionLog 
INNER JOIN tbUsers ON tbUsers.userID = tbActionLog.logUserID
WHERE tbActionLog.logDate >= @startDate


	
END

