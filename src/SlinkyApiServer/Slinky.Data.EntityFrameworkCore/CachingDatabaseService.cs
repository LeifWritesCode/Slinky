using Slinky.Data.Model;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Slinky.Data.EntityFrameworkCore
{
    /// <summary>
    /// Wrapper class for DataProvider implementing caching.
    /// </summary>
    internal class CachingDatabaseService : DatabaseService
    {
        private readonly IDistributedCache cache;

        public CachingDatabaseService(
            ILogger<CachingDatabaseService> logger,
            SlinkyContext shortlinkContextBase,
            IDistributedCache distributedCache)
            : base(logger, shortlinkContextBase)
        {
            cache = distributedCache;
        }

        public async override Task<Shortlink> ReadShortlinkAsync(string id)
        {
            var result = default(Shortlink);

            var json = await cache.GetStringAsync(id);
            if (!string.IsNullOrWhiteSpace(json))
            {
                try
                {
                    result = JsonSerializer.Deserialize<Shortlink>(json);
                    await cache.RefreshAsync(id);
                }
                catch (Exception)
                {
                    logger.LogWarning("shortlink {} was malformed in cache - will refreshing from database", id);
                }
            }

            // Pull from the database if a cache miss (either malformed or not exists)
            if (result is default(Shortlink))
            {
                result = await base.ReadShortlinkAsync(id);
                if (result is null)
                {
                    throw new InvalidOperationException("something went horribly wrong here");
                }

                // update the cache, too
                json = JsonSerializer.Serialize(result);
                await cache.SetStringAsync(id, json);
            }

            return result;
        }

        public override async Task<Shortlink> CreateShortlinkAsync(string id, Uri uri)
        {
            var shortlink = await base.CreateShortlinkAsync(id, uri);
            var json = JsonSerializer.Serialize(shortlink);
            await cache.SetStringAsync(id, json);
            return shortlink;
        }
    }
}
