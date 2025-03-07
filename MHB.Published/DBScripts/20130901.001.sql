CREATE TABLE tbProductParameterType
(
	ID INT NOT NULL PRIMARY KEY,
	Name NVARCHAR(50) NOT NULL
)

INSERT INTO tbProductParameterType (ID, Name) VALUES (0, 'Default')

INSERT INTO tbProductParameterType (ID, Name) VALUES (1, 'Milage')

ALTER TABLE tbProductParameters ADD ProductParameterType INT NOT NULL FOREIGN KEY REFERENCES tbProductParameterType(ID)