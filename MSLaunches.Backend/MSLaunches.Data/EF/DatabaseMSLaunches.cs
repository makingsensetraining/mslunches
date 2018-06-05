namespace MSLunches.Data.EF
{
    using Microsoft.EntityFrameworkCore;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    /// <summary>
    /// Helper class to MSLunches sample data into a <see cref="MSLunchesContext"/>
    /// </summary>
    public static class DatabaseMSLunches
    {
        /// <summary>
        /// Initializes a <see cref="MSLunchesContext"/> with sample Data
        /// </summary>
        /// <param name="dbContext">Context to be initialized with sample data</param>
        public static void Initialize(MSLunchesContext dbContext)
        {
            dbContext.Database.EnsureCreated();

            if (!dbContext.MealTypes.Any())
            {
                dbContext.MealTypes.Add(new MealType { Description = "Calórico", IsSelectable = true });
                dbContext.MealTypes.Add(new MealType { Description = "Light", IsSelectable = true });
                dbContext.MealTypes.Add(new MealType { Description = "Vegetariano", IsSelectable = true });
                dbContext.MealTypes.Add(new MealType { Description = "Sandwich", IsSelectable = true });
                dbContext.MealTypes.Add(new MealType { Description = "Postre", IsSelectable = false });

                dbContext.SaveChanges();
            }

            Meal[] meals = null;

            if (!dbContext.Meals.Any())
            {
                var mealTypes = dbContext.MealTypes.ToList();
                var idCalorico = mealTypes.FirstOrDefault(a => a.Description == "Calórico")?.Id ?? 0;
                var idLight = mealTypes.FirstOrDefault(a => a.Description == "Light")?.Id ?? 0;
                var idVegetariano = mealTypes.FirstOrDefault(a => a.Description == "Vegetariano")?.Id ?? 0;
                var idSandwich = mealTypes.FirstOrDefault(a => a.Description == "Sandwich")?.Id ?? 0;
                var idPostre = mealTypes.FirstOrDefault(a => a.Description == "Postre")?.Id ?? 0;

                meals = new[]
                {
                    new Meal { Id = Guid.NewGuid(), CreatedBy = "System", CreatedOn = DateTime.Now, Name = "Crepes de verdura y calabza", TypeId = idLight},
                    new Meal { Id = Guid.NewGuid(), CreatedBy = "System", CreatedOn = DateTime.Now, Name = "Pechugas grilladas al limón con arroz al curry", TypeId = idLight},
                    new Meal { Id = Guid.NewGuid(), CreatedBy = "System", CreatedOn = DateTime.Now, Name = "Milhojas de vegetales con ensalada", TypeId = idLight},
                    new Meal { Id = Guid.NewGuid(), CreatedBy = "System", CreatedOn = DateTime.Now, Name = "Medallones de pescado con puré de calabaza", TypeId = idLight},
                    new Meal { Id = Guid.NewGuid(), CreatedBy = "System", CreatedOn = DateTime.Now, Name = "Triangulitos integrales con vegetales y ensalada", TypeId = idLight},
                    new Meal { Id = Guid.NewGuid(), CreatedBy = "System", CreatedOn = DateTime.Now, Name = "Arrollado de pollo con ensalada de champignones y papas al horno", TypeId = idCalorico},
                    new Meal { Id = Guid.NewGuid(), CreatedBy = "System", CreatedOn = DateTime.Now, Name = "Canelones de jamón y queso con salsa bolognesa", TypeId = idCalorico},
                    new Meal { Id = Guid.NewGuid(), CreatedBy = "System", CreatedOn = DateTime.Now, Name = "Milanesas de pecero napilitana con arroz primavera", TypeId = idCalorico},
                    new Meal { Id = Guid.NewGuid(), CreatedBy = "System", CreatedOn = DateTime.Now, Name = "Filet de merluza con puré", TypeId = idCalorico},
                    new Meal { Id = Guid.NewGuid(), CreatedBy = "System", CreatedOn = DateTime.Now, Name = "Hamburguesas caseras completas con fritas", TypeId = idCalorico},
                    new Meal { Id = Guid.NewGuid(), CreatedBy = "System", CreatedOn = DateTime.Now, Name = "Milanesas de soja rellenas con ensalada", TypeId = idVegetariano},
                    new Meal { Id = Guid.NewGuid(), CreatedBy = "System", CreatedOn = DateTime.Now, Name = "Medallones de lentejas con ensalada", TypeId = idVegetariano},
                    new Meal { Id = Guid.NewGuid(), CreatedBy = "System", CreatedOn = DateTime.Now, Name = "Tarta capresse con ensalada", TypeId = idVegetariano},
                    new Meal { Id = Guid.NewGuid(), CreatedBy = "System", CreatedOn = DateTime.Now, Name = "Budincitos de acelga y calabaza con ensalada", TypeId = idVegetariano},
                    new Meal { Id = Guid.NewGuid(), CreatedBy = "System", CreatedOn = DateTime.Now, Name = "Hamburguesas veganas con calabazas en cubo", TypeId = idVegetariano},
                    new Meal { Id = Guid.NewGuid(), CreatedBy = "System", CreatedOn = DateTime.Now, Name = "Baguette de milanesa de carne con lechuga y tomate", TypeId = idSandwich},
                    new Meal { Id = Guid.NewGuid(), CreatedBy = "System", CreatedOn = DateTime.Now, Name = "Postre de vainillas", TypeId = idPostre},
                    new Meal { Id = Guid.NewGuid(), CreatedBy = "System", CreatedOn = DateTime.Now, Name = "Alfajores de maizena", TypeId = idPostre},
                    new Meal { Id = Guid.NewGuid(), CreatedBy = "System", CreatedOn = DateTime.Now, Name = "Roll de dulce de leche", TypeId = idPostre},
                    new Meal { Id = Guid.NewGuid(), CreatedBy = "System", CreatedOn = DateTime.Now, Name = "Gelatina con frutas", TypeId = idPostre},
                    new Meal { Id = Guid.NewGuid(), CreatedBy = "System", CreatedOn = DateTime.Now, Name = "Pastelitos escaraperla", TypeId = idPostre}
                };

                dbContext.Meals.AddRange(meals);

                dbContext.SaveChanges();
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
