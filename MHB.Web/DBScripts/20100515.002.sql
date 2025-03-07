

/****** Object:  Table [dbo].[tbControlTypes]    Script Date: 05/15/2010 23:28:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tbControlTypes](
	[ID] [int] NOT NULL,
	[ControlType] [nchar](100) NULL,
	[ControlTypeText] [nchar](100) NULL,
 CONSTRAINT [PK_tbControlTypes] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


/****** Object:  Table [dbo].[tbCustomPageControls]    Script Date: 05/15/2010 23:28:30 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tbCustomPageControls](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[ControlID] [nvarchar](100) NULL,
	[ControlTypeID] [int] NULL,
	[ControlValue] [nchar](300) NULL,
	[Calculable] [bit] NULL,
	[Selected] [bit] NULL,
	[Label] [nvarchar](100) NULL,
 CONSTRAINT [PK_tbCustomPageControls] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tbCustomPageControls]  WITH CHECK ADD  CONSTRAINT [FK_tbCustomPageControls_tbControlTypes] FOREIGN KEY([ControlTypeID])
REFERENCES [dbo].[tbControlTypes] ([ID])
GO

ALTER TABLE [dbo].[tbCustomPageControls] CHECK CONSTRAINT [FK_tbCustomPageControls_tbControlTypes]
GO

ALTER TABLE [dbo].[tbCustomPageControls]  WITH CHECK ADD  CONSTRAINT [FK_tbCustomPageControls_tbUsers] FOREIGN KEY([UserID])
REFERENCES [dbo].[tbUsers] ([userID])
GO

ALTER TABLE [dbo].[tbCustomPageControls] CHECK CONSTRAINT [FK_tbCustomPageControls_tbUsers]
GO

ALTER TABLE [dbo].[tbCustomPageControls] ADD  CONSTRAINT [DF_tbCustomPageControls_Calculable]  DEFAULT ((0)) FOR [Calculable]
GO

ALTER TABLE [dbo].[tbCustomPageControls] ADD  CONSTRAINT [DF_tbCustomPageControls_Selected]  DEFAULT ((0)) FOR [Selected]
GO

ALTER TABLE [dbo].[tbCustomPageControls] ADD  CONSTRAINT [DF_tbCustomPageControls_Label]  DEFAULT ('') FOR [Label]
GO


/****** Object:  Table [dbo].[tbCustomListItems]    Script Date: 05/15/2010 23:28:56 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tbCustomListItems](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ControlID] [int] NULL,
	[ListItemValue] [nvarchar](100) NULL,
	[ListItemText] [nvarchar](100) NULL,
	[Selected] [bit] NULL,
 CONSTRAINT [PK_tbCustomListItems] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tbCustomListItems]  WITH CHECK ADD  CONSTRAINT [FK_tbCustomListItems_tbCustomPageControls] FOREIGN KEY([ControlID])
REFERENCES [dbo].[tbCustomPageControls] ([ID])
GO

ALTER TABLE [dbo].[tbCustomListItems] CHECK CONSTRAINT [FK_tbCustomListItems_tbCustomPageControls]
GO

ALTER TABLE [dbo].[tbCustomListItems] ADD  CONSTRAINT [DF_tbCustomListItems_Selected]  DEFAULT ((0)) FOR [Selected]
GO

