using MSLunches.Data.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace MSLunches.Api.Models
{
    public class InputDailyLunchDto
    {
        [Required]
        public Guid LunchId { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}
