using Microsoft.EntityFrameworkCore;
using RuleEngine.Models;

namespace RuleEngine.Database
{
    internal class DatabaseContext : DbContext
    {
        private string _path;

        public DbSet<Record> Records { get; set; }

        public DatabaseContext(string path)
        {
            _path = path;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={_path}");
        }
    }
}
