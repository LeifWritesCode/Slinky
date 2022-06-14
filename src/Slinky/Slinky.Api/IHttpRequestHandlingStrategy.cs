using Microsoft.AspNetCore.Mvc;

namespace Slinky.Api
{
    internal interface IHttpRequestHandlingStrategy<TRequestParams>
    {
        Task<IActionResult> HandleRequest(ControllerBase controller, TRequestParams requestParams);
    }
}
