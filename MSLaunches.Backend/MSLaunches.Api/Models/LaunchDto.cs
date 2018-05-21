using System.ComponentModel.DataAnnotations;

namespace MSLaunches.Api.Models
{
    public class LaunchDto
    {
        /// <summary>
        /// LaunchName of the launch.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string LaunchName { get; set; }

        /// <summary>
        /// FK to LaunchType.
        /// </summary>
        public int LaunchTypeId { get; set; }
    }
}
