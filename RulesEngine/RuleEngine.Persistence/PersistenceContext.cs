using Microsoft.EntityFrameworkCore;
using RuleEngine.Models;

namespace RuleEngine.Persistence
{
    public class PersistenceContext : DbContext
    {
        private string _path;

        public DbSet<Record> Records { get; set; }

        public PersistenceContext(string path)
        {
            _path = path;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={_path}");
        }
    }
}
