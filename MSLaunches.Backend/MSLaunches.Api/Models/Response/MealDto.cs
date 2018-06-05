using System;

namespace MSLunches.Api.Models.Response
{
    public class MealDto
    {
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
