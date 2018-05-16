namespace MSLaunches.Data.EF
{
    using MSLaunches.Data.Models;
    using System;
    using System.Linq;

    /// <summary>
    /// Helper class to MSLaunches sample data into a <see cref="WebApiCoreMSLaunchesContext"/>
    /// </summary>
    public static class DatabaseMSLaunches
    {
        /// <summary>
        /// Initializes a <see cref="WebApiCoreMSLaunchesContext"/> with sample Data
        /// </summary>
        /// <param name="dbContext">Context to be initialized with sample data</param>
        public static void Initialize(WebApiCoreMSLaunchesContext dbContext)
        {
            dbContext.Database.EnsureCreated();

            if (!dbContext.Users.Any())
            { 
                var users = new[]
                {
                    new User { CreatedBy = "System", CreatedOn = DateTime.Now, Email = "noreply@makingsense.com", FirstName = "John", LastName = "Doe", UserName = "JohnDoe" },
                };

                dbContext.Users.AddRange(users);
            }

            dbContext.SaveChanges();
        }
    }
}
