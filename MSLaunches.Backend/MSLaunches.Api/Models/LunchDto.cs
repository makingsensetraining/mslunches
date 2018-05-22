using MSLunches.Data.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace MSLunches.Api.Models
{
    public class LunchDto
    {
        public LunchDto() { }

        public LunchDto(Lunch lunch)
        {
            Id = lunch.Id;
            CreatedOn = lunch.CreatedOn;
            UpdatedOn = lunch.UpdatedOn;
            LunchName = lunch.LunchName;
            LunchTypeId = lunch.LunchTypeId;
        }

        public Guid Id { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset UpdatedOn { get; set; }

        public string LunchName { get; set; }
        public int LunchTypeId { get; set; }
    }
}
