ALTER TABLE tbCurrencyExchangeRates ADD ExchangeRateBGNCHF MONEY NOT NULL

ALTER TABLE tbCurrencyExchangeRates DROP CONSTRAINT [DF__tbCurrenc__Excha__690797E6]
ALTER TABLE tbCurrencyExchangeRates ALTER COLUMN ExchangeRateBGNEUR MONEY NOT NULL

ALTER TABLE tbCurrencyExchangeRates DROP CONSTRAINT [DF__tbCurrenc__Excha__69FBBC1F]
ALTER TABLE tbCurrencyExchangeRates ALTER COLUMN ExchangeRateBGNUSD MONEY NOT NULL

ALTER TABLE tbCurrencyExchangeRates DROP CONSTRAINT [DF__tbCurrenc__Excha__6AEFE058]
ALTER TABLE tbCurrencyExchangeRates ALTER COLUMN ExchangeRateBGNGBP MONEY NOT NULL

 UPDATE tbCurrencyExchangeRates SET ExchangeRateBGNCHF=1.84061, ExchangeRateBGNGBP=2.63553, ExchangeRateBGNUSD=1.71444 WHERE ID=38975
 UPDATE tbCurrencyExchangeRates SET ExchangeRateBGNCHF=1.83974, ExchangeRateBGNGBP=2.63093, ExchangeRateBGNUSD=1.71339 WHERE ID=38991
 UPDATE tbCurrencyExchangeRates SET ExchangeRateBGNCHF=1.82668, ExchangeRateBGNGBP=2.65305, ExchangeRateBGNUSD=1.71986 WHERE ID=39018
 UPDATE tbCurrencyExchangeRates SET ExchangeRateBGNCHF=1.81162, ExchangeRateBGNGBP=2.65054, ExchangeRateBGNUSD=1.7176  WHERE ID=39032
 UPDATE tbCurrencyExchangeRates SET ExchangeRateBGNCHF=1.82481, ExchangeRateBGNGBP=2.66172, ExchangeRateBGNUSD=1.73113 WHERE ID=39054
 UPDATE tbCurrencyExchangeRates SET ExchangeRateBGNCHF=1.82328, ExchangeRateBGNGBP=2.66027, ExchangeRateBGNUSD=1.73113 WHERE ID=39110
 UPDATE tbCurrencyExchangeRates SET ExchangeRateBGNCHF=1.81853, ExchangeRateBGNGBP=2.66317, ExchangeRateBGNUSD=1.72654 WHERE ID=39130
 UPDATE tbCurrencyExchangeRates SET ExchangeRateBGNCHF=1.81549, ExchangeRateBGNGBP=2.66898, ExchangeRateBGNUSD=1.72381 WHERE ID=39179
 UPDATE tbCurrencyExchangeRates SET ExchangeRateBGNCHF=1.82022, ExchangeRateBGNGBP=2.67885, ExchangeRateBGNUSD=1.72822 WHERE ID=39212
 UPDATE tbCurrencyExchangeRates SET ExchangeRateBGNCHF=1.83888, ExchangeRateBGNGBP=2.68732, ExchangeRateBGNUSD=1.74006 WHERE ID=39233
 UPDATE tbCurrencyExchangeRates SET ExchangeRateBGNCHF=1.8289, ExchangeRateBGNGBP=2.69435, ExchangeRateBGNUSD=1.75821 WHERE ID=39285
 UPDATE tbCurrencyExchangeRates SET ExchangeRateBGNCHF=1.82839, ExchangeRateBGNGBP=2.69732, ExchangeRateBGNUSD=1.76694 WHERE ID=39301
 UPDATE tbCurrencyExchangeRates SET ExchangeRateBGNCHF=1.82788, ExchangeRateBGNGBP=2.70891, ExchangeRateBGNUSD=1.78403 WHERE ID=39323
 UPDATE tbCurrencyExchangeRates SET ExchangeRateBGNCHF=1.82942, ExchangeRateBGNGBP=2.72172, ExchangeRateBGNUSD=1.80095 WHERE ID=39387
 UPDATE tbCurrencyExchangeRates SET ExchangeRateBGNCHF=1.82737, ExchangeRateBGNGBP=2.74387, ExchangeRateBGNUSD=1.82141 WHERE ID=39411
 UPDATE tbCurrencyExchangeRates SET ExchangeRateBGNCHF=1.8356, ExchangeRateBGNGBP=2.77994, ExchangeRateBGNUSD=1.84896 WHERE ID=39423
 UPDATE tbCurrencyExchangeRates SET ExchangeRateBGNCHF=1.83888, ExchangeRateBGNGBP=2.75819, ExchangeRateBGNUSD=1.84286 WHERE ID=39446
 
 
 
 
