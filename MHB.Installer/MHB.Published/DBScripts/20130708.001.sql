CREATE TABLE tbMeasureTypes
(
	ID INT NOT NULL PRIMARY KEY,
	Name NVARCHAR(200) NOT NULL DEFAULT '',
	[Description] NVARCHAR(400),
	IsDeleted BIT NOT NULL DEFAULT 0
)

INSERT INTO tbMeasureTypes (ID, Name, [Description], IsDeleted) VALUES (1, 'Weight', '', 0)
INSERT INTO tbMeasureTypes (ID, Name, [Description], IsDeleted) VALUES (2, 'Volume', '', 0)
INSERT INTO tbMeasureTypes (ID, Name, [Description], IsDeleted) VALUES (3, 'Count', '', 0)

-- I messed the data type so I had to drop the Amount column and recreate it; It has to by of a Money data type as will have decimal values for the weight measurements;

--ALTER TABLE tbDetailsTable01 ALTER COLUMN Amount INT NULL
--ALTER TABLE tbDetailsTable02 ALTER COLUMN Amount INT NULL
--ALTER TABLE tbDetailsTable03 ALTER COLUMN Amount INT NULL
--ALTER TABLE tbDetailsTable04 ALTER COLUMN Amount INT NULL
--ALTER TABLE tbDetailsTable05 ALTER COLUMN Amount INT NULL

--ALTER TABLE tbDetailsTable01 DROP CONSTRAINT DF__tbDetails__Amoun__26CFC035
--ALTER TABLE tbDetailsTable02 DROP CONSTRAINT DF__tbDetails__Amoun__27C3E46E
--ALTER TABLE tbDetailsTable03 DROP CONSTRAINT DF__tbDetails__Amoun__28B808A7
--ALTER TABLE tbDetailsTable04 DROP CONSTRAINT DF__tbDetails__Amoun__29AC2CE0
--ALTER TABLE tbDetailsTable05 DROP CONSTRAINT DF__tbDetails__Amoun__2AA05119

--ALTER TABLE tbDetailsTable01 DROP COLUMN Amount
--ALTER TABLE tbDetailsTable02 DROP COLUMN Amount
--ALTER TABLE tbDetailsTable03 DROP COLUMN Amount
--ALTER TABLE tbDetailsTable04 DROP COLUMN Amount
--ALTER TABLE tbDetailsTable05 DROP COLUMN Amount

ALTER TABLE tbDetailsTable01 ADD Amount MONEY NOT NULL DEFAULT 1
ALTER TABLE tbDetailsTable02 ADD Amount MONEY NOT NULL DEFAULT 1
ALTER TABLE tbDetailsTable03 ADD Amount MONEY NOT NULL DEFAULT 1
ALTER TABLE tbDetailsTable04 ADD Amount MONEY NOT NULL DEFAULT 1
ALTER TABLE tbDetailsTable05 ADD Amount MONEY NOT NULL DEFAULT 1

ALTER TABLE tbDetailsTable01 ADD MeasureTypeID INT NOT NULL DEFAULT 3
ALTER TABLE tbDetailsTable02 ADD MeasureTypeID INT NOT NULL DEFAULT 3
ALTER TABLE tbDetailsTable03 ADD MeasureTypeID INT NOT NULL DEFAULT 3
ALTER TABLE tbDetailsTable04 ADD MeasureTypeID INT NOT NULL DEFAULT 3
ALTER TABLE tbDetailsTable05 ADD MeasureTypeID INT NOT NULL DEFAULT 3


ALTER TABLE tbDetailsTable01 WITH CHECK ADD CONSTRAINT [FK_tbDetailsTable01_tbMeasureTypes] FOREIGN KEY(MeasureTypeID)
REFERENCES tbMeasureTypes ([ID])

ALTER TABLE tbDetailsTable02 WITH CHECK ADD CONSTRAINT [FK_tbDetailsTable02_tbMeasureTypes] FOREIGN KEY(MeasureTypeID)
REFERENCES tbMeasureTypes ([ID])

ALTER TABLE tbDetailsTable03 WITH CHECK ADD CONSTRAINT [FK_tbDetailsTable03_tbMeasureTypes] FOREIGN KEY(MeasureTypeID)
REFERENCES tbMeasureTypes ([ID])

ALTER TABLE tbDetailsTable04 WITH CHECK ADD CONSTRAINT [FK_tbDetailsTable04_tbMeasureTypes] FOREIGN KEY(MeasureTypeID)
REFERENCES tbMeasureTypes ([ID])

ALTER TABLE tbDetailsTable05 WITH CHECK ADD CONSTRAINT [FK_tbDetailsTable05_tbMeasureTypes] FOREIGN KEY(MeasureTypeID)
REFERENCES tbMeasureTypes ([ID])