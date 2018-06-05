using Microsoft.EntityFrameworkCore;
using MSLunches.Data.Models;

namespace MSLunches.Data.EF
{
    public class MSLunchesContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MSLunchesContext"/> class.
        ///
        /// DbContextOptions parameter is required by AspNet core initialization
        /// </summary>
        /// <param name="options">Options used to create this <see cref="MSLunchesContext"/> instance </param>
        public MSLunchesContext(DbContextOptions options) : base(options) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MSLunchesContext"/> class without parameters.
        /// </summary>
        public MSLunchesContext() : base() { }

        public virtual DbSet<Meal> Meals { get; set; }
        public virtual DbSet<MealType> MealTypes { get; set; }
        public virtual DbSet<Lunch> Lunches { get; set; }
        public virtual DbSet<UserLunch> UserLunches { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Meal>()
                        .ToTable("Meal")
                        .HasIndex(x => x.Id)
                        .IsUnique();

            modelBuilder.Entity<MealType>()
                        .ToTable("MealType")
                        .HasIndex(x => x.Id)
                        .IsUnique();

            modelBuilder.Entity<UserLunch>()
                        .ToTable("UserLunch")
                        .HasIndex(x => x.Id)
                        .IsUnique();

            modelBuilder.Entity<Lunch>()
                     .ToTable("Lunch")
                     .HasIndex(x => x.Id)
                     .IsUnique();

            modelBuilder.Entity<MealType>()
                        .HasMany(e => e.Meals)
                        .WithOne(e => e.Type)
                        .HasForeignKey(e => e.TypeId);

            modelBuilder.Entity<Lunch>()
                      .HasMany(e => e.UserLunches)
                      .WithOne(e => e.Lunch)
                      .HasForeignKey(e => e.LunchId);
        }
    }
}
