USE [100_Test01Db]
GO


-- =============================================
-- Author:		Dimitar Rusev
-- Create date: 20.05.2011
-- Modified date: 
-- Description:	New transactions table
-- =============================================



CREATE TABLE dbo.tbTransactionLog (
[ID] INT IDENTITY (1,1) NOT NULL,
[UserID] INT NOT NULL DEFAULT 0,
[ExpenseID] INT NOT NULL DEFAULT 0,
[NewValue] MONEY NOT NULL DEFAULT 0,
[OldValue] MONEY NOT NULL DEFAULT 0,
[TransactionText] NVARCHAR(300) NOT NULL DEFAULT '',
[DateModified] DATETIME NULL
)

ALTER TABLE dbo.tbTransactionLog ADD CONSTRAINT PK_TRANID PRIMARY KEY (ID)



INSERT INTO [dbo].[tbLanguage]
           ([ControlID]
           ,[ControlTextEN]
           ,[ControlTextBG]
           ,[ControlTextDE])
     VALUES
           ('LinkButtonTransactionsToolTip'
           ,'Show transactions. NOTE: Always leave the site using the Exit button in the top right corner otherwise transactions that you have made earlier may not be recorded'
           ,'Покажи всички промени на дадената сметка. ЗАБЕЛЕЖКА: Винаги напускайте сайта като натиснете бутона "Изход" в горния десен ъгъл на екрана, в противен случай транзакциите може да не бъдат записани'
           ,'Transaktionen anzeigen. HINWEIS: Schließen Sie die Website mittels die Abmelden Taste oben links, sonst wuerden ihre Transaktionen nicht gespeichert.')


