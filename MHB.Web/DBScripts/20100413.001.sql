
DECLARE @s varchar(50)
declare @sum varchar(50)

DECLARE fil SCROLL CURSOR FOR
	select userid from tbusers	
			
														
OPEN fil
																				
FETCH NEXT FROM fil INTO @s

WHILE @@fetch_status = 0 BEGIN

set @sum = (SELECT [BudgetJan]
FROM [dbo].[tbMonthlyBudget]
where year = 2009 and [BudgetJan] is not null and [BudgetJan] <> 0 and [UserID] = @s)

INSERT INTO [dbo].[tbIncomes]
           ([IncomeName]
           ,[IncomeValue]
           ,[UserID]
           ,[Month]
           ,[Year])
     VALUES
           ('месечен доход'
           ,@sum
           ,@s
           ,1
           ,2009)
--////////////////////////////////////////////////////////////////////////////////////////////
set @sum = (SELECT BudgetFeb
FROM [dbo].[tbMonthlyBudget]
where year = 2009 and BudgetFeb is not null and BudgetFeb <> 0 and [UserID] = @s)

INSERT INTO [dbo].[tbIncomes]
           ([IncomeName]
           ,[IncomeValue]
           ,[UserID]
           ,[Month]
           ,[Year])
     VALUES
           ('месечен доход'
           ,@sum
           ,@s
           ,2
           ,2009)
--////////////////////////////////////////////////////////////////////////////////////////////
set @sum = (SELECT BudgetMar
FROM [dbo].[tbMonthlyBudget]
where year = 2009 and BudgetMar is not null and BudgetMar <> 0 and [UserID] = @s)

INSERT INTO [dbo].[tbIncomes]
           ([IncomeName]
           ,[IncomeValue]
           ,[UserID]
           ,[Month]
           ,[Year])
     VALUES
           ('месечен доход'
           ,@sum
           ,@s
           ,3
           ,2009)
--////////////////////////////////////////////////////////////////////////////////////////////
set @sum = (SELECT BudgetApr
FROM [dbo].[tbMonthlyBudget]
where year = 2009 and BudgetApr is not null and BudgetApr <> 0 and [UserID] = @s)

INSERT INTO [dbo].[tbIncomes]
           ([IncomeName]
           ,[IncomeValue]
           ,[UserID]
           ,[Month]
           ,[Year])
     VALUES
           ('месечен доход'
           ,@sum
           ,@s
           ,4
           ,2009)
--////////////////////////////////////////////////////////////////////////////////////////////
set @sum = (SELECT BudgetMay
FROM [dbo].[tbMonthlyBudget]
where year = 2009 and BudgetMay is not null and BudgetMay <> 0 and [UserID] = @s)

INSERT INTO [dbo].[tbIncomes]
           ([IncomeName]
           ,[IncomeValue]
           ,[UserID]
           ,[Month]
           ,[Year])
     VALUES
           ('месечен доход'
           ,@sum
           ,@s
           ,5
           ,2009)
	FETCH NEXT FROM fil INTO @s
--////////////////////////////////////////////////////////////////////////////////////////////
set @sum = (SELECT BudgetJune
FROM [dbo].[tbMonthlyBudget]
where year = 2009 and BudgetJune is not null and BudgetJune <> 0 and [UserID] = @s)

INSERT INTO [dbo].[tbIncomes]
           ([IncomeName]
           ,[IncomeValue]
           ,[UserID]
           ,[Month]
           ,[Year])
     VALUES
           ('месечен доход'
           ,@sum
           ,@s
           ,6
           ,2009)
--////////////////////////////////////////////////////////////////////////////////////////////
set @sum = (SELECT BudgetJuly
FROM [dbo].[tbMonthlyBudget]
where year = 2009 and BudgetJuly is not null and BudgetJuly <> 0 and [UserID] = @s)

INSERT INTO [dbo].[tbIncomes]
           ([IncomeName]
           ,[IncomeValue]
           ,[UserID]
           ,[Month]
           ,[Year])
     VALUES
           ('месечен доход'
           ,@sum
           ,@s
           ,7
           ,2009)
--////////////////////////////////////////////////////////////////////////////////////////////
set @sum = (SELECT BudgetAug
FROM [dbo].[tbMonthlyBudget]
where year = 2009 and BudgetAug is not null and BudgetAug <> 0 and [UserID] = @s)

INSERT INTO [dbo].[tbIncomes]
           ([IncomeName]
           ,[IncomeValue]
           ,[UserID]
           ,[Month]
           ,[Year])
     VALUES
           ('месечен доход'
           ,@sum
           ,@s
           ,8
           ,2009)
--////////////////////////////////////////////////////////////////////////////////////////////
set @sum = (SELECT BudgetSept
FROM [dbo].[tbMonthlyBudget]
where year = 2009 and BudgetSept is not null and BudgetSept <> 0 and [UserID] = @s)

INSERT INTO [dbo].[tbIncomes]
           ([IncomeName]
           ,[IncomeValue]
           ,[UserID]
           ,[Month]
           ,[Year])
     VALUES
           ('месечен доход'
           ,@sum
           ,@s
           ,9
           ,2009)
--////////////////////////////////////////////////////////////////////////////////////////////
set @sum = (SELECT BudgetOct
FROM [dbo].[tbMonthlyBudget]
where year = 2009 and BudgetOct is not null and BudgetOct <> 0 and [UserID] = @s)

INSERT INTO [dbo].[tbIncomes]
           ([IncomeName]
           ,[IncomeValue]
           ,[UserID]
           ,[Month]
           ,[Year])
     VALUES
           ('месечен доход'
           ,@sum
           ,@s
           ,10
           ,2009)
--////////////////////////////////////////////////////////////////////////////////////////////
set @sum = (SELECT BudgetNov
FROM [dbo].[tbMonthlyBudget]
where year = 2009 and BudgetNov is not null and BudgetNov <> 0 and [UserID] = @s)

INSERT INTO [dbo].[tbIncomes]
           ([IncomeName]
           ,[IncomeValue]
           ,[UserID]
           ,[Month]
           ,[Year])
     VALUES
           ('месечен доход'
           ,@sum
           ,@s
           ,11
           ,2009)
--////////////////////////////////////////////////////////////////////////////////////////////
set @sum = (SELECT BudgetDec
FROM [dbo].[tbMonthlyBudget]
where year = 2009 and BudgetDec is not null and BudgetDec <> 0 and [UserID] = @s)

INSERT INTO [dbo].[tbIncomes]
           ([IncomeName]
           ,[IncomeValue]
           ,[UserID]
           ,[Month]
           ,[Year])
     VALUES
           ('месечен доход'
           ,@sum
           ,@s
           ,12
           ,2009)
	FETCH NEXT FROM fil INTO @s
END
																				
CLOSE fil
DEALLOCATE fil