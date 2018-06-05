using MSLunches.Data.Models;
using System;

namespace MSLunches.Api.Models.Response
{
    public class LunchDto
    {
        public LunchDto() { }

        public LunchDto(Lunch lunch)
        {
            Id = lunch.Id;
            CreatedOn = lunch.CreatedOn;
            UpdatedOn = lunch.UpdatedOn;
            MealId = lunch.MealId;
            Date = lunch.Date;
        }

        /// <summary>
        /// Lunch Identifier
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Creation Date
        /// </summary>
        public DateTimeOffset CreatedOn { get; set; }

        /// <summary>
        /// Last Update Date
        /// </summary>
        public DateTimeOffset UpdatedOn { get; set; }

        /// <summary>
        /// Meal Identifier attached to this lunch
        /// </summary>
        public Guid MealId { get; set; }

        /// <summary>
        /// Date of the lunch
        /// </summary>
        public DateTime Date { get; set; }
    }
}
