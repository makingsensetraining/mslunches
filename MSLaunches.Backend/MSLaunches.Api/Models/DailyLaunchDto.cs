using System;
using System.ComponentModel.DataAnnotations;

namespace MSLunches.Api.Models
{
    public class DailyLunchDto
    {
        [Required]
        public Guid LunchId { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}
