CREATE TABLE [dbo].[ManagerGameWeek](
    [ManagerGameWeekId] [uniqueidentifier] NOT NULL DEFAULT newid(),
    [ManagerName] [nvarchar](max) NOT NULL,
    [FantasyTeamName] [nvarchar](max) NOT NULL,
    [FantasyEntryId] [int] NOT NULL,
    --[FantasyResultId] [int] NOT NULL,
    [GameWeek] [int] NOT NULL,
    [ActiveChip] [nvarchar](max) NULL,

    CONSTRAINT [PK_ManagerGameWeek] PRIMARY KEY CLUSTERED([ManagerGameWeekId] ASC)
    )

CREATE TABLE [dbo].[ManagerGameWeekLineup](
    [ManagerGameWeekLineupId] [uniqueidentifier] NOT NULL DEFAULT newid(),
    [FkManagerGameWeekId] [uniqueidentifier] NOT NULL,
    [Multiplier] [int] NOT NULL,
    [IsCaptain] [bit] NOT NULL,
    [IsViceCaptain] [bit] NOT NULL,
    [FantasyPlayerEventId] [int] NOT NULL,

    -- Player data
    [FirstName] [nvarchar](max) NOT NULL,
    [LastName] [nvarchar](max) NOT NULL,
    [WebName] [nvarchar](max) NOT NULL,
    --[FantasyPlayerId] [int] NOT NULL,
    --[Code] [int] NOT NULL,
    [ElementType] [int] NOT NULL,
    --[News] [nvarchar](max) NOT NULL,
    --[NewsAdded] [datetime2](7) NULL,
    --[Photo] [nvarchar](max) NOT NULL,
    --[Status] [nvarchar](max) NOT NULL,

    -- Team data
    [FantasyTeamId] [int] NOT NULL,
    [FantasyTeamCode] [int] NOT NULL,
    [TeamName] [nvarchar](max) NOT NULL,
    [TeamShortName] [nvarchar](max) NOT NULL,

    -- GameWeek data
    [Minutes] [int] NOT NULL,
    [GoalsScored] [int] NOT NULL,
    [Assists] [int] NOT NULL,
    [CleanSheets] [int] NOT NULL,
    [GoalsConceded] [int] NOT NULL,
    [OwnGoals] [int] NOT NULL,
    [PenaltiesSaved] [int] NOT NULL,
    [PenaltiesMissed] [int] NOT NULL,
    [YellowCards] [int] NOT NULL,
    [RedCards] [int] NOT NULL,
    [Saves] [int] NOT NULL,
    [Bonus] [int] NOT NULL,
    [Bps] [int] NOT NULL,
    [Influence] [nvarchar](max) NOT NULL,
    [Creativity] [nvarchar](max) NOT NULL,
    [Threat] [nvarchar](max) NOT NULL,
    [IctIndex] [nvarchar](max) NOT NULL,
    [TotalPoints] [int] NOT NULL,
    [InDreamteam] [bit] NOT NULL,
    --[Selected] [int] NULL,
    --[TransferBalance] [int] NULL,
    --[TransfersIn] [int] NULL,
    --[TransfersOut] [int] NULL,
    --[Value] [decimal](18, 2) NULL,

    CONSTRAINT [PK_ManagerGameWeekLineup] PRIMARY KEY CLUSTERED([ManagerGameWeekLineupId] ASC),
    CONSTRAINT FK_ManagerGameWeek_UserGameWeekLineup FOREIGN KEY (FkManagerGameWeekId) REFERENCES ManagerGameWeek(ManagerGameWeekId)
    )
