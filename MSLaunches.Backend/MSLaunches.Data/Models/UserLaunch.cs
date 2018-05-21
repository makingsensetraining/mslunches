namespace MSLaunches.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class UserLaunch : BaseEntity
    {
        [Required]
        public int DailyLaunchId { get; set; }

        [Required]
        public int UserId { get; set; }

        public bool Approved { get; set; }

        public virtual DailyLaunch DailyLaunch { get; set; }
        public virtual User User { get; set; }
    }
}