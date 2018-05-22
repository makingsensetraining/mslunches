using MSLunches.Data.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace MSLunches.Api.Models
{
    public class DailyLunchDto
    {
        public DailyLunchDto(DailyLunch dailyLunch)
        {
            Id = dailyLunch.Id;
            CreatedOn = dailyLunch.CreatedOn;
            UpdatedOn = dailyLunch.UpdatedOn;
            LunchId = dailyLunch.LunchId;
            Date = dailyLunch.Date;
        }

        public Guid Id { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset UpdatedOn { get; set; }

        public Guid LunchId { get; set; }
        public DateTime Date { get; set; }
    }
}
