using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Slinky.Data.EntityFrameworkCore
{
    internal class InMemorySlinkyContext : SlinkyContext
    {
        public InMemorySlinkyContext(IConfiguration configuration)
            : base (configuration)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseInMemoryDatabase("shortlinkDebugContext",
                options => options.EnableNullChecks(true));
        }
    }
}
