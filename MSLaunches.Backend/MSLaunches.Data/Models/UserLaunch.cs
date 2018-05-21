namespace MSLaunches.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class UserLaunch : BaseEntity
    {
        [Required]
        public int LaunchId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        DateTime Date { get; set; }

        public bool Approved { get; set; }

        public virtual Launch Launch { get; set; }
        public virtual User User { get; set; }
    }
}