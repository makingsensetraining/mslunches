using MSLunches.Data.Models;

namespace MSLunches.Api.Models.Response
{
    public class MealTypeDto
    {
        public MealTypeDto() { }

        public MealTypeDto(MealType m)
        {
            Id = m.Id;
            Description = m.Description;
            IsSelectable = m.IsSelectable;
        }

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
