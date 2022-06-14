namespace Slinky.Api.Protocol
{
    public class PostShortlinkRequest
    {
        public string Uri { get; set; }

        public PostShortlinkRequest(string uri)
        {
            Uri = uri;
        }
    }
}
