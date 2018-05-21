﻿using Microsoft.EntityFrameworkCore;
using MSLaunches.Data.Models;

namespace MSLaunches.Data.EF
{
    public class WebApiCoreLaunchesContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebApiCoreLaunchesContext"/> class.
        ///
        /// DbContextOptions parameter is required by AspNet core initialization
        /// </summary>
        /// <param name="options">Options used to create this <see cref="WebApiCoreLaunchesContext"/> instance </param>
        public WebApiCoreLaunchesContext(DbContextOptions options) : base(options) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebApiCoreLaunchesContext"/> class without parameters.
        /// </summary>
        public WebApiCoreLaunchesContext() : base() { }

        /// <summary> All users registered on WebApiCoreMSLaunches database</summary>
        public DbSet<User> Users { get; set; }
        public DbSet<Launch> Launches { get; set; }
        public DbSet<LaunchType> LaunchTypes { get; set; }
        public DbSet<DailyLaunch> DailyLaunch { get; set; }
        public DbSet<UserLaunch> UserLaunch { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Table names should be singular but DbSet properties should be plural.
            modelBuilder.Entity<User>()
                        .ToTable("User")
                        .HasIndex(x => x.UserName)
                        .IsUnique();

            modelBuilder.Entity<Launch>()
                        .ToTable("Launch")
                        .HasIndex(x => x.Id)
                        .IsUnique();

            modelBuilder.Entity<LaunchType>()
                        .ToTable("LaunchType")
                        .HasIndex(x => x.LaunchTypeId)
                        .IsUnique();

            modelBuilder.Entity<UserLaunch>()
                        .ToTable("UserLaunch")
                        .HasIndex(x => x.Id)
                        .IsUnique();

            modelBuilder.Entity<DailyLaunch>()
                     .ToTable("DailyLaunch")
                     .HasIndex(x => x.Id)
                     .IsUnique();

            modelBuilder.Entity<LaunchType>()
                        .HasMany(e => e.Launches)
                        .WithOne(e => e.LaunchType)
                        .HasForeignKey(e => e.LaunchTypeId);

            modelBuilder.Entity<DailyLaunch>()
                      .HasMany(e => e.UserLaunches)
                      .WithOne(e => e.DailyLaunch)
                      .HasForeignKey(e => e.DailyLaunchId);

            modelBuilder.Entity<User>()
                    .HasMany(e => e.UserLaunches)
                    .WithOne(e => e.User)
                    .HasForeignKey(e => e.UserId);
        }
    }
}
