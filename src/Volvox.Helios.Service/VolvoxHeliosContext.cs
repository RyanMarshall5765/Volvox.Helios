﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Volvox.Helios.Core.Modules.StreamAnnouncer;
using Volvox.Helios.Domain.Module;
using Volvox.Helios.Domain.Module.ChatTracker;
using Volvox.Helios.Domain.ModuleSettings;

namespace Volvox.Helios.Service
{
    public class VolvoxHeliosContext : DbContext
    {
        public VolvoxHeliosContext(DbContextOptions options)
            : base(options)
        {
        }

        #region Streamer

        public DbSet<StreamerSettings> StreamerSettings { get; set; }

        public DbSet<StreamerChannelSettings> StreamerChannelSettings { get; set; }

        public DbSet<StreamAnnouncerMessage> StreamAnnouncerMessages { get; set;  }

        #endregion

        public DbSet<PollSettings> PollSettings { get; set; }

        #region ChatTracker

        public DbSet<ChatTrackerSettings> ChatTrackerSettings { get; set; }

        public DbSet<Message> Messages { get; set; }

        #endregion

        #region Reminder
        public DbSet<RemembotSettings> ReminderSettings { get; set; }
        public DbSet<RecurringReminderMessage> RecurringReminderMessages { get; set; }
        #endregion

        #region Reaction Roles
        public DbSet<ReactionRoleSettings> ReactionRoleSettings { get; set; }
        public DbSet<ReactionRolesMessage> ReactionRoleMessages { get; set; }
        public DbSet<ReactionRolesEmoteMapping> ReactionRoleEmoteMappings { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            SetupForReminderSchema(modelBuilder);
            SetupForReactionRoleSchema(modelBuilder);
        }

        private void SetupForReactionRoleSchema(ModelBuilder modelBuilder)
        {
            var settingsModel = modelBuilder.Entity<ReactionRoleSettings>();
            var reactionRoleMessageModel = modelBuilder.Entity<ReactionRolesMessage>();
            var emoteRoleMappingModel = modelBuilder.Entity<ReactionRolesEmoteMapping>();

            reactionRoleMessageModel.HasKey(x => x.Id)
                .ForSqlServerIsClustered();

            reactionRoleMessageModel.Property(x => x.Id)
                .UseSqlServerIdentityColumn();

            reactionRoleMessageModel.HasIndex(x => new
            {
                x.GuildId,
                x.MessageId,
                x.ChannelId
            });

            reactionRoleMessageModel
                .HasMany(x => x.RollMappings);

            emoteRoleMappingModel.HasKey(x => x.Id)
                .ForSqlServerIsClustered();

            emoteRoleMappingModel.Property(x => x.Id)
                .UseSqlServerIdentityColumn();

            emoteRoleMappingModel.HasIndex(x => x.ReactionRoleMessageId);
        }

        private void SetupForReminderSchema(ModelBuilder modelBuilder)
        {
            var reminderModel = modelBuilder.Entity<RemembotSettings>();
            var recurringReminderModel = modelBuilder.Entity<RecurringReminderMessage>();

            reminderModel.HasMany(x => x.RecurringReminders);

            recurringReminderModel.HasKey(x => x.Id)
                .ForSqlServerIsClustered();

            recurringReminderModel.Property(x => x.Id)
                .UseSqlServerIdentityColumn();

            recurringReminderModel
                .HasIndex(x => x.GuildId);

            recurringReminderModel.Property(x => x.Enabled)
                .IsRequired();

            recurringReminderModel.Property(x => x.ChannelId)
                .IsRequired();

            recurringReminderModel.Property(x => x.Message)
                .IsRequired();

            recurringReminderModel.Property(x => x.CronExpression)
                .HasMaxLength(255)
                .IsRequired();

            recurringReminderModel.Property(x => x.Fault)
                .HasDefaultValue(RecurringReminderMessage.FaultType.None)
                .IsRequired();

            modelBuilder.Entity<Message>()
                .HasIndex(m => m.GuildId);
        }
    }
}