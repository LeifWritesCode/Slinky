using Slinky.Api.Protocol;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Slinky.Api.ModelBinding
{
    internal class GetShortlinkRequestModerBinderProvider : IModelBinderProvider
    {
        private readonly IList<IInputFormatter> formatters;
        private readonly IHttpRequestStreamReaderFactory readerFactory;

        public GetShortlinkRequestModerBinderProvider(IList<IInputFormatter> formatters, IHttpRequestStreamReaderFactory readerFactory)
        {
            this.formatters = formatters;
            this.readerFactory = readerFactory;
        }

        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            if (context.Metadata.ModelType == typeof(GetShortlinkRequest))
            {
                return new GetShortlinkRequestModelBinder(formatters, readerFactory);
            }

            return null;
        }
    }
}
