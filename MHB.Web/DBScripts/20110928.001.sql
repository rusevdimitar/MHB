USE [100_Test01Db]
GO


-- =============================================
-- Author:		Dimitar Rusev
-- Create date: 28.09.2011
-- Modified date: 
-- Description:	New translation
-- =============================================


INSERT INTO [dbo].[tbLanguage]
           ([ControlID]
           ,[ControlTextEN]
           ,[ControlTextBG]
           ,[ControlTextDE])
     VALUES
           ('paidlessthismonth'
           ,'less: '
           ,'този мес. по-мал.: '
           ,'weniger: ')
           
INSERT INTO [dbo].[tbLanguage]
           ([ControlID]
           ,[ControlTextEN]
           ,[ControlTextBG]
           ,[ControlTextDE])
     VALUES
           ('paidmorethismonth'
           ,'more: '
           ,'този мес. повече: '
           ,'mehr: ')

