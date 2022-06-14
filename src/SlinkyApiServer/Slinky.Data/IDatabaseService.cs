using Slinky.Data.Model;

namespace Slinky.Data
{
    public interface IDatabaseService
    {
        Task<Shortlink> CreateShortlinkAsync(string id, Uri uri);

        Task<Shortlink> ReadShortlinkAsync(string id);

        Task<Audit> CreateAuditAsync(Shortlink shortlink, AuditEvent auditEvent);
    }
}
