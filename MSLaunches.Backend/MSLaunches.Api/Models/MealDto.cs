using MSLunches.Data.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace MSLunches.Api.Models
{
    public class MealDto
    {
        public MealDto() { }

        public MealDto(Meal meal)
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
