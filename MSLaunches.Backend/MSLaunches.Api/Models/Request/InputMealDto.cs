using System.ComponentModel.DataAnnotations;

namespace MSLunches.Api.Models.Request
{
    public class InputMealDto
    {
        /// <summary>
        /// Name of the meal.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// FK to MealType.
        /// </summary>
        public int TypeId { get; set; }
    }
}
