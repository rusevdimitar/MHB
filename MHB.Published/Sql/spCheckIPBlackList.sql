USE [Test01Db]
GO
/****** Object:  StoredProcedure [dbo].[spCheckIPBlacklisted]    Script Date: 12/24/2016 10:06:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:			Rusev, Dimitar
-- Create date:		2012-12-18
-- Modified date:	2016-12-24
-- Description:		Checks whether an user's ip address and/or user id is in the black list
-- =============================================
ALTER PROCEDURE [dbo].[spCheckIPBlacklisted]
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
	
	IF @userID IS NULL	
		SELECT 0	
	ELSE			
		SELECT 1	

END
