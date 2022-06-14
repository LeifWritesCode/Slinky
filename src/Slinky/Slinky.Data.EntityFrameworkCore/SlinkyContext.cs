using Microsoft.EntityFrameworkCore;
using System.Net;
using Slinky.Data.Model;
using Microsoft.Extensions.Configuration;

namespace Slinky.Data.EntityFrameworkCore
{
    internal abstract class SlinkyContext : DbContext
    {
        private readonly IConfiguration configuration;

        public DbSet<Audit> Audits { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Shortlink> Shortlinks { get; set; }

        public SlinkyContext(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Shortlink>()
                .Property(t => t.Uri)
                .HasConversion(v => v.ToString(), v => new Uri(v));
            modelBuilder.Entity<User>()
                .Property(t => t.IpAddress)
                .HasConversion(v => v.ToString(), v => IPAddress.Parse(v));
        }
    }
}
