USE [Test01Db]
GO
/****** Object:  StoredProcedure [dbo].[spGetActionLogData]    Script Date: 04/25/2012 16:33:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Rusev, Dimitar
-- Create date: 2012-04-25
-- Description:	Get ActionLog and Localization data for visiting users
-- =============================================

CREATE PROCEDURE [dbo].[spGetActionLogData]
	@startDate DATETIME

AS
BEGIN
	
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT tbTransactionLog.TransactionText, tbActionLog.ID, tbActionLog.logAction, tbActionLog.logUserID, ISNULL(tbUsers.email, '') AS UserEmail, ISNULL(CAST(tbUsers.[password] AS VARCHAR), '') as UserPassword, tbActionLog.logDate, tbActionLog.logMessage, tbActionLog.logIP, 
	ISNULL(GeoIP.CountryCode, '') AS CountryCode, ISNULL(GeoIP.City, '') AS City, ISNULL(GeoIP.RegionName, '') AS RegionName
	FROM tbActionLog 
	LEFT JOIN tbUsers ON tbUsers.userID = tbActionLog.logUserID
	LEFT JOIN (SELECT DISTINCT Ip, CountryCode, City, RegionName FROM tbUsersGeoLocationData) GeoIP ON GeoIP.Ip = tbActionLog.logIP COLLATE Cyrillic_General_CI_AS
	LEFT JOIN tbTransactionLog ON CONVERT(VARCHAR, tbTransactionLog.DateModified, 120) = CONVERT(VARCHAR, tbActionLog.logDate, 120)
	WHERE tbActionLog.logDate >= @startDate
	ORDER BY tbActionLog.logDate DESC

	
END

