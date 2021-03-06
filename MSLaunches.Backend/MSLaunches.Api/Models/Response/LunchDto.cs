﻿using System;

namespace MSLunches.Api.Models.Response
{
    public class LunchDto
    {
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
        /// Meal
        /// </summary>
        public MealDto Meal { get; set; }

        /// <summary>
        /// Date of the lunch
        /// </summary>
        public DateTime Date { get; set; }
    }
}
