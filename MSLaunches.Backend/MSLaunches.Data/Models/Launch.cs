namespace MSLaunches.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents a person in our system
    /// </summary>
    public class Launch : BaseEntity
    {
        /// <summary>
        /// LaunchName of the launch.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string LaunchName { get; set; }

        /// <summary>
        /// LaunchDescription of the launch.
        /// </summary>
        [MaxLength(100)]
        public string LaunchDescription { get; set; }

        /// <summary>
        /// FK to LaunchType.
        /// </summary>
        [Required]
        public int LaunchTypeId { get; set; }

        public virtual LaunchType LaunchType { get; set; }

    }
}
