CREATE TABLE dbo.tbUsersBlackList
(
	ID INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	UserID INT NOT NULL,
	IPAddressV4 BINARY(4),
	IPAddressTextV4 VARCHAR(15) NOT NULL DEFAULT '',
	IPAddressV6 BINARY(16),
	IPAddressTextV6 VARCHAR(39) NOT NULL DEFAULT '',
	DateAdded DATETIME,	
)

ALTER TABLE dbo.tbUsersBlackList 

ADD CONSTRAINT FK_BLACKLISTUSERID FOREIGN KEY (UserID) REFERENCES tbUsers(userID)       

GO


CREATE FUNCTION dbo.fnBinaryIPv4(@ip AS VARCHAR(15)) RETURNS BINARY(4)
AS
BEGIN
    DECLARE @bin AS BINARY(4)

    SELECT @bin = CAST( CAST( PARSENAME( @ip, 4 ) AS INTEGER) AS BINARY(1))
                + CAST( CAST( PARSENAME( @ip, 3 ) AS INTEGER) AS BINARY(1))
                + CAST( CAST( PARSENAME( @ip, 2 ) AS INTEGER) AS BINARY(1))
                + CAST( CAST( PARSENAME( @ip, 1 ) AS INTEGER) AS BINARY(1))

    RETURN @bin
END
go


CREATE FUNCTION [dbo].[fnDisplayIPv4](@ip AS BINARY(4)) RETURNS VARCHAR(15)
AS
BEGIN
    DECLARE @str AS VARCHAR(15) 

    SELECT @str = CAST( CAST( SUBSTRING( @ip, 1, 1) AS INTEGER) AS VARCHAR(3) ) + '.'
                + CAST( CAST( SUBSTRING( @ip, 2, 1) AS INTEGER) AS VARCHAR(3) ) + '.'
                + CAST( CAST( SUBSTRING( @ip, 3, 1) AS INTEGER) AS VARCHAR(3) ) + '.'
                + CAST( CAST( SUBSTRING( @ip, 4, 1) AS INTEGER) AS VARCHAR(3) );

    RETURN @str
END

go



SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Rusev, Dimitar
-- Create date: 2012-12-18
-- Description:	Adds an user to a black list; IP address and User ID;
-- =============================================
CREATE PROCEDURE spAddUserToBlackList
	-- Add the parameters for the stored procedure here
	@UserID INT,
	@IPAddressV4 VARCHAR(15),
	@IPAddressV6 VARCHAR(39)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO tbUsersBlackList (UserID, IPAddressV4, IPAddressTextV4, IPAddressV6, IPAddressTextV6, DateAdded)
	VALUES (@UserID, dbo.fnBinaryIPv4(@IPAddressV4), @IPAddressV4, CAST(@IPAddressV6 AS BINARY), @IPAddressV6, GETDATE())

END
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Rusev, Dimitar
-- Create date: 2012-12-18
-- Description:	Checks whether an user's ip address and/or user id is in the black list
-- =============================================
CREATE PROCEDURE spCheckIPBlacklisted
	-- Add the parameters for the stored procedure here	
	@ipAddressV4 VARCHAR(15),
	@ipAddressV6 VARCHAR(39)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @userID AS INT

	SET @userID = (SELECT UserID FROM dbo.tbUsersBlackList WHERE IPAddressV4 = dbo.fnBinaryIPv4(@ipAddressV4) OR IPAddressV6 = dbo.fnBinaryIPv4(@ipAddressV6))
	
	SELECT ISNULL(@userID, 0) AS UserID

END
GO
