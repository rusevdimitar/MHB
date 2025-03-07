USE [100_Test01Db]
GO


-- =============================================
-- Author:		Dimitar Rusev
-- Create date: 09.07.2011
-- Modified date: 
-- Description:	New categories table
-- =============================================


USE Test01Db


CREATE TABLE dbo.tbCategories
(
	ID int NOT NULL,
	CategoryName NVARCHAR(70) NOT NULL DEFAULT '',
	IconPath NVARCHAR(300) NOT NULL DEFAULT '',
	IsPayIconVisible BIT NOT NULL DEFAULT 0,
	UserCategoryID INT NOT NULL DEFAULT 0
 )

ALTER TABLE dbo.tbCategories 

ADD CONSTRAINT PK_CATEGORY PRIMARY KEY (ID)



INSERT INTO dbo.tbCategories
           (ID
           ,CategoryName
           ,IconPath
           ,IsPayIconVisible)
     VALUES
           (1, 'Fuel', '../Images/gas_pump.png', 0)
           
INSERT INTO dbo.tbCategories
           (ID
           ,CategoryName
           ,IconPath
           ,IsPayIconVisible)
     VALUES
           (2, 'Electricity', '../Images/electricity_icon.jpg', 1)           

INSERT INTO dbo.tbCategories
           (ID
           ,CategoryName
           ,IconPath
           ,IsPayIconVisible)
     VALUES
           (3, 'Phone', '../Images/phone_icon.jpg', 1)   
           
INSERT INTO dbo.tbCategories
           (ID
           ,CategoryName
           ,IconPath
           ,IsPayIconVisible)
     VALUES
           (4, 'Rent', '../Images/rent_icon.jpg', 0)             

INSERT INTO dbo.tbCategories
           (ID
           ,CategoryName
           ,IconPath
           ,IsPayIconVisible)
     VALUES
           (5, 'Savings', '../Images/pig_icon.png', 0)    
           
INSERT INTO dbo.tbCategories
           (ID
           ,CategoryName
           ,IconPath
           ,IsPayIconVisible)
     VALUES
           (6, 'Internet', '../Images/internet_category_icon.gif', 1)             
           
INSERT INTO dbo.tbCategories
           (ID
           ,CategoryName
           ,IconPath
           ,IsPayIconVisible)
     VALUES
           (7, 'Food', '../Images/apple.gif', 0)                  
           
INSERT INTO dbo.tbCategories
           (ID
           ,CategoryName
           ,IconPath
           ,IsPayIconVisible)
     VALUES
           (8, 'Car', '../Images/car_sedan_green.gif', 0)       
           
INSERT INTO dbo.tbCategories
           (ID
           ,CategoryName
           ,IconPath
           ,IsPayIconVisible)
     VALUES
           (9, 'Loan', '../Images/loan.gif', 1)      
           
INSERT INTO dbo.tbCategories
           (ID
           ,CategoryName
           ,IconPath
           ,IsPayIconVisible)
     VALUES
           (10, 'Medical', '../Images/doctor_icon.gif', 1)                         
           
           
ALTER TABLE dbo.tbCostCategories

ADD CONSTRAINT FK_CATID FOREIGN KEY (CostCategoryID) REFERENCES tbCategories(ID)           


--ALTER TABLE tbCostCategories
--DROP CONSTRAINT FK_CATID


