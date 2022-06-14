using Slinky.Api.Protocol;
using Slinky.Data;
using Slinky.Data.Model;
using Microsoft.AspNetCore.Mvc;

namespace Slinky.Api.Strategies
{
    internal class GetShortlinkRequestHandlingStrategy : IHttpRequestHandlingStrategy<GetShortlinkRequest>
    {
        private readonly ILogger logger;
        private readonly IDatabaseService databaseService;

        public GetShortlinkRequestHandlingStrategy(
            ILogger<GetShortlinkRequestHandlingStrategy> logger,
            IDatabaseService databaseService)
        {
            this.logger = logger;
            this.databaseService = databaseService;
        }

        public async Task<IActionResult> HandleRequest(ControllerBase controller, GetShortlinkRequest requestParams)
        {
            if (string.IsNullOrWhiteSpace(requestParams.Id))
            {
                logger.LogDebug("received an empty {}", nameof(GetShortlinkRequest));
                return controller.BadRequest();
            }

            var shortlink = await databaseService.ReadShortlinkAsync(requestParams.Id);
            if (shortlink is null)
            {
                logger.LogDebug("{} not previously mapped", requestParams.Id);
                return controller.NotFound();
            }

            logger.LogDebug("OK found {} => {}", requestParams.Id, shortlink.Uri);
            await databaseService.CreateAuditAsync(shortlink, AuditEvent.Access);

            return controller.Ok(new GetShortlinkResponse(shortlink.Uri));
        }
    }
}
