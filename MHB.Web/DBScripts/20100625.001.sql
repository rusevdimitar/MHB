
insert into tblanguage values ('ButtonSaveCurrentLanguage', 'Save new language', 'Запази език', 'Sprache speichern')
insert into tblanguage values ('LabelCurrentLanguage', 'Choose language:', 'Изберете език:', 'Sprache einstellen:')


/****** Object:  Table [dbo].[tbLanguages]    Script Date: 06/25/2010 15:00:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbLanguages](
	[Language] [int] NOT NULL,
	[LanguageName] [nvarchar](50) NULL,
 CONSTRAINT [PK_tbLanguages] PRIMARY KEY CLUSTERED 
(
	[Language] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
