namespace MSLunches.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents a type of lunch in our system
    /// </summary>
    public class LunchType
    {
        public LunchType()
        {
            Lunches = new HashSet<Lunch>();
        }

        /// <summary>
        /// Lunch Type Id.
        /// </summary>
        [Key]
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// Description of the lunch type.
        /// </summary>
        [MaxLength(100)]
        public string Description { get; set; }

        public virtual ICollection<Lunch> Lunches { get; set; }
    }
}
