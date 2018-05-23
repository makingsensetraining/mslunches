namespace MSLunches.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents the meal in our system
    /// </summary>
    public class Meal : BaseEntity
    {
        /// <summary>
        /// Name of the meal.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// Id of the meal type
        /// </summary>
        [Required]
        public int TypeId { get; set; }

        public virtual MealType MealType { get; set; }

    }
}
