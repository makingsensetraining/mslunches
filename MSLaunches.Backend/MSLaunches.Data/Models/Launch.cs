namespace MSLunches.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents the meal in our system
    /// </summary>
    public class Lunch : BaseEntity
    {
        /// <summary>
        /// Name of the lunch.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string LunchName { get; set; }

        /// <summary>
        /// Id of the lunch type
        /// </summary>
        [Required]
        public int LunchTypeId { get; set; }

        public virtual LunchType LunchType { get; set; }

    }
}
