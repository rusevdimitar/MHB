USE [Test01Db]
GO

/****** Object:  Table [dbo].[tbMainTableTemp]    Script Date: 10/28/2011 17:27:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[tbMainTableTemp](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[Month] [int] NULL,
	[Year] [int] NULL,
	[FieldName] [nvarchar](50) NULL,
	[FieldDescription] [nvarchar](500) NULL,
	[FieldValue] [money] NULL,
	[FieldExpectedValue] [money] NULL,
	[DueDate] [datetime] NULL,
	[DateRecordUpdated] [datetime] NULL,
	[IsPaid] [bit] NULL,
	[HasDetails] [bit] NULL,
	[Attachment] [varbinary](max) NULL,
	[AttachmentFileType] [nchar](5) NULL,
	[HasAttachment] [bit] NULL,
	[OrderID] [int] NULL,
	[CostCategory] [int] NULL,
	[Notified] [bit] NULL,
	[NotificationDate] [datetime] NULL,
	[Flagged] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[FieldOldValue] [money] NOT NULL,
 CONSTRAINT [PK_tbMainTableTemp] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[tbMainTableTemp] ADD  CONSTRAINT [DF_tbMainTableTemp_Notified]  DEFAULT ((0)) FOR [Notified]
GO

ALTER TABLE [dbo].[tbMainTableTemp] ADD  CONSTRAINT [DF_tbMainTableTemp_Flagged]  DEFAULT ((0)) FOR [Flagged]
GO

ALTER TABLE [dbo].[tbMainTableTemp] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO

ALTER TABLE [dbo].[tbMainTableTemp] ADD  DEFAULT ((0)) FOR [FieldOldValue]
GO


