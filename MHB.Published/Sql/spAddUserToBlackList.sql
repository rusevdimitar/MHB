
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
