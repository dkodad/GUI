using Microsoft.EntityFrameworkCore;
using Razor.Models.Entities;

namespace Razor.Data
{
    public class RazorDbContext : DbContext
    {
        public RazorDbContext(DbContextOptions<RazorDbContext> options) : base(options) 
        {
        
        }

        public DbSet<Article> Articles { get; set; }
    }
}
