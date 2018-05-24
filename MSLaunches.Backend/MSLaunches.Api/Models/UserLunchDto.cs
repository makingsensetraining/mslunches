using MSLunches.Data.Models;
using System;

namespace MSLunches.Api.Models
{
    public class UserLunchDto
    {
        public UserLunchDto() { }

        public UserLunchDto(UserLunch userLunch)
        {
            Id = userLunch.Id;
            CreatedOn = userLunch.CreatedOn;
            UpdatedOn = userLunch.UpdatedOn;
            LunchId = userLunch.LunchId;
            UserId = userLunch.UserId;
            Approved = userLunch.Approved;
        }

        public Guid Id { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset UpdatedOn { get; set; }

        public Guid LunchId { get; set; }
        public string UserId { get; set; }
        public bool Approved { get; set; }
    }
}
