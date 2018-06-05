using MSLunches.Data.Models;
using System;

namespace MSLunches.Api.Models.Response
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

        /// <summary>
        /// User Lunch Identifier
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Creation Date
        /// </summary>
        public DateTimeOffset CreatedOn { get; set; }

        /// <summary>
        /// Last Update date
        /// </summary>
        public DateTimeOffset UpdatedOn { get; set; }

        /// <summary>
        /// Lunch Identifier
        /// </summary>
        public Guid LunchId { get; set; }

        /// <summary>
        /// User Identifier
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Aprovation
        /// </summary>
        public bool Approved { get; set; }
    }
}
