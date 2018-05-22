namespace MSLunches.Data.EF
{
    using Models;
    using System;
    using System.Linq;

    /// <summary>
    /// Helper class to MSLunches sample data into a <see cref="WebApiCoreLunchesContext"/>
    /// </summary>
    public static class DatabaseMSLunches
    {
        /// <summary>
        /// Initializes a <see cref="WebApiCoreLunchesContext"/> with sample Data
        /// </summary>
        /// <param name="dbContext">Context to be initialized with sample data</param>
        public static void Initialize(WebApiCoreLunchesContext dbContext)
        {
            dbContext.Database.EnsureCreated();
            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            var id3 = Guid.NewGuid();
            var id4 = Guid.NewGuid();

            if (!dbContext.Users.Any())
            {
                var users = new[]
                {
                    new User { Id = id2, CreatedBy = "System", CreatedOn = DateTime.Now, Email = "noreply@makingsense.com", FirstName = "John", LastName = "Doe", UserName = "JohnDoe" },
                };

                dbContext.Users.AddRange(users);
            }

            if (!dbContext.LunchTypes.Any())
            {
                var lunchTypes = new[]
                {
                    new LunchType {Id = 1 ,Description = "Calórico"},
                    new LunchType {Id = 2 ,Description = "Light"},
                    new LunchType {Id = 3 ,Description = "Vegetariano"},
                    new LunchType {Id = 4 ,Description = "Sandwich"},
                    new LunchType {Id = 5 ,Description = "Postre"}
                };

                dbContext.LunchTypes.AddRange(lunchTypes);
            }

            if (!dbContext.Lunches.Any())
            {
                var lunches = new[]
                {
                    new Lunch {Id = id4, CreatedBy = "System", CreatedOn = DateTime.Now, LunchName = "Crepes de verdura y calabza", LunchTypeId = 2},
                    new Lunch { CreatedBy = "System", CreatedOn = DateTime.Now, LunchName = "Pechugas grilladas al limón con arroz al curry", LunchTypeId = 2},
                    new Lunch { CreatedBy = "System", CreatedOn = DateTime.Now, LunchName = "Milhojas de vegetales con ensalada", LunchTypeId = 2},
                    new Lunch { CreatedBy = "System", CreatedOn = DateTime.Now, LunchName = "Medallones de pescado con puré de calabaza", LunchTypeId = 2},
                    new Lunch { CreatedBy = "System", CreatedOn = DateTime.Now, LunchName = "Triangulitos integrales con vegetales y ensalada", LunchTypeId = 2},
                    new Lunch { CreatedBy = "System", CreatedOn = DateTime.Now, LunchName = "Arrollado de pollo con ensalada de champignones y papas al horno", LunchTypeId = 1},
                    new Lunch { CreatedBy = "System", CreatedOn = DateTime.Now, LunchName = "Canelones de jamón y queso con salsa bolognesa", LunchTypeId = 1},
                    new Lunch { CreatedBy = "System", CreatedOn = DateTime.Now, LunchName = "Milanesas de pecero napilitana con arroz primavera", LunchTypeId = 1},
                    new Lunch { CreatedBy = "System", CreatedOn = DateTime.Now, LunchName = "Filet de merluza con puré", LunchTypeId = 1},
                    new Lunch { CreatedBy = "System", CreatedOn = DateTime.Now, LunchName = "Hamburguesas caseras completas con fritas", LunchTypeId = 1},
                    new Lunch { CreatedBy = "System", CreatedOn = DateTime.Now, LunchName = "Milanesas de soja rellenas con ensalada", LunchTypeId = 3},
                    new Lunch { CreatedBy = "System", CreatedOn = DateTime.Now, LunchName = "Medallones de lentejas con ensalada", LunchTypeId = 3},
                    new Lunch { CreatedBy = "System", CreatedOn = DateTime.Now, LunchName = "Tarta capresse con ensalada", LunchTypeId = 3},
                    new Lunch { CreatedBy = "System", CreatedOn = DateTime.Now, LunchName = "Budincitos de acelga y calabaza con ensalada", LunchTypeId = 3},
                    new Lunch { CreatedBy = "System", CreatedOn = DateTime.Now, LunchName = "Hamburguesas veganas con calabazas en cubo", LunchTypeId = 3},
                    new Lunch { CreatedBy = "System", CreatedOn = DateTime.Now, LunchName = "Baguette de milanesa de carne con lechuga y tomate", LunchTypeId = 4},
                    new Lunch { CreatedBy = "System", CreatedOn = DateTime.Now, LunchName = "Postre de vainillas", LunchTypeId = 5},
                    new Lunch { CreatedBy = "System", CreatedOn = DateTime.Now, LunchName = "Alfajores de maizena", LunchTypeId = 5},
                    new Lunch { CreatedBy = "System", CreatedOn = DateTime.Now, LunchName = "Roll de dulce de leche", LunchTypeId = 5},
                    new Lunch { CreatedBy = "System", CreatedOn = DateTime.Now, LunchName = "Gelatina con frutas", LunchTypeId = 5},
                    new Lunch { CreatedBy = "System", CreatedOn = DateTime.Now, LunchName = "Pastelitos escaraperla", LunchTypeId = 5}
                };


                if (!dbContext.DailyLunches.Any())
                {
                    var dailyLunches = new[]
                    {
                    new DailyLunch {Id = id1,
                        LunchId =id4,
                        Date = DateTime.Today
                    },
                };

                    dbContext.DailyLunches.AddRange(dailyLunches);
                }

                if (!dbContext.UserLunches.Any())
                {
                    var userLunches = new[]
                    {
                    new UserLunch {Id = id3,
                        UserId = id2,
                        DailyLunchId = id1
                    },
                };

                    dbContext.UserLunches.AddRange(userLunches);
                }

                dbContext.Lunches.AddRange(lunches);
            }

            dbContext.SaveChanges();
        }
    }
}
