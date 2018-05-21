using System.ComponentModel.DataAnnotations;

namespace MSLunches.Api.Models
{
    public class LunchDto
    {
        /// <summary>
        /// LunchName of the lunch.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string LunchName { get; set; }

        /// <summary>
        /// FK to LunchType.
        /// </summary>
        public int LunchTypeId { get; set; }
    }
}
