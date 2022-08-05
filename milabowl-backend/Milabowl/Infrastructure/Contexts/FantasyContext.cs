using Microsoft.EntityFrameworkCore;
using Milabowl.Infrastructure.Models;

namespace Milabowl.Infrastructure.Contexts
{
    public class FantasyContext: DbContext
    {
        public FantasyContext(DbContextOptions<FantasyContext> options): base(options) { }

        public DbSet<Player> Players { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Fixture> Fixtures { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<League> Leagues { get; set; }
        public DbSet<UserLeague> UserLeagues { get; set; }
        public DbSet<PlayerEvent> PlayerEvents { get; set; }
        public DbSet<UserHeadToHeadEvent> UserHeadToHeadEvents { get; set; }
        public DbSet<Lineup> Lineups { get; set; }
        public DbSet<PlayerEventLineup> PlayerEventLineups { get; set; }
        public DbSet<MilaGWScore> MilaGWScores { get; set; }
        public DbSet<MilaTotalScore> MilaTotalScores { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Team>()
                .HasMany(t => t.Players)
                .WithOne(p => p.Team)
                .HasForeignKey(t => t.FkTeamId);

            modelBuilder.Entity<Team>()
                .HasMany(t => t.AwayFixtures)
                .WithOne(p => p.TeamAway)
                .HasForeignKey(t => t.FkTeamAwayId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Team>()
                .HasMany(t => t.HomeFixtures)
                .WithOne(p => p.TeamHome)
                .HasForeignKey(t => t.FkTeamHomeId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Player>()
                .HasMany(p => p.PlayerEvents)
                .WithOne(pe => pe.Player)
                .HasForeignKey(pe => pe.FkPlayerId);

            modelBuilder.Entity<Event>()
                .HasMany(p => p.PlayerEvents)
                .WithOne(pe => pe.Event)
                .HasForeignKey(pe => pe.FkEventId);

            modelBuilder.Entity<Event>()
                .HasMany(p => p.PlayerHeadToHeadEvents)
                .WithOne(pe => pe.Event)
                .HasForeignKey(pe => pe.FkEventId);

            modelBuilder.Entity<Event>()
                .HasMany(e => e.Lineups)
                .WithOne(l => l.Event)
                .HasForeignKey(l => l.FkEventId);

            modelBuilder.Entity<Event>()
                .HasMany(e => e.Fixtures)
                .WithOne(l => l.Event)
                .HasForeignKey(l => l.FkEventId);

            modelBuilder.Entity<League>()
                .HasMany(l => l.UserLeagues)
                .WithOne(ul => ul.League)
                .HasForeignKey(ul => ul.FkLeagueId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.UserLeagues)
                .WithOne(ul => ul.User)
                .HasForeignKey(ul => ul.FkUserId);

            modelBuilder.Entity<User>()
                .HasMany(p => p.HeadToHeadEvents)
                .WithOne(pe => pe.User)
                .HasForeignKey(pe => pe.FkUserId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Lineups)
                .WithOne(l => l.User)
                .HasForeignKey(l => l.FkUserId);

            modelBuilder.Entity<Lineup>()
                .HasMany(l => l.PlayerEventLineups)
                .WithOne(pel => pel.Lineup)
                .HasForeignKey(pel => pel.FkLineupId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<PlayerEvent>()
                .HasMany(pe => pe.PlayerEventLineups)
                .WithOne(pel => pel.PlayerEvent)
                .HasForeignKey(pel => pel.FkPlayerEventId)
                .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<UserHeadToHeadEvent>();
        }
    }
}
