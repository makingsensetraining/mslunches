namespace MSLaunches.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class DailyLaunch : BaseEntity
    {
        public DailyLaunch()
        {
            UserLaunches = new HashSet<UserLaunch>();
        }

        [Required]
        public int LaunchId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public virtual Launch Launch { get; set; }
        public virtual ICollection<UserLaunch> UserLaunches { get; set; }

    }
}
