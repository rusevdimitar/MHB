UPDATE tbLanguage SET ControlID = 'ButtonDetailsTableAttachBottom' WHERE ControlID = 'ButtonDetailsAttachBottom'

INSERT INTO [dbo].[tbLanguage]
           ([ControlID]
           ,[ControlTextEN]
           ,[ControlTextBG]
           ,[ControlTextDE])
     VALUES
           ('LabelSumPerDayText'
           ,'Budget per day:'
           ,'Бюджет на ден:'
           ,'Budget pro Tag:')