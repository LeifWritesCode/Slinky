using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Snowflake.Core;
using Slinky.Api.Protocol;
using Slinky.Api.Strategies;
using Slinky.Api.ModelBinding;
using Slinky.Data.EntityFrameworkCore;

namespace Slinky.Api
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            var app = BuildApplication(args);
            ConfigureApplication(app);
            await app.RunAsync();
        }

        private static WebApplication BuildApplication(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services
                .AddTransient<ISnowflakeProvider>(services =>
                {
                    var configuration = builder.Services.BuildServiceProvider().GetRequiredService<IConfiguration>();
                    var machineId = configuration.GetValue<uint>("MachineId");
                    var workerId = configuration.GetValue<uint>("WorkerId");

                    return ActivatorUtilities.CreateInstance<SnowflakeProvider>(services, machineId, workerId);
                })
                .AddTransient<IHttpRequestHandlingStrategy<GetShortlinkRequest>, GetShortlinkRequestHandlingStrategy>()
                .AddTransient<IHttpRequestHandlingStrategy<PostShortlinkRequest>, PostShortlinkRequestHandlingStrategy>()
                .UseCachingDebugEFCoreDatabaseServices()
                .AddApiVersioning(options =>
                {
                    options.DefaultApiVersion = new ApiVersion(1, 0);
                    options.AssumeDefaultVersionWhenUnspecified = true;
                })
                .AddVersionedApiExplorer(options =>
                {
                    options.GroupNameFormat = "'v'VVV";
                    options.SubstituteApiVersionInUrl = true;
                })
                .AddSwaggerGen()
                .AddControllers()
                .AddMvcOptions(options =>
                {
                    var readerFactory = builder.Services.BuildServiceProvider().GetRequiredService<IHttpRequestStreamReaderFactory>();
                    options.ModelBinderProviders.Insert(0, new GetShortlinkRequestModerBinderProvider(options.InputFormatters, readerFactory));
                });
            return builder.Build();
        }

        private static void ConfigureApplication(WebApplication application)
        {
            // Configure the HTTP request pipeline.
            if (application.Environment.IsDevelopment())
            {
                application.UseSwagger();
                application.UseSwaggerUI();
            }
            application.UseAuthorization();

            application.MapControllers();
        }
    }
}
