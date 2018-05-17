namespace MSLaunches.Data.EF
{
    using MSLaunches.Data.Models;
    using System;
    using System.Linq;

    /// <summary>
    /// Helper class to MSLaunches sample data into a <see cref="WebApiCoreLaunchesContext"/>
    /// </summary>
    public static class DatabaseMSLaunches
    {
        /// <summary>
        /// Initializes a <see cref="WebApiCoreLaunchesContext"/> with sample Data
        /// </summary>
        /// <param name="dbContext">Context to be initialized with sample data</param>
        public static void Initialize(WebApiCoreLaunchesContext dbContext)
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

            if (!dbContext.LaunchTypes.Any())
            {
                var launchTypes = new[]
                {
                    new LaunchType {LaunchTypeId = 1 ,LaunchTypeDescription = "Calórico"},
                    new LaunchType {LaunchTypeId = 2 ,LaunchTypeDescription = "Light"},
                    new LaunchType {LaunchTypeId = 3 ,LaunchTypeDescription = "Vegetariano"},
                    new LaunchType {LaunchTypeId = 4 ,LaunchTypeDescription = "Sandwich"},
                    new LaunchType {LaunchTypeId = 5 ,LaunchTypeDescription = "Postre"}
                };

                dbContext.LaunchTypes.AddRange(launchTypes);
            }

            if (!dbContext.Launches.Any())
            {
                var launches = new[]
                {
                    new Launch { CreatedBy = "System", CreatedOn = DateTime.Now, LaunchName = "Crepes de verdura y calabza", LaunchTypeId = 2},
                    new Launch { CreatedBy = "System", CreatedOn = DateTime.Now, LaunchName = "Pechugas grilladas al limón con arroz al curry", LaunchTypeId = 2},
                    new Launch { CreatedBy = "System", CreatedOn = DateTime.Now, LaunchName = "Milhojas de vegetales con ensalada", LaunchTypeId = 2},
                    new Launch { CreatedBy = "System", CreatedOn = DateTime.Now, LaunchName = "Medallones de pescado con puré de calabaza", LaunchTypeId = 2},
                    new Launch { CreatedBy = "System", CreatedOn = DateTime.Now, LaunchName = "Triangulitos integrales con vegetales y ensalada", LaunchTypeId = 2},
                    new Launch { CreatedBy = "System", CreatedOn = DateTime.Now, LaunchName = "Arrollado de pollo con ensalada de champignones y papas al horno", LaunchTypeId = 1},
                    new Launch { CreatedBy = "System", CreatedOn = DateTime.Now, LaunchName = "Canelones de jamón y queso con salsa bolognesa", LaunchTypeId = 1},
                    new Launch { CreatedBy = "System", CreatedOn = DateTime.Now, LaunchName = "Milanesas de pecero napilitana con arroz primavera", LaunchTypeId = 1},
                    new Launch { CreatedBy = "System", CreatedOn = DateTime.Now, LaunchName = "Filet de merluza con puré", LaunchTypeId = 1},
                    new Launch { CreatedBy = "System", CreatedOn = DateTime.Now, LaunchName = "Hamburguesas caseras completas con fritas", LaunchTypeId = 1},
                    new Launch { CreatedBy = "System", CreatedOn = DateTime.Now, LaunchName = "Milanesas de soja rellenas con ensalada", LaunchTypeId = 3},
                    new Launch { CreatedBy = "System", CreatedOn = DateTime.Now, LaunchName = "Medallones de lentejas con ensalada", LaunchTypeId = 3},
                    new Launch { CreatedBy = "System", CreatedOn = DateTime.Now, LaunchName = "Tarta capresse con ensalada", LaunchTypeId = 3},
                    new Launch { CreatedBy = "System", CreatedOn = DateTime.Now, LaunchName = "Budincitos de acelga y calabaza con ensalada", LaunchTypeId = 3},
                    new Launch { CreatedBy = "System", CreatedOn = DateTime.Now, LaunchName = "Hamburguesas veganas con calabazas en cubo", LaunchTypeId = 3},
                    new Launch { CreatedBy = "System", CreatedOn = DateTime.Now, LaunchName = "Baguette de milanesa de carne con lechuga y tomate", LaunchTypeId = 4},
                    new Launch { CreatedBy = "System", CreatedOn = DateTime.Now, LaunchName = "Postre de vainillas", LaunchTypeId = 5},
                    new Launch { CreatedBy = "System", CreatedOn = DateTime.Now, LaunchName = "Alfajores de maizena", LaunchTypeId = 5},
                    new Launch { CreatedBy = "System", CreatedOn = DateTime.Now, LaunchName = "Roll de dulce de leche", LaunchTypeId = 5},
                    new Launch { CreatedBy = "System", CreatedOn = DateTime.Now, LaunchName = "Gelatina con frutas", LaunchTypeId = 5},
                    new Launch { CreatedBy = "System", CreatedOn = DateTime.Now, LaunchName = "Pastelitos escaraperla", LaunchTypeId = 5}
                };

                dbContext.Launches.AddRange(launches);
            }

            dbContext.SaveChanges();
        }
    }
}
