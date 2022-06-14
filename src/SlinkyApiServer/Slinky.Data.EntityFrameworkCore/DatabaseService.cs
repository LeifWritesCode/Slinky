using Slinky.Data.EntityFrameworkCore;
using Slinky.Data.Model;
using Microsoft.Extensions.Logging;

namespace Slinky.Data.EntityFrameworkCore
{
    internal class DatabaseService : IDatabaseService
    {
        protected readonly ILogger logger;
        private readonly SlinkyContext shortlinkContextBase;

        public DatabaseService(
            ILogger<DatabaseService> logger,
            SlinkyContext shortlinkContextBase)
        {
            this.logger = logger;
            this.shortlinkContextBase = shortlinkContextBase;
        }

        public virtual async Task<Shortlink> ReadShortlinkAsync(string id)
        {
            await Task.CompletedTask;
            return shortlinkContextBase.Shortlinks.SingleOrDefault(e => e.Id == id);
        }

        public virtual async Task<Shortlink> CreateShortlinkAsync(string id, Uri uri)
        {
            var shortlink = new Shortlink()
            {
                Id = id,
                Uri = uri,
            };

            await shortlinkContextBase.Shortlinks.AddAsync(shortlink);
            await shortlinkContextBase.SaveChangesAsync();

            return shortlink;
        }

        public virtual async Task<Audit> CreateAuditAsync(Shortlink shortlink, AuditEvent auditEvent)
        {
            var audit = new Audit()
            {
                AuditEvent = auditEvent,
                DateTime = DateTime.UtcNow,
                ShortLinkId = shortlink.Id
            };

            await shortlinkContextBase.Audits.AddAsync(audit);
            await shortlinkContextBase.SaveChangesAsync();

            return audit;
        }
    }
}
