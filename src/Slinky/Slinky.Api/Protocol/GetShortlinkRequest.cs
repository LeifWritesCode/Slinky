using Microsoft.AspNetCore.Mvc;

namespace Slinky.Api.Protocol
{
    public class GetShortlinkRequest
    {
        public string Id { get; set; }

        public GetShortlinkRequest(string id)
        {
            Id = id;
        }
    }
}
