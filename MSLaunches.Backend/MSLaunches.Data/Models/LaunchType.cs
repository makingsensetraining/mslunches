namespace MSLaunches.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents a person in our system
    /// </summary>
    public class LaunchType
    {
        public LaunchType()
        {
            Launches = new HashSet<Launch>();
        }

        /// <summary>
        /// Launch Type Id.
        /// </summary>
        [Key]
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// LaunchDescription of the launch type.
        /// </summary>
        [MaxLength(100)]
        public string Description { get; set; }

        public virtual ICollection<Launch> Launches { get; set; }
    }
}
