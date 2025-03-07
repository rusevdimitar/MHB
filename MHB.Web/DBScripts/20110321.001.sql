USE [100_Test01Db]
GO
/****** Object:  StoredProcedure [dbo].[spDeleteDemoEntries]    Script Date: 03/21/2011 18:23:16 ******/

-- =============================================
-- Author:		Dimitar Rusev
-- Create date: 13.12.2009
-- Modified date: 21.03.2011
-- Description:	Get categories table
-- =============================================


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[spDeleteDemoEntries]
	
	
AS
BEGIN
	DECLARE @interval INT
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

   SET @interval = (SELECT DATEDIFF(MINUTE, 0, GETDATE()) - DATEDIFF(MINUTE, 0, MAX(logDate)) FROM dbo.tbActionLog WHERE logAction = 22)

	IF @interval > 15
	BEGIN
	
		DELETE FROM dbo.tbDetailsTable01 WHERE ExpenditureID IN (SELECT ID FROM dbo.tbMainTable01 WHERE UserID = 25) 
		DELETE FROM dbo.tbMainTable01 WHERE UserID = 25
		DELETE FROM tbMonthlyBudget WHERE UserID = 25
		DELETE FROM tbMonthlyExpenses WHERE UserID = 25
		DELETE FROM tbMonthlySavings WHERE UserID = 25
		  
		INSERT INTO [dbo].[tbMainTable01] VALUES
           (25
           ,MONTH(GETDATE())
           ,YEAR(GETDATE())
           ,'Телефон'
           ,'Домашен телефон'
           ,37.50
           ,35
           ,GETDATE() -1 
           ,GETDATE()
           ,0
           ,0
           ,0
           ,NULL
           ,0
           ,0
           ,3
           ,1
           ,GETDATE(),0, 0, 13)
           
           INSERT INTO [dbo].[tbMainTable01] VALUES
           (25
           ,MONTH(GETDATE())
           ,YEAR(GETDATE())
           ,'Мобилен'
           ,'моят мобилен телефон'
           ,48.26
           ,60
           ,GETDATE() + 4 
           ,GETDATE()
           ,0
           ,0
           ,0
           ,NULL
           ,0
           ,0
           ,3
           ,1
           ,GETDATE(),0, 0, 13)
           
INSERT INTO [dbo].[tbMainTable01] VALUES
           (25
           ,MONTH(GETDATE())
           ,YEAR(GETDATE())
           ,'Наем'
           ,'наем квартира'
           ,450
           ,450
           ,GETDATE() + 10 
           ,GETDATE()
           ,0
           ,0
           ,0
           ,NULL
           ,0
           ,0
           ,4
           ,1
           ,GETDATE(),0, 0, 13)
           
INSERT INTO [dbo].[tbMainTable01] VALUES
           (25
           ,MONTH(GETDATE())
           ,YEAR(GETDATE())
           ,'Ток'
           ,'последна сметка тримесечие'
           ,84.90
           ,100
           ,GETDATE() + 10 
           ,GETDATE()
           ,0
           ,0
           ,0
           ,NULL
           ,0
           ,0
           ,2
           ,1
           ,GETDATE(),0, 0, 13)
INSERT INTO [dbo].[tbMainTable01] VALUES
           (25
           ,MONTH(GETDATE())
           ,YEAR(GETDATE())
           ,'Кола'
           ,'годишно обслужване, смяна масло филтри'
           ,450
           ,500
           ,NULL 
           ,GETDATE()
           ,0
           ,1
           ,0
           ,NULL
           ,0
           ,0
           ,8
           ,1
           ,GETDATE(),0, 0, 13)
           
INSERT INTO [dbo].[tbMainTable01] VALUES
           (25
           ,MONTH(GETDATE())
           ,YEAR(GETDATE())
           ,'Храна'
           ,''
           ,78
           ,350
           ,NULL 
           ,GETDATE()
           ,0
           ,1
           ,0
           ,NULL
           ,0
           ,0
           ,7
           ,1
           ,GETDATE(),0, 0, 13)      
           INSERT INTO [dbo].[tbMainTable01] VALUES
           (25
           ,MONTH(GETDATE())
           ,YEAR(GETDATE())
           ,'Медицински'
           ,'профилактичен преглед, зъболекар'
           ,156.40
           ,200
           ,GETDATE() 
           ,GETDATE()
           ,1
           ,0 -- платено
           ,0
           ,NULL
           ,0
           ,0
           ,10
           ,1
           ,GETDATE(),0, 0, 13)      
           INSERT INTO [dbo].[tbMainTable01] VALUES
           (25
           ,MONTH(GETDATE())
           ,YEAR(GETDATE())
           ,'Интернет'
           ,'+ такса включване'
           ,35
           ,40
           ,GETDATE() 
           ,GETDATE()
           ,1-- платено
           ,0 
           ,0
           ,NULL
           ,0
           ,0
           ,6
           ,1
           ,GETDATE(),1, 0, 13)      
           INSERT INTO [dbo].[tbMainTable01] VALUES
           (25
           ,MONTH(GETDATE())
           ,YEAR(GETDATE())
           ,'Бензин'
           ,''
           ,250
           ,40
           ,GETDATE() 
           ,GETDATE()
           ,0-- платено
           ,0 
           ,0
           ,NULL
           ,0
           ,0
           ,1
           ,1
           ,GETDATE(),0, 0, 13)    
             
            INSERT INTO [dbo].[tbMainTable01] VALUES
           (25
           ,MONTH(GETDATE())
           ,YEAR(GETDATE())
           ,'Кредит'
           ,''
           ,250
           ,250
           ,GETDATE() 
           ,GETDATE()
           ,1-- платено
           ,0 
           ,0
           ,NULL
           ,0
           ,0
           ,9
           ,1
           ,GETDATE(),1, 0, 13)      
           
             INSERT INTO [dbo].[tbMainTable01] VALUES
           (25
           ,MONTH(GETDATE())
           ,YEAR(GETDATE())
           ,'Спестени'
           ,''
           ,250
           ,300
           ,GETDATE() 
           ,GETDATE()
           ,1-- платено
           ,0 
           ,0
           ,NULL
           ,0
           ,0
           ,5
           ,1
           ,GETDATE(),0, 0, 13)      
