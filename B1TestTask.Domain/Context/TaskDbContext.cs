using B1TestTask.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace B1TestTask.DataAccess.Context
{
    public class TaskDbContext : DbContext
    {
        public DbSet<MergedFile> Files { get; set; }
        public DbSet<FileLine> FileLines { get; set; }
        public DbSet<ExelReport> Reports { get; set; }
        public DbSet<ExelRow> ExelRows { get; set; }
        public DbSet<BankAccountClass> AccountClasses { get; set; }

        public TaskDbContext(DbContextOptions<TaskDbContext> options) : base(options)
        {
            Database.Migrate();
        }
    }
}
