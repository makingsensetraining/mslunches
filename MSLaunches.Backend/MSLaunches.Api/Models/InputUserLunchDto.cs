using System;
using System.ComponentModel.DataAnnotations;

namespace MSLunches.Api.Models
{
    public class InputUserLunchDto
    {
        [Required]
        public Guid LunchId { get; set; }

        [Required]
        public Guid UserId { get; set; }

        public bool Approved { get; set; }
    }
}
