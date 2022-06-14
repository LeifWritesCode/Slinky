namespace Slinky.Api.Protocol
{
    internal class PostShortlinkResponse
    {
        public string Shortlink { get; set; }

        public PostShortlinkResponse(string shortlink)
        {
            Shortlink = shortlink;
        }
    }
}
