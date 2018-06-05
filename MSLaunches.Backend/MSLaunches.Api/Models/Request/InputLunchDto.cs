using System;
using System.ComponentModel.DataAnnotations;

namespace MSLunches.Api.Models.Request
{
    public class InputLunchDto
    {
        [Required]
        public Guid MealId { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}
