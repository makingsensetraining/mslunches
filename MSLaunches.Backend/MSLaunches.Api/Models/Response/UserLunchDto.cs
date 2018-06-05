using System;

namespace MSLunches.Api.Models.Response
{
    public class UserLunchDto
    {
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
