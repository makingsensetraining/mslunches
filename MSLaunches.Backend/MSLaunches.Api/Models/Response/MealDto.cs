using MSLunches.Data.Models;
using System;

namespace MSLunches.Api.Models.Response
{
    public class MealDto
    {
        public MealDto() { }

        public MealDto(Meal meal)
        {
            Id = meal.Id;
            CreatedOn = meal.CreatedOn;
            UpdatedOn = meal.UpdatedOn;
            Name = meal.Name;
            TypeId = meal.TypeId;
        }

        /// <summary>
        /// Meal Identifier
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Creation Date
        /// </summary>
        public DateTimeOffset CreatedOn { get; set; }

        /// <summary>
        /// Last Update date
        /// </summary>
        public DateTimeOffset UpdatedOn { get; set; }

        /// <summary>
        /// Name of the meal
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Type of the meal
        /// </summary>
        public int TypeId { get; set; }
    }
}
