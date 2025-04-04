CREATE TABLE dbo.tbCurrencyExchangeRates
(
	ID int NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	ExchangeRateXml nvarchar(max) NOT NULL DEFAULT '',
	ExchangeRateHtml nvarchar(max) NOT NULL DEFAULT '',
	ExchangeRateBGNEUR nvarchar(50) NOT NULL DEFAULT '',
	ExchangeRateBGNUSD nvarchar(50) NOT NULL DEFAULT '',
	ExchangeRateBGNGBP nvarchar(50) NOT NULL DEFAULT '',
	ExchangeRateDate datetime NULL,
	IsVisible bit NOT NULL DEFAULT 0,
	IsDeleted bit NOT NULL DEFAULT 0,
)


