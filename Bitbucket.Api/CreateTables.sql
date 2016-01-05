
CREATE TABLE [dbo].[Branch](
	[Name] [nvarchar](500) NULL,
	[Author] [nvarchar](500) NULL,
	[LastModifiedDateTime] [datetime] NULL,
	[LastModifiedMessage] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


CREATE TABLE [dbo].[Commit](
	[Author] [nvarchar](500) NULL,
	[Date] [datetime] NULL,
	[Message] [nvarchar](max) NULL,
	[Hash] [char](64) NULL,
	[Branch] [nvarchar](500) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

CREATE TABLE [dbo].[PullRequest](
	[Title] [nvarchar](500) NULL,
	[Description] [nvarchar](max) NULL,
	[Author] [nvarchar](500) NULL,
	[State] [nvarchar](20) NULL,
	[LastUpdateDateTime] [datetime] NULL,
	[Branch] [nvarchar](500) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO