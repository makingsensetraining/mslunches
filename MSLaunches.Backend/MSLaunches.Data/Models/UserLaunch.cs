namespace MSLunches.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents the selected meal by user in our system.
    /// </summary>
    public class UserLunch : BaseEntity
    {
        [Required]
        public Guid LunchId { get; set; }

        [Required]
        public string UserId { get; set; }

        public bool Approved { get; set; }

        public virtual Lunch Lunch { get; set; }

        public virtual User User { get; set; }
    }
}