using System;
using System.ComponentModel.DataAnnotations;

namespace MSLunches.Api.Models.Request
{
    public class UserLunchRequest
    {
        [Required]
        public Guid LunchId { get; set; }

        public string UserId { get; set; }

        public bool Approved { get; set; }
    }
}