INSERT INTO [dbo].[tbMonthlyBudget]
           ([UserID]
           ,[Year]
           ,[BudgetJan]
           ,[BudgetFeb]
           ,[BudgetMar]
           ,[BudgetApr]
           ,[BudgetMay]
           ,[BudgetJune]
           ,[BudgetJuly]
           ,[BudgetAug]
           ,[BudgetSept]
           ,[BudgetOct]
           ,[BudgetNov]
           ,[BudgetDec])
     VALUES
           (25
           ,YEAR(GETDATE())
           ,ROUND(((3500 - 2500 -1) * RAND() + 2500), 0)
           ,ROUND(((3500 - 2500 -1) * RAND() + 2500), 0)
           ,ROUND(((3500 - 2500 -1) * RAND() + 2500), 0)
           ,ROUND(((3500 - 2500 -1) * RAND() + 2500), 0)
           ,ROUND(((3500 - 2500 -1) * RAND() + 2500), 0)
           ,ROUND(((3500 - 2500 -1) * RAND() + 2500), 0)
           ,ROUND(((3500 - 2500 -1) * RAND() + 2500), 0)
           ,ROUND(((3500 - 2500 -1) * RAND() + 2500), 0)
           ,ROUND(((3500 - 2500 -1) * RAND() + 2500), 0)
           ,ROUND(((3500 - 2500 -1) * RAND() + 2500), 0)
           ,ROUND(((3500 - 2500 -1) * RAND() + 2500), 0)
           ,ROUND(((3500 - 2500 -1) * RAND() + 2500), 0))
INSERT INTO [dbo].[tbMonthlyExpenses]
           ([UserID]
           ,[Year]
           ,[SumJanuary]
           ,[SumFebruary]
           ,[SumMarch]
           ,[SumApril]
           ,[SumMay]
           ,[SumJune]
           ,[SumJuly]
           ,[SumAugust]
           ,[SumSeptember]
           ,[SumOctober]
           ,[SumNovember]
           ,[SumDecember])
     VALUES
           (25
           ,YEAR(GETDATE())
           ,ROUND(((3100 - 2500 -1) * RAND() + 2500), 0)
           ,ROUND(((3100 - 2500 -1) * RAND() + 2500), 0)
           ,ROUND(((3100 - 2500 -1) * RAND() + 2500), 0)
           ,ROUND(((3100 - 2500 -1) * RAND() + 2500), 0)
           ,ROUND(((3100 - 2500 -1) * RAND() + 2500), 0)
           ,ROUND(((3100 - 2500 -1) * RAND() + 2500), 0)
           ,ROUND(((3100 - 2500 -1) * RAND() + 2500), 0)
           ,ROUND(((3100 - 2500 -1) * RAND() + 2500), 0)
           ,ROUND(((3100 - 2500 -1) * RAND() + 2500), 0)
           ,ROUND(((3100 - 2500 -1) * RAND() + 2500), 0)
           ,ROUND(((3100 - 2500 -1) * RAND() + 2500), 0)
           ,ROUND(((3100 - 2500 -1) * RAND() + 2500), 0))
INSERT INTO [dbo].[tbMonthlySavings]
           ([UserID]
           ,[Year]
           ,[SavingsJan]
           ,[SavingsFeb]
           ,[SavingsMar]
           ,[SavingsApr]
           ,[SavingsMay]
           ,[SavingsJune]
           ,[SavingsJuly]
           ,[SavingsAug]
           ,[SavingsSept]
           ,[SavingsOct]
           ,[SavingsNov]
           ,[SavingsDec])
     VALUES
           (25
           ,YEAR(GETDATE())
           ,ROUND(((300 - 100 -1) * RAND() + 100), 0)
           ,ROUND(((300 - 100 -1) * RAND() + 100), 0)
           ,ROUND(((300 - 100 -1) * RAND() + 100), 0)
           ,ROUND(((300 - 100 -1) * RAND() + 100), 0)
           ,ROUND(((300 - 100 -1) * RAND() + 100), 0)
           ,ROUND(((300 - 100 -1) * RAND() + 100), 0)
           ,ROUND(((300 - 100 -1) * RAND() + 100), 0)
           ,ROUND(((300 - 100 -1) * RAND() + 100), 0)
           ,ROUND(((300 - 100 -1) * RAND() + 100), 0)
           ,ROUND(((300 - 100 -1) * RAND() + 100), 0)
           ,ROUND(((300 - 100 -1) * RAND() + 100), 0)
           ,ROUND(((300 - 100 -1) * RAND() + 100), 0))

UPDATE [dbo].[tbUsers] SET [currency] ='лв.', [language] = 0 WHERE userID = 25





		
		SELECT 1
	END
	ELSE 
	BEGIN
		SELECT 0
	END
	
END
