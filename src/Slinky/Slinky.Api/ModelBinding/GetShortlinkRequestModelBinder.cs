using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Slinky.Api.Protocol;

namespace Slinky.Api.ModelBinding
{
    internal class GetShortlinkRequestModelBinder : IModelBinder
    {
        private readonly BodyModelBinder defaultBinder;

        public GetShortlinkRequestModelBinder(IList<IInputFormatter> formatters, IHttpRequestStreamReaderFactory readerFactory) // : base(formatters, readerFactory)
        {
            defaultBinder = new BodyModelBinder(formatters, readerFactory);
        }

        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            await defaultBinder.BindModelAsync(bindingContext);

            if (bindingContext.Result.IsModelSet)
            {
                if (bindingContext.Result.Model is GetShortlinkRequest data)
                {
                    var value = bindingContext.ValueProvider.GetValue("id").FirstValue;
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        data.Id = value;
                    }

                    bindingContext.Result = ModelBindingResult.Success(data);
                }
            }
        }
    }
}
