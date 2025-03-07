USE [Test01Db]
GO
/****** Object:  StoredProcedure [dbo].[spGetExchangeRates]    Script Date: 03/26/2013 19:24:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Rusev, Dimitar
-- Create date: 2012-12-17
-- Modified:	2013-03-26
-- Description:	Parse rss feed from BNB rss service
-- =============================================
ALTER PROCEDURE [dbo].[spGetExchangeRates]
	@startDate DATETIME
AS
BEGIN

	SET NOCOUNT ON;

	
DECLARE @description AS NVARCHAR(MAX)
DECLARE @StartIndexStrongTag AS INT
DECLARE @EndIndexStrongTag AS INT
DECLARE @USDRate AS MONEY
DECLARE @GBPRate AS MONEY
DECLARE @CHFRate AS MONEY
DECLARE @ExchangeRateDate AS DATETIME



DECLARE @ExchangeRatesTemp TABLE
(
  UsdToBgn MONEY,
  GbpToBgn MONEY,
  ChfToBgn MONEY,
  DateRetreived DATETIME
)

-- Delete empty html
DELETE tbCurrencyExchangeRates
FROM tbCurrencyExchangeRates
WHERE LEN(ExchangeRateHtml) < 10

-- Delete duplicates
DELETE tbCurrencyExchangeRates 
FROM tbCurrencyExchangeRates
LEFT OUTER JOIN (
   SELECT MIN(ID) as RowId, ExchangeRateXml
   FROM tbCurrencyExchangeRates 
   GROUP BY ExchangeRateXml
) as KeepRows ON
   tbCurrencyExchangeRates.ID = KeepRows.RowId
WHERE
   KeepRows.RowId IS NULL
-- =====

DECLARE ratesCursor CURSOR FOR

	SELECT
	CAST
	(
		REPLACE(CAST(ExchangeRateXml AS VARCHAR(MAX)), 'encoding="utf-8"', '') AS XML
	)
	.value('(/rss/channel/item/description/node())[1]', 'NVARCHAR(MAX)') as [Description],
	ExchangeRateDate
		
	FROM tbCurrencyExchangeRates WHERE ExchangeRateDate > @startDate

OPEN ratesCursor

FETCH NEXT FROM ratesCursor INTO @description, @ExchangeRateDate

WHILE @@FETCH_STATUS = 0
BEGIN

	DECLARE @usd NVARCHAR(6)
	DECLARE @gbp NVARCHAR(6)
	DECLARE @chf NVARCHAR(6)

-- ==================================================================================================================
--														USD
-- ==================================================================================================================
	SET @StartIndexStrongTag = CHARINDEX('<strong>', @description) + LEN('<strong>')
	SET @EndIndexStrongTag = CHARINDEX('</strong>', @description) - CHARINDEX('<strong>', @description) - LEN('</strong>')

	SET @usd = SUBSTRING(@description, @StartIndexStrongTag, @EndIndexStrongTag)

	IF ISNUMERIC(@usd) = 1
	BEGIN
		SET @USDRate = CAST(@usd AS MONEY)
	END

-- 8888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888 <<<<< END

	SET @description = SUBSTRING(@description, @StartIndexStrongTag, LEN(@description))


-- ==================================================================================================================
--														GBP
-- ==================================================================================================================
	SET @StartIndexStrongTag = CHARINDEX('<strong>', @description) + LEN('<strong>')
	SET @EndIndexStrongTag = CHARINDEX('</strong>', @description) - 2

	SET @gbp = SUBSTRING(@description, @StartIndexStrongTag, @EndIndexStrongTag)

	IF ISNUMERIC(@gbp) = 1
	BEGIN
		SET @GBPRate = CAST(@gbp AS MONEY)
	END

-- 8888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888 <<<<< END


-- ==================================================================================================================
--														CHF
-- ==================================================================================================================
	SET @description = SUBSTRING(@description, @StartIndexStrongTag, LEN(@description))

	SET @StartIndexStrongTag = CHARINDEX('<strong>', @description) + LEN('<strong>')
	SET @EndIndexStrongTag = CHARINDEX('</strong>', @description) - 2

	SET @chf = SUBSTRING(@description, @StartIndexStrongTag, @EndIndexStrongTag)

	IF ISNUMERIC(@chf) = 1
	BEGIN
		SET @CHFRate = CAST(@chf AS MONEY)
	END

-- 8888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888 <<<<< END

	INSERT INTO @ExchangeRatesTemp VALUES(@USDRate, @GBPRate, @CHFRate, @ExchangeRateDate)
	
	FETCH NEXT FROM ratesCursor INTO @description, @ExchangeRateDate

END
CLOSE ratesCursor
DEALLOCATE ratesCursor


SELECT * FROM @ExchangeRatesTemp


 
END
