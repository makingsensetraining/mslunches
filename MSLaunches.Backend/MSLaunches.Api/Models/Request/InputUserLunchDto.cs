using System;
using System.ComponentModel.DataAnnotations;

namespace MSLunches.Api.Models.Request
{
    public class InputUserLunchDto
    {
        [Required]
        public Guid LunchId { get; set; }

        public string UserId { get; set; }

        public bool Approved { get; set; }
    }
}
