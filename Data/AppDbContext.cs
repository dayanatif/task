using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TSManager.Models;

namespace TSManager.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<SoftwareSubscription> SoftwareSubscriptions => Set<SoftwareSubscription>();
        public DbSet<EmailUser> EmailUsers => Set<EmailUser>();
        public DbSet<DomainDetail> Domains => Set<DomainDetail>();
        public DbSet<NotificationLog> NotificationLogs => Set<NotificationLog>();

    }
}
