CREATE TABLE [dbo].[Events](
    [EventId] [uniqueidentifier] NOT NULL,
    [FantasyEventId] [int] NOT NULL,
    [Deadline] [datetime2](7) NOT NULL,
    [Finished] [bit] NOT NULL,
    [DataChecked] [bit] NOT NULL,
    [Name] [nvarchar](max) NOT NULL,
    [GameWeek] [int] NOT NULL,
    [MostCaptainedPlayerID] [int] NULL,
    [MostSelectedPlayerID] [int] NULL,
    [MostTransferredInPlayerID] [int] NULL,
    [MostViceCaptainedPlayerID] [int] NULL,
    CONSTRAINT [PK_Events] PRIMARY KEY CLUSTERED([EventId] ASC)

CREATE TABLE [dbo].[Fixtures](
    [FixtureId] [uniqueidentifier] NOT NULL,
    [Finished] [bit] NOT NULL,
    [FinishedProvisional] [bit] NOT NULL,
    [FantasyFixtureId] [int] NOT NULL,
    [KickoffTime] [datetime2](7) NULL,
    [Minutes] [int] NOT NULL,
    [ProvisionalStartTime] [bit] NOT NULL,
    [Started] [bit] NULL,
    [TeamAwayScore] [int] NULL,
    [TeamHomeScore] [int] NULL,
    [TeamHomeDifficulty] [int] NOT NULL,
    [TeamAwayDifficulty] [int] NOT NULL,
    [FkTeamAwayId] [uniqueidentifier] NOT NULL,
    [FkTeamHomeId] [uniqueidentifier] NOT NULL,
    [FkEventId] [uniqueidentifier] NOT NULL,
    CONSTRAINT [PK_Fixtures] PRIMARY KEY CLUSTERED([FixtureId] ASC)

CREATE TABLE [dbo].[Leagues](
    [LeagueId] [uniqueidentifier] NOT NULL,
    [FantasyLeagueId] [int] NOT NULL,
    [Name] [nvarchar](max) NOT NULL,
    [Created] [datetime2](7) NOT NULL,
    [Closed] [bit] NOT NULL,
    [LeagueType] [nvarchar](max) NOT NULL,
    [Scoring] [nvarchar](max) NOT NULL,
    [AdminEntry] [int] NOT NULL,
    [StartEvent] [int] NOT NULL,
    [CodePrivacy] [nvarchar](max) NOT NULL,
    CONSTRAINT [PK_Leagues] PRIMARY KEY CLUSTERED([LeagueId] ASC)

CREATE TABLE [dbo].[Lineups](
    [LineupId] [uniqueidentifier] NOT NULL,
    [FkEventId] [uniqueidentifier] NOT NULL,
    [FkUserId] [uniqueidentifier] NOT NULL,
    [ActiveChip] [nvarchar](max) NULL,
    CONSTRAINT [PK_Lineups] PRIMARY KEY CLUSTERED([LineupId] ASC)

CREATE TABLE [dbo].[PlayerEventLineups](
    [PlayerEventLineupId] [uniqueidentifier] NOT NULL,
    [FkPlayerEventId] [uniqueidentifier] NOT NULL,
    [FkLineupId] [uniqueidentifier] NOT NULL,
    [Multiplier] [int] NOT NULL,
    [IsCaptain] [bit] NOT NULL,
    [IsViceCaptain] [bit] NOT NULL,
    CONSTRAINT [PK_PlayerEventLineups] PRIMARY KEY CLUSTERED([PlayerEventLineupId] ASC)

CREATE TABLE [dbo].[PlayerEvents](
    [PlayerEventId] [uniqueidentifier] NOT NULL,
    [FantasyPlayerEventId] [int] NOT NULL,
    [FkPlayerId] [uniqueidentifier] NOT NULL,
    [FkEventId] [uniqueidentifier] NOT NULL,
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
    [Selected] [int] NULL,
    [TransferBalance] [int] NULL,
    [TransfersIn] [int] NULL,
    [TransfersOut] [int] NULL,
    [Value] [decimal](18, 2) NULL,
    CONSTRAINT [PK_PlayerEvents] PRIMARY KEY CLUSTERED([PlayerEventId] ASC)

CREATE TABLE [dbo].[Players](
    [PlayerId] [uniqueidentifier] NOT NULL,
    [FirstName] [nvarchar](max) NOT NULL,
    [LastName] [nvarchar](max) NOT NULL,
    [NowCost] [int] NOT NULL,
    [FantasyPlayerId] [int] NOT NULL,
    [FkTeamId] [uniqueidentifier] NOT NULL,
    [Code] [int] NOT NULL,
    [ElementType] [int] NOT NULL,
    [EventPoints] [int] NOT NULL,
    [Form] [nvarchar](max) NOT NULL,
    [InDreamteam] [bit] NOT NULL,
    [News] [nvarchar](max) NOT NULL,
    [NewsAdded] [datetime2](7) NULL,
    [Photo] [nvarchar](max) NOT NULL,
    [PointsPerGame] [nvarchar](max) NOT NULL,
    [SelectedByPercent] [nvarchar](max) NOT NULL,
    [Special] [bit] NOT NULL,
    [Status] [nvarchar](max) NOT NULL,
    [TotalPoints] [int] NOT NULL,
    [TransfersIn] [int] NOT NULL,
    [TransfersInEvent] [int] NOT NULL,
    [TransfersOut] [int] NOT NULL,
    [TransfersOutEvent] [int] NOT NULL,
    [ValueForm] [nvarchar](max) NOT NULL,
    [ValueSeason] [nvarchar](max) NOT NULL,
    [WebName] [nvarchar](max) NOT NULL,
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
    CONSTRAINT [PK_Players] PRIMARY KEY CLUSTERED([PlayerId] ASC)

CREATE TABLE [dbo].[Teams](
    [TeamId] [uniqueidentifier] NOT NULL,
    [FantasyTeamId] [int] NOT NULL,
    [FantasyTeamCode] [int] NOT NULL,
    [TeamName] [nvarchar](max) NOT NULL,
    [TeamShortName] [nvarchar](max) NOT NULL,
    CONSTRAINT [PK_Teams] PRIMARY KEY CLUSTERED([TeamId] ASC)

CREATE TABLE [dbo].[UserHeadToHeadEvents](
    [UserHeadToHeadEventID] [uniqueidentifier] NOT NULL,
    [FantasyUserHeadToHeadEventID] [int] NOT NULL,
    [FkUserId] [uniqueidentifier] NOT NULL,
    [Points] [int] NOT NULL,
    [Win] [int] NOT NULL,
    [Draw] [int] NOT NULL,
    [Loss] [int] NOT NULL,
    [Total] [int] NOT NULL,
    [FkEventId] [uniqueidentifier] NOT NULL,
    [IsKnockout] [bit] NOT NULL,
    [LeagueID] [int] NOT NULL,
    [IsBye] [bit] NOT NULL,
     CONSTRAINT [PK_UserHeadToHeadEvents] PRIMARY KEY CLUSTERED([UserHeadToHeadEventID] ASC)

CREATE TABLE [dbo].[UserHistory](
    [UserHistoryId] [uniqueidentifier] NOT NULL,
    [FkUserId] [uniqueidentifier] NOT NULL,
    [SeasonName] [nvarchar](max) NOT NULL,
    [TotalPoints] [int] NOT NULL,
    [Rank] [int] NOT NULL,
    CONSTRAINT [PK_UserHistory] PRIMARY KEY CLUSTERED([UserHistoryId] ASC)

CREATE TABLE [dbo].[UserLeagues](
    [UserLeagueId] [uniqueidentifier] NOT NULL,
    [FkUserId] [uniqueidentifier] NOT NULL,
    [FkLeagueId] [uniqueidentifier] NOT NULL,
     CONSTRAINT [PK_UserLeagues] PRIMARY KEY CLUSTERED([UserLeagueId] ASC)

CREATE TABLE [dbo].[Users](
    [UserId] [uniqueidentifier] NOT NULL,
    [UserName] [nvarchar](max) NOT NULL,
    [EntryName] [nvarchar](max) NOT NULL,
    [FantasyEntryId] [int] NOT NULL,
    [FantasyResultId] [int] NOT NULL,
    [Rank] [int] NOT NULL,
    [LastRank] [int] NOT NULL,
    [EventTotal] [int] NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED([UserId] ASC)

ALTER TABLE [dbo].[Fixtures]  WITH CHECK ADD  CONSTRAINT [FK_Fixtures_Events_FkEventId] FOREIGN KEY([FkEventId])
    REFERENCES [dbo].[Events] ([EventId])

ALTER TABLE [dbo].[Fixtures]  WITH CHECK ADD  CONSTRAINT [FK_Fixtures_Teams_FkTeamAwayId] FOREIGN KEY([FkTeamAwayId])
    REFERENCES [dbo].[Teams] ([TeamId])

ALTER TABLE [dbo].[Fixtures]  WITH CHECK ADD  CONSTRAINT [FK_Fixtures_Teams_FkTeamHomeId] FOREIGN KEY([FkTeamHomeId])
    REFERENCES [dbo].[Teams] ([TeamId])

ALTER TABLE [dbo].[Lineups]  WITH CHECK ADD  CONSTRAINT [FK_Lineups_Events_FkEventId] FOREIGN KEY([FkEventId])
    REFERENCES [dbo].[Events] ([EventId])

ALTER TABLE [dbo].[Lineups]  WITH CHECK ADD  CONSTRAINT [FK_Lineups_Users_FkUserId] FOREIGN KEY([FkUserId])
    REFERENCES [dbo].[Users] ([UserId])

ALTER TABLE [dbo].[PlayerEventLineups]  WITH CHECK ADD  CONSTRAINT [FK_PlayerEventLineups_Lineups_FkLineupId] FOREIGN KEY([FkLineupId])
    REFERENCES [dbo].[Lineups] ([LineupId])

ALTER TABLE [dbo].[PlayerEventLineups]  WITH CHECK ADD  CONSTRAINT [FK_PlayerEventLineups_PlayerEvents_FkPlayerEventId] FOREIGN KEY([FkPlayerEventId])
    REFERENCES [dbo].[PlayerEvents] ([PlayerEventId])

ALTER TABLE [dbo].[PlayerEvents]  WITH CHECK ADD  CONSTRAINT [FK_PlayerEvents_Events_FkEventId] FOREIGN KEY([FkEventId])
    REFERENCES [dbo].[Events] ([EventId])

ALTER TABLE [dbo].[PlayerEvents]  WITH CHECK ADD  CONSTRAINT [FK_PlayerEvents_Players_FkPlayerId] FOREIGN KEY([FkPlayerId])
    REFERENCES [dbo].[Players] ([PlayerId])

ALTER TABLE [dbo].[Players]  WITH CHECK ADD  CONSTRAINT [FK_Players_Teams_FkTeamId] FOREIGN KEY([FkTeamId])
    REFERENCES [dbo].[Teams] ([TeamId])

ALTER TABLE [dbo].[UserHeadToHeadEvents]  WITH CHECK ADD  CONSTRAINT [FK_UserHeadToHeadEvents_Events_FkEventId] FOREIGN KEY([FkEventId])
    REFERENCES [dbo].[Events] ([EventId])

ALTER TABLE [dbo].[UserHeadToHeadEvents]  WITH CHECK ADD  CONSTRAINT [FK_UserHeadToHeadEvents_Users_FkUserId] FOREIGN KEY([FkUserId])
    REFERENCES [dbo].[Users] ([UserId])

ALTER TABLE [dbo].[UserHistory]  WITH CHECK ADD  CONSTRAINT [FK_UserHistory_Users_FkUserId] FOREIGN KEY([FkUserId])
    REFERENCES [dbo].[Users] ([UserId])

ALTER TABLE [dbo].[UserLeagues]  WITH CHECK ADD  CONSTRAINT [FK_UserLeagues_Leagues_FkLeagueId] FOREIGN KEY([FkLeagueId])
    REFERENCES [dbo].[Leagues] ([LeagueId])

ALTER TABLE [dbo].[UserLeagues]  WITH CHECK ADD  CONSTRAINT [FK_UserLeagues_Users_FkUserId] FOREIGN KEY([FkUserId])
    REFERENCES [dbo].[Users] ([UserId])

CREATE TABLE [dbo].[MilaGWScores](
    [MilaGWScoreId] [uniqueidentifier] NOT NULL,
    [GW] [nvarchar](max) NOT NULL,
    [TeamName] [nvarchar](max) NOT NULL,
    [CapFail] [decimal](18, 2) NOT NULL,
    [BenchFail] [decimal](18, 2) NOT NULL,
    [CapKeep] [decimal](18, 2) NOT NULL,
    [CapDef] [decimal](18, 2) NOT NULL,
    [GWPosition] [decimal](18, 2) NOT NULL,
    [GW69] [decimal](18, 2) NOT NULL,
    [RedCard] [decimal](18, 2) NOT NULL,
    [YellowCard] [decimal](18, 2) NOT NULL,
    [MinusIsPlus] [decimal](18, 2) NOT NULL,
    [IncreaseStreak] [decimal](18, 2) NOT NULL,
    [EqualStreak] [decimal](18, 2) NOT NULL,
    [GWScore] [decimal](18, 2) NOT NULL,
    [GameWeek] [int] NOT NULL,
    [UserName] [nvarchar](max) NOT NULL,
    [GWPositionScore] [decimal](18, 2) NOT NULL,
    [MilaPoints] [decimal](18, 2) NOT NULL,
    [HeadToHeadMeta] [decimal](18, 2) NOT NULL,
    [SixtyNineSub] [decimal](18, 2) NOT NULL,
    [UniqueCap] [decimal](18, 2) NOT NULL,
    [TrendyBitch] [decimal](18, 2) NOT NULL,
    [ActiveChip] [nvarchar](max) NULL,
    [RedShell] [decimal](18, 2) NOT NULL,
    [GreenShell] [decimal](18, 2) NOT NULL,
    [Mushroom] [decimal](18, 2) NOT NULL,
    [BombPoints] [decimal](18, 2) NOT NULL,
    [BombState] [nvarchar](100) NULL,
    [Banana] [decimal](18, 2) NOT NULL,
    [UserId] [int] NOT NULL,
    [DarthMaulPoints] [decimal](18, 2) NOT NULL,
    [IsDarthMaul] [bit] NULL,
    [IsDarthMaulContender] [bit] NULL,
    [MissedPenalties] [decimal](18, 2) NOT NULL,
    [Sellout] [decimal](18, 2) NOT NULL,
    CONSTRAINT [PK_MilaGWScores] PRIMARY KEY CLUSTERED
(
[MilaGWScoreId] ASC
)
