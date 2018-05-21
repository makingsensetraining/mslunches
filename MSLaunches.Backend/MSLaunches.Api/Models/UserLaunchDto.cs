using System.ComponentModel.DataAnnotations;

namespace MSLaunches.Api.Models
{
    public class UserLaunchDto
    {
        [Required]
        public int DailyLaunchId { get; set; }

        [Required]
        public int UserId { get; set; }
    }
}
