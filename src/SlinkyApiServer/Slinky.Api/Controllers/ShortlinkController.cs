using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Slinky.Api.Protocol;

namespace Slinky.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ShortlinkController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly IServiceProvider serviceProvider;

        public ShortlinkController(
            ILogger<ShortlinkController> logger,
            IServiceProvider serviceProvider)
        {
            this.logger = logger;
            this.serviceProvider = serviceProvider;
        }

        [HttpPost]
        [AllowAnonymous]
        [MapToApiVersion("1.0")]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetShortlinkResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get([FromBody] GetShortlinkRequest getShortlinkRequest)
        {
            using var scope = serviceProvider.CreateScope();
            var strategy = scope.ServiceProvider
                .GetService<IHttpRequestHandlingStrategy<GetShortlinkRequest>>();

            if (strategy is null)
            {
                logger.LogError($"no handler registered for {nameof(GetShortlinkRequest)}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return await strategy.HandleRequest(this, getShortlinkRequest);
        }

        [HttpPost]
        [AllowAnonymous]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PostShortlinkResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Post([FromBody] PostShortlinkRequest postShortlinkRequest)
        {
            using var scope = serviceProvider.CreateScope();
            var strategy = scope.ServiceProvider
                .GetService<IHttpRequestHandlingStrategy<PostShortlinkRequest>>();

            if (strategy is null)
            {
                logger.LogError($"no handler registered for {nameof(PostShortlinkRequest)}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return await strategy.HandleRequest(this, postShortlinkRequest);
            
        }
    }
}
