INSERT INTO dbo.tbLanguage
           (ControlID
           ,ControlTextEN
           ,ControlTextBG
           ,ControlTextDE)
     VALUES
           ('lblNewIncomeDate'
           ,'Date of receipt:'
           ,'Получаване на:'
           ,'Eingegangen am:')

ALTER TABLE dbo.tbIncomes ADD IncomeDate DATETIME NULL