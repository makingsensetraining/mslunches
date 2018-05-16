using Microsoft.EntityFrameworkCore;
using MSLaunches.Data.Models;

namespace MSLaunches.Data.EF
{
    public class WebApiCoreMSLaunchesContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebApiCoreMSLaunchesContext"/> class.
        ///
        /// DbContextOptions parameter is required by AspNet core initialization
        /// </summary>
        /// <param name="options">Options used to create this <see cref="WebApiCoreMSLaunchesContext"/> instance </param>
        public WebApiCoreMSLaunchesContext(DbContextOptions options) : base(options) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebApiCoreMSLaunchesContext"/> class without parameters.
        /// </summary>
        public WebApiCoreMSLaunchesContext() : base() { }

        /// <summary> All users registered on WebApiCoreMSLaunches database</summary>
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Table names should be singular but DbSet properties should be plural.
            modelBuilder.Entity<User>().ToTable("User").HasIndex(x => x.UserName).IsUnique();
        }
    }
}
