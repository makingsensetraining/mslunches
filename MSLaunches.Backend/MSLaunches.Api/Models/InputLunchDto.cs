using MSLunches.Data.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace MSLunches.Api.Models
{
    public class InputLunchDto
    {
        [Required]
        public Guid MealId { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}
