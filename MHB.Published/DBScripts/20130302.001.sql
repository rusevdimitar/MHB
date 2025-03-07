UPDATE tbLanguage SET ControlTextBG = 'Ст.цена' WHERE ControlID = 'StandardCost'
UPDATE tbLanguage SET ControlTextBG = 'Цен.листа' WHERE ControlID = 'ListPrice'


INSERT INTO dbo.tbLanguage (ControlID, ControlTextEN, ControlTextBG, ControlTextDE) VALUES
('CategoryName', 'Category','Категория','Kategorie')

INSERT INTO dbo.tbLanguage (ControlID, ControlTextEN, ControlTextBG, ControlTextDE) VALUES
('Vendor', 'Vendor','Доставчик','Verkäufer')

UPDATE tbProducts SET Color='FFFFFF'