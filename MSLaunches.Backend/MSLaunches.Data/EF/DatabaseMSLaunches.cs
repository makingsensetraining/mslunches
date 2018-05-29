namespace MSLunches.Data.EF
{
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
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

            if (!dbContext.MealTypes.Any())
            {
                var mealTypes = new[]
                {
                    new MealType { Description = "Calórico", IsSelectable = true},
                    new MealType { Description = "Light", IsSelectable = true},
                    new MealType { Description = "Vegetariano",IsSelectable = true},
                    new MealType { Description = "Sandwich", IsSelectable = true},
                    new MealType { Description = "Postre", IsSelectable = false}
                };

                dbContext.MealTypes.AddRange(mealTypes);
            }

            Meal[] meals = null;

            if (!dbContext.Meals.Any())
            {
                meals = new[]
                {
                    new Meal { Id = Guid.NewGuid(), CreatedBy = "System", CreatedOn = DateTime.Now, Name = "Crepes de verdura y calabza", TypeId = 2},
                    new Meal { Id = Guid.NewGuid(), CreatedBy = "System", CreatedOn = DateTime.Now, Name = "Pechugas grilladas al limón con arroz al curry", TypeId = 2},
                    new Meal { Id = Guid.NewGuid(), CreatedBy = "System", CreatedOn = DateTime.Now, Name = "Milhojas de vegetales con ensalada", TypeId = 2},
                    new Meal { Id = Guid.NewGuid(), CreatedBy = "System", CreatedOn = DateTime.Now, Name = "Medallones de pescado con puré de calabaza", TypeId = 2},
                    new Meal { Id = Guid.NewGuid(), CreatedBy = "System", CreatedOn = DateTime.Now, Name = "Triangulitos integrales con vegetales y ensalada", TypeId = 2},
                    new Meal { Id = Guid.NewGuid(), CreatedBy = "System", CreatedOn = DateTime.Now, Name = "Arrollado de pollo con ensalada de champignones y papas al horno", TypeId = 1},
                    new Meal { Id = Guid.NewGuid(), CreatedBy = "System", CreatedOn = DateTime.Now, Name = "Canelones de jamón y queso con salsa bolognesa", TypeId = 1},
                    new Meal { Id = Guid.NewGuid(), CreatedBy = "System", CreatedOn = DateTime.Now, Name = "Milanesas de pecero napilitana con arroz primavera", TypeId = 1},
                    new Meal { Id = Guid.NewGuid(), CreatedBy = "System", CreatedOn = DateTime.Now, Name = "Filet de merluza con puré", TypeId = 1},
                    new Meal { Id = Guid.NewGuid(), CreatedBy = "System", CreatedOn = DateTime.Now, Name = "Hamburguesas caseras completas con fritas", TypeId = 1},
                    new Meal { Id = Guid.NewGuid(), CreatedBy = "System", CreatedOn = DateTime.Now, Name = "Milanesas de soja rellenas con ensalada", TypeId = 3},
                    new Meal { Id = Guid.NewGuid(), CreatedBy = "System", CreatedOn = DateTime.Now, Name = "Medallones de lentejas con ensalada", TypeId = 3},
                    new Meal { Id = Guid.NewGuid(), CreatedBy = "System", CreatedOn = DateTime.Now, Name = "Tarta capresse con ensalada", TypeId = 3},
                    new Meal { Id = Guid.NewGuid(), CreatedBy = "System", CreatedOn = DateTime.Now, Name = "Budincitos de acelga y calabaza con ensalada", TypeId = 3},
                    new Meal { Id = Guid.NewGuid(), CreatedBy = "System", CreatedOn = DateTime.Now, Name = "Hamburguesas veganas con calabazas en cubo", TypeId = 3},
                    new Meal { Id = Guid.NewGuid(), CreatedBy = "System", CreatedOn = DateTime.Now, Name = "Baguette de milanesa de carne con lechuga y tomate", TypeId = 4},
                    new Meal { Id = Guid.NewGuid(), CreatedBy = "System", CreatedOn = DateTime.Now, Name = "Postre de vainillas", TypeId = 5},
                    new Meal { Id = Guid.NewGuid(), CreatedBy = "System", CreatedOn = DateTime.Now, Name = "Alfajores de maizena", TypeId = 5},
                    new Meal { Id = Guid.NewGuid(), CreatedBy = "System", CreatedOn = DateTime.Now, Name = "Roll de dulce de leche", TypeId = 5},
                    new Meal { Id = Guid.NewGuid(), CreatedBy = "System", CreatedOn = DateTime.Now, Name = "Gelatina con frutas", TypeId = 5},
                    new Meal { Id = Guid.NewGuid(), CreatedBy = "System", CreatedOn = DateTime.Now, Name = "Pastelitos escaraperla", TypeId = 5}
                };

                dbContext.Meals.AddRange(meals);
            }

            if (!dbContext.Lunches.Any())
            {
                var lunches = new List<Lunch>();

                if (meals != null)
                {
                    var firstDayOfTheWeek = GetFirstDayOfWeek(DateTime.Today, CultureInfo.CurrentCulture);
                    if (firstDayOfTheWeek.DayOfWeek == DayOfWeek.Sunday)
                        firstDayOfTheWeek = firstDayOfTheWeek.AddDays(1);

                    var type = meals.First().TypeId;
                    var date = new DateTime(firstDayOfTheWeek.Ticks);
                    meals = meals.OrderBy(a => a.TypeId).ToArray();

                    foreach (var meal in meals)
                    {
                        date = date.AddDays(1);
                        if (type != meal.TypeId)
                        {
                            type = meal.TypeId;
                            date = new DateTime(firstDayOfTheWeek.Ticks);
                        }

                        lunches.Add(new Lunch
                        {
                            MealId = meal.Id,
                            CreatedBy = "System",
                            CreatedOn = DateTime.Now,
                            Id = Guid.NewGuid(),
                            Date = date
                        });
                    }
                }
                dbContext.Lunches.AddRange(lunches);
            }

            dbContext.SaveChanges();
        }

        /// <summary>
        /// Returns the first day of the week that the specified date 
        /// is in. 
        /// </summary>
        private static DateTime GetFirstDayOfWeek(DateTime dayInWeek, CultureInfo cultureInfo)
        {
            DayOfWeek firstDay = cultureInfo.DateTimeFormat.FirstDayOfWeek;
            DateTime firstDayInWeek = dayInWeek.Date;
            while (firstDayInWeek.DayOfWeek != firstDay)
                firstDayInWeek = firstDayInWeek.AddDays(-1);

            return firstDayInWeek;
        }
    }
}
