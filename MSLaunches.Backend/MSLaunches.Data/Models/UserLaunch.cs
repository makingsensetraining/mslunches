namespace MSLunches.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents the selected lunch by user in our system.
    /// </summary>
    public class UserLunch : BaseEntity
    {
        [Required]
        public Guid DailyLunchId { get; set; }

        [Required]
        public Guid UserId { get; set; }

        public bool Approved { get; set; }

        public virtual DailyLunch DailyLunch { get; set; }
        public virtual User User { get; set; }
    }
}