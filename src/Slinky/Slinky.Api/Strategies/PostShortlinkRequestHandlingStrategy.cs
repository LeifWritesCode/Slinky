using Slinky.Api.Protocol;
using Microsoft.AspNetCore.Mvc;
using Snowflake.Core;
using Slinky.Data.Model;
using Base62;
using Slinky.Data;

namespace Slinky.Api.Strategies
{
    internal class PostShortlinkRequestHandlingStrategy : IHttpRequestHandlingStrategy<PostShortlinkRequest>
    {
        private readonly ILogger logger;
        private readonly IDatabaseService databaseService;
        private readonly ISnowflakeProvider snowflakeProvider;
        private readonly string rootUri;

        public PostShortlinkRequestHandlingStrategy(
            ILogger<GetShortlinkRequestHandlingStrategy> logger,
            IDatabaseService databaseService,
            ISnowflakeProvider snowflakeProvider,
            IConfiguration configuration)
        {
            this.logger = logger;
            this.databaseService = databaseService;
            this.snowflakeProvider = snowflakeProvider;
            rootUri = configuration.GetValue<string>("ApiRootUri");
        }

        public async Task<IActionResult> HandleRequest(ControllerBase controller, PostShortlinkRequest requestParams)
        {
            if (string.IsNullOrEmpty(requestParams.Uri))
            {
                logger.LogDebug("received an empty {}", nameof(PostShortlinkRequest));
                return controller.BadRequest(
                    ErrorResponse.FromMessage("The api server received an empty request"));
            }

            if (!Uri.TryCreate(requestParams.Uri, new UriCreationOptions(), out Uri? uri))
            {
                logger.LogDebug("{} contained an invalid uri", nameof(PostShortlinkRequest));
                return controller.BadRequest(
                    ErrorResponse.FromMessage("The URI was invalid"));
            }

            var id = await snowflakeProvider.GetNextAsync();
            var idAsBase62 = id.ToBase62();
            logger.LogDebug("OK mapping {} => {}", idAsBase62, requestParams.Uri);

            var shortlink = await databaseService.CreateShortlinkAsync(idAsBase62, uri);
            await databaseService.CreateAuditAsync(shortlink, AuditEvent.Create);

            return controller.Ok(new PostShortlinkResponse($"{rootUri}/{idAsBase62}"));
        }
    }
}
