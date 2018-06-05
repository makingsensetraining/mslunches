using System;

namespace MSLunches.Api.Models.Request
{
    public class InputBatchLunchDto : InputLunchDto
    {
        public Guid? Id { get; set; }
    }
}
