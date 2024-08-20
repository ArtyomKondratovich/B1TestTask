using B1TestTask.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace B1TestTask.Domain.Context
{
    public class FirstTaskDbContext : DbContext
    {
        public DbSet<MergedFile> Files { get; set; }
        public DbSet<FileLine> FileLines { get; set; }

        public FirstTaskDbContext(DbContextOptions<FirstTaskDbContext> options) : base(options) { }
    }
}
