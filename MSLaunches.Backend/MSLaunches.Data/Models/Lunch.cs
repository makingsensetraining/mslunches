namespace MSLunches.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents the lunches available by day in our system
    /// </summary>
    public class Lunch : BaseEntity
    {
        public Lunch()
        {
            UserLunches = new HashSet<UserLunch>();
        }

        /// <summary>
        /// Meal Id.
        /// </summary>
        [Required]
        public Guid MealId { get; set; }

        /// <summary>
        /// Day of the meal
        /// </summary>
        [Required]
        public DateTime Date { get; set; }

        public virtual Meal Meal { get; set; }
        public virtual ICollection<UserLunch> UserLunches { get; set; }
    }
}
