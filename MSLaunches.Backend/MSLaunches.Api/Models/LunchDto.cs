using MSLunches.Data.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace MSLunches.Api.Models
{
    public class LunchDto
    {
        public LunchDto(Lunch lunch)
        {
            Id = lunch.Id;
            CreatedOn = lunch.CreatedOn;
            UpdatedOn = lunch.UpdatedOn;
            MealId = lunch.MealId;
            Date = lunch.Date;
        }

        public Guid Id { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset UpdatedOn { get; set; }

        public Guid MealId { get; set; }
        public DateTime Date { get; set; }
    }
}
