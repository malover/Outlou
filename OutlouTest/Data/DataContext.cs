using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OutlouTest.Models;

namespace OutlouTest.Data
{
    public class DataContext : IdentityDbContext<AppUser>
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<FeedItem> FeedItems { get; set; }
        public DbSet<FeedSource> FeedSources { get; set; }
    }
}
