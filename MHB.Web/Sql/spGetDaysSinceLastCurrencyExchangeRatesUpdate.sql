USE [Test01Db]
GO
/****** Object:  StoredProcedure [dbo].[spGetDaysSinceLastCurrencyExchangeRatesUpdate]    Script Date: 07/02/2013 10:25:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Rusev, Dimitar
-- Create date: 2013-06-28
-- Modified:	2013-07-02
-- Description:	Returns days passed since last tbCurrencyExchangeRates update
-- =============================================
ALTER PROCEDURE [dbo].[spGetDaysSinceLastCurrencyExchangeRatesUpdate]
	
AS
BEGIN

	SET NOCOUNT ON;

	DECLARE @latestDate DATETIME	

	SET @latestDate = (SELECT MAX(ExchangeRateDate) FROM tbCurrencyExchangeRates)

	SELECT DATEDIFF(D, @latestDate, GETDATE())
END
