using Microsoft.EntityFrameworkCore;
using MSLunches.Data.Models;

namespace MSLunches.Data.EF
{
    public class WebApiCoreLunchesContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebApiCoreLunchesContext"/> class.
        ///
        /// DbContextOptions parameter is required by AspNet core initialization
        /// </summary>
        /// <param name="options">Options used to create this <see cref="WebApiCoreLunchesContext"/> instance </param>
        public WebApiCoreLunchesContext(DbContextOptions options) : base(options) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebApiCoreLunchesContext"/> class without parameters.
        /// </summary>
        public WebApiCoreLunchesContext() : base() { }

        /// <summary> All users registered on WebApiCoreMSLunches database</summary>
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Lunch> Lunches { get; set; }
        public virtual DbSet<LunchType> LunchTypes { get; set; }
        public virtual DbSet<DailyLunch> DailyLunches { get; set; }
        public virtual DbSet<UserLunch> UserLunches { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Table names should be singular but DbSet properties should be plural.
            modelBuilder.Entity<User>()
                        .ToTable("User")
                        .HasIndex(x => x.UserName)
                        .IsUnique();

            modelBuilder.Entity<Lunch>()
                        .ToTable("Lunch")
                        .HasIndex(x => x.Id)
                        .IsUnique();

            modelBuilder.Entity<LunchType>()
                        .ToTable("LunchType")
                        .HasIndex(x => x.Id)
                        .IsUnique();

            modelBuilder.Entity<UserLunch>()
                        .ToTable("UserLunch")
                        .HasIndex(x => x.Id)
                        .IsUnique();

            modelBuilder.Entity<DailyLunch>()
                     .ToTable("DailyLunch")
                     .HasIndex(x => x.Id)
                     .IsUnique();

            modelBuilder.Entity<LunchType>()
                        .HasMany(e => e.Lunches)
                        .WithOne(e => e.LunchType)
                        .HasForeignKey(e => e.LunchTypeId);

            modelBuilder.Entity<DailyLunch>()
                      .HasMany(e => e.UserLunches)
                      .WithOne(e => e.DailyLunch)
                      .HasForeignKey(e => e.DailyLunchId);

            modelBuilder.Entity<User>()
                    .HasMany(e => e.UserLunches)
                    .WithOne(e => e.User)
                    .HasForeignKey(e => e.UserId);
        }
    }
}
