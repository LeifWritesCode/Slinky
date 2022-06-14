namespace Slinky.Api.Protocol
{
    public class ErrorResponse
    {
        public string Error { get; set; }

        public ErrorResponse(string error)
        {
            Error = error;
        }

        public static ErrorResponse FromMessage(string error)
        {
            return new ErrorResponse(error);
        }
    }
}
