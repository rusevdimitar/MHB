
/****** Object:  Table [dbo].[tbActionLog]    Script Date: 01/22/2010 20:41:38 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tbActionLog](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[logAction] [int] NOT NULL,
	[logUserID] [int] NULL,
	[logDate] [datetime] NULL,
	[logMessage] [nvarchar](200) NULL,
	[logIP] [nvarchar](50) NULL,
 CONSTRAINT [PK_tbActionLog] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO