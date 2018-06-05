namespace MSLunches.Api.Models.Response
{
    /// <summary>
    /// Error data transfer object
    /// </summary>
    internal class ErrorDto
    {
        /// <summary>
        /// Creates a new instance of an error Response
        /// </summary>
        /// <param name="error"></param>
        public ErrorDto(string error)
        {
            Error = error;
        }

        /// <summary>
        /// Error message
        /// </summary>
        public string Error { get; }
    }
}