namespace MSLunches.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents the lunches available by day in our system
    /// </summary>
    public class DailyLunch : BaseEntity
    {
        public DailyLunch()
        {
            UserLunches = new HashSet<UserLunch>();
        }

        /// <summary>
        /// Lunch Id.
        /// </summary>
        [Required]
        public Guid LunchId { get; set; }

        /// <summary>
        /// Day of the lunch
        /// </summary>
        [Required]
        public DateTime Date { get; set; }

        public virtual Lunch Lunch { get; set; }
        public virtual ICollection<UserLunch> UserLunches { get; set; }
    }
}
