namespace Slinky.Api.Protocol
{
    internal class GetShortlinkResponse
    {
        public Uri Uri { get; init; }

        public GetShortlinkResponse(Uri uri)
        {
            Uri = uri;
        }
    }
}
