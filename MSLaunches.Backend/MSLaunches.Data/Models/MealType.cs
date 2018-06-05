using System.ComponentModel.DataAnnotations.Schema;

namespace MSLunches.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents a type of meal in our system
    /// </summary>
    public class MealType
    {
        public MealType()
        {
            Meals = new HashSet<Meal>();
        }

        /// <summary>
        /// Meal Type Id.
        /// </summary>
        [Key]
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// Description of the meal type.
        /// </summary>
        [MaxLength(100)]
        public string Description { get; set; }

        /// <summary>
        /// Set if the type of menue is selectable or not.-
        /// </summary>
        public bool IsSelectable { get; set; }

        public virtual ICollection<Meal> Meals { get; set; }
    }
}
