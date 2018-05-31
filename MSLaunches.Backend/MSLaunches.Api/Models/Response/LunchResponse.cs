using MSLunches.Data.Models;
using System;

namespace MSLunches.Api.Models.Response
{
    public class LunchResponse
    {
        public LunchResponse(Lunch lunch)
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
