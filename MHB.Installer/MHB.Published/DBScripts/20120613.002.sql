INSERT INTO dbo.tbLanguage
           (ControlID
           ,ControlTextEN
           ,ControlTextBG
           ,ControlTextDE)
     VALUES
           ('ButtonStatistics'
           ,'General statistics'
           ,'Обща статистика'
           ,'Gesamtbericht')

INSERT INTO dbo.tbLanguage
           (ControlID
           ,ControlTextEN
           ,ControlTextBG
           ,ControlTextDE)
     VALUES
           ('LabelEndYear'
           ,'End year:'
           ,'Крайна година:'
           ,'Endjahr:')

INSERT INTO dbo.tbLanguage
           (ControlID
           ,ControlTextEN
           ,ControlTextBG
           ,ControlTextDE)
     VALUES
           ('LabelGeneralStatisticsInfo'
           ,'Info: Starting year bars are located in the innermost part of the chart'
           ,'Съвет: Стълбчетата на началната година са разположени в най-вътрешната част на графиката'
           ,'Startjahr Balken befinden sich im innersten Teil des Balkendiagramms')

INSERT INTO dbo.tbLanguage
           (ControlID
           ,ControlTextEN
           ,ControlTextBG
           ,ControlTextDE)
     VALUES
           ('ButtonPlotChart'
           ,'Plot'
           ,'Изчертай'
           ,'Zeichnen')

INSERT INTO dbo.tbLanguage
           (ControlID
           ,ControlTextEN
           ,ControlTextBG
           ,ControlTextDE)
     VALUES
           ('ButtonRotateLeft'
           ,'Rotate left'
           ,'Завърти наляво'
           ,'Links drehen')

INSERT INTO dbo.tbLanguage
           (ControlID
           ,ControlTextEN
           ,ControlTextBG
           ,ControlTextDE)
     VALUES
           ('ButtonRotateRight'
           ,'Rotate right'
           ,'Завърти надясно'
           ,'Rechts drehen')
		   
		  

UPDATE tbLanguage SET 
ControlTextBG = 'Експорт PDF/Excel',
ControlTextEN = 'Export PDF/Excel',
ControlTextDE = 'Exportieren PDF/Excel'
WHERE ControlID = 'ButtonAnnualReportForExport'

UPDATE tbLanguage SET 
ControlTextBG = 'Разходи/Бюджети/Спестен.',
ControlTextEN = 'Expenses/Income/Savings',
ControlTextDE = 'Kosten/Budget/Ersparnisse'
WHERE ControlID = 'ButtonAnnualSummary'

UPDATE tbLanguage SET 
ControlTextBG = 'Похарчено по категория',
ControlTextEN = 'Spent by category',
ControlTextDE = 'Ausgegeben nach Kategorie'
WHERE ControlID = 'ButtonAnnualSummaryPerCategory'
