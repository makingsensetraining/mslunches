using System;
using System.ComponentModel.DataAnnotations;

namespace MSLaunches.Api.Models
{
    public class DailyLaunchDto
    {
        [Required]
        public int LaunchId { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}
