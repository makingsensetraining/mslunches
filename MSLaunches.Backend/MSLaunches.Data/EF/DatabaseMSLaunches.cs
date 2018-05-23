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

            if (!dbContext.MealTypes.Any())
            {
                var mealTypes = new[]
                {
                    new MealType { Description = "Calórico"},
                    new MealType { Description = "Light"},
                    new MealType { Description = "Vegetariano"},
                    new MealType { Description = "Sandwich"},
                    new MealType { Description = "Postre"}
                };

                dbContext.MealTypes.AddRange(mealTypes);
            }

            if (!dbContext.Meals.Any())
            {
                var meals = new[]
                {
                    new Meal {Id = id4, CreatedBy = "System", CreatedOn = DateTime.Now, Name = "Crepes de verdura y calabza", TypeId = 2},
                    new Meal { CreatedBy = "System", CreatedOn = DateTime.Now, Name = "Pechugas grilladas al limón con arroz al curry", TypeId = 2},
                    new Meal { CreatedBy = "System", CreatedOn = DateTime.Now, Name = "Milhojas de vegetales con ensalada", TypeId = 2},
                    new Meal { CreatedBy = "System", CreatedOn = DateTime.Now, Name = "Medallones de pescado con puré de calabaza", TypeId = 2},
                    new Meal { CreatedBy = "System", CreatedOn = DateTime.Now, Name = "Triangulitos integrales con vegetales y ensalada", TypeId = 2},
                    new Meal { CreatedBy = "System", CreatedOn = DateTime.Now, Name = "Arrollado de pollo con ensalada de champignones y papas al horno", TypeId = 1},
                    new Meal { CreatedBy = "System", CreatedOn = DateTime.Now, Name = "Canelones de jamón y queso con salsa bolognesa", TypeId = 1},
                    new Meal { CreatedBy = "System", CreatedOn = DateTime.Now, Name = "Milanesas de pecero napilitana con arroz primavera", TypeId = 1},
                    new Meal { CreatedBy = "System", CreatedOn = DateTime.Now, Name = "Filet de merluza con puré", TypeId = 1},
                    new Meal { CreatedBy = "System", CreatedOn = DateTime.Now, Name = "Hamburguesas caseras completas con fritas", TypeId = 1},
                    new Meal { CreatedBy = "System", CreatedOn = DateTime.Now, Name = "Milanesas de soja rellenas con ensalada", TypeId = 3},
                    new Meal { CreatedBy = "System", CreatedOn = DateTime.Now, Name = "Medallones de lentejas con ensalada", TypeId = 3},
                    new Meal { CreatedBy = "System", CreatedOn = DateTime.Now, Name = "Tarta capresse con ensalada", TypeId = 3},
                    new Meal { CreatedBy = "System", CreatedOn = DateTime.Now, Name = "Budincitos de acelga y calabaza con ensalada", TypeId = 3},
                    new Meal { CreatedBy = "System", CreatedOn = DateTime.Now, Name = "Hamburguesas veganas con calabazas en cubo", TypeId = 3},
                    new Meal { CreatedBy = "System", CreatedOn = DateTime.Now, Name = "Baguette de milanesa de carne con lechuga y tomate", TypeId = 4},
                    new Meal { CreatedBy = "System", CreatedOn = DateTime.Now, Name = "Postre de vainillas", TypeId = 5},
                    new Meal { CreatedBy = "System", CreatedOn = DateTime.Now, Name = "Alfajores de maizena", TypeId = 5},
                    new Meal { CreatedBy = "System", CreatedOn = DateTime.Now, Name = "Roll de dulce de leche", TypeId = 5},
                    new Meal { CreatedBy = "System", CreatedOn = DateTime.Now, Name = "Gelatina con frutas", TypeId = 5},
                    new Meal { CreatedBy = "System", CreatedOn = DateTime.Now, Name = "Pastelitos escaraperla", TypeId = 5}
                };

                dbContext.Meals.AddRange(meals);
            }

            if (!dbContext.Lunches.Any())
            {
                var lunches = new[]
                {
                    new Lunch {Id = id1,
                        MealId =id4,
                        Date = DateTime.Today,
                        CreatedBy = "System"
                    },
                };
                dbContext.Lunches.AddRange(lunches);
            }

            if (!dbContext.UserLunches.Any())
            {
                var userLunches = new[]
                {
                    new UserLunch {Id = id3,
                        UserId = id2,
                        LunchId = id1,
                        CreatedBy = "System"
                    },
                };

                dbContext.UserLunches.AddRange(userLunches);
            }

            dbContext.SaveChanges();
        }
    }
}
