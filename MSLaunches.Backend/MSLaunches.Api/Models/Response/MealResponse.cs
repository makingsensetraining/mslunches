using MSLunches.Data.Models;
using System;

namespace MSLunches.Api.Models.Response
{
    public class MealResponse
    {
        public MealResponse() { }

        public MealResponse(Meal meal)
        {
            Id = meal.Id;
            CreatedOn = meal.CreatedOn;
            UpdatedOn = meal.UpdatedOn;
            Name = meal.Name;
            TypeId = meal.TypeId;
        }

        public Guid Id { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset UpdatedOn { get; set; }

        public string Name { get; set; }
        public int TypeId { get; set; }
    }
}
