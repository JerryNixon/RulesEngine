using RuleEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RuleEngine.Database
{
    public class DatabaseService
    {
        private IConfiguration _configuration;
        private DatabaseContext _context;

        public DatabaseService()
            : this(new Configuration())
        {
            // empty
        }

        public DatabaseService(IConfiguration configuration)
        {
            _configuration = configuration;
            CreateContext();
        }

        private void CreateContext()
        {
            _context = new DatabaseContext(_configuration.DatabasePath);
            _context.Database.EnsureCreated();
        }

        public void DeleteDatabase()
        {
            _context.Database.EnsureDeleted();
        }

        public int RecordCount()
        {
            return _context.Records.Count();
        }

        public bool TryAddRecord(Record record)
        {
            try
            {
                _context.Records.Add(record);
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool TryGetRecords(string key, DateTime from, out IEnumerable<Record> records)
        {
            try
            {
                records = _context.Records
                  .Where(x => x.Key == key)
                  .Where(x => x.DateTime >= from);
                return true;
            }
            catch (Exception)
            {
                records = default(IEnumerable<Record>);
                return false;
            }
        }

        public bool TryClearRecords(string key)
        {
            try
            {
                _context.Records
                    .RemoveRange(_context.Records.Where(x => x.Key == key));
                return !_context.Records.Any();
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool TryClearRecords()
        {
            try
            {
                _context.Records
                    .RemoveRange(_context.Records);
                return !_context.Records.Any();
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
