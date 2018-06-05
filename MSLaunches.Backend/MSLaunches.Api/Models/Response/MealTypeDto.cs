using MSLunches.Data.Models;

namespace MSLunches.Api.Models.Response
{
    public class MealTypeDto
    {
        /// <summary>
        /// Meal Type identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Meal Type's description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Says whether is a selectable type or not
        /// </summary>
        public bool IsSelectable { get; set; }
    }
}
